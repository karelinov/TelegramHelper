using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TL;
using WTelegram;
using TL.Methods;

namespace SSNotifier
{
  /// <summary>
  /// Класс - монитор сообщений Telegram
  /// Содержит функцию Monitor для отслеживания (новых) сообщений от указанных пользователей 
  /// и 1) показа их в UI 2) перенаправления в специальный чат
  /// </summary>
  public class MonitorHelper
  {
    private static bool _StopMonitor = false; // флаг для остановки функции мониторинга, выставляется внешними функциями, запустившими поток с задачей мониторинга
    private static Object _StopMonitorObj = new Object();
    public static bool StopMonitor
    {
      get
      {
        lock(_StopMonitorObj)
        {
          return _StopMonitor;
        }
      }
      set
      {
        lock (_StopMonitorObj)
        {
          _StopMonitor = value;
        }
      }
    }

    private static long[] chatsToMonitor;
    private static long[] usersToMonitor;
    private static long chatToForwardId;
    //private static long userToForwardId;


    /// <summary>
    /// Состояния (обновлений) чатов и каналов
    /// </summary>
    private static Updates_State curChatPts;
    private static Dictionary<long, int> curChannelPtss;


    private static Object _syncronizedTimeSyncObj = new object();
    private static DateTime _syncronizedTime = DateTime.MinValue;
    private static DateTime SyncronizedTime
    {
      get
      {
        lock(_syncronizedTimeSyncObj)
        {
          return _syncronizedTime;
        }
      }
      set
      {
        lock (_syncronizedTimeSyncObj)
        {
          _syncronizedTime = value;
        }
      }
    }

    public static void Monitor()
    {
      System.Console.WriteLine("Begin monitoring...");
      StopMonitor = false;

      try
      {

        // Читаем настройки отслеживаемых чатов и пользователей
        chatsToMonitor = ConfigurationManager.AppSettings["monitor chats"].Split(',').Select(item => long.Parse(item)).ToArray();
        usersToMonitor = ConfigurationManager.AppSettings["monitor users"].Split(',').Select(item => long.Parse(item)).ToArray();
        chatToForwardId = long.Parse(ConfigurationManager.AppSettings["chatToForward"]);
        //userToForwardId = long.Parse(ConfigurationManager.AppSettings["userToForward"]);

        curChatPts = TelegramClientHelper.UserClient.Updates_GetState().GetAwaiter().GetResult();
        curChannelPtss = new Dictionary<long, int>();
        foreach (long curChatId in chatsToMonitor)
        {
          if (chatsToMonitor.Contains(curChatId) && ChatListHelper.GetCachedChat(curChatId) is Channel)
          {
            int channelInitialPts = (TelegramClientHelper.UserClient.Channels_GetFullChannel(ChatListHelper.GetCachedChat(curChatId) as Channel).GetAwaiter().GetResult().full_chat as ChannelFull).pts;
            channelInitialPts = channelInitialPts > 15 ? channelInitialPts - 100 : 1;
            curChannelPtss.Add(curChatId, channelInitialPts);
          }
        }

        // Бесконечный цикл ручной проверки обновлений каждый N секунд
        while (!_StopMonitor)
        {
          HandleChatUpdates();
          HandleChannelUpdates();
          if (SyncronizedTime < DateTime.Now) // отмечаем время, которое уже обработали, чтобы при перезапуске понимать, какие сообщения уже не надо пересылать
            SyncronizedTime = DateTime.Now; 

          System.Threading.Thread.Sleep(3000);
        }

      }
      catch (Exception e)
      {
        System.Console.WriteLine("Monitor Function Exception "+e.Message);
      }

    }

    /// <summary>
    /// Функция проверки обновлений чатов (отличны от обновлений каналов и супргрупп)
    /// Вызывает Updates_GetDifference, при наличии отслеживаемых сообщений форвардит их 
    /// и запоминает последнее состояние для следующей проверки наличия обновлений
    /// </summary>
    private static void HandleChatUpdates()
    {
      System.Console.WriteLine("status:Получаем обновления для chats ...");

      Client userClient = TelegramClientHelper.UserClient;


      Updates_DifferenceBase differences = null;
      Task<Updates_DifferenceBase> taskDifferences = userClient.Updates_GetDifference(curChatPts.pts, curChatPts.date, curChatPts.qts);
      bool intime = taskDifferences.Wait(5000);
      if (intime)
      {
        differences = taskDifferences.Result;
      }
      else
        throw new TimeoutException();
      

      foreach (MessageBase newMessage in differences.NewMessages)
      {
        if (MessageToForward(newMessage))
        {
          if(newMessage.Date >= SyncronizedTime) // отправляем только сообщения, которые после отмеченного времени 
          {
            ForwardMessage(newMessage as Message, GetInputPeerbyChatId(chatToForwardId));
            ForwardBotHelper.NotifyForwardMessage(newMessage as Message);
            System.Console.WriteLine("forwarded message " + newMessage.Date.ToString("yyyy-MM-dd hh:mm:ss") + " " + (newMessage as Message).message);
          }
        }
      }

      if (differences is Updates_DifferenceEmpty)
      {
        // нет изменений ну и ладно
      }
      if (differences is Updates_DifferenceSlice)
      {
        curChatPts = (differences as Updates_DifferenceSlice).State;
      }
      else if (differences is Updates_Difference)
      {
        curChatPts = (differences as Updates_Difference).State;
      }
      else if (differences is Updates_DifferenceTooLong)
      {
        curChatPts = (differences as Updates_DifferenceTooLong).State;
      }
      System.Console.WriteLine("status:Завершено получение обновлений для chats ");
    }

