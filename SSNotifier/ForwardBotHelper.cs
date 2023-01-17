using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using TL;
using System.IO;
using TL.Methods;
using System.Threading.Tasks;
using System.Net.Http;

namespace SSNotifier
{
  class ForwardBotHelper
  {
    private static Dictionary<long, InputPeer> resolvedPeers = new Dictionary<long, InputPeer>(); // закэшированные вычисленные пиры для чатов 


    /// <summary>
    /// Нотифицирует о перенаправленном сообщении, посылая "!" в чат, куда перенаправляются сообщения
    /// Нужно для того, чтобы было показано , что в чате новые сообщения, т.к. при перенаправлении они отмечаются как прочитанные
    /// </summary>
    /// <param name="MessageToForward"></param>
    public static void NotifyForwardMessage(Message MessageToForward)
    {
      string chatToForwardId = ConfigurationManager.AppSettings["botChatToForward"];

      HttpClient httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri("https://api.telegram.org");
      var content = new FormUrlEncodedContent(new[] 
        {
          new KeyValuePair<string, string>("chat_id", chatToForwardId), 
          new KeyValuePair<string, string>("text", "!") 
        });
      HttpResponseMessage response = httpClient.PostAsync("/bot" + ConfigurationManager.AppSettings["bot_token"]+"/sendMessage",content).Result;




    }

  }
}
