using UnityEngine;
using GreedyLogger.Settings;
using System.Linq;

namespace GreedyLogger
{
    public static partial class GLogger
    {
        private static LoggingSettings _settings;

        internal static void Initialize(LoggingSettings settings)
        {
            _settings = settings;

            TryLog("GreedyLogger successfully initialized!");
        }

        private static void TryLog(string message, LogImportance logImportance = LogImportance.Default, LogContext context = LogContext.None)
        {
            if (_settings == null || !_settings.LoggingEnabled)
                return;

            LoggingLevelSettings levelSettings = _settings.LogLevels.First(c => c.Name == logImportance.ToString());

            if (context != LogContext.None)
            {
                message = $"[{context}] {message}";
            }

            string hexColor = ColorUtility.ToHtmlStringRGBA(levelSettings.Color);
            message = $"<color=#{hexColor}>{message}</color>";

            message = GetEmphasizedMessage(levelSettings.Emphasis, message);

            switch (levelSettings.Type)
            {
                case LogType.Log:
                    Debug.Log(message); 
                    break;

                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;

                case LogType.Error:
                    Debug.LogError(message);
                    break;

                case LogType.Assert:
                    Debug.LogAssertion(message);
                    break;

                case LogType.Exception:
                    Debug.LogError(message);
                    break;
            }
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
