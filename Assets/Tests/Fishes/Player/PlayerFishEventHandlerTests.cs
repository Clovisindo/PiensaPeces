using Game.Events;
using NSubstitute;
using NUnit.Framework;

namespace Game.Fishes.Tests
{
    public class PlayerFishEventHandlerTests
    {
        private PlayerFishEventHandler playerFishEventHandler;
        private IFishIntentScheduler fishIntentScheduler;
        private IHungerComponent hungerComponent;
        private IEventBus<FoodEaten> foodEatenEventBus;
        private IEventBus<FoodSpawned> foodSpawnedEventBus;
        private IEventBus<HungryEvent> hungryEventEventBus;

        [SetUp]
        public void Setup()
        {
            fishIntentScheduler = Substitute.For<IFishIntentScheduler>();
            hungerComponent = Substitute.For<IHungerComponent>();
            foodEatenEventBus = Substitute.For<IEventBus<FoodEaten>>();
            foodSpawnedEventBus = Substitute.For<IEventBus<FoodSpawned>>();
            hungryEventEventBus = Substitute.For<IEventBus<HungryEvent>>();
        }

        [Test]
        public void Constructor_WithValidDependencies_CreateInstance()
        {
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
              foodSpawnedEventBus, hungryEventEventBus);

            Assert.IsNotNull(playerFishEventHandler);
        }

        [Test]
        public void RegisterEvents_VerifyBindingAndBusRegister()
        {
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
              foodSpawnedEventBus, hungryEventEventBus);

            playerFishEventHandler.RegisterEvents();

            foodEatenEventBus.Received(1).Register(Arg.Any<IEventBinding<FoodEaten>>());
            foodSpawnedEventBus.Received(1).Register(Arg.Any<IEventBinding<FoodSpawned>>());
            hungryEventEventBus.Received(1).Register(Arg.Any<IEventBinding<HungryEvent>>());
        }

        [Test]
        public void DeregisterEvents_VerifyEventBus()
        {
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
              foodSpawnedEventBus, hungryEventEventBus);

            playerFishEventHandler.DeregisterEvents();

            foodEatenEventBus.Received(1).Deregister(Arg.Any<IEventBinding<FoodEaten>>());
            foodSpawnedEventBus.Received(1).Deregister(Arg.Any<IEventBinding<FoodSpawned>>());
            hungryEventEventBus.Received(1).Deregister(Arg.Any<IEventBinding<HungryEvent>>());
        }

        [Test]
        public void OnFoodEathen_VerifyHungerAndIntentSchedulerCalls()
        {
            foodEatenEventBus = new EventBus<FoodEaten>();
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
              foodSpawnedEventBus, hungryEventEventBus);
            playerFishEventHandler.RegisterEvents();

            foodEatenEventBus.Raise(new FoodEaten());

            hungerComponent.Received(1).ResetHunger();
            fishIntentScheduler.Received(1).Stop();
            fishIntentScheduler.Received(1).EvaluateNow();
        }

        [Test]
        public void OnFoodSpawned_IfIsHungry_ThenEvaluateNow()
        {
            foodSpawnedEventBus = new EventBus<FoodSpawned>();
            hungerComponent.IsHungry.Returns(true);
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
             foodSpawnedEventBus, hungryEventEventBus);
            playerFishEventHandler.RegisterEvents();

            foodSpawnedEventBus.Raise(new FoodSpawned());

            fishIntentScheduler.Received(1).EvaluateNow();
        }

        [Test]
        public void OnFoodSpawned_IfNotHungry_ThenNotEvaluateNow()
        {
            foodSpawnedEventBus = new EventBus<FoodSpawned>();
            hungerComponent.IsHungry.Returns(false);
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
             foodSpawnedEventBus, hungryEventEventBus);
            playerFishEventHandler.RegisterEvents();

            foodSpawnedEventBus.Raise(new FoodSpawned());

            fishIntentScheduler.Received(0).EvaluateNow();
        }

        [Test]
        public void OnHungry_ThenStartEvaluatingPeriod()
        {
            hungryEventEventBus = new EventBus<HungryEvent>();
            playerFishEventHandler = new PlayerFishEventHandler(fishIntentScheduler, hungerComponent, foodEatenEventBus,
             foodSpawnedEventBus, hungryEventEventBus);
            playerFishEventHandler.RegisterEvents();

            hungryEventEventBus.Raise(new HungryEvent());

            fishIntentScheduler.Received(1).StartEvaluatingPeriodically();
        }
    }
}
