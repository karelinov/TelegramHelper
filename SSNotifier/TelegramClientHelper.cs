using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Configuration;
using WTelegram;
using System.Security.Cryptography;

namespace SSNotifier
{
  class TLCH : TelegramClientHelper { }; // просто как короткий алиас основного класса

  public class TelegramClientHelper
  {
    /// <summary>
    /// Клиент. При обращении производится создание, подключение, аутентификация объекта клиента
    /// </summary>
    private static WTelegram.Client _telegramClient = null;
    private static WTelegram.Client _telegramBotClient = null;
    public static TL.User CurrentUser = null;
    public static TL.User CurrentBot = null;

    public static WTelegram.Client UserClient
    {
      get
      {
        if (_telegramClient == null)
        {
          _telegramClient = new Client(Config);


          if (!bool.Parse(ConfigurationManager.AppSettings["enableConsoleLog"]))
          {
            //WTelegram.Helpers.Log += (lvl, str) => System.Diagnostics.Debug.WriteLine(str); // перенаправление логов в VS
            WTelegram.Helpers.Log = (lvl, str) => { };
          }

          CurrentUser = _telegramClient.LoginUserIfNeeded().GetAwaiter().GetResult();
        }


        return _telegramClient;
      }
    }

    public static WTelegram.Client BotClient
    {
      get
      {
        if (_telegramBotClient == null)
        {
          _telegramBotClient = new Client(BotConfig);

          CurrentBot = _telegramBotClient.LoginBotIfNeeded().GetAwaiter().GetResult();
        }


        return _telegramBotClient;
      }
    }



    private static string Config(string confiValue)
    {
      System.Console.WriteLine("Requested config value " + confiValue);
      switch (confiValue)
      {
        case "session_pathname":
          return Path.Combine(
            Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar)))
            ?? AppDomain.CurrentDomain.BaseDirectory,
            "WTelegram.session." /*+ ConfigurationManager.AppSettings["apikey"]+"."*/+ ConfigurationManager.AppSettings["server"]);
        case "phone_number": return ConfigurationManager.AppSettings["phone"];
        //case "user_id": return "1145017817";
        case "api_id": return ConfigurationManager.AppSettings["apikey"];
        case "api_hash": return ConfigurationManager.AppSettings["apihash"];
        case "server_address": return ConfigurationManager.AppSettings["server"] + ":" + ConfigurationManager.AppSettings["serverport"];
        case "verification_code": return ValidationCodeProvider.Provider.GetValidationCode();  //Console.Write("Для входа в приложение требуется код подтверждения:: "); return Console.ReadLine();
        case "first_name": return "Oleg";      // if sign-up is required
        case "last_name": return "Oleg";        // if sign-up is required

        default: return null;                  // let WTelegramClient decide the default config
      }
    }

    private static string BotConfig(string confiValue)
    {
      System.Console.WriteLine("Requested Bot config value " + confiValue);
      switch (confiValue)
      {
        case "session_pathname":
          return Path.Combine(
            Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar)))
            ?? AppDomain.CurrentDomain.BaseDirectory,
            "WTelegram.session.bot" /*+ ConfigurationManager.AppSettings["apikey"]+"."*/+ ConfigurationManager.AppSettings["server"]);
        case "bot_token": return ConfigurationManager.AppSettings["bot_token"];


        default: return Config(confiValue);
      }
    }

  }
}
