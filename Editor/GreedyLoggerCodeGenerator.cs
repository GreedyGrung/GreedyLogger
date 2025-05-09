﻿using UnityEngine;
using System.IO;
using GreedyLogger.Settings;
using UnityEditor;
using System.Text;

namespace GreedyLogger.Editor
{
    internal static class GreedyLoggerCodeGenerator
    {
        private const string Indent = "    ";
        private const string NewLine = "\n";

        private const string HeaderText =
            "// <auto-generated>" + NewLine +
            "// This file was generated by GreedyLogger code generation utility. Do not modify manually." + NewLine +
            "// </auto-generated>";

        internal static void Generate()
        {
            Debug.Log("Generation process for GreedyLogger started...");

            string settingsPath = Utils.FindAssetPath(Constants.SettingsAssetFilter);
            LoggingSettings settings = AssetDatabase.LoadAssetAtPath<LoggingSettings>(settingsPath);

            if (settings == null)
            {
                Debug.LogError($"Failed to load LoggingSettings asset at {settingsPath}");

                return;
            }

            string packagePath = Utils.FindPackageRoot();
            string generatedDir = Path.Combine(packagePath, "Runtime", "Generated");

            if (!Directory.Exists(generatedDir))
            {
                Directory.CreateDirectory(generatedDir);
            }

            string loggerGeneratedFile = Path.Combine(generatedDir, "GLogger.Generated.cs");
            string loggerCode = GenerateLoggerCode(settings);
            File.WriteAllText(loggerGeneratedFile, loggerCode);

            string logImportanceFile = Path.Combine(generatedDir, "LogImportance.Generated.cs");
            string importanceCode = GenerateLogImportanceEnum(settings);
            File.WriteAllText(logImportanceFile, importanceCode);

            string logContextFile = Path.Combine(generatedDir, "LogContext.Generated.cs");
            string contextCode = GenerateLogContextEnum(settings);
            File.WriteAllText(logContextFile, contextCode);

            AssetDatabase.Refresh();

            Debug.Log("Generation process completed successfully!");
        }

        private static string GenerateLoggerCode(LoggingSettings settings)
        {
            StringBuilder sb = new();
            int baseIndent = 0;

            GenerateHeader(sb, baseIndent);
            AppendIndentedLine(sb, baseIndent, "namespace GreedyLogger");
            AppendIndentedLine(sb, baseIndent, "{");
            AppendIndentedLine(sb, baseIndent + 1, "public static partial class GLogger");
            AppendIndentedLine(sb, baseIndent + 1, "{");

            foreach (var level in settings.LogLevels)
            {
                string methodName = "Log" + level.Name;

                if (level.Name == "Default")
                {
                    methodName = "Log";
                }

                AppendIndentedLine(sb, baseIndent + 2, "/// <summary>");
                AppendIndentedLine(sb, baseIndent + 2, "/// Logs a message with " + level.Name + " importance.");
                AppendIndentedLine(sb, baseIndent + 2, "/// </summary>");
                AppendIndentedLine(sb, baseIndent + 2, $"public static void {methodName}(string message, LogContext context = LogContext.None)");
                AppendIndentedLine(sb, baseIndent + 2, "{");
                AppendIndentedLine(sb, baseIndent + 3, $"TryLog(message, LogImportance.{level.Name}, context);");
                AppendIndentedLine(sb, baseIndent + 2, "}");
                sb.AppendLine();
            }

            if (settings.LogExceptions)
            {
                AppendIndentedLine(sb, baseIndent + 2, "/// <summary>");
                AppendIndentedLine(sb, baseIndent + 2, "/// Logs an exception.");
                AppendIndentedLine(sb, baseIndent + 2, "/// </summary>");
                AppendIndentedLine(sb, baseIndent + 2, $"public static void LogException(System.Exception exception)");
                AppendIndentedLine(sb, baseIndent + 2, "{");
                AppendIndentedLine(sb, baseIndent + 3, $"TryLogException(exception);");
                AppendIndentedLine(sb, baseIndent + 2, "}");
            }

            AppendIndentedLine(sb, baseIndent + 1, "}");
            AppendIndentedLine(sb, baseIndent, "}");

            return sb.ToString();
        }

        private static string GenerateLogImportanceEnum(LoggingSettings settings)
        {
            StringBuilder sb = new();
            int baseIndent = 0;

            GenerateHeader(sb, baseIndent);
            AppendIndentedLine(sb, baseIndent, "namespace GreedyLogger");
            AppendIndentedLine(sb, baseIndent, "{");
            AppendIndentedLine(sb, baseIndent + 1, "public enum LogImportance");
            AppendIndentedLine(sb, baseIndent + 1, "{");

            for (int i = 0; i < settings.LogLevels.Count; i++)
            {
                AppendIndentedLine(sb, baseIndent + 2, $"{settings.LogLevels[i].Name} = {i},");
            }

            AppendIndentedLine(sb, baseIndent + 1, "}");
            AppendIndentedLine(sb, baseIndent, "}");

            return sb.ToString();
        }

        private static string GenerateLogContextEnum(LoggingSettings settings)
        {
            StringBuilder sb = new();
            int baseIndent = 0;

            GenerateHeader(sb, baseIndent);
            AppendIndentedLine(sb, baseIndent, "namespace GreedyLogger");
            AppendIndentedLine(sb, baseIndent, "{");
            AppendIndentedLine(sb, baseIndent + 1, "[System.Flags]");
            AppendIndentedLine(sb, baseIndent + 1, "public enum LogContext");
            AppendIndentedLine(sb, baseIndent + 1, "{");
            AppendIndentedLine(sb, baseIndent + 2, "None = 1 << 0,");

            for (int i = 0; i < settings.Contexts.Count; i++)
            {
                if (!string.IsNullOrEmpty(settings.Contexts[i]))
                {
                    AppendIndentedLine(sb, baseIndent + 2, $"{settings.Contexts[i]} = 1 << {i + 1},");
                }
            }

            AppendIndentedLine(sb, baseIndent + 1, "}");
            AppendIndentedLine(sb, baseIndent, "}");

            return sb.ToString();
        }

        private static void GenerateHeader(StringBuilder sb, int indentLevel) 
            => AppendIndentedLine(sb, indentLevel, HeaderText);

        private static void AppendIndentedLine(StringBuilder sb, int indentLevel, string line)
        {
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append(Indent);
            }

            sb.AppendLine(line);
        }
    }
}
