using Assets.Scripts.Components;
using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;

namespace Assets.Scripts.Fish.Player
{
    public class PlayerFishEventHandler
    {
        private readonly PlayerFishIntentScheduler intentScheduler;
        private readonly HungerComponent hungerComponent;
        private readonly SFXManager sFXManager;

        private readonly IEventBus<FoodEaten> foodEatenBus;
        private readonly IEventBus<FoodSpawned> foodSpawnedBus;
        private readonly IEventBus<HungryEvent> hungryBus;
        private readonly IEventBus<SFXEvent> sfxBus;
        private EventBinding<FoodEaten> foodEatenBinding;
        private EventBinding<FoodSpawned> foodSpawnedBinding;
        private EventBinding<HungryEvent> hungryBinding;
        private EventBinding<SFXEvent> sfxBiding;

        public PlayerFishEventHandler(
            PlayerFishIntentScheduler fishIntentScheduler,
            HungerComponent hungerComponent,
            SFXManager sFXManager,
            IEventBus<FoodEaten> foodEatenBus,
            IEventBus<FoodSpawned> foodSpawnedBus,
            IEventBus<HungryEvent> hungryBus,
            IEventBus<SFXEvent> sfxBus)
        {
            this.intentScheduler = fishIntentScheduler;
            this.hungerComponent = hungerComponent;
            this.sFXManager = sFXManager;
            this.foodEatenBus = foodEatenBus;
            this.foodSpawnedBus = foodSpawnedBus;
            this.sfxBus = sfxBus;
            this.hungryBus = hungryBus;
        }

        public void RegisterEvents()
        {
            foodEatenBinding = new EventBinding<FoodEaten>(OnFoodEaten);
            foodSpawnedBinding = new EventBinding<FoodSpawned>(OnFoodSpawned);
            hungryBinding = new EventBinding<HungryEvent>(OnHungry);
            sfxBiding = new EventBinding<SFXEvent>(OnSFXInvoke);

            foodEatenBus.Register(foodEatenBinding);
            foodSpawnedBus.Register(foodSpawnedBinding);
            hungryBus.Register(hungryBinding);
            sfxBus.Register(sfxBiding);
        }

        public void DeregisterEvents()
        {
            foodEatenBus?.Deregister(foodEatenBinding);
            foodSpawnedBus?.Deregister(foodSpawnedBinding);
            hungryBus?.Deregister(hungryBinding);
            sfxBus?.Deregister(sfxBiding);
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
            intentScheduler.StartEvaluatingPeriodically(UnityEngine.Random.Range(0.5f, 3f));
        }

        private void OnSFXInvoke(SFXEvent e)
        {
            sFXManager.onPlaySFXPitched(e.sfxData);
        }
    }
}
