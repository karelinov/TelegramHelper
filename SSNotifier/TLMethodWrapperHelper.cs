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
  class TLMethodWrapperHelper
  {
    public static Messages_Chats GetAllChats()
    {
      Messages_Chats result;

      var request = new Messages_GetAllChats() { except_ids  = new long[0] };
      var task = TelegramClientHelper.Client.Invoke(request);
      var awaiter = task.GetAwaiter();
      while (!awaiter.IsCompleted) System.Threading.Thread.Sleep(1000);
      result = awaiter.GetResult();

      return result;
    }
    /*
    public static TLAbsChats GetChats(int chatId)
    {
      TLAbsChats result;

      var request = new TLRequestGetChats() { Id = new TLVector<int>() { chatId } };
      result = TelegramClientHelper.Client.SendRequestAsync<TLAbsChats>(request).GetAwaiter().GetResult();

      return result;
    }

    public static TeleSharp.TL.Messages.TLChatFull GetFullChat(int chatId)
    {
      TeleSharp.TL.Messages.TLChatFull result;

      var request = new TLRequestGetFullChat() { ChatId = chatId } ;
      result = TelegramClientHelper.Client.SendRequestAsync<TeleSharp.TL.Messages.TLChatFull>(request).GetAwaiter().GetResult();

      return result;
    }
    */
    public static Contacts_Contacts GetAllContacts()
    {
      Contacts_Contacts result = null;

      var request = new Contacts_GetContacts() { hash = 0 } ;
      result = TelegramClientHelper.Client.Invoke(request).GetAwaiter().GetResult();

      return result;
    }

    public static UserBase[] GetUsers(long[] userIds)
    {
      UserBase[] result = null;

      InputUserBase[] inputUserBases = userIds.Select(uid => new InputUser() { user_id = uid }).ToArray();

      var request = new Users_GetUsers() { id = inputUserBases };
      result = TelegramClientHelper.Client.Invoke(request).GetAwaiter().GetResult();

      return result;
    }

    /*
    public static TLState GetState()
    {
      TLState result;

      var request = new TLRequestGetState();
      result = TelegramClientHelper.Client.SendRequestAsync<TLState>(request).GetAwaiter().GetResult();

      return result;
    }


    public static TLAbsDifference GetDifference(TLState pts)
    {
      TLAbsDifference result;

      var request = new TeleSharp.TL.Updates.TLRequestGetDifference() { Pts = pts.Pts, Date = pts.Date  };
      result = TelegramClientHelper.Client.SendRequestAsync<TLAbsDifference>(request).GetAwaiter().GetResult();

      return result;
    }

    public static TLAbsUpdates ForwardMessage(int MessageId, TLAbsInputPeer peer)
    {
      TLAbsUpdates result;

      var request = new TLRequestForwardMessage() { Peer = peer, Id = MessageId };
      result = TelegramClientHelper.Client.SendRequestAsync<TLAbsUpdates>(request).GetAwaiter().GetResult();

      return result;
    }
*/
  }
}
