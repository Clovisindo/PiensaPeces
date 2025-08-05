using UnityEngine;

namespace Game.Events
{
    public class EventBusContainer : MonoBehaviour
    {
        public EventBus<FoodEaten> foodEatenEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();

    }
}
