using System.Collections.Generic;

namespace Game.Utilities
{
    public class TestLogger : ILogger
    {
        public readonly List<string> Infos = new();
        public readonly List<string> Warnings = new();
        public readonly List<string> Errors = new();

        public void LogError(string message) => Errors.Add(message);

        public void LogInfo(string message) => Infos.Add(message);

        public void LogWarning(string message) => Warnings.Add(message);
    }
}
