using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Fish.Player
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class PlayerFishIntentScheduler : IFishIntentScheduler
    {
        private readonly MonoBehaviour context;
        private readonly Func<FishIntent> evaluateIntent;
        private readonly Action<FishIntent> applyIntent;
        private Coroutine routine;

        public PlayerFishIntentScheduler(MonoBehaviour context, Func<FishIntent> evaluateIntent, Action<FishIntent> applyIntent)
        {
            this.context = context;
            this.evaluateIntent = evaluateIntent;
            this.applyIntent = applyIntent;
        }

        public void StartEvaluatingPeriodically(float intervalSeconds)
        {
            Stop();
            routine = context.StartCoroutine(EvaluatePeriodically(intervalSeconds));
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

        private IEnumerator EvaluatePeriodically(float intervalSeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalSeconds);
                applyIntent(evaluateIntent());
            }
        }
    }

}
