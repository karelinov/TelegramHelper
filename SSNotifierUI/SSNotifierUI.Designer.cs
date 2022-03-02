
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
      this.SuspendLayout();
      // 
      // SSnotifyIcon
      // 
      this.SSnotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("SSnotifyIcon.Icon")));
      this.SSnotifyIcon.Text = "SSnotifyIcon";
      this.SSnotifyIcon.DoubleClick += new System.EventHandler(this.SSnotifyIcon_DoubleClick);
      // 
      // SSNotifierUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SSNotifierUI";
      this.Text = "SSNotifier";
      this.Resize += new System.EventHandler(this.SSNotifierUI_Resize);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.NotifyIcon SSnotifyIcon;
  }
}

