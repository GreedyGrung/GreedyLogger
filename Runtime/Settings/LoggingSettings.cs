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

        [Tooltip("Do not forget to press 'Generate' button below to apply changes!")]
        [SerializeField] private List<string> _contexts;

        [Tooltip("Do not forget to press 'Generate' button below to apply changes!")]
        [SerializeField] private List<LoggingLevelSettings> _logLevels = GetDefaults();


        [Tooltip("Do not forget to press 'Generate' button below to apply changes!")]
        [SerializeField] private bool _logExceptions;

        public bool LoggingEnabled => _loggingEnabled;
        public bool WriteLogsToFiles => _writeLogsToFiles;
        public int MaxFilesCount => _maxFilesCount;
        public IReadOnlyList<string> Contexts => _contexts;
        public IReadOnlyList<LoggingLevelSettings> LogLevels => _logLevels;
        public bool LogExceptions => _logExceptions;

        public void RestoreToDefaults() => _logLevels = GetDefaults();

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
    }
}