    /// <summary>
    /// Функция оббаботки обновлений каналов
    /// Крутится, пока не выгребет из отслеживаемых каналов все обновления, 
    /// отслеживаемые сообщения из обновлений форвардятся 
    /// Вызывает Updates_GetChannelDifference, по результату обработки сохраняет состояние обновления в разрезе канала
    /// </summary>
    private static void HandleChannelUpdates()
    {
      // Для каждого канала, который мониторим
      long[] Ids = curChannelPtss.Keys.ToArray(); // приходится для прохода выгружать отдельно список, а то элементы меняются и foreach ругается
      foreach (long channelToMonitorId in Ids)
      {
        bool finalFlag = false; // будем выгребать канал, пока не придёт флаг, что всё
        while (!finalFlag)
        {
          // Получаем обновления
          System.Console.WriteLine("status:Получаем обновления для channel= " + channelToMonitorId + "...");

          int curChannelPts = curChannelPtss[channelToMonitorId];
          Channel curChannel = ChatListHelper.GetCachedChat(channelToMonitorId) as Channel;

          Updates_ChannelDifferenceBase differences = TelegramClientHelper.UserClient.Updates_GetChannelDifference(curChannel, new ChannelMessagesFilter(){ }, curChannelPts, 100).GetAwaiter().GetResult();

          // Каждое полученное в обновлении сообщение перенаправляем (если оно от нужных пользователей)
          foreach (MessageBase newMessage in differences.NewMessages)
          {
            if (MessageToForward(newMessage))
            {
              if (newMessage is Message)
              {
                if (newMessage.Date >= SyncronizedTime) // отправляем только сообщения, которые после отмеченного времени 
                {
                  ForwardMessage(newMessage as Message, GetInputPeerbyChatId(chatToForwardId));
                  ForwardBotHelper.NotifyForwardMessage(newMessage as Message);
                  System.Console.WriteLine("forwarded message " + newMessage.Date.ToString("yyyy-MM-dd hh:mm:ss")+" " + (newMessage as Message).message);
                }
              }
            }
          }

          // Проверяем и выставляем флаг, что вынули из канала все обновления - это позволит перейти к следующему каналу в списке
          if (differences is Updates_ChannelDifference)
          {
            curChannelPtss[channelToMonitorId] = (differences as Updates_ChannelDifference).pts;
            finalFlag = (differences as Updates_ChannelDifference).Final;
          }
          else if (differences is Updates_ChannelDifferenceEmpty)
          {
            finalFlag = true; // ничего нового
          }
          else if (differences is Updates_ChannelDifferenceTooLong)
          {
            // хреновый вариант, обновлений слишком много, обновления не будут получены, надо начинать получать обновления с текущего времени
            curChannelPtss[channelToMonitorId] = (TelegramClientHelper.UserClient.Channels_GetFullChannel(ChatListHelper.GetCachedChat(channelToMonitorId) as Channel).GetAwaiter().GetResult().full_chat as ChannelFull).pts;
            System.Console.WriteLine("обнаружено, что невозможно получить последние обновления для channel= " + channelToMonitorId+" - будут извлекаться только новые");
          }
        }
      }
      System.Console.WriteLine("status:Завершено получение обновлений для channels ");
    }


    public static UpdatesBase ForwardMessage(Message message, InputPeer peerTo, InputPeer peerFrom = null)
    {
      UpdatesBase result;

      if (peerFrom == null)
      {
        if (message.Peer is PeerChannel || message.Peer is PeerChat)
          peerFrom = GetInputPeerbyChatId(message.Peer.ID);
        else if (message.Peer is PeerUser)
          peerFrom = GetInputPeerByUserId(message.Peer.ID);
        else
          throw new Exception("Неизвестный тип источника сообщения для перенаправляения =" + message.From.GetType().Name);
      }

      result = TLCH.UserClient.Messages_ForwardMessages(peerFrom, new int[] { message.ID }, new long[] { new Random().Next(0, int.MaxValue) }, peerTo).Result;

      return result;
    }


    /// <summary>
    /// Вспомогательная функция конструирует объект Peer по chatId (для отправки сообщения)
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    public static InputPeer GetInputPeerbyChatId(long chatId)
    {
      InputPeer result;

      result = ChatListHelper.GetCachedChat(chatId).ToInputPeer();

      return result;
    }


    /// <summary>
    /// Вспомогательная функция конструирует объект InputPeer по Peer (для отправки сообщения)
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    public static InputPeer GetInputPeerByUserId(long userId)
    {
      InputPeer result;

      User user = UserListHelper.GetUser(userId);
      result = user.ToInputPeer();

      return result;
    }

    /// <summary>
    /// Функция определяет, надо ли перенаправлять сообщение 
    /// проверяя, что оно из отслеживаемого чата и от отслеживаемого пользователя
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static bool MessageToForward(MessageBase message)
    {
      bool result = false;

      if (chatsToMonitor.Contains(message.Peer.ID))
      {
        if (usersToMonitor.Contains(message.From.ID))
        {
          result = true;
        }
      }

      return result;
    }

  }
}

