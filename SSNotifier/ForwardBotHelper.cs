using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using TL;
using System.IO;
using TL.Methods;
using System.Threading.Tasks;
using System.Net.Http;

namespace SSNotifier
{
  class ForwardBotHelper
  {
    //private static long chatToForwardId;
    //private static InputPeer chatToForwardPeer = null;
    //private static Dictionary<long,InputPeer> monitorChatPeers = null;
    private static Dictionary<long, InputPeer> resolvedPeers = new Dictionary<long, InputPeer>(); // закэшированные вычисленные пиры для чатов 


    /*
    public static Process StartBotProcess(Process parentMonitorProcess)
    {
      // получаем конфигурацию и создаём InputPeer для переадресации
      // параметры пира надо передать боту, потому что он не может (или я не понял как)
      // вызывать методы для получения access_hash
      chatToForwardId = long.Parse(ConfigurationManager.AppSettings["chatToForward"]);
      InputPeer inputPeer = TLMH.GetInputPeerbyChatId(chatToForwardId);
      long chatToForwardHash = -1;
      if (inputPeer is InputPeerChannel)
        chatToForwardHash = (inputPeer as InputPeerChannel).access_hash;

      // Получаем конфигурацию и создаём InputPeer для отслеживаемых чатов
      long[] chatsToMonitor = ConfigurationManager.AppSettings["monitor chats"].Split(',').Select(item => long.Parse(item)).ToArray();
      string chatsToMonitorStrInfo = "";
      foreach (long chatToMonitor in chatsToMonitor)
      {
        InputPeer inputPeer1 = TLMH.GetInputPeerbyChatId(chatToMonitor);
        if (chatsToMonitorStrInfo != "") chatsToMonitorStrInfo += ";";
        if (inputPeer1 is InputPeerChannel)
          chatsToMonitorStrInfo += chatToMonitor.ToString() + ":" + (inputPeer1 as InputPeerChannel).access_hash;
        else if (inputPeer1 is InputPeerChat)
          chatsToMonitorStrInfo += chatToMonitor.ToString() + ":-1";
      }

        // пускаем дочерний процесс
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
          FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".exe"),
          //Arguments = RunMode.notifybot.ToString() + " " + parentMonitorProcess.Id + " " + chatToForwardHash + " "+chatsToMonitorStrInfo
      };
      Process botProcess = Process.Start(processStartInfo);

      return botProcess;
    }
    */

    /*
    /// <summary>
    /// Тело программы бота
    /// Создаёт клиента, подключает к нему обработчик событий Update 
    /// и бесконечно крутится в ожидании завершения родительского процесса (тогда и сам ... того)
    /// </summary>
    public static void Forward(string[] args)
    {
      System.Console.WriteLine("Begin notifying...");
      System.Console.ReadLine();
      // получаем конфигурацию и создаём InputPeer для переадресации
      chatToForwardId = long.Parse(ConfigurationManager.AppSettings["chatToForward"]);
      long chatToForwardHash = long.Parse(args[2]);
      if (chatToForwardHash == -1)
        chatToForwardPeer = new InputPeerChat(chatToForwardId);
      else
        chatToForwardPeer = new InputPeerChannel(chatToForwardId, chatToForwardHash);

      
      // получаем список InputPeer отслеживаемых чатов
      monitorChatPeers = new Dictionary<long, InputPeer>();
      if(args.Length > 3)
      {
        string[] cal = args[3].Split(";");
        Tuple<long, long>[] cat = cal.Select(s => new Tuple<long, long>(long.Parse(s.Split(":")[0]), long.Parse(s.Split(":")[1]))).ToArray();
        foreach (var catinfo in cat)
        {
          InputPeer inputPeer;
          if (catinfo.Item2 == -1)
            inputPeer = new InputPeerChat(catinfo.Item1);
          else
            inputPeer = new InputPeerChannel(catinfo.Item1, catinfo.Item2);
          monitorChatPeers.Add(catinfo.Item1, inputPeer);
        }
      }


      TelegramClientHelper.BotClient.Update += Client_Update;

      // получаем из аргументов ID родительского процесса
      int parentProcessID;
      if (!(args.Length > 1) || !(int.TryParse(args[1], out parentProcessID)))
      {
        System.Console.WriteLine(args.Length);
        foreach (var arg in args)
          System.Console.WriteLine(arg);

        throw new Exception("Cannot acquire parent process ID ");
      }
        


      // бесконечно крутимся в ожидании завершения родительского процесса
      while (!Process.GetProcessById(parentProcessID).HasExited)
      {
        System.Threading.Thread.Sleep(1000); // тут ничего делать не надо, т.к. вся работа - в асинхронном Client_Update
      }

    }
    */

