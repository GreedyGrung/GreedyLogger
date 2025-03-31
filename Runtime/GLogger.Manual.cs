using UnityEngine;
using GreedyLogger.Settings;
using System;
using System.Text;

namespace GreedyLogger
{
    public static partial class GLogger
    {
        private static readonly StringBuilder _builder = new(256);
        private static LoggingSettings _settings;

        internal static void Initialize(LoggingSettings settings)
        {
            _settings = settings;

            TryLog("GreedyLogger successfully initialized!");
        }

        internal static void TryLog(string message, LogImportance logImportance = LogImportance.Default, LogContext context = LogContext.None)
        {
            if (_settings == null || !_settings.LoggingEnabled || !_settings.ContextsFilter.HasFlag(context))
            {
                return;
            }

            LoggingLevelSettings levelSettings = GetSettings(logImportance);
            string hexColor = ColorUtility.ToHtmlStringRGBA(levelSettings.Color);

            _builder.Clear();

            if (context != LogContext.None)
            {
                if (levelSettings.ColorizeContextOnly)
                {
                    _builder.Append("<color=#").Append(hexColor).Append(">[")
                            .Append(context).Append("]</color> ");
                }
                else
                {
                    _builder.Append("[").Append(context).Append("] ");
                }

                _builder.Append(message);
                message = _builder.ToString();
            }
            
            if (!levelSettings.ColorizeContextOnly)
            {
                _builder.Clear();
                _builder.Append("<color=#").Append(hexColor).Append(">")
                        .Append(message).Append("</color>");
                message = _builder.ToString();
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

        private static LoggingLevelSettings GetSettings(LogImportance logImportance)
        {
            string logImportanceString = logImportance.ToString();
            LoggingLevelSettings levelSettings = null;

            for (int i = 0; i < _settings.LogLevels.Count; i++)
            {
                if (_settings.LogLevels[i].Name == logImportanceString)
                {
                    levelSettings = _settings.LogLevels[i];
                    break;
                }
            }

            levelSettings ??= _settings.LogLevels[0];

            return levelSettings;
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

            _builder.Clear();

            if (logEmphasis.HasFlag(LogEmphasis.Bold)) _builder.Append("<b>");
            if (logEmphasis.HasFlag(LogEmphasis.Italic)) _builder.Append("<i>");
            if (logEmphasis.HasFlag(LogEmphasis.Underline)) _builder.Append("<u>");

            _builder.Append(message);

            if (logEmphasis.HasFlag(LogEmphasis.Underline)) _builder.Append("</u>");
            if (logEmphasis.HasFlag(LogEmphasis.Italic)) _builder.Append("</i>");
            if (logEmphasis.HasFlag(LogEmphasis.Bold)) _builder.Append("</b>");

            return _builder.ToString();
        }
    }
}
