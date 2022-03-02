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
  }
}
