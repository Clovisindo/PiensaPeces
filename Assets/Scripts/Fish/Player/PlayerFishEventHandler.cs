using Assets.Scripts.Components;
using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;

namespace Assets.Scripts.Fish.Player
{
    public class PlayerFishEventHandler
    {
        private readonly PlayerFishController controller;

        private readonly IEventBus<FoodEaten> foodEatenBus;
        private readonly IEventBus<FoodSpawned> foodSpawnedBus;
        private readonly IEventBus<HungryEvent> hungryBus;
        private EventBinding<FoodEaten> foodEatenBinding;
        private EventBinding<FoodSpawned> foodSpawnedBinding;
        private EventBinding<HungryEvent> hungryBinding;

        public PlayerFishEventHandler(PlayerFishController controller,
            IEventBus<FoodEaten> foodEatenBus,
            IEventBus<FoodSpawned> foodSpawnedBus,
            IEventBus<HungryEvent> hungryBus)
        {
            this.controller = controller;
            this.foodEatenBus = foodEatenBus;
            this.foodSpawnedBus = foodSpawnedBus;
            this.hungryBus = hungryBus;
        }

        public void RegisterEvents()
        {
            foodEatenBinding = new EventBinding<FoodEaten>(OnFoodEaten);
            foodSpawnedBinding = new EventBinding<FoodSpawned>(OnFoodSpawned);
            hungryBinding = new EventBinding<HungryEvent>(OnHungry);

            foodEatenBus.Register(foodEatenBinding);
            foodSpawnedBus.Register(foodSpawnedBinding);
            hungryBus.Register(hungryBinding);
        }

        public void DeregisterEvents()
        {
            foodEatenBus?.Deregister(foodEatenBinding);
            foodSpawnedBus?.Deregister(foodSpawnedBinding);
            hungryBus?.Deregister(hungryBinding);
        }

        private void OnFoodEaten()
        {
            controller.HandleFoodEatenEvent();
        }

        private void OnFoodSpawned(FoodSpawned e)
        {
            controller.HandleFoodSpawnedEvent();
        }

        private void OnHungry(HungryEvent e)
        {
            controller.HandleHungryEvent();
        }
    }
}
