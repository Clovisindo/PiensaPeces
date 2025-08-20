using Game.Context;
using Game.FishLogic;
using Game.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;

namespace Game.States.Tests
{
    public class ExitScreenStateTests
    {
        private IFish fish;
        private IExitable exitBehaviour;
        private IBoundsService bounds;
        private Transform fishTransform;

        private ExitScreenContext context;
        private ExitScreenState exitScreenState;

        private const float SPEED = 1.0f;

        [SetUp]
        public void Setup()
        {
            var go = new GameObject("Fish");
            fishTransform = go.transform;
            fishTransform.transform.position = Vector3.zero;

            fish = Substitute.For<IFish>();
            fish.GetTransform().Returns(fishTransform);

            bounds = Substitute.For<IBoundsService>();
            bounds.GetMinBounds().Returns(Vector2.zero);
            bounds.GetMaxBounds().Returns(new Vector2(10, 10));

            exitBehaviour = Substitute.For<IExitable>();

            context = new ExitScreenContext(fish.GetTransform(), bounds, exitBehaviour, SPEED);

            
        }
        [TestCase(0, -1f, 0f)] // 0 → izquierda
        [TestCase(1, 1f, 0f)] // 1 → derecha
        public void Enter_ThenMoveExitDirection(int fakeIndex, float expectedX, float expectedY)
        {
            Func<int, int, int> fakeRandom = (min, max) => fakeIndex;

            exitScreenState = new ExitScreenState(context, fakeRandom);

            exitScreenState.Enter();
            exitScreenState.Update();

            exitBehaviour.Received(1).MoveInDirection(context.Transform,
                Arg.Is<Vector2>(v => Mathf.Approximately(v.x, expectedX) && Mathf.Approximately(v.y, expectedY)),
                context.Speed);
        }

        [Test]
        public void Exit_ShouldNotThrow()
        {
            var state = new ExitScreenState(context);
            Assert.DoesNotThrow(() => state.Exit());
        }

        [Test]
        public void Update_WhenOutOfBounds_ShouldNotifyExited()
        {
            var state = new ExitScreenState(context);
            state.Enter();

            exitBehaviour.IsOutOfBounds(fish.GetTransform(), context.BoundsService).Returns(true);
            state.Update();

            exitBehaviour.Received(1).NotifyExited();
        }
    }
}
