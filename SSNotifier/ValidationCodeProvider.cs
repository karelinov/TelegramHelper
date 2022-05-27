using System;
using System.Collections.Generic;
using System.Text;

namespace SSNotifier
{

  /// <summary>
  /// Интерфейс для получения кода валидации. Реализуется компонентами UI, которые записывают 
  /// в статическую переменную ниже созданный класс для реализации интерфейса
  /// (реализация заключается в показе формы с запросом и возврата что там пользователь на форме навводил)
  /// </summary>
  public interface IValidationCodeProvider
  {
    string GetValidationCode();
  }

  public class ValidationCodeProvider 
  {
    public static IValidationCodeProvider Provider = null;
  }
}
