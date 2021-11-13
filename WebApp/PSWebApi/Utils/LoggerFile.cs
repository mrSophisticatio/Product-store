using PSWebApi.Models;
using System;
using System.IO;
using System.Text;

namespace PSWebApi.Utils
{
    static class LoggerFile
    {
        static string LogPath = FileSystemManager.GetLogPath();
        public static void Info(string text)
        {
            using (var sw = new StreamWriter(LogPath + $"log_{Constants.CURRENT_DATE}.log", true, Encoding.UTF8))
            {
                sw.WriteLine($"{DateTime.Now:s} INFO:\t" + text);
            }
        }

        public static void Warning(string text)
        {
            using (var sw = new StreamWriter(LogPath + $"log_{Constants.CURRENT_DATE}.log", true, Encoding.UTF8))
            {
                sw.WriteLine($"{DateTime.Now:s} WARNING:\t" + text);
            }
        }

        public static void Error(string text)
        {
            using (var sw = new StreamWriter(LogPath + $"log_{Constants.CURRENT_DATE}.log", true, Encoding.UTF8))
            {
                sw.WriteLine($"{DateTime.Now:s} ERROR:\t" + text);
            }
        }
    }
}