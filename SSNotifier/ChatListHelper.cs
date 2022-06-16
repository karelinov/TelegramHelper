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
  public class ChatListHelper
  {
    // Кэшированный Список чатов/каналов (из GetAllChats) для получения InputPeer и определения типов чатов (чат/канал)
    // Вспомогательная обёртка для перезагрузки чата и раиза ошибки при отсутствии чата с указанным id
    private static Dictionary<long, ChatBase> _cachedChatsInfo = null;
    public static Dictionary<long, ChatBase> CachedChatsInfo
    {
      get
      {
        if (_cachedChatsInfo == null)
        {
          Messages_Chats messages_Chats = TLCH.UserClient.Messages_GetAllChats().Result;
          _cachedChatsInfo = messages_Chats.chats;
        }
        return _cachedChatsInfo; 
      }
      set
      {
        _cachedChatsInfo = value;
      }
    }
    public static ChatBase GetCachedChat(long chatId)
    {
      if (!CachedChatsInfo.ContainsKey(chatId))  // может что изменилось перегружаем список чатов
      {
        CachedChatsInfo = null;

        if (!CachedChatsInfo.ContainsKey(chatId))
          throw new Exception("Chat " + chatId.ToString() + " not found among user available chats");

      }
      return CachedChatsInfo[chatId];
    }

    /// <summary>
    /// Функция возвращает список чатов пользователя и выбрасывает его на экран
    /// Саписок фильтруется по значению второго параметра программы (если есть)
    /// </summary>
    /// <param name="args"></param>
    public static string[] GetChatList()
    {
      string[] result = new string[] { };


      Messages_Chats chats = TLCH.UserClient.Messages_GetAllChats(new long[0]).Result;

      //System.Console.WriteLine("Found "+ chats.chats.Count.ToString() + (args.Length>1?" (filtered) ":"") + " chats: ");

      //bool doWrite;
      string chatTite;
      long chatId;
      foreach (ChatBase chat in chats.chats.Values)
      {
        
        if (chat is Chat)
        {
          chatTite = (chat as Chat).Title;
          chatId = (chat as Chat).ID;
        }
        else if (chat is Channel)
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

        /*
        doWrite = true;
        if ((args.Length > 1) && (!chatTite.ToUpper().Contains(args[1].ToString().ToUpper())))
          doWrite = false;

        if (doWrite)
        {
          string chatdata = "chat_id=" + chatId + " title=" + chatTite;
          System.Console.WriteLine(chatdata);
        }
        */
        result = result.Append(chatId + " ," + chatTite).ToArray();

      }


      return result;
      //System.Console.WriteLine("\r\nPress any key ...");
      //System.Console.ReadKey();
    }

  
  }
}
