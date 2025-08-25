using Game.FishLogic;
using Game.Services;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.States.Tests
{ 
    public class FollowTargetStateTests
    {
        private IFish fish;
        private ITimeService timeService;
        private Transform fishTransform;
        private Transform targetTransform;
        private SpriteRenderer spriteRenderer;
        private const float SPEED = 1f;

        [SetUp]
        public void Setup()
        {
            var go = new GameObject("Fish");
            fishTransform = go.transform;
            fishTransform.transform.position = Vector3.zero;
            spriteRenderer = go.AddComponent<SpriteRenderer>();
            fish = Substitute.For<IFish>();
            fish.GetSpriteRenderer().Returns(spriteRenderer);
            fish.GetTransform().Returns(fishTransform);

            timeService = Substitute.For<ITimeService>();
            
            var goTarget = new GameObject("target");
            targetTransform = goTarget.transform;
            targetTransform.position = Vector3.zero;
            
        }

        [TestCase(-1f, 0f, true, TestName = "Update_MoveToTarget_Left")] // izquierda
        [TestCase(1f, 0f, false, TestName = "Update_MoveToTarget_Right")] // derecha
        [TestCase(0f, 0f, null, TestName = "Update_MoveToTarget_Center")] // no giro
        public void Update_MoveToTarget(float targetX,float targetY, bool? expectedFlipX)
        {
            targetTransform.position = new Vector3(targetX, targetY, 0);
            var initialFlipValue = spriteRenderer.flipX;
            var initialDistance = Vector3.Distance(fish.GetTransform().position, targetTransform.position);
            FollowTargetState followTargetState = new FollowTargetState(fish, timeService, SPEED, targetTransform);

            followTargetState.Enter();
            timeService.DeltaTime.Returns(1.0f);
            followTargetState.Update();

            var currentDistance = Vector3.Distance(fish.GetTransform().position, targetTransform.position);
            var currentFlipValue = spriteRenderer.flipX;
            if (expectedFlipX.HasValue)
            {
                Assert.AreEqual(expectedFlipX.Value, currentFlipValue);
                Assert.Less(currentDistance, initialDistance);
            }
            else
            {
                Assert.AreEqual(initialFlipValue, currentFlipValue);
                Assert.AreEqual(currentDistance, initialDistance);
            }
        }

        [Test]
        public void Update_MultipleFrames_FishMovesCloserEachFrame()
        {
            // Target a la derecha en (3,0)
            targetTransform.position = new Vector3(3f, 0f, 0f);
            var state = new FollowTargetState(fish, timeService, SPEED, targetTransform);
            state.Enter();

            // Distancia inicial
            float prevDistance = Vector3.Distance(fishTransform.position, targetTransform.position);

            // Simulamos 5 frames con deltaTime = 0.5f
            timeService.DeltaTime.Returns(0.5f);
            for (int i = 0; i < 10; i++)
            {
                state.Update();
                float currentDistance = Vector3.Distance(fishTransform.position, targetTransform.position);

                // El pez nunca debe alejarse más del target
                Assert.LessOrEqual(currentDistance, prevDistance);
                prevDistance = currentDistance;
            }

            // Después de varias actualizaciones, debería estar muy cerca del target
            Assert.Less(prevDistance, 0.1f);
        }

    }
}
