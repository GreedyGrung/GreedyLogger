using UnityEngine;
using GreedyLogger.Settings;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace GreedyLogger
{
    internal static class FileLogger
    {
        private static StreamWriter _writer;
        private static string _logFilePath;
        private static LoggingSettings _settings;

        internal static void Initialize(LoggingSettings settings)
        {
            _settings = settings;

            if (!_settings.WriteLogsToFiles)
            {
                return;
            }

            string directory = string.IsNullOrEmpty(_settings.LogFileDirectory)
                ? Application.persistentDataPath
                : _settings.LogFileDirectory;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            int maxFiles = _settings.MaxFilesCount;
            var logFiles = Directory.GetFiles(directory, "*_player_log.txt", SearchOption.TopDirectoryOnly);

            if (logFiles.Length >= maxFiles)
            {
                var oldestFile = logFiles.OrderBy(f => File.GetCreationTime(f)).First();

                try
                {
                    File.Delete(oldestFile);
                }
                catch (Exception ex)
                {
                    GLogger.TryLog("Failed to delete oldest log file: " + ex.Message);
                }
            }

            string fileName = DateTime.Now.ToString("HH-mm-ss_dd-MM-yyyy") + "_player_log.txt";
            _logFilePath = Path.Combine(directory, fileName);

            try
            {
                _writer = new StreamWriter(_logFilePath, false) { AutoFlush = true };
                Application.logMessageReceived += HandleLog;
                GLogger.TryLog("FileLogger initialized. Logging to: " + _logFilePath);
            }
            catch (Exception ex)
            {
                GLogger.TryLog("Failed to initialize FileLogger: " + ex.Message);
            }
        }

        internal static void Shutdown()
        {
            if (!_settings.WriteLogsToFiles)
            {
                return;
            }

            Application.logMessageReceived -= HandleLog;

            if (_writer != null)
            {
                _writer.Close();
                _writer = null;
            }
        }

        private static void HandleLog(string logString, string stackTrace, UnityEngine.LogType type)
        {
            if (_writer == null)
            {
                return;
            }

            string plainLogString = Regex.Replace(logString, "<.*?>", string.Empty);
            string plainStackTrace = Regex.Replace(stackTrace, "<.*?>", string.Empty);

            string timeStamp = DateTime.Now.ToString("HH:mm:ss");
            string logEntry = $"[{timeStamp}] {plainLogString}";

            if (!string.IsNullOrEmpty(plainStackTrace))
            {
                logEntry += "\n" + plainStackTrace;
            }

            _writer.WriteLine(logEntry);
        }
    }
}
