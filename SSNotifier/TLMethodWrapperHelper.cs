using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTelegram;
using TL;
using TL.Methods;


namespace SSNotifier
{
  class TLMH : TLMethodWrapperHelper { }; // просто как короткий алиас основного класса
  class TLMethodWrapperHelper
  {
    public static Messages_Chats GetAllChats()
    {
      Messages_Chats result;
      result = TLCH.UserClient.Messages_GetAllChats(new long[0]).Result;

      //var request = new Messages_GetAllChats() { except_ids  = new long[0] };
      //var task = TelegramClientHelper.Client.Invoke(request);
      //var awaiter = task.GetAwaiter();
      //while (!awaiter.IsCompleted) System.Threading.Thread.Sleep(1000);
      //result = awaiter.GetResult();

      return result;
    }
    
    public static Messages_Chats GetChats(long[] chatIds, WTelegram.Client client = null)
    {
      Messages_Chats result;
      if (client == null) client = TLCH.UserClient;
      result = client.Messages_GetChats(chatIds).Result;

      return result;
    }

    public static Messages_ChatFull GetFullChat(long chatId)
    {
      Messages_ChatFull result = null;

      result = TLCH.UserClient.GetFullChat(GetInputPeerbyChatId(chatId)).Result;

      return result;
    }


    public static UserBase[] GetUsers(long[] userIds, WTelegram.Client client = null)
    {
      UserBase[] result = null;

      InputUserBase[] inputUserBases = userIds.Select(uid => new InputUser() { user_id = uid }).ToArray();
      if (client == null) client = TLCH.UserClient;
      result = client.Users_GetUsers(inputUserBases).Result;

      return result;
    }

    public static Users_UserFull GetUserFull(long userId, WTelegram.Client client = null)
    {
      Users_UserFull result = null;

      InputUserBase inputUserBase = new InputUser() { user_id = userId };
      if (client == null) client = TLCH.UserClient;
      result = client.Users_GetFullUser(inputUserBase).Result;


      return result;
    }

    public static Updates_State GetState(WTelegram.Client client = null)
    {
      Updates_State result;
      if (client == null) client = TLCH.UserClient;
      result = client.Updates_GetState().Result;

      return result;
    }


    public static Updates_DifferenceBase GetDifference(Updates_State pts, WTelegram.Client client = null)
    {
      Updates_DifferenceBase result;
      if (client == null) client = TLCH.UserClient;
      result = client.Updates_GetDifference(pts.pts, pts.date, pts.qts).Result;

      return result;
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

    public static Contacts_Contacts GetAllContacts()
    {
      Contacts_Contacts result;

      result = TLCH.UserClient.Contacts_GetContacts(TLCH.CurrentUser.access_hash).Result ;

      return result;
    }

    public static Messages_AffectedMessages DeleteMessage(Message message)
    {
      Messages_AffectedMessages result;

      Channel channel = null;
      if (message.Peer is PeerChannel)
      {
        if(ChatListHelper.CachedChatsInfo.ContainsKey(message.Peer.ID))
        {
          channel = ChatListHelper.CachedChatsInfo[message.Peer.ID] as Channel;
        }
      }
      if(channel == null)
      {
        System.Console.WriteLine("Cannot construct InputChannelObject from Peer " + message.Peer == null ? "null" : message.Peer.GetType().Name);
        return null;
      }

      result = TLCH.UserClient.Channels_DeleteMessages(channel, new int[] { message.ID }).Result;
      return result;
    }

  }
}
