using SSNotifier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSNotifierUI
{
  public partial class SSNotifierUI : Form
  {
    private Task MonitorTask;


    public SSNotifierUI()
    {
      InitializeComponent();
    }

    private void SSNotifierUI_Resize(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
      {
        Hide();
        SSnotifyIcon.Visible = true;
      }
    }

    private void SSnotifyIcon_DoubleClick(object sender, EventArgs e)
    {
      Show();
      this.WindowState = FormWindowState.Normal;
      SSnotifyIcon.Visible = false;
    }

    private void SSNotifierUI_Load(object sender, EventArgs e)
    {
      Console.SetOut(new TextBoxWriter(textBox1, tsslStatusMessage));
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      textBox1.SelectionStart = textBox1.Text.Length;
      textBox1.ScrollToCaret();
    }

    private void tsbRun_Help_Click(object sender, EventArgs e)
    {
      Task task = new Task(() =>
      {
        string[] helpLines = RunModeHelper.GetHelp();
        foreach(string helpLine in helpLines)
        {
          System.Console.WriteLine(helpLine);
        }
      }
      );
      task.Start();
    }

    private void tsbRunMonitor_Click(object sender, EventArgs e)
    {
      if (SSNotifier.ValidationCodeProvider.Provider == null)
        SSNotifier.ValidationCodeProvider.Provider = new FormValidationCode();

      if (tsbRunMonitor.CheckState == CheckState.Unchecked) {
        System.Console.WriteLine("Starting monitor task...");
        MonitorTask = new Task(() =>
          {
          while (!MonitorHelper.StopMonitor) // при обрывах связи функция мониторинга может завершиться, кружимся в цикле и перезапускаем её
          {
            MonitorHelper.Monitor();
            if (!MonitorHelper.StopMonitor)
              System.Threading.Thread.Sleep(2000); // если задача прервана не пользователем, ждём N сек, чтобы клиент переконнектился
            }
          }
        );
        MonitorTask.Start();
        System.Console.WriteLine("Monitor task Started");
        tsbRunMonitor.CheckState = CheckState.Checked;
      }
      else
      {
        Task task = new Task(() =>
          {
            System.Console.WriteLine("Stopping monitor task...");
            MonitorHelper.StopMonitor = true;

            while (this.MonitorTask.Status == TaskStatus.Running)
              System.Threading.Thread.Sleep(1000);
            System.Console.WriteLine("Monitor task Stopped");
            this.Invoke(new Action(() =>  tsbRunMonitor.CheckState = CheckState.Unchecked));
          }
        );
        task.Start();
      }
    }

    private void tsbRunMonitor_CheckStateChanged(object sender, EventArgs e)
    {
      if (tsbRunMonitor.Checked)
        tsbRunMonitor.Text = "Stop Monitoring";
      else
        tsbRunMonitor.Text = "Run Monitoring";
    }

    private void tsbGetChatList_Click(object sender, EventArgs e)
    {
      Task ChatListTask = new Task(() => {
        foreach (string chatInfo in ChatListHelper.GetChatList())
        {
          System.Console.WriteLine(chatInfo);
        }
      }
      );
      ChatListTask.Start();

    }

    private void tsbGetUserList_Click(object sender, EventArgs e)
    {
      Task ChatListTask = new Task(() => {
        foreach (string userInfo in UserListHelper.GetContactList(tstbGetContactList.Text))
        {
          System.Console.WriteLine(userInfo);
        }
      }
      );
      ChatListTask.Start();
    }

    private void tstbGetContactList_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.Handled = true;
        tsbGetUserList_Click(null, null);
      }
    }
  }
}
