using UnityEngine;
using GreedyLogger.Settings;
using System.Linq;
using System;

namespace GreedyLogger
{
    public static partial class GLogger
    {
        private static LoggingSettings _settings;

        internal static void Initialize(LoggingSettings settings)
        {
            _settings = settings;

            FileLogger.Initialize(settings);

            TryLog("GreedyLogger successfully initialized!");
        }

        internal static void TryLog(string message, LogImportance logImportance = LogImportance.Default, LogContext context = LogContext.None)
        {
            if (_settings == null || !_settings.LoggingEnabled || !_settings.ContextsFilter.HasFlag(context))
            {
                return;
            }

            LoggingLevelSettings levelSettings = _settings.LogLevels.First(c => c.Name == logImportance.ToString());

            string hexColor = ColorUtility.ToHtmlStringRGBA(levelSettings.Color);

            if (context != LogContext.None)
            {
                message = levelSettings.ColorizeContextOnly 
                    ? $"<color=#{hexColor}>[{context}]</color> {message}" 
                    : $"[{context}] {message}";
            }

            if (!levelSettings.ColorizeContextOnly)
            {
                message = $"<color=#{hexColor}>{message}</color>";
            }

            message = GetEmphasizedMessage(levelSettings.Emphasis, message);

            switch (levelSettings.Type)
            {
                case Settings.LogType.Log:
                    Debug.Log(message); 
                    break;

                case Settings.LogType.Warning:
                    Debug.LogWarning(message);
                    break;

                case Settings.LogType.Error:
                    Debug.LogError(message);
                    break;

                case Settings.LogType.Assert:
                    Debug.LogAssertion(message);
                    break;
            }
        }

        private static void TryLogException(Exception exception)
        {
            if (_settings == null || !_settings.LoggingEnabled)
            {
                return;
            }

            Debug.LogException(exception);
        }

        private static string GetEmphasizedMessage(LogEmphasis logEmphasis, string message)
        {
            if (logEmphasis == LogEmphasis.None)
            {
                return message;
            }

            if (logEmphasis.HasFlag(LogEmphasis.Bold))
            {
                message = $"<b>{message}</b>";
            }

            if (logEmphasis.HasFlag(LogEmphasis.Italic))
            {
                message = $"<i>{message}</i>";
            }

            if (logEmphasis.HasFlag(LogEmphasis.Underline))
            {
                message = $"<u>{message}</u>";
            }

            return message;
        }
    }
}
