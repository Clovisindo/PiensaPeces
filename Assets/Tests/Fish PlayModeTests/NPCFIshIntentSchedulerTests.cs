using Game.Data;
using Game.Fishes;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Fish.Tests
{
    public class NPCFIshIntentSchedulerTests
    {
        private MonoBehaviour _testMonoBehaviour;
        private NPCFishIntentScheduler _scheduler;
        private FishConfig _config;
        private FishIntent _lastAppliedIntent;
        private int _applyCallCount;

        [SetUp]
        public void Setup()
        {
            var gameObject = new GameObject("FishIntent");
            _testMonoBehaviour = gameObject.AddComponent<TestMonoBehaviour>();

            _config = new FishConfig { intervalEvaluateIntent = 0.1f };
            _lastAppliedIntent = FishIntent.Idle;
            _applyCallCount = 0;

            _scheduler = new NPCFishIntentScheduler(
                new UnityCoroutineRunner(_testMonoBehaviour),
                new UnityYieldInstruction(),
                _config,
                EvaluateFishIntent,
                ApplyFishIntent
            );
        }

        private FishIntent EvaluateFishIntent()
        {
            // Lógica simple de ejemplo: rotar entre diferentes estados
            return _applyCallCount switch
            {
                0 => FishIntent.SwimRandomly,
                1 => FishIntent.FollowFood,
                2 => FishIntent.Idle,
                3 => FishIntent.EndLife,
                _ => FishIntent.SwimRandomly
            };
        }

        private void ApplyFishIntent(FishIntent intent)
        {
            _lastAppliedIntent = intent;
            _applyCallCount++;

            // Aquí irían las acciones reales del pez según el intent
            switch (intent)
            {
                case FishIntent.Idle:
                    Debug.Log("Fish is idle");
                    break;
                case FishIntent.SwimRandomly:
                    Debug.Log("Fish is swimming randomly");
                    break;
                case FishIntent.FollowFood:
                    Debug.Log("Fish is following food");
                    break;
                case FishIntent.EndLife:
                    Debug.Log("Fish life ended");
                    break;
            }
        }

        [UnityTest]
        public IEnumerator IntegrationTest_ShouldExecutePeriodicEvaluationWithDifferentIntents()
        {
            _scheduler.StartEvaluatingPeriodically();

            yield return new WaitForSeconds(0.5f);

            Assert.Greater(_applyCallCount, 2, "Should have executed at least 3 evaluations");
            Assert.IsTrue(Enum.IsDefined(typeof(FishIntent), _lastAppliedIntent), "Should have a valid FishIntent");

            _scheduler.Stop();
        }

        [UnityTest]
        public IEnumerator EvaluateNow_ShouldExecuteImmediately()
        {
            _scheduler.EvaluateNow();
            yield return null;

            Assert.AreEqual(1, _applyCallCount);
            Assert.AreEqual(FishIntent.SwimRandomly, _lastAppliedIntent);
        }

        [UnityTest]
        public IEnumerator Stop_ShouldStopPeriodicEvaluation()
        {
            _scheduler.StartEvaluatingPeriodically();
            yield return null;
            var callCountBeforeStop = _applyCallCount;

            _scheduler.Stop();
            yield return null;

            Assert.AreEqual(callCountBeforeStop, _applyCallCount, "Should not execute more intents after Stop()");
        }

        [UnityTest]
        public IEnumerator StartEvaluatingPeriodically_CalledMultipleTimes_ShouldNotCreateMultipleCoroutines()
        {
            _scheduler.StartEvaluatingPeriodically();
            yield return new WaitForSeconds(0.5f);
            var callCountAfterFirst = _applyCallCount;

            _scheduler.StartEvaluatingPeriodically();
            yield return new WaitForSeconds(0.5f);
            var callCountAfterSecond = _applyCallCount;

            Assert.Greater(callCountAfterSecond, callCountAfterFirst, "Should continue executing after restart");

            _scheduler.Stop();
        }

        [TearDown]
        public void TearDown()
        {
            _scheduler?.Stop();
            if (_testMonoBehaviour?.gameObject != null)
                UnityEngine.Object.DestroyImmediate(_testMonoBehaviour.gameObject);
        }
    }

    // Helper MonoBehaviour para tests de integración
    public class TestMonoBehaviour : MonoBehaviour
    {
        // Clase vacía solo para poder usar StartCoroutine/StopCoroutine
    }
}
