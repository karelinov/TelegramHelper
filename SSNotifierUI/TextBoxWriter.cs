using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SSNotifierUI
{
  public delegate void StringArgReturningVoidDelegate(string text);

  /// <summary>
  /// Thread-safe Класс для записи текста на форму (в TextBox, передаваемый в класс)
  /// Используется функциями, исполняющимися в отдельных потоках, для формирования сообщений в UI
  /// </summary>
  public class TextBoxWriter : TextWriter
  {
    private readonly TextBox _textBox;
    public TextBoxWriter(TextBox textBox)
    {
      _textBox = textBox;
    }

    public override void Write(char value)
    {
      SetText(value.ToString());
    }

    public override void Write(string value)
    {
      SetText(value);
    }

    public override void WriteLine(char value)
    {
      SetText(value + Environment.NewLine);
    }

    public override void WriteLine(string value)
    {
      SetText(value + Environment.NewLine);
    }

    public override Encoding Encoding => Encoding.ASCII;

    //Write to your UI object in thread safe way:
    private void SetText(string text)
    {
      // InvokeRequired required compares the thread ID of the  
      // calling thread to the thread ID of the creating thread.  
      // If these threads are different, it returns true.  
      if (_textBox.InvokeRequired)
      {
        var d = new StringArgReturningVoidDelegate(SetText);
        _textBox.Invoke(d, text);
      }
      else
      {
        _textBox.Text += text;
      }
    }
  }
}
