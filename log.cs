using System;
using System.IO;

public static class Log
{
    private static readonly string logFilePath = "log.txt"; // путь к файлу логов
    public static void LogException(Exception ex)
    {
        try
        {
            // формируется строка лога
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                              $"Исключение: {ex.GetType().Name}\n" +
                              $"Сообщение: {ex.Message}\n" +
                              $"Стек вызовов: {ex.StackTrace}\n";

            // запись лог в файл
            File.AppendAllText(logFilePath, logEntry);
        }
        catch (Exception logEx)
        {
            
            Console.WriteLine("Ошибка записи лога: " + logEx.Message);
        }
    }

    
    /// <param name="message">сообщение для записи.</param>
    public static void LogMessage(string message)
    {
        try
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Сообщение: {message}\n";
            File.AppendAllText(logFilePath, logEntry);
        }
        catch (Exception logEx)
        {
            Console.WriteLine("Ошибка записи лога: " + logEx.Message);
        }
    }
}
