using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Configuration;
using TLSharp.Core;
using TeleSharp.TL;


namespace SSNotifier
{
  class TelegramClientHelper
  {
    /// <summary>
    /// Клиент. При обращении производится создание, подключение, аутентификация объекта клиента
    /// </summary>
    private static TelegramClient _telegramClient = null;
    public static TelegramClient Client
    {
      get {
        if (_telegramClient == null)
        {
          _telegramClient = new TelegramClient(
            int.Parse(ConfigurationManager.AppSettings["apikey"]),
            ConfigurationManager.AppSettings["apihash"]
          );
          _telegramClient.ConnectAsync().GetAwaiter().GetResult();
        }

        if (!Authenticate(_telegramClient))
          throw new Exception("Error Authentication");

        return _telegramClient;
      }
    }



    /// <summary>
    /// Функция аутентификации клиента, при необходимости производит запрос кода
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    private static bool Authenticate(TelegramClient client)
    {
      bool result  = false;

      try
      {

        if (client.Session.TLUser == null)
        {
          Tuple<string, string> hashAndCode = GetTelegramCode(client);
          if (hashAndCode != null)
          {
            TLUser user = client.MakeAuthAsync(ConfigurationManager.AppSettings["phone"], hashAndCode.Item1, hashAndCode.Item2).GetAwaiter().GetResult();
            result = true;
            System.Console.Out.WriteLine("Successuffly connected, user.FirstName=" + user.FirstName);
          }
          else
          {
            System.Console.Out.WriteLine("Cannot retrieve code");
          }

        }
        else
        {
          result = true;
          System.Console.Out.WriteLine("Already connected, user.FirstName=" + client.Session.TLUser.FirstName);
        }


      }
      catch (Exception ex)
      {
        System.Console.WriteLine(ex.StackTrace);
      }


      return result;
    }

    private static Tuple<string, string> GetTelegramCode(TelegramClient client) 
    {
      string sentCodeHash = client.SendCodeRequestAsync(ConfigurationManager.AppSettings["phone"]).GetAwaiter().GetResult() ;

      System.Console.WriteLine("Для входа в приложение требуется код подтверждения:");
      string code = System.Console.ReadLine();
        //Microsoft.VisualBasic.Interaction.InputBox("Введите код", "для входа в приложение требуется код подтверждения");

      if (code != "")
        return new Tuple<string, string>(sentCodeHash, code);
      else
        return null;
    }


  }
}
