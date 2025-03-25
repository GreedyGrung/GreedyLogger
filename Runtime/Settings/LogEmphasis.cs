using System;

namespace GreedyLogger.Settings
{
    [Flags]
    public enum LogEmphasis
    {
        None = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4
    }
}