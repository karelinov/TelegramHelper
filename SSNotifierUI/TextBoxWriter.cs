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
    private readonly ToolStripStatusLabel _toolStripStatusLabel;
    public TextBoxWriter(TextBox textBox, ToolStripStatusLabel toolStripStatusLabel)
    {
      _textBox = textBox;
      _toolStripStatusLabel = toolStripStatusLabel;
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
        if (text.StartsWith("status:",StringComparison.OrdinalIgnoreCase)) // если вначале "status" пишем в статус
        {
          _toolStripStatusLabel.Text = DateTime.Now.ToString("yyyyMMdd hh:mm:ss ") + text;
        }
        else // иначе пишем в textbox
        {
          if (_textBox.Text.Length > 1000000)
          {
            _textBox.Text = _textBox.Text.Substring(50000);
          }

          if (_textBox.Text.Length == 0 || (_textBox.Text.Length > 0 && _textBox.Text[_textBox.Text.Length - 1] == '\n'))
            _textBox.Text += DateTime.Now.ToString("yyyyMMdd hh:mm:ss ");

          _textBox.Text += text;
        }


      }
    }
  }
}
