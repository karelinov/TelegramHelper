using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TLSharp.Core;



namespace SSNotifier
{
  class Program
  {
    static void Main(string[] args)
    {
      RunMode runMode = RunModeHelper.DetectMode(args);

      if (runMode == RunMode.help)
      {
        RunModeHelper.ShowHelp();
        return;
      }

      switch (runMode)
      {
        case RunMode.listchat:
          ChatUserListRetrievalHelper.GetChatList(args);
          break;
        case RunMode.listсontact:
          ChatUserListRetrievalHelper.GetContactList(args);
          break;
        case RunMode.monitor:
          MonitorHelper.Monitor();
          break;
      }


    }
  }
}
