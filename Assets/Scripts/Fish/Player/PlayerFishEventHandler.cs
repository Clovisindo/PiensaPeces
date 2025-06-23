using Assets.Scripts.Components;
using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;

namespace Assets.Scripts.Fish.Player
{
    public class PlayerFishEventHandler
    {
        private readonly IFishIntentScheduler intentScheduler;
        private readonly HungerComponent hungerComponent;

        private readonly IEventBus<FoodEaten> foodEatenBus;
        private readonly IEventBus<FoodSpawned> foodSpawnedBus;
        private readonly IEventBus<HungryEvent> hungryBus;
        private EventBinding<FoodEaten> foodEatenBinding;
        private EventBinding<FoodSpawned> foodSpawnedBinding;
        private EventBinding<HungryEvent> hungryBinding;

        public PlayerFishEventHandler(
            IFishIntentScheduler fishIntentScheduler,
            HungerComponent hungerComponent,
            SFXManager sFXManager,
            IEventBus<FoodEaten> foodEatenBus,
            IEventBus<FoodSpawned> foodSpawnedBus,
            IEventBus<HungryEvent> hungryBus)
        {
            this.intentScheduler = fishIntentScheduler;
            this.hungerComponent = hungerComponent;
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
            hungerComponent.ResetHunger();
            intentScheduler.Stop();
            intentScheduler.EvaluateNow();
        }

        private void OnFoodSpawned(FoodSpawned e)
        {
            if (hungerComponent.IsHungry)
            {
                intentScheduler.EvaluateNow();
            }
        }

        private void OnHungry(HungryEvent e)
        {
            intentScheduler.StartEvaluatingPeriodically();
        }
    }
}
