using Game.Events;
using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class HungerComponent : MonoBehaviour,IHungerComponent
    {
        [SerializeField] private float hungerDelay = 5f;

        public bool IsHungry { get; private set; }

        private IEventBus<HungryEvent> hungryBus;

        public void Init(IEventBus<HungryEvent> hungryBus)
        {
            this.hungryBus = hungryBus;
            StartCoroutine(HungerTimer());
        }

        public void ResetHunger()
        {
            IsHungry = false;
            StopAllCoroutines();
            StartCoroutine(HungerTimer());
        }

        private IEnumerator HungerTimer()
        {
            yield return new WaitForSeconds(hungerDelay);
            IsHungry = true;
            hungryBus?.Raise(new HungryEvent());
        }
    }

}
