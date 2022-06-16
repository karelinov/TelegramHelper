using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TL;
using System.Configuration;

namespace SSNotifier
{
  public class UserListHelper
  {
    /// <summary>
    /// Список контактов пользователя
    /// </summary>
    /// 
    private static Dictionary<long, Contact> _contacts = null;
    public static Dictionary<long, Contact> Contacts
    {
      get
      {
        if (_contacts == null)
        {
          _contacts = TLCH.UserClient.Contacts_GetContacts().Result.contacts.ToDictionary(c => c.user_id); 
        }

        return _contacts;
      }
      set
      {
        _contacts = value;
      }
    }

    /// <summary>
    /// Список пользователей, первоначально из контактов пользователя
    /// </summary>
    /// 
    private static Dictionary<long, User> _users = null;// new Dictionary<long, User>();

    public static Dictionary<long, User> Users
    {
      get
      { 
        if (_users == null)
        {
          _users = new Dictionary<long, User>();

          Messages_Dialogs dialogs = TLCH.UserClient.Messages_GetAllDialogs().Result;
          foreach(User user in dialogs.users.Values.ToArray())
          {
            _users.Add(user.ID, user);
          }
        }

        return _users;
      }
      set
      {
        _users = value;
      }
    }


    public static User GetUser(long userId)
    {
      User result = null;

      if (Users.ContainsKey(userId));
        result = Users[userId];

      return result;
    }

    /// <summary>
    /// Функция возвращает список контактов пользователя и выбрасывает его на экран
    /// Саписок фильтруется по значению параметра (если есть)
    /// </summary>
    public static string[] GetContactList(string filter)
    {
      string[] result = new string[] { };


      //System.Console.WriteLine("Found " + Contacts.Count.ToString() + (args.Length > 1 ? " (filtered) " : "") + " contacts: ");
      //UserBase[] users =TLMH.GetUsers(Contacts.Keys.ToArray<long>());

      Contacts_Contacts contacts = TLCH.UserClient.Contacts_GetContacts().Result;

      string userName, userFI;
      long userId;

      for (int i = 0; i < contacts.contacts.Length; i++)
      {
        Contact contact = contacts.contacts[i];
        UserBase user = contacts.users.Values.ToArray()[i];

        if (user.GetType() == typeof(User))
        {
          userFI = (((User)user).first_name + ' ' + ((User)user).last_name); userFI = (userFI == null? "": userFI);
          userId = user.ID;
          userName = ((User)user).username; userName = (userName == null? "": userName);
        }
        else
        {
          userFI = "<EmptyUser>";
          userId = user.ID;
          userName = "<EmptyUser>";
        }

        string suserInfo = "user_id=" + userId + " userFI=" + userFI + " userName=" + userName;
        if (filter != "")
        {
          if (userFI.Contains(filter, StringComparison.OrdinalIgnoreCase) || userName.Contains(filter, StringComparison.OrdinalIgnoreCase))
            result = result.Append(suserInfo).ToArray();
        }
        else
          result = result.Append(suserInfo).ToArray();

      }

      return result;
    }
  }
}
