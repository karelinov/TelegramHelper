using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSNotifierUI
{
  static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      //int workerThreads;
      //int comppoolThreads;

      //System.Threading.ThreadPool.GetMaxThreads(out workerThreads, out comppoolThreads);
      ////var tcpClient = new System.Net.Sockets.TcpClient();
      ////tcpClient.ConnectAsync("149.154.167.50", 443).GetAwaiter().GetResult();
      //SSNotifier.ValidationCodeProvider.Provider = new FormValidationCode();
      //var user = SSNotifier.TelegramClientHelper.UserClient;



      Application.SetHighDpiMode(HighDpiMode.SystemAware);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new SSNotifierUI());
    }
  }
}
