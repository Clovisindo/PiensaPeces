using Assets.Scripts.Events.Events;
using UnityEngine;

namespace Assets.Scripts.Events.EventBus
{
    public class EventBusContainer : MonoBehaviour
    {
        public EventBus<FoodEaten> foodEatenEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();

    }
}
