using System;
using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class PlayerFishIntentScheduler : IFishIntentScheduler
    {
        private readonly MonoBehaviour context;
        private readonly Func<FishIntent> evaluateIntent;
        private readonly Action<FishIntent> applyIntent;
        private Coroutine routine;
        private FishConfig config;

        public PlayerFishIntentScheduler(MonoBehaviour context, FishConfig config, Func<FishIntent> evaluateIntent, Action<FishIntent> applyIntent)
        {
            this.context = context;
            this.config = config;
            this.evaluateIntent = evaluateIntent;
            this.applyIntent = applyIntent;
        }

        public void StartEvaluatingPeriodically()
        {
            Stop();
            routine = context.StartCoroutine(EvaluatePeriodically());
        }

        public void EvaluateNow()
        {
            applyIntent(evaluateIntent());
        }

        public void Stop()
        {
            if (routine != null)
            {
                context.StopCoroutine(routine);
                routine = null;
            }
        }

        private IEnumerator EvaluatePeriodically()
        {
            while (true)
            {
                yield return new WaitForSeconds(config.intervalEvaluateIntent);
                applyIntent(evaluateIntent());
            }
        }
    }

}
