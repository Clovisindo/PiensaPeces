using Game.Events;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Fishes.Tests
{
    public class HungerComponentTests
    {
        private HungerComponent hungerComponent;
        private ICoroutineRunner runner;
        private IYieldInstruction yieldInstruction;
        private IEventBus<HungryEvent> eventBus;



        [SetUp]
        public void Setup()
        {
            runner = Substitute.For<ICoroutineRunner>();
            yieldInstruction = Substitute.For<IYieldInstruction>();
            eventBus = Substitute.For<IEventBus<HungryEvent>>();

            hungerComponent = new GameObject().AddComponent<HungerComponent>();
        }


        [Test]
        public void Init_WithCorrectValues_And_RunCoroutine()
        {
            hungerComponent.Init(eventBus, runner, yieldInstruction);

            runner.Received(1).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }


        [Test]
        public void ResetHunger_ThenStopAndStartCoroutine()
        {
            hungerComponent.Init(eventBus, runner, yieldInstruction);

            hungerComponent.ResetHunger();

            Assert.IsFalse(hungerComponent.IsHungry);
            runner.Received(1).StopCurrentDisplayCoroutine();
            runner.Received(2).StartDisplayCoroutine(Arg.Any<IEnumerator>());//una es del init

        }

        // HungerTimer wait es llamado, is hungry true y se llama al event
        [Test]
        public void HungerTimer_ThenIsHungryTrue_And_EventInvoked()
        {
            var runner = new FakeCoroutineRunner(hungerComponent);//runer real para forzar la ejecucion de coroutine sin test en playmode
            hungerComponent.Init(eventBus, runner, yieldInstruction);

            runner.RunCurrentCoroutine();

            Assert.IsTrue(hungerComponent.IsHungry);
            eventBus.Received(1).Raise(Arg.Any<HungryEvent>());

        }
    }

    public class FakeCoroutineRunner : ICoroutineRunner
    {
        private Coroutine _currentCoroutine;
        private IEnumerator _testRoutine;
        private readonly MonoBehaviour _monoBehaviour;

        public bool HasActiveCoroutine => _testRoutine != null;

        public FakeCoroutineRunner(MonoBehaviour monoBehaviour) => _monoBehaviour = monoBehaviour;

        public Coroutine StartDisplayCoroutine(IEnumerator routine)
        {
            _testRoutine = routine;
            StopCurrentDisplayCoroutine();
            return _currentCoroutine = _monoBehaviour.StartCoroutine(routine);
        }

        public void StopCurrentDisplayCoroutine()
        {
            if (_currentCoroutine != null)
            {
                _monoBehaviour.StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }

        public void RunCurrentCoroutine()
        {
            _monoBehaviour.StartCoroutine(_testRoutine);
        }
    }
}
