using Game.Events;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class HungerComponent : MonoBehaviour,IHungerComponent
    {
        [SerializeField] private float hungerDelay = 5f;
        private ICoroutineRunner _runner;
        private IYieldInstruction _yieldInstruction;

        public bool IsHungry { get; private set; }

        private IEventBus<HungryEvent> _hungryBus;

        public void Init(IEventBus<HungryEvent> hungryBus, ICoroutineRunner runner, IYieldInstruction yieldInstruction = null)
        {
            _hungryBus = hungryBus;
            _runner = runner ?? throw new ArgumentNullException(nameof(runner));
            _yieldInstruction = yieldInstruction ?? new UnityYieldInstruction();

            _runner.StartDisplayCoroutine(HungerTimer());
        }

        public void ResetHunger()
        {
            IsHungry = false;
            _runner.StopCurrentDisplayCoroutine();
            _runner.StartDisplayCoroutine(HungerTimer());
        }

        private IEnumerator HungerTimer()
        {
            yield return _yieldInstruction.WaitForSeconds(hungerDelay);
            IsHungry = true;
            _hungryBus?.Raise(new HungryEvent());
        }
    }

}