    /*
    /// <summary>
    /// Встроенное в WTelegramClient событие получения обновлений
    /// Сообщения из обновлений отражаются отправителю
    /// (Подразумевается, что отправитель сбрасывает в чат бота важные сообщения, а бот отвечает на них, порождая нотификации)
    /// </summary>
    /// <param name="arg"></param>
    private static void Client_Update(IObject arg)
    {
      System.Console.WriteLine("BOT received update...");

      if (arg is Updates)
      {
        System.Console.WriteLine("arg is Updates...");
        foreach (Update update in (arg as Updates).updates)
        {
          System.Console.WriteLine("handling update...");
          if (update is UpdateNewMessage)
          {
            System.Console.WriteLine("update is UpdateNewMessage...");
            MessageBase messageBase = (update as UpdateNewMessage).message;
            if (messageBase is Message) 
            {
              System.Console.WriteLine("messageBase is Message... id=" + (messageBase as Message).id);
              Message message = messageBase as Message;
              if (message.fwd_from == null || (message.fwd_from != null & message.fwd_from.from_id.ID != chatToForwardId))
              {
                //try
                //{
                //  var request = new Contacts_ResolveUsername() { username = "Oleg_Karelin" };
                //  var resultTask = TLCH.Client.Invoke(request);
                //  var awaiter = resultTask.GetAwaiter();
                //  while (!awaiter.IsCompleted) System.Threading.Thread.Sleep(1000);
                //  var result = awaiter.GetResult();
                //}
                //catch (Exception ex)
                //{
                //  throw ex;
                //}
                //InputPeer ip = rp.UserOrChat.ToInputPeer();

                System.Console.WriteLine("forwarding...");
                //TLCH.Client.SendMessageAsync(ip, message.message);

                //TLMH.ForwardMessage(message, inputPeer, inputPeer);
                //if (monitorChatPeers.ContainsKey(message.Peer.ID)) {
                //  TLMH.ForwardMessage(message, chatToForwardPeer, chatToForwardPeer);
                  System.Console.WriteLine("BOT forwarded message id=" + message.ID + " " + message.message);
                //}
                //else
                //  System.Console.WriteLine("BOT NOT forwarded message, InputPeer not Found. id=" + message.ID + " " + message.message);
              }
            }
          }
        }
      }
    }
    */

    public static void ForwardMessage(Message MessageToForward, long chatToForwardId)
    {
      InputPeer peerToForward = null;

      HttpClient httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri("https://api.telegram.org");
      var content = new FormUrlEncodedContent(new[] 
        {
          new KeyValuePair<string, string>("chat_id", "-100"+chatToForwardId.ToString()), 
          new KeyValuePair<string, string>("text", "!") 
        });
      HttpResponseMessage response = httpClient.PostAsync("/bot" + ConfigurationManager.AppSettings["bot_token"]+"/sendMessage",content).Result;

      /*
      if (resolvedPeers.ContainsKey(chatToForwardId))
        peerToForward = resolvedPeers[chatToForwardId];
      else
      {
        Contacts_ResolvedPeer crp = TLCH.BotClient.Contacts_ResolveUsername("+XES2ZiMhpJBkOTE6").Result;

        Messages_Chats chats = TLCH.BotClient.Messages_GetChats(new long[] { chatToForwardId }).Result;
        ChatBase chat = chats.chats.First().Value;

        if (chats.chats.Count == 0)
          System.Console.WriteLine("Бот не имеет доступа к чату " + chatToForwardId);
        else
        {
          resolvedPeers.Add(chatToForwardId, chats.chats[chatToForwardId]);
          peerToForward = chats.chats[chatToForwardId];
        }
      }

      if (peerToForward != null)
        TLCH.BotClient.SendMessageAsync(peerToForward, "NewMessage! "+ MessageToForward.message);

      Task<Message> st = TLCH.BotClient.SendMessageAsync(peerToForward, "NewMessage! ");
      st.Wait();
      var r = st.Result;
      */

    }

  }
}
