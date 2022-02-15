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
  class MonitorHelper
  {
    private static long[] chatsToMonitor;
    private static long[] usersToMonitor;
    private static long chatToForwardId;
    //private static long userToForwardId;

    /// <summary>
    /// Состояния (обновлений) чатов и каналов
    /// </summary>
    private static Updates_State curChatPts;
    private static Dictionary<long, int> curChannelPtss;

    public static void Monitor()
    {
      System.Console.WriteLine("Begin monitoring...");

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
          channelInitialPts = channelInitialPts > 15? channelInitialPts - 100: 1;
          curChannelPtss.Add(curChatId, channelInitialPts);
        }
      }

      // Запуск дочернего процесса бота нотификаций, который заменяет пересланные мониторингом
      // сообщения на такие же, но свои, чтобы были нотификации
      Process childBotProcess = null; 

      // Бесконечный цикл ручной проверки обновлений каждый N секунд
      while (true)
      {
        //// ззапускает дочерний процесс бота по необходимости
        //if (childBotProcess == null || childBotProcess.HasExited)
        //{
        //  childBotProcess = ForwardBotHelper.StartBotProcess(Process.GetCurrentProcess());
        //  System.Console.WriteLine("Spawnet bot process "+ childBotProcess.Id);
        //}

        HandleChatUpdates();
        HandleChannelUpdates();

        System.Threading.Thread.Sleep(1000);
      }



    }

    /// <summary>
    /// Функция проверки обновлений чатов (отличны от обновлений каналов и супргрупп)
    /// Вызывает Updates_GetDifference, при наличии отслеживаемых сообщений форвардит их 
    /// и запоминает последнее состояние для следующей проверки наличия обновлений
    /// </summary>
    private static void HandleChatUpdates()
    {
      Updates_DifferenceBase differences = TelegramClientHelper.UserClient.Updates_GetDifference(curChatPts.pts, curChatPts.date, curChatPts.qts).GetAwaiter().GetResult();

      foreach (MessageBase newMessage in differences.NewMessages)
      {
        if (MessageToForward(newMessage))
        {
          TLMH.ForwardMessage(newMessage as Message, TLMH.GetInputPeerbyChatId(chatToForwardId));
          System.Console.WriteLine("forwarded message id=" + newMessage.ID + " " + (newMessage as Message).message);
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
                TLMH.ForwardMessage(newMessage as Message, TLMH.GetInputPeerbyChatId(chatToForwardId));
                //var res = TelegramClientHelper.UserClient.Messages_MarkDialogUnread(new InputDialogPeer() { peer = ChatListHelper.GetCachedChat(channelToMonitorId).ToInputPeer() }, false).GetAwaiter().GetResult(); 
                InputPeer from_peer = TLMH.GetInputPeerbyChatId(newMessage.Peer.ID);
                //from_peer = TLMH.GetInputPeerByUserId(newMessage.From.ID);
                InputPeer to_peer = TLMH.GetInputPeerbyChatId(chatToForwardId);

                Contacts_ResolvedPeer contacts_ResolvedPeer = TLCH.BotClient.Contacts_ResolveUsername("Oleg_Karelin").Result;

                //Message sentMessage = TLCH.BotClient.SendMessageAsync(contacts_ResolvedPeer.UserOrChat.ToInputPeer(), (newMessage as Message).message).Result;

                //UpdatesBase updatesBase = TLCH.BotClient.Messages_ForwardMessages(from_peer, new int[] { newMessage.ID }, new long[] { new Random().Next(0, int.MaxValue) }, to_peer).Result;

                System.Console.WriteLine("forwarded message id=" + newMessage.ID + " "+ (newMessage as Message).message);
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
          }
        }
      }
    }


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

