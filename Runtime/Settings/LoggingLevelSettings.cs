using System;
using UnityEngine;

namespace GreedyLogger.Settings
{
    [Serializable]
    internal class LoggingLevelSettings
    {
        public string Name;
        public Color Color;
        public bool ColorizeContextOnly;
        public LogEmphasis Emphasis;
        public LogType Type;
    }
}