using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TL;
using System.Configuration;

namespace SSNotifier
{
  class UserListHelper
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
          _contacts = TLMH.GetAllContacts().contacts.ToDictionary(c => c.user_id);  //Select(c => new KeyValuePair<long, Contact>(c.user_id, c)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
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
    private static Dictionary<long, User> _users = null;
    public static Dictionary<long, User> Users
    {
      get
      {
        if (_users == null)
        {
          _users = TLMH.GetUsers(Contacts.Keys.ToArray()).Where(user => user is User).ToDictionary(user => user.ID, user => user as User);
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

      if (!Users.ContainsKey(userId)) // если контакта с таким user_id нет в списке, получаем отдельным вызовом
      {
        var users = TLMH.GetUsers(new long[] { userId });
        if (users.Length > 0)
        {
          if (users[0] is User)
          {
            Users.Add(userId, users[0] as User);
          }
          else
            throw new Exception("полученный объект " + users[0].GetType().Name + "не является объектом User");
        }
        else if (userId == long.Parse(ConfigurationManager.AppSettings["userToForward"]))
        {
          Contacts_ResolvedPeer contacts_ResolvedPeer = TLCH.UserClient.Contacts_ResolveUsername(ConfigurationManager.AppSettings["userNameToForward"]).Result;
          Users.Add(userId, contacts_ResolvedPeer.User);
        }


      }

      result = Users[userId];

      return result;
    }



    /// <summary>
    /// Функция получает список контактов пользователя и выбрасывает его на экран
    /// Саписок фильтруется по значению второго параметра программы (если есть)
    /// </summary>
    /// <param name="args"></param>
    public static void WriteContactList(string[] args)
    {
      System.Console.WriteLine("Found " + Contacts.Count.ToString() + (args.Length > 1 ? " (filtered) " : "") + " contacts: ");

      UserBase[] users = TLMH.GetUsers(Contacts.Keys.ToArray<long>());

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
        if ((args.Length > 1) && (!(userFI + " " + userName).ToUpper().Contains(args[1].ToString().ToUpper())))
          doWrite = false;

        if (doWrite)
        {
          string userData = "user_id=" + userId + " userFI=" + userFI + " userName=" + userName;
          System.Console.WriteLine(userData);
        }
      }
      System.Console.WriteLine("\r\nPress any key ...");
      System.Console.ReadKey();
    }
  }
}
