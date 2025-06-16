using System.Collections;

namespace Assets.Scripts.Fish
{
    public interface IFishIntentScheduler
    {

        public void StartEvaluatingPeriodically();
        public void EvaluateNow();
        public void Stop();
    }
}
