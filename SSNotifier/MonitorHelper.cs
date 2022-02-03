using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TeleSharp.TL.Updates;

namespace SSNotifier
{
  class MonitorHelper
  {
    public static void Monitor()
    {
      int curPts = 0;
      System.Console.WriteLine("Begin monitoring...");

      int[] chatsToMonitor = ConfigurationManager.AppSettings["monitor chats"].Split().Select(item => int.Parse(item)).ToArray();
      int[] usersToMonitor = ConfigurationManager.AppSettings["monitor users"].Split(',').Select(item => int.Parse(item)).ToArray();
      int chatToForwardId = int.Parse(ConfigurationManager.AppSettings["chatToForward"]);

      TLAbsInputPeer forwardPeer = GetForwardPeer(chatToForwardId); // Сразу получаем объект для пересылки сообщений, чтобы не создавать каждый раз 

      while (true)
      {
        int newPts = TLMethodWrapperHelper.GetState();

        if (newPts > curPts)
        {
          System.Console.WriteLine("Update state changed to " + newPts.ToString() + "...");

          TLDifference tLDifference = (TLDifference)TLMethodWrapperHelper.GetDifference(curPts);

          foreach (TLMessage newMessage in tLDifference.NewMessages)
          {
            if (CheckMessageInChatList(newMessage, chatsToMonitor))
            {
              if (usersToMonitor.Contains(newMessage.FromId != null ? (int)newMessage.FromId : -1))
              {
                TLAbsUpdates tLAbsUpdates = TLMethodWrapperHelper.ForwardMessage(newMessage.Id, forwardPeer);
                System.Console.WriteLine("forwarded message id=" + newMessage.ToString());
              }
            }

            curPts = newPts;
          }


          System.Threading.Thread.Sleep(1000);
        }



      }
    }

    private static bool CheckMessageInChatList(TLMessage message, int[] chatsToMonitor)
    {
      bool result = false;
      int chat_id = -1;

      if (message.ToId.GetType() == typeof(TLPeerChat))
        chat_id = ((TLPeerChat)message.ToId).ChatId;
      else if ((message.ToId.GetType() == typeof(TLPeerChannel)))
        chat_id = ((TLPeerChannel)message.ToId).ChannelId;

      result = chatsToMonitor.Contains(chat_id);

      return result;
    }

    /// <summary>
    /// Вспомогательная функция конструирует объект Peer по chatId (для отправки сообщения)
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    private static TLAbsInputPeer GetForwardPeer(int chatId)
    {
      TLAbsInputPeer result;

      //TLChats tLAbsChats = (TLChats)TLMethodWrapperHelper.GetChats(chatId);
      TeleSharp.TL.Messages.TLChatFull tLChatFull = TLMethodWrapperHelper.GetFullChat(chatId);

      if (tLChatFull.Chats.Count > 0)
      {
        if (tLChatFull.Chats[0].GetType() == typeof(TLChat))
          result = new TLInputPeerChat() { ChatId = chatId };
        else if (tLChatFull.Chats[0].GetType() == typeof(TLChannel))
          result = new TLInputPeerChannel() { ChannelId = chatId };
        else
          throw new Exception("unknown chat type =" + tLChatFull.Chats[0].GetType().ToString());

      }
      else
        throw new Exception("Char id=" + chatId.ToString() + " not found");


      return result;
    }

  }
}
