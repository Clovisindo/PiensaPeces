using UnityDeltaTime = UnityEngine.Time;

namespace Game.Utilities
{
    public class UnityTimeService : ITimeService
    {
        public float DeltaTime => UnityDeltaTime.deltaTime;

        public float Time => UnityDeltaTime.time;
    }
}
