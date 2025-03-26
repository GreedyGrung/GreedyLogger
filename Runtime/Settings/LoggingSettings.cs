using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GreedyLogger.Settings
{
    [CreateAssetMenu(fileName = "GreedyLoggerSettings", menuName = "Greedy Logger Settings")]
    public class LoggingSettings : ScriptableObject
    {
        [SerializeField] private bool _loggingEnabled;
        [SerializeField] private bool _writeLogsToFiles;
        [SerializeField] private int _maxFilesCount;
        [SerializeField] private string _logFileDirectory;

        [Tooltip("Do not forget to press 'Generate' button below to apply changes!")]
        [SerializeField] private List<string> _contexts;

        [SerializeField] private LogContext _contextsFilter;

        [Tooltip("Do not forget to press 'Generate' button below to apply changes!")]
        [SerializeField] private List<LoggingLevelSettings> _logLevels = GetDefaults();

        [Tooltip("Do not forget to press 'Generate' button below to apply changes!")]
        [SerializeField] private bool _logExceptions;

        public bool LoggingEnabled => _loggingEnabled;
        public bool WriteLogsToFiles => _writeLogsToFiles;
        public int MaxFilesCount => _maxFilesCount;
        public string LogFileDirectory => _logFileDirectory;
        public IReadOnlyList<string> Contexts => _contexts;
        public LogContext ContextsFilter => _contextsFilter;
        public IReadOnlyList<LoggingLevelSettings> LogLevels => _logLevels;
        public bool LogExceptions => _logExceptions;

        public void RestoreToDefaults()
        {
            _loggingEnabled = true;
            _writeLogsToFiles = true;
            _maxFilesCount = 10;

            _contexts = new()
            {
                "Gameplay",
                "UI",
                "Meta",
                "Infrastructure"
            };

            _contextsFilter = GetAllFlags<LogContext>();
            _logLevels = GetDefaults();
            _logExceptions = true;
        }

        public bool CanBeGenerated() 
            => _logLevels.Any(item => item.Name == "Default") 
            && !_logLevels.Any(item => item.Name == "Exception");

        private void OnValidate()
        {
            if (_logLevels.Count == 0)
            {
                Debug.LogWarning("You must have at least 1 logging level!");

                _logLevels.Add(new() { Name = "Default", Color = Color.white, Emphasis = LogEmphasis.None, Type = LogType.Log });
            }
        }

        private static List<LoggingLevelSettings> GetDefaults()
        {
            return new()
            {
                new() { Name = "Default", Color = Color.white, Emphasis = LogEmphasis.None, Type = LogType.Log },
                new() { Name = "Warning", Color = Color.yellow, Emphasis = LogEmphasis.None, Type = LogType.Warning },
                new() { Name = "Error", Color = Color.red, Emphasis = LogEmphasis.None, Type = LogType.Error }
            };
        }

        private TEnum GetAllFlags<TEnum>() where TEnum : Enum
        {
            TEnum result = (TEnum)Enum.Parse(typeof(TEnum), "0");

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                result = (TEnum)(object)(Convert.ToInt32(result) | Convert.ToInt32(value));
            }

            return result;
        }
    }
}