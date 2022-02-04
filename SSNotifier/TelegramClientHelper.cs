using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Configuration;
using WTelegram;


namespace SSNotifier
{
  class TelegramClientHelper
  {
    /// <summary>
    /// Клиент. При обращении производится создание, подключение, аутентификация объекта клиента
    /// </summary>
    private static WTelegram.Client _telegramClient = null;
    public static WTelegram.Client Client
    {
      get {
        if (_telegramClient == null)
        {
          _telegramClient = new Client(Config);


          if (!bool.Parse(ConfigurationManager.AppSettings["enableConsoleLog"]))
          {
            //WTelegram.Helpers.Log += (lvl, str) => System.Diagnostics.Debug.WriteLine(str); // перенаправление логов в VS
            WTelegram.Helpers.Log = (lvl, str) => { }; 
          }
        }

        _telegramClient.LoginUserIfNeeded();


        return _telegramClient;
      }
    }


    private static string Config(string confiValue)
    {
      System.Console.WriteLine("Requested config value " + confiValue);
      switch (confiValue)
      {
        case "phone_number": return ConfigurationManager.AppSettings["phone"];
        case "api_id": return ConfigurationManager.AppSettings["apikey"];
        case "api_hash": return ConfigurationManager.AppSettings["apihash"];
        case "server_address": return ConfigurationManager.AppSettings["server"]+":"+ConfigurationManager.AppSettings["serverport"];
        case "verification_code": Console.Write("Для входа в приложение требуется код подтверждения:: "); return Console.ReadLine();

        default: return null;                  // let WTelegramClient decide the default config
      }
    }


  }
}
