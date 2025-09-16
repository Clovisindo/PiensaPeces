using Game.Data;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class NPCFishIntentScheduler : IFishIntentScheduler
    {
        private readonly Func<FishIntent> evaluateIntent;
        private readonly Action<FishIntent> applyIntent;
        private ICoroutineRunner _coroutineRunner;
        private IYieldInstruction _yieldInstruction;
        private FishConfig config;

        public NPCFishIntentScheduler(ICoroutineRunner coroutineRunner, IYieldInstruction yieldInstruction, FishConfig config, Func<FishIntent> evaluateIntent, Action<FishIntent> applyIntent)
        {
            this._coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            this._yieldInstruction = yieldInstruction ?? new UnityYieldInstruction();
            this.config = config;
            this.evaluateIntent = evaluateIntent;
            this.applyIntent = applyIntent;
        }
        public void StartEvaluatingPeriodically()
        {
            Stop();
            _coroutineRunner.StartDisplayCoroutine(EvaluatePeriodically());
        }

        public void EvaluateNow()
        {
            applyIntent(evaluateIntent());
        }

        public void Stop()
        {
           _coroutineRunner.StopCurrentDisplayCoroutine();
           
        }

        private IEnumerator EvaluatePeriodically()
        {
            while (true)
            {
                yield return _yieldInstruction.WaitForSeconds(config.intervalEvaluateIntent);
                applyIntent(evaluateIntent());
            }
        }
    }
}
