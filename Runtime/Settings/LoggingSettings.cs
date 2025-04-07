using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GreedyLogger.Settings
{
    [CreateAssetMenu(fileName = "GreedyLoggerSettings", menuName = "Greedy Logger Settings")]
    public class LoggingSettings : ScriptableObject
    {
        private static readonly Color DefaultLogColor = new(0.778f, 0.778f, 0.778f);

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

        internal bool LoggingEnabled => _loggingEnabled;
        internal bool WriteLogsToFiles => _writeLogsToFiles;
        internal int MaxFilesCount => _maxFilesCount;
        internal string LogFileDirectory => _logFileDirectory;
        internal IReadOnlyList<string> Contexts => _contexts;
        internal LogContext ContextsFilter => _contextsFilter;
        internal IReadOnlyList<LoggingLevelSettings> LogLevels => _logLevels;
        internal bool LogExceptions => _logExceptions;

        internal void RestoreToDefaults()
        {
            _loggingEnabled = true;
            _writeLogsToFiles = false;
            _maxFilesCount = 10;

            _contexts = new()
            {
                "Gameplay",
                "UI",
                "Meta",
                "Infrastructure"
            };

            ResetFilter();
            _logLevels = GetDefaults();
            _logExceptions = true;
        }

        internal bool CanBeGenerated() 
            => _logLevels.Any(item => item.Name == "Default") 
            && !_logLevels.Any(item => item.Name == "Exception");

        internal void ResetFilter() => _contextsFilter = GetAllLogContexts();

        private void OnValidate()
        {
            if (_logLevels.Count == 0)
            {
                Debug.LogWarning("You must have at least 1 logging level!");

                _logLevels.Add(new() { Name = "Default", Color = DefaultLogColor, Emphasis = LogEmphasis.None, Type = LogType.Log });
            }
        }

        private static List<LoggingLevelSettings> GetDefaults()
        {
            return new()
            {
                new() { Name = "Default", Color = DefaultLogColor, Emphasis = LogEmphasis.None, Type = LogType.Log },
                new() { Name = "Warning", Color = Color.yellow, Emphasis = LogEmphasis.None, Type = LogType.Warning },
                new() { Name = "Error", Color = Color.red, Emphasis = LogEmphasis.None, Type = LogType.Error }
            };
        }

        private LogContext GetAllLogContexts()
        {
            LogContext result = 0;

            foreach (LogContext ctx in Enum.GetValues(typeof(LogContext)))
            {
                result |= ctx;
            }

            return result;
        }
    }
}