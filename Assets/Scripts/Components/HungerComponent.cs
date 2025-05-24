using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class HungerComponent : MonoBehaviour
    {
        [SerializeField] private float hungerDelay = 5f;
        private bool isHungry;

        public bool IsHungry => isHungry;

        private IEventBus<HungryEvent> hungryBus;

        public void Init(IEventBus<HungryEvent> hungryBus)
        {
            this.hungryBus = hungryBus;
            StartCoroutine(HungerTimer());
        }

        public void ResetHunger()
        {
            isHungry = false;
            StopAllCoroutines();
            StartCoroutine(HungerTimer());
        }

        private IEnumerator HungerTimer()
        {
            yield return new WaitForSeconds(hungerDelay);
            isHungry = true;
            hungryBus?.Raise(new HungryEvent());
        }
    }

}
