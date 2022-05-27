using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSNotifier
{
  /*
  /// <summary>
  /// Режимы работы программы
  /// </summary>
  public enum RunMode
  {
    help, // показ хэлпа
    listchat, // показ списка чатов пользователя
    listcontact, // показ списка контактов пользователя
    monitor, // мониторинг и переадресация сообщений
    notifybot // режим бота, возвращаеюего все присылаемые ему сообщения
  }
  */

  public class RunModeHelper
  {
    /*
    public static RunMode DetectMode(string[] args)
    {
      RunMode result = RunMode.help;

      try
      {
        if (args.Length > 0)
        {
          Enum.TryParse<RunMode>(args[0], out result);
        }
      }
      catch(Exception ex)
      {
        System.Console.WriteLine(ex.Message);
      }

      return result;
    }
    */

    public static string[] GetHelp()
    {
      string[] result;

      string fname = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? AppDomain.CurrentDomain.BaseDirectory;
      fname = Path.Combine(fname, "Readme.txt");
      if (!File.Exists(fname))
        result = new string[] { "Файл " + fname + " не найден" };
      else
        result = File.ReadAllLines(fname);

      return result;
    }
  }
}
