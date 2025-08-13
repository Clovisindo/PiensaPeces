using UnityEngine;

namespace Game.Utilities
{ 
    public class UnityLogger : ILogger
    {
        public void LogError(string message) => Debug.LogError(message);

        public void LogInfo(string message) => Debug.Log(message);

        public void LogWarning(string message) => Debug.LogWarning(message);
    }
}
