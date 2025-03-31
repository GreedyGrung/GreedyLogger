using GreedyLogger.Settings;
using NUnit.Framework;
using System;
using System.Reflection;
using UnityEngine.TestTools;
using UnityEngine;

namespace GreedyLogger.Tests
{
    internal class GLoggerTests
    {
        private const string SettingsPrivateFieldName = "_settings";
        private const string TestMessage = "Test message";

        [Test]
        public void GetEmphasizedMessage_CombinesFlagsCorrectly()
        {
            MethodInfo getEmphMethod = typeof(GLogger).GetMethod("GetEmphasizedMessage", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(getEmphMethod, "Method GetEmphasizedMessage not found.");

            LogEmphasis flags = LogEmphasis.Bold | LogEmphasis.Italic;
            string result = (string)getEmphMethod.Invoke(null, new object[] { flags, TestMessage });
            Assert.AreEqual($"<b><i>{TestMessage}</i></b>", result);
        }

        [Test]
        public void TryLog_DoesNotLog_WhenLoggingDisabled()
        {
            LoggingSettings settings = CreateTestSettings(false, GetAllLogContexts());
            SetStaticPrivateField(typeof(GLogger), SettingsPrivateFieldName, settings);
            
            GLogger.TryLog(TestMessage, LogImportance.Default);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void TryLog_DoesNotLog_WhenContextDoesNotMatchFilter()
        {
            LoggingSettings settings = CreateTestSettings(true, GetFirstContextOrNone());
            SetStaticPrivateField(typeof(GLogger), SettingsPrivateFieldName, settings);

            GLogger.TryLog(TestMessage, LogImportance.Default);

            if (settings.ContextsFilter != LogContext.None)
            {
                LogAssert.NoUnexpectedReceived();
            }
        }

        private void SetStaticPrivateField(Type type, string fieldName, object value)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field, $"Static field {fieldName} not found in {type.Name}");
            field.SetValue(null, value);
        }

        private LoggingSettings CreateTestSettings(bool loggingEnabled, LogContext contextsFilter)
        {
            LoggingSettings settings = ScriptableObject.CreateInstance<LoggingSettings>();

            SetPrivateField(settings, "_loggingEnabled", loggingEnabled);
            SetPrivateField(settings, "_contextsFilter", contextsFilter);

            return settings;
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                Assert.Fail($"Field {fieldName} not found in {obj.GetType().Name}");
            }
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

        private LogContext GetFirstContextOrNone()
        {
            var values = (LogContext[])Enum.GetValues(typeof(LogContext));

            foreach (var v in values)
            {
                if (v != LogContext.None)
                {
                    return v;
                }
            }

            return LogContext.None;
        }
    }
}
