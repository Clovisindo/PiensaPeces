using Game.Services;
using UnityDeltaTime = UnityEngine.Time;

namespace Assets.Scripts.Services.TimeService
{
    public class UnityTimeService : ITimeService
    {
        public float DeltaTime => UnityDeltaTime.deltaTime;

        public float Time => UnityDeltaTime.time;
    }
}
