using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTelegram;
using TL.Methods;
using TL;

namespace SSNotifier
{
  class ChatUserListRetrievalHelper
  {
    /// <summary>
    /// Функция получает список чатов пользователя и выбрасывает его на экран
    /// Саписок фильтруется по значению второго параметра программы (если есть)
    /// </summary>
    /// <param name="args"></param>
    public static void GetChatList(string[] args)
    {
      Messages_Chats chats = TLMethodWrapperHelper.GetAllChats();

      System.Console.WriteLine("Found "+ chats.chats.Count.ToString() + (args.Length>1?" (filtered) ":"") + " chats: ");

      bool doWrite;
      string chatTite;
      long chatId;
      foreach (ChatBase chat in chats.chats.Values)
      {
        
        if (chat.GetType() == typeof(Chat))
        {
          chatTite = ((Chat)chat).Title;
          chatId = ((Chat)chat).ID;
        }
        else if (chat.GetType() == typeof(Channel))
        {
          chatTite = ((Channel)chat).Title;
          chatId = ((Channel)chat).ID;
        }
        else if (chat.GetType() == typeof(ChannelForbidden) || chat.GetType() == typeof(ChatForbidden) || chat.GetType() == typeof(ChatEmpty))
        {
          chatTite = "unsupported chat type " + chat.GetType().Name;
          chatId = -1;
        }
        else
        {
          chatTite = "unknown chat type "+chat.GetType().Name;
          chatId = -1;
        }

        doWrite = true;
        if ((args.Length > 1) && (!chatTite.ToUpper().Contains(args[1].ToString().ToUpper())))
          doWrite = false;

        if (doWrite)
        {
          string chatdata = "chat_id=" + chatId + " title=" + chatTite;
          System.Console.WriteLine(chatdata);
        }
      }
      System.Console.WriteLine("\r\nPress any key ...");
      System.Console.ReadKey();
    }

    /// <summary>
    /// Функция получает список контактов пользователя и выбрасывает его на экран
    /// Саписок фильтруется по значению второго параметра программы (если есть)
    /// </summary>
    /// <param name="args"></param>
    public static void GetContactList(string[] args)
    {
      Contacts_Contacts contacts =TLMethodWrapperHelper.GetAllContacts();

      System.Console.WriteLine("Found " + contacts.contacts.Length.ToString() + (args.Length > 1 ? " (filtered) " : "") + " contacts: ");

      long[] userIds = contacts.contacts.Select(c => c.user_id).ToArray<long>();
      UserBase[] users = TLMethodWrapperHelper.GetUsers(userIds);

      bool doWrite;
      string userName, userFI;
      long userId;
      foreach (UserBase user in users)
      {
        if (user.GetType() == typeof(User))
        {
          userFI = ((User)user).first_name + ' ' + ((User)user).last_name;
          userId = user.ID;
          userName = ((User)user).username;
        }
        else
        {
          userFI = "<EmptyUser>";
          userId = user.ID;
          userName = "<EmptyUser>";
        }

        doWrite = true;
        if ((args.Length > 1) && (!(userFI+" "+ userName).ToUpper().Contains(args[1].ToString().ToUpper())))
          doWrite = false;

        if (doWrite)
        {
          string userData = "user_id=" + userId + " userFI=" + userFI+" userName="+userName;
          System.Console.WriteLine(userData);
        }
      }
      System.Console.WriteLine("\r\nPress any key ...");
      System.Console.ReadKey();
    }


  }
}
