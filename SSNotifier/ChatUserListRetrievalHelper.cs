using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;

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
      TLChats chats = TLMethodWrapperHelper.GetAllChats();

      System.Console.WriteLine("Found "+ chats.Chats.Count.ToString() + (args.Length>1?" (filtered) ":"") + " chats: ");

      bool doWrite;
      string chatTite;
      int chatId;
      foreach (TLAbsChat chat in chats.Chats)
      {
        
        if (chat.GetType() == typeof(TLChat))
        {
          chatTite = ((TLChat)chat).Title;
          chatId = ((TLChat)chat).Id;
        }
        else if (chat.GetType() == typeof(TLChannel))
        {
          chatTite = ((TLChannel)chat).Title;
          chatId = ((TLChannel)chat).Id;
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
      TLContacts contacts = (TLContacts)TLMethodWrapperHelper.GetAllContacts();

      System.Console.WriteLine("Found " + contacts.Contacts.Count.ToString() + (args.Length > 1 ? " (filtered) " : "") + " contacts: ");

      int[] userIds = contacts.Contacts.Select(c => c.UserId).ToArray<int>();
      TLAbsUser[] users = TLMethodWrapperHelper.GetUsers(userIds);

      bool doWrite;
      string userName, userFI;
      int userId;
      foreach (TLUser user in users)
      {
        userFI = user.FirstName + ' ' + user.LastName;
        userId = user.Id;
        userName = user.Username;

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
