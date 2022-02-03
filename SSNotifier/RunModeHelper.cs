using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSNotifier
{

  /// <summary>
  /// Режимы работы программы
  /// </summary>
  public enum RunMode
  {
    help, // показ хэлпа
    listchat, // показ списка чатов пользователя
    listсontact, // показ списка контактов пользователя
    monitor // мониторинг и переадресация сообщений
  }


  class RunModeHelper
  {
    public static RunMode DetectMode(string[] args)
    {
      RunMode result = RunMode.help;

      try
      {
        if (args.Length > 0)
        {
          if (args[0].ToUpper() == RunMode.listchat.ToString().ToUpper())
          {
            result = RunMode.listchat;
          }
          if (args[0].ToUpper() == RunMode.listсontact.ToString().ToUpper())
          {
            result = RunMode.listсontact;
          }
          if (args[0].ToUpper() == RunMode.monitor.ToString().ToUpper())
          {
            result = RunMode.monitor;
          }

        }

      }
      catch(Exception ex)
      {
        System.Console.WriteLine(ex.Message);
      }

      return result;
    }

    public static void ShowHelp()
    {
      var helpText =
        "Программа отслеживания телеграм сообщений \r\n" +
        "Мониторит сообщения важных пользователей и сбрасывает их в указанный чат \r\n" +
        "Для работы программы укажите параметры: \r\n" +
        " "+ RunMode.listchat.ToString() + " = показ списка всех чатов (название, идентификатор), в которые добавлен пользователь. Возможно указать второй параметр, который будет фильтровать список по наличию указанного текста (CI) \r\n" +
        " " + RunMode.listсontact.ToString() + " = показ списка всех контактов - пользователей (имя, название, идентификатор). Возможно указать второй параметр, который будет фильтровать список по наличию указанного текста (CI) \r\n" +
        " " + RunMode.monitor.ToString() + " = мониторинг в (указанных) чатах сообщений от (указанных) пользователей и перенаправление сообщений в указанный чат \r\n" +
        "\r\n" +
        "Для работы программы требуется настроить конфигурационный файл *.Config (как что заполнить описано в самом файле) \r\n";

      System.Console.WriteLine(helpText);
      System.Console.WriteLine("\r\nPress any key ...");
      System.Console.ReadKey();
    }
  }
}
