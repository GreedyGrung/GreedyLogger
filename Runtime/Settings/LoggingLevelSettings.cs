using System;
using UnityEngine;

namespace GreedyLogger.Settings
{
    [Serializable]
    public class LoggingLevelSettings
    {
        public string Name;
        public Color Color;
        public LogEmphasis Emphasis;
        public LogType Type;
    }
}