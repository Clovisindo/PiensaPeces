using Game.Events;
using System;

namespace Game.Fishes
{
    public class PlayerFishEventHandler
    {
        private readonly IFishIntentScheduler _intentScheduler;
        private readonly IHungerComponent _hungerComponent;

        private readonly IEventBus<FoodEaten> _foodEatenBus;
        private readonly IEventBus<FoodSpawned> _foodSpawnedBus;
        private readonly IEventBus<HungryEvent> _hungryBus;
        private EventBinding<FoodEaten> foodEatenBinding;
        private EventBinding<FoodSpawned> foodSpawnedBinding;
        private EventBinding<HungryEvent> hungryBinding;

        public PlayerFishEventHandler(
            IFishIntentScheduler fishIntentScheduler,
            IHungerComponent hungerComponent,
            IEventBus<FoodEaten> foodEatenBus,
            IEventBus<FoodSpawned> foodSpawnedBus,
            IEventBus<HungryEvent> hungryBus)
        {
            _intentScheduler = fishIntentScheduler ?? throw new ArgumentNullException(nameof(_intentScheduler));
            _hungerComponent = hungerComponent ?? throw new ArgumentNullException(nameof(_hungerComponent));
            _foodEatenBus = foodEatenBus ?? throw new ArgumentNullException(nameof(_foodEatenBus));
            _foodSpawnedBus = foodSpawnedBus ?? throw new ArgumentNullException(nameof(_foodSpawnedBus));
            _hungryBus = hungryBus ?? throw new ArgumentNullException(nameof(_hungryBus));
        }

        public void RegisterEvents()
        {
            foodEatenBinding = new EventBinding<FoodEaten>(OnFoodEaten);
            foodSpawnedBinding = new EventBinding<FoodSpawned>(OnFoodSpawned);
            hungryBinding = new EventBinding<HungryEvent>(OnHungry);

            _foodEatenBus.Register(foodEatenBinding);
            _foodSpawnedBus.Register(foodSpawnedBinding);
            _hungryBus.Register(hungryBinding);
        }

        public void DeregisterEvents()
        {
            _foodEatenBus?.Deregister(foodEatenBinding);
            _foodSpawnedBus?.Deregister(foodSpawnedBinding);
            _hungryBus?.Deregister(hungryBinding);
        }

        private void OnFoodEaten()
        {
            _hungerComponent.ResetHunger();
            _intentScheduler.Stop();
            _intentScheduler.EvaluateNow();
        }

        private void OnFoodSpawned()
        {
            if (_hungerComponent.IsHungry)
            {
                _intentScheduler.EvaluateNow();
            }
        }

        private void OnHungry()
        {
            _intentScheduler.StartEvaluatingPeriodically();
        }
    }
}
