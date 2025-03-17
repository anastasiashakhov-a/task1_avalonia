using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WorkListAvalonia.Models
{
    public enum LogType
    {
        Error,
        Warning,
        Information
    }

    public struct LogMessage
    {
        public LogType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
    }

    public class LogManager
    {
        private List<LogMessage> _logs = new List<LogMessage>();

        public int Count => _logs.Count;

        public LogMessage this[int index]
        {
            get => _logs[index];
            set => _logs[index] = value;
        }

        public void AddLog(LogType type, string message)
        {
            _logs.Add(new LogMessage
            {
                Type = type,
                DateTime = DateTime.Now,
                Message = message
            });
        }

        public IEnumerable<LogMessage> GetLogsByType(LogType type)
        {
            return _logs.Where(log => log.Type == type);
        }

        public IEnumerable<LogMessage> GetLogsByDateRange(DateTime start, DateTime end)
        {
            return _logs.Where(log => log.DateTime >= start && log.DateTime <= end);
        }

        public void SaveLogsToFile(string filePath)
        {
            string logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string fullPath = Path.Combine(logsDirectory, filePath);
            Console.WriteLine($"Logs will be saved to: {fullPath}");

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (var log in _logs)
                {
                    writer.WriteLine($"{log.DateTime}: [{log.Type}] {log.Message}");
                }
            }
        }
    }
}