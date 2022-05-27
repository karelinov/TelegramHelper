
namespace SSNotifierUI
{
  partial class FormValidationCode
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.btnOk = new System.Windows.Forms.Button();
      this.tbValidationCode = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btnOk
      // 
      this.btnOk.Location = new System.Drawing.Point(468, 53);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(142, 29);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // tbValidationCode
      // 
      this.tbValidationCode.Location = new System.Drawing.Point(420, 9);
      this.tbValidationCode.Name = "tbValidationCode";
      this.tbValidationCode.Size = new System.Drawing.Size(190, 27);
      this.tbValidationCode.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(401, 20);
      this.label1.TabIndex = 2;
      this.label1.Text = "Для входа в приложение требуется код подтверждения:";
      // 
      // FormValidationCode
      // 
      this.AcceptButton = this.btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(625, 108);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tbValidationCode);
      this.Controls.Add(this.btnOk);
      this.Name = "FormValidationCode";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "FormValidationCode";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Label label1;
    public System.Windows.Forms.TextBox tbValidationCode;
  }
}