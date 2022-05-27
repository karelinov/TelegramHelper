using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SSNotifier;

namespace SSNotifierUI
{
  public partial class FormValidationCode : Form, IValidationCodeProvider
  {
    public FormValidationCode()
    {
      InitializeComponent();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }

    public string GetValidationCode()
    {
      DialogResult res = this.ShowDialog();
      if (res == DialogResult.OK)
        return this.tbValidationCode.Text;
      else 
        return null;

    }


  }
}
