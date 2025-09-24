using Game.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Fishes.Tests
{
    public class NPCFishIntentSchedulerTests
    {
        private NPCFishIntentScheduler _scheduler;
        private ICoroutineRunner _mockCoroutineRunner;
        private IYieldInstruction _mockYieldInstruction;
        private FishConfig _fishConfig;
        private Func<FishIntent> _mockEvaluateIntent;
        private Action<FishIntent> _mockApplyIntent;


        [SetUp]
        public void Setup()
        {
            _mockCoroutineRunner = Substitute.For<ICoroutineRunner>();
            _mockYieldInstruction = Substitute.For<IYieldInstruction>();
            _mockEvaluateIntent = Substitute.For<Func<FishIntent>>();
            _mockApplyIntent = Substitute.For<Action<FishIntent>>();

            _fishConfig = ScriptableObject.CreateInstance<FishConfig>();
            _fishConfig.intervalEvaluateIntent = 0.1f;
            _mockYieldInstruction = new UnityYieldInstruction();

            _mockEvaluateIntent().Returns(FishIntent.SwimRandomly);

            _scheduler = new NPCFishIntentScheduler(
                _mockCoroutineRunner,
                _mockYieldInstruction,
                _fishConfig,
                _mockEvaluateIntent,
                _mockApplyIntent
                );
        }

        [Test]
        public void StartEvaluatingPeriodically_ShouldStopCurrentCoroutineFirst()
        {
            _scheduler.StartEvaluatingPeriodically();

            _mockCoroutineRunner.Received(1).StopCurrentDisplayCoroutine();
        }

        [Test]
        public void StartEvaluatingPeriodically_ShouldStartNewCoroutine()
        {
            _scheduler.StartEvaluatingPeriodically();

            _mockCoroutineRunner.Received(1).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }

        [Test]
        public void StartEvaluatingPeriodically_ShouldCallStopAndThenStart()
        {
            _scheduler.StartEvaluatingPeriodically();

            Received.InOrder(() =>
            {
                _mockCoroutineRunner.Received(1).StopCurrentDisplayCoroutine();
                _mockCoroutineRunner.Received(1).StartDisplayCoroutine(Arg.Any<IEnumerator>());
            });
        }

        [Test]
        public void EvaluateNow_ShouldCallEvaluateIntentOnce()
        {
            _scheduler.EvaluateNow();

            _mockEvaluateIntent.Received(1).Invoke();
        }

        [Test]
        public void EvaluateNow_ShouldApplyEvaluateIntent()
        {
            _scheduler.EvaluateNow();

            _mockApplyIntent.Received(1).Invoke(FishIntent.SwimRandomly);
        }

        [TestCase(FishIntent.Idle, TestName = "EvaluateWithIdle")]
        [TestCase(FishIntent.SwimRandomly, TestName = "EvaluateWithSwimRandomly")]
        [TestCase(FishIntent.FollowFood, TestName = "EvaluateWithFollowFood")]
        [TestCase(FishIntent.EndLife, TestName = "EvaluateWithEndLife")]
        [Test]
        public void EvaluateNow_WithDifferentIntents_ShouldApplyCorrectIntent(FishIntent expectedIntent)
        {
            _mockEvaluateIntent().Returns(expectedIntent);
            _mockApplyIntent.ClearReceivedCalls();

            _scheduler.EvaluateNow();

            _mockApplyIntent.Received(1).Invoke(expectedIntent);
        }

        [Test]
        public void Stop_ShouldStopCurrentCoroutine()
        {
            _scheduler.Stop();

            _mockCoroutineRunner.Received(1).StopCurrentDisplayCoroutine();
        }

        [Test]
        public void Constructor_WithNullCoroutineRunner_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new NPCFishIntentScheduler(
                    null,
                    _mockYieldInstruction,
                    _fishConfig,
                    _mockEvaluateIntent,
                    _mockApplyIntent
                )
            );
        }

        [Test]
        public void EvaluatePeriodically_WhenExecutedManually_ShouldApplyIntent()
        {
            IEnumerator capturedCoroutine = null;
            _mockCoroutineRunner
                .When(x => x.StartDisplayCoroutine(Arg.Any<IEnumerator>()))
                .Do(x => capturedCoroutine = x.Arg<IEnumerator>());

            _scheduler.StartEvaluatingPeriodically();

            // Simular ejecución manual de la corrutina
            Assert.IsNotNull(capturedCoroutine);
            capturedCoroutine.MoveNext(); // Inicia el while(true)
            capturedCoroutine.MoveNext(); // Ejecuta después del yield return

            _mockEvaluateIntent.Received(1).Invoke();
            _mockApplyIntent.Received(1).Invoke(FishIntent.SwimRandomly);
        }

        [Test]
        public void EvaluatePeriodically_ShouldUseCorrectInterval()
        {
            IEnumerator capturedCoroutine = null;
            _mockCoroutineRunner
                .When(x => x.StartDisplayCoroutine(Arg.Any<IEnumerator>()))
                .Do(x => capturedCoroutine = x.Arg<IEnumerator>());

            _scheduler.StartEvaluatingPeriodically();

            Assert.IsNotNull(capturedCoroutine);

            // Verificar que se usa el YieldInstruction correcto
            capturedCoroutine.MoveNext(); // Primera iteración del while
            var currentYield = capturedCoroutine.Current;

            Assert.IsInstanceOf<WaitForSeconds>(currentYield);
        }

        [Test]
        public void EvaluatePeriodically_ShouldRunInLoop()
        {
            IEnumerator capturedCoroutine = null;
            _mockCoroutineRunner
                .When(x => x.StartDisplayCoroutine(Arg.Any<IEnumerator>()))
                .Do(x => capturedCoroutine = x.Arg<IEnumerator>());

            _scheduler.StartEvaluatingPeriodically();

            Assert.IsNotNull(capturedCoroutine);

            // Primera iteración
            capturedCoroutine.MoveNext(); // yield return WaitForSeconds
            capturedCoroutine.MoveNext(); // después del yield, ejecuta applyIntent

            // Segunda iteración
            capturedCoroutine.MoveNext(); // yield return WaitForSeconds nuevamente
            capturedCoroutine.MoveNext(); // después del yield, ejecuta applyIntent otra vez

            // Assert - debería haberse llamado 3 veces tal como funciona moveNext aqui
            _mockApplyIntent.Received(3)(FishIntent.SwimRandomly);
        }

        [Test]
        public void StartEvaluatingPeriodically_CalledTwice_ShouldStopPreviousCoroutine()
        {
            _scheduler.StartEvaluatingPeriodically();
            _scheduler.StartEvaluatingPeriodically();

            _mockCoroutineRunner.Received(2).StopCurrentDisplayCoroutine();
            _mockCoroutineRunner.Received(2).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }
    }
}
            
