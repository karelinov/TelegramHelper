using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TeleSharp.TL.Contacts;
using TLSharp.Core;
using TeleSharp.TL.Users;
using TeleSharp.TL.Updates;

namespace SSNotifier
{
  class TLMethodWrapperHelper
  {
    public static TLChats GetAllChats()
    {
      var request = new TLRequestGetAllChats() { ExceptIds = new TLVector<int>() };
      var task = TelegramClientHelper.Client.SendRequestAsync<TLChats>(request);
      var awaiter = task.GetAwaiter();
      while (!awaiter.IsCompleted) System.Threading.Thread.Sleep(1000);
      var res = awaiter.GetResult();

      return res;
    }

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

    public static TLAbsContacts GetAllContacts()
    {
      TLAbsContacts result = null;

      var request = new TLRequestGetContacts() { Hash = ""} ;
      result = TelegramClientHelper.Client.SendRequestAsync<TLAbsContacts>(request).GetAwaiter().GetResult();

      return result;
    }

    public static TLAbsUser[] GetUsers(int[] userIds)
    {
      TLAbsUser[] result = null;

      TLVector<TLAbsInputUser> tLAbsInputUsers = new TLVector<TLAbsInputUser>();
      foreach (int userId in userIds)
        tLAbsInputUsers.Add(new TLInputUser() { UserId = userId });


      var request = new TLRequestGetUsers() { Id = tLAbsInputUsers };
      TLVector<TLAbsUser> users = TelegramClientHelper.Client.SendRequestAsync<TLVector<TLAbsUser>>(request).GetAwaiter().GetResult();
      result = users.ToArray<TLAbsUser>();

      return result;
    }

    public static int GetState()
    {
      int result = -1;

      var request = new TLRequestGetState();
      result = TelegramClientHelper.Client.SendRequestAsync<int>(request).GetAwaiter().GetResult();

      return result;
    }


    public static TLAbsDifference GetDifference(int pts)
    {
      TLAbsDifference result;

      var request = new TeleSharp.TL.Updates.TLRequestGetDifference() { Pts = pts };
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

  }
}
