
namespace SSNotifierUI
{
  partial class SSNotifierUI
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSNotifierUI));
      this.SSnotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsbRun_Help = new System.Windows.Forms.ToolStripButton();
      this.tsbRunMonitor = new System.Windows.Forms.ToolStripButton();
      this.tsbGetChatList = new System.Windows.Forms.ToolStripButton();
      this.tsbGetContacdtList = new System.Windows.Forms.ToolStripButton();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.tstbGetContactList = new System.Windows.Forms.ToolStripTextBox();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // SSnotifyIcon
      // 
      this.SSnotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("SSnotifyIcon.Icon")));
      this.SSnotifyIcon.Text = "SSnotifyIcon";
      this.SSnotifyIcon.DoubleClick += new System.EventHandler(this.SSnotifyIcon_DoubleClick);
      // 
      // textBox1
      // 
      this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox1.Location = new System.Drawing.Point(0, 27);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBox1.Size = new System.Drawing.Size(1161, 457);
      this.textBox1.TabIndex = 0;
      this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      // 
      // toolStrip1
      // 
      this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRun_Help,
            this.tsbRunMonitor,
            this.tsbGetChatList,
            this.tsbGetContacdtList,
            this.toolStripLabel1,
            this.tstbGetContactList});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1161, 27);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tsbRun_Help
      // 
      this.tsbRun_Help.Image = ((System.Drawing.Image)(resources.GetObject("tsbRun_Help.Image")));
      this.tsbRun_Help.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbRun_Help.Name = "tsbRun_Help";
      this.tsbRun_Help.Size = new System.Drawing.Size(65, 24);
      this.tsbRun_Help.Text = "Help";
      this.tsbRun_Help.Click += new System.EventHandler(this.tsbRun_Help_Click);
      // 
      // tsbRunMonitor
      // 
      this.tsbRunMonitor.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunMonitor.Image")));
      this.tsbRunMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbRunMonitor.Name = "tsbRunMonitor";
      this.tsbRunMonitor.Size = new System.Drawing.Size(111, 24);
      this.tsbRunMonitor.Text = "RunMonitor";
      this.tsbRunMonitor.CheckStateChanged += new System.EventHandler(this.tsbRunMonitor_CheckStateChanged);
      this.tsbRunMonitor.Click += new System.EventHandler(this.tsbRunMonitor_Click);
      // 
      // tsbGetChatList
      // 
      this.tsbGetChatList.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetChatList.Image")));
      this.tsbGetChatList.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbGetChatList.Name = "tsbGetChatList";
      this.tsbGetChatList.Size = new System.Drawing.Size(108, 24);
      this.tsbGetChatList.Text = "GetChatList";
      this.tsbGetChatList.Click += new System.EventHandler(this.tsbGetChatList_Click);
      // 
      // tsbGetContacdtList
      // 
      this.tsbGetContacdtList.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetContacdtList.Image")));
      this.tsbGetContacdtList.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbGetContacdtList.Name = "tsbGetContacdtList";
      this.tsbGetContacdtList.Size = new System.Drawing.Size(129, 24);
      this.tsbGetContacdtList.Text = "GetContactList";
      this.tsbGetContacdtList.Click += new System.EventHandler(this.tsbGetUserList_Click);
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(40, 24);
      this.toolStripLabel1.Text = "filter";
      this.toolStripLabel1.ToolTipText = "будут отфильтрованы только контакты, содержащие указанный текст";
      // 
      // tstbGetContactList
      // 
      this.tstbGetContactList.Name = "tstbGetContactList";
      this.tstbGetContactList.Size = new System.Drawing.Size(100, 27);
      this.tstbGetContactList.ToolTipText = "будут отфильтрованы только контакты, содержащие указанный текст";
      this.tstbGetContactList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tstbGetContactList_KeyDown);
      // 
      // SSNotifierUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1161, 484);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.toolStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "SSNotifierUI";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SSNotifier";
      this.Load += new System.EventHandler(this.SSNotifierUI_Load);
      this.Resize += new System.EventHandler(this.SSNotifierUI_Resize);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NotifyIcon SSnotifyIcon;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsbRun_Help;
    private System.Windows.Forms.ToolStripButton tsbRunMonitor;
    private System.Windows.Forms.ToolStripButton tsbGetChatList;
    private System.Windows.Forms.ToolStripButton tsbGetContacdtList;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripTextBox tstbGetContactList;
  }
}

