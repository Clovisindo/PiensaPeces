using System.Collections;

namespace Assets.Scripts.Fish
{
    public interface IFishIntentScheduler
    {

        public void StartEvaluatingPeriodically(float intervalSeconds = 1.0f);
        public void EvaluateNow();
        public void Stop();
    }
}
