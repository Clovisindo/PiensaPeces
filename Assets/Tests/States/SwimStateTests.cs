using Game.FishLogic;
using Game.Services;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.States.Tests
{
    public class SwimStateTests
    {
        private IFish fish;
        private IBoundsService bounds;
        private ITimeService time;
        private SpriteRenderer spriteRenderer;
        private Transform fishTransform;

        private const float SPEED = 10f;
        private const float RANDOM = 5f;


        [SetUp]
        public void Setup()
        {
            var go = new GameObject("Fish");
            fishTransform = go.transform;
            fishTransform.transform.position = Vector3.zero;
            spriteRenderer = go.AddComponent<SpriteRenderer>();

            fish = Substitute.For<IFish>();
            fish.GetTransform().Returns(fishTransform);
            fish.GetSpriteRenderer().Returns(spriteRenderer);

            bounds = Substitute.For<IBoundsService>();
            bounds.GetMinBounds().Returns(Vector2.zero);
            bounds.GetMaxBounds().Returns(new Vector2(10, 10));

            time = Substitute.For<ITimeService>();
        }

        [Test]
        public void SwimStateEnter_ThenSetAndArraiveNewDestination()
        {
            float FakeRandom(float min, float max) => RANDOM;
            var swimState = new SwimState(fish, bounds, time, SPEED, FakeRandom);

            swimState.Enter();

            time.DeltaTime.Returns(1f);
            swimState.Update();

            Assert.AreEqual(RANDOM, fishTransform.position.x, 0.5f);
            Assert.AreEqual(RANDOM, fishTransform.position.y, 0.5f);
        }

        [Test]
        public void SwinStateExit_ThenDoNothing()
        {
            var swimState = new SwimState(fish, bounds, time, SPEED);

            Assert.DoesNotThrow(() => swimState.Exit());
        }

        [Test]
        public void Update_ReachingNewDestination()
        {
            float FakeRandom(float min, float max) => RANDOM;
            var swimState = new SwimState(fish, bounds, time, SPEED, FakeRandom);
            swimState.Enter();
            var initialPosition = Vector2.Distance(fishTransform.position, new Vector2(5, 5));

            time.DeltaTime.Returns(0.5f);
            swimState.Update();

            var currentPosition = Vector2.Distance(fishTransform.position, new Vector2(5, 5));

            Assert.Less(currentPosition, initialPosition);
        }

        [Test]
        public void Update_CheckCalculateDistance()
        {
            float FakeRandom(float min, float max) => RANDOM;
            var swimState = new SwimState(fish, bounds, time, SPEED, FakeRandom);
            swimState.Enter();
            var initialPosition = fishTransform.position;
            time.DeltaTime.Returns(0.5f);

            swimState.Update();
            var currentPosition = fishTransform.position;

            float distance = Vector2.Distance(initialPosition, currentPosition);
            Assert.AreEqual(SPEED * 0.5f, distance, 0.001f);
        }

        [TestCase(-10f, true, TestName = "SpriteFlip_WhenDestinationIsLeft")]
        [TestCase(10f, false, TestName = "SpriteFlip_WhenDestinationIsRight")]
        [TestCase(0f, null, TestName = "SpriteFlip_WhenDestinationIsCenter")]
        public void SpriteFlip_CheckBehaviour(float randomValue, bool? expectedFlipX)
        {
            float FakeRandom(float min, float max) => randomValue;
            var swimState = new SwimState(fish, bounds, time, SPEED, FakeRandom);
            swimState.Enter();
            var initialValue = spriteRenderer.flipX;

            time.DeltaTime.Returns(0.5f);

            swimState.Update();
            var currentValue = spriteRenderer.flipX;

            if (expectedFlipX.HasValue)
            {
                Assert.AreEqual(expectedFlipX.Value,currentValue);
            }
            else
            {
                Assert.AreEqual(initialValue,currentValue);
            }
        }

        [Test]
        public void ChangeDestination_WhenReachedAndTimeEnough_ShouldInvokeRandomAgain()
        {
            int randomCalls = 0;
            float FakeRandom(float min, float max)
            {
                randomCalls++;
                return 5f;
            };

            var swimState = new SwimState(fish, bounds, time, 1f, FakeRandom);
            swimState.Enter();
            Assert.AreEqual(2, randomCalls, "Enter() should call random twice (x and y)");

            //simulamos que ya está en destino
            fishTransform.position = new Vector3(5f, 5f, 0);
            time.DeltaTime.Returns(1.0f);

            swimState.Update();

            Assert.Greater(randomCalls, 2, "Update() should call random again when destination is reached.");
        }

        /// <summary>
        /// Este test no funciona si se modifica el valor de intervalo a menos de los establecido aqui
        /// </summary>
        [Test]
        public void ChangeDestination_WhenReachedButTimeNotEnough_ShouldNotInvokeRandom()
        {
            int randomCalls = 0;
            float FakeRandom(float min, float max)
            {
                randomCalls++;
                return 5f;
            };

            var swimState = new SwimState(fish, bounds, time, 1f, FakeRandom);
            swimState.Enter();
            Assert.AreEqual(2, randomCalls, "Enter() should call random twice (x and y)");

            //simulamos que ya está en destino
            fishTransform.position = new Vector3(5f, 5f, 0);
            time.DeltaTime.Returns(0.5f);// tiempo de intervalo no se cumple

            swimState.Update();

            Assert.AreEqual(2, randomCalls, "Random should not be called again if timeSinceLastDirectionChange < 1s.");
        }

        [Test]
        public void Time_AccumulatesTime_ThenChangesDestination()
        {
            // Arrange
            var initialPosition = new Vector3(0, 0, 0);
            fish.GetTransform().position = initialPosition;
            
            Vector2 firstDestination = new Vector2(5, 5);
            Vector2 secondDestination = new Vector2(-5, -5);
            int callCount = 0;
            float FakeRandom(float min, float max)
            {
                callCount++;
                return callCount == 1 ? firstDestination.x : secondDestination.x;
            }

            var swimState = new SwimState(fish, bounds, time, SPEED, FakeRandom);
            swimState.Enter();

            // Act
            // Simulamos frames antes de minDirectionChangeInterval
            time.DeltaTime.Returns(0.3f);
            swimState.Update(); // 0.3
            swimState.Update(); // 0.6
            swimState.Update(); // 0.9

            var dirBefore = (firstDestination - (Vector2)fish.GetTransform().position).normalized;

            time.DeltaTime.Returns(0.2f);
            swimState.Update(); // 1.1 acumulado cambio destino

            var dirAfter = (secondDestination - (Vector2)fish.GetTransform().position).normalized;

            // Assert
            // primera direccion
            Assert.AreEqual(dirBefore, (firstDestination - (Vector2)fish.GetTransform().position).normalized);
            // Segunda direccion
            Assert.AreNotEqual(dirBefore, dirAfter);
        }
    }
}
