using System.Collections;

namespace Game.Fishes
{
    public interface IFishIntentScheduler
    {

        public void StartEvaluatingPeriodically();
        public void EvaluateNow();
        public void Stop();
    }
}
