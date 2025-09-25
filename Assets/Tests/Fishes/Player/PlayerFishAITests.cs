using Game.FishFood;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.Fishes.Tests
{
    public class PlayerFishAITests
    {
        private GameObject fishGO;
        private PlayerFishAI playerFishAI;
        private Transform fishTransform;
        private IHungerComponent hungerComponent;
        private IFoodManagerService foodManagerService;

        [SetUp]
        public void Setup()
        {
            fishGO = new GameObject("fish");
            fishTransform = fishGO.transform;
            hungerComponent = Substitute.For<IHungerComponent>();
            foodManagerService = Substitute.For<IFoodManagerService>();

        }
        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(fishGO);
        }

        [Test]
        public void Constructor_WithValidDependencies_CreateInstance()
        {
            playerFishAI = new PlayerFishAI(fishTransform, hungerComponent, foodManagerService);
            Assert.NotNull(playerFishAI);
        }
        [Test]
        public void EvaluateIntent_WhenIsNotHungry_ThenSwim()
        {
            hungerComponent.IsHungry.Returns(false);
            playerFishAI = new PlayerFishAI(fishTransform, hungerComponent, foodManagerService);

            var result = playerFishAI.EvaluateIntent();

            Assert.AreEqual(FishIntent.SwimRandomly, result);
        }

        [Test]
        public void EvaluateIntent_WhenIsHungryAndHasFood_ThenReturnFollodFood()
        {
            hungerComponent.IsHungry.Returns(true);
            foodManagerService.GetClosestFood(Arg.Any<Vector3>()).Returns(new GameObject("FoodMock"));
            playerFishAI = new PlayerFishAI(fishTransform, hungerComponent, foodManagerService);

            var result = playerFishAI.EvaluateIntent();

            Assert.AreEqual(FishIntent.FollowFood, result);
        }

        [Test]
        public void EvaluateIntent_WhenIsHungryAndHasNotFood_ThenSwim()
        {
            hungerComponent.IsHungry.Returns(true);
            playerFishAI = new PlayerFishAI(fishTransform, hungerComponent, foodManagerService);

            var result = playerFishAI.EvaluateIntent();

            Assert.AreEqual(FishIntent.SwimRandomly, result);
        }

        [Test]
        public void GetTargetFood_IfExistReturnTransform()
        {
            var food = new GameObject("FoodMock");
            foodManagerService.GetClosestFood(Arg.Any<Vector3>()).Returns(food);
            playerFishAI = new PlayerFishAI(fishTransform, hungerComponent, foodManagerService);

            var result = playerFishAI.GetTargetFood();
            Assert.AreEqual(food.transform, result);
        }

        [Test]
        public void GetTargetFood_IfNotExist_ThenReturnNull()
        {
            playerFishAI = new PlayerFishAI(fishTransform, hungerComponent, foodManagerService);

            var result = playerFishAI.GetTargetFood();
            Assert.IsNull(result);
        }
    }
}
