using Game.Data;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class NPCFishIntentScheduler : IFishIntentScheduler
    {
        private readonly Func<FishIntent> _evaluateIntent;
        private readonly Action<FishIntent> _applyIntent;
        private ICoroutineRunner _coroutineRunner;
        private IYieldInstruction _yieldInstruction;
        private FishConfig _config;

        public NPCFishIntentScheduler(ICoroutineRunner coroutineRunner, IYieldInstruction yieldInstruction, FishConfig config, Func<FishIntent> evaluateIntent, Action<FishIntent> applyIntent)
        {
            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _yieldInstruction = yieldInstruction ?? new UnityYieldInstruction();
            _config = config;
            _evaluateIntent = evaluateIntent;
            _applyIntent = applyIntent;
        }
        public void StartEvaluatingPeriodically()
        {
            Stop();
            _coroutineRunner.StartDisplayCoroutine(EvaluatePeriodically());
        }

        public void EvaluateNow()
        {
            _applyIntent(_evaluateIntent());
        }

        public void Stop()
        {
           _coroutineRunner.StopCurrentDisplayCoroutine();
           
        }

        private IEnumerator EvaluatePeriodically()
        {
            while (true)
            {
                yield return _yieldInstruction.WaitForSeconds(_config.intervalEvaluateIntent);
                _applyIntent(_evaluateIntent());
            }
        }
    }
}
