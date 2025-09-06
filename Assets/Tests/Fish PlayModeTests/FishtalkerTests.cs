using Assets.Scripts.Fish.Dialogue;
using Game.Data;
using Game.Events;
using Game.Fishes;
using Game.Services;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Fish.Tests
{
    public class FishtalkerTests : MonoBehaviour
    {
        private GameObject testGameObject;
        private FishTalker fishTalker;
        private FishTalkerDependencies mockDependencies;
        private GameObject mockSpeechBubblePrefab;
        private Transform bubbleAnchor;
        // Mocks
        private IRandomService mockRandomService;
        private ITimeService mockTimeService;
        private IResourceLoader mockResourceLoader;
        private IGameObjectFactory mockGameObjectFactory;
        private IEventBus<SFXEvent> mockEventBus;
        private IDialogueEvaluator mockEvaluator;
        private IDialoguePathResolver mockPathResolver;
        private FishConfig fishConfig;
        private IGlobal mockGlobal;

        private const float minTimeInterval = 1f;// Intervalo acotado entre los valores de update, para llamar solo 1 vez
        private const float maxTimeInterval = 1.5f;
        private const float randomInterval = 2.0f;
        private const int passedDays = 1;
        private const string playerPath = "test_player";
        private const string npcPath = "test_npc";


        [SetUp]
        public void Setup()
        {
            //crear gameobject fishtalker
            testGameObject = new GameObject("TestFishTalker");
            fishTalker = testGameObject.AddComponent<FishTalker>();
            //crear anchors burbujas
            var anchorGO = new GameObject("BubbleAnchor");
            anchorGO.transform.SetParent(testGameObject.transform);
            bubbleAnchor = anchorGO.transform;
            //prefab speechBubble
            mockSpeechBubblePrefab = new GameObject("SpeechBubblePrefabs");
            mockSpeechBubblePrefab.AddComponent<SpeechBubbleUI>().InitForTests();

            SetupMockDependencies();
        }

        private void SetupMockDependencies()
        {
            //mocks
            mockRandomService = Substitute.For<IRandomService>();
            mockTimeService = Substitute.For<ITimeService>();
            mockResourceLoader = Substitute.For<IResourceLoader>();
            mockGameObjectFactory = Substitute.For<IGameObjectFactory>();
            mockEventBus = Substitute.For<IEventBus<SFXEvent>>();
            mockEvaluator = Substitute.For<IDialogueEvaluator>();
            mockPathResolver = Substitute.For<IDialoguePathResolver>();
            fishConfig =ScriptableObject.CreateInstance<FishConfig>();
            mockGlobal = Substitute.For<IGlobal>();

            //configurar comportamientos basicos
            mockRandomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(randomInterval);
            mockRandomService.Range(Arg.Any<int>(), Arg.Any<int>()).Returns(0);
            mockTimeService.DeltaTime.Returns(0.016f); // ~60fps
            mockPathResolver.Resolver(Arg.Any<IDialogueEvaluator>(), Arg.Any<string>(), Arg.Any<string>()).Returns("test_dialogue");
            fishConfig.sftTalk = ScriptableObject.CreateInstance<AudioEmitterData>();
            fishConfig.intervalTalking = 1.0f;
            mockGlobal.GameSpeed.Returns(1.0f);

            //csv
            var csvContent = "id;text;condicion\n1;Hello from fake day1;true";
            var TextAsset = new TextAsset(csvContent);
            mockResourceLoader.LoadText(Arg.Any<string>()).Returns(TextAsset);

            mockDependencies = new FishTalkerDependencies(mockEvaluator,
                mockPathResolver, mockResourceLoader, mockGameObjectFactory,
                mockTimeService, mockGlobal, mockEventBus, fishConfig, passedDays);

        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
                Object.DestroyImmediate(testGameObject);
            if (mockSpeechBubblePrefab != null)
                Object.DestroyImmediate(mockSpeechBubblePrefab);
        }

        #region tests de integracion

        /// <summary>
        /// Verifica inicialización completa
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_InitializesCorrectly()
        {
            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                playerPath,
                npcPath,
                1.0f,
                2.0f);
            yield return null;

            mockResourceLoader.Received().LoadText(Arg.Any<string>());
            mockPathResolver.Received().Resolver(mockEvaluator, playerPath, npcPath);
            Assert.That(fishTalker.IsValidConfiguration(),Is.True);
            Assert.That(fishTalker.DialogueLineCount, Is.GreaterThan(0));
        }
        /// <summary>
        /// flujo completo de hablar
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_SpeaksAfterInterval_WhenConditionsMet()
        {
            // Arrange
            mockEvaluator.Evaluate(Arg.Any<string>()).Returns(true);
            mockRandomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(0.1f);
            mockGameObjectFactory.Instantiate(Arg.Any<GameObject>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>())
                .Returns(mockSpeechBubblePrefab);

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                playerPath,
                npcPath,
                minTimeInterval,
                maxTimeInterval
            );
            // Act
            yield return new WaitForSeconds(minTimeInterval);

            // Assert
            mockGameObjectFactory.Received().Instantiate(
                mockSpeechBubblePrefab,
                Arg.Any<Vector3>(),
                Arg.Any<Quaternion>(),
                Arg.Any<Transform>());
            mockEventBus.Received().Raise(Arg.Any<SFXEvent>());

            Object.DestroyImmediate(mockSpeechBubblePrefab);
        }

        /// <summary>
        /// Comportamiento en el tiempo
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_HandlesMultipleUpdates_WithCorrectTiming()
        {
            // Arrange
            mockEvaluator.Evaluate(Arg.Any<string>()).Returns(true);
            mockRandomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(randomInterval);

            var bubbleInstances = new List<GameObject>();
            mockGameObjectFactory.Instantiate(Arg.Any<GameObject>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>())
                .Returns(x => {
                    var instance = new GameObject("MockBubble");
                    instance.AddComponent<SpeechBubbleUI>().InitForTests();
                    bubbleInstances.Add(instance);
                    return instance;
                });

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                playerPath,
                npcPath,
                minTimeInterval,
                maxTimeInterval
            );
            yield return null;

            // Act - Esperar múltiples intervalos
            yield return new WaitForSeconds(minTimeInterval * 2.5f); // Primera vez
            yield return new WaitForSeconds(minTimeInterval * 3.5f); // Segunda vez

            // Assert
            Assert.That(mockGameObjectFactory.ReceivedCalls().Count(call =>
                call.GetMethodInfo().Name == "Instantiate"), Is.AtLeast(2));

            // Cleanup
            foreach (var bubble in bubbleInstances)
            {
                if (bubble != null) Object.DestroyImmediate(bubble);
            }
        }

        #endregion

        #region tests de comportamiento
        /// <summary>
        /// validacion de condiciones
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_DoesNotSpeak_WhenNoValidConditions()
        {
            // Arrange
            mockEvaluator.Evaluate(Arg.Any<string>()).Returns(false); // Todas las condiciones fallan
            mockRandomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(0.1f);

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                playerPath,
                npcPath,
                minTimeInterval,
                maxTimeInterval
            );
            // Act
            yield return new WaitForSeconds(minTimeInterval);

            // Assert
            mockGameObjectFactory.DidNotReceive().Instantiate(Arg.Any<GameObject>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>());
            mockEventBus.DidNotReceive().Raise(Arg.Any<SFXEvent>());
        }

        /// <summary>
        /// Anti solapamiento
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_PreventsOverlappingSpeech_WhenBubbleActive()
        {
            // Arrange
            mockEvaluator.Evaluate(Arg.Any<string>()).Returns(true);
            mockRandomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(0.1f);

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                playerPath,
                npcPath,
                minTimeInterval,
                maxTimeInterval
            );
           

            // Crear burbuja existente DESPUÉS de la inicialización
            var existingBubble = new GameObject("ExistingBubble");
            existingBubble.transform.SetParent(bubbleAnchor);
            existingBubble.AddComponent<SpeechBubbleUI>();

            // Act
            yield return new WaitForSeconds(minTimeInterval);

            // Assert
            mockGameObjectFactory.DidNotReceive().Instantiate(Arg.Any<GameObject>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>());
            Assert.That(fishTalker.HasActiveBubble, Is.True);

            Object.DestroyImmediate(existingBubble);
        }

        /// <summary>
        /// limpieza de burbuja correcta
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_ResetTalker_ClearsActiveBubble()
        {
            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor
            );

            var activeBubble = new GameObject("ActiveBubble");
            activeBubble.transform.SetParent(bubbleAnchor);
            activeBubble.AddComponent<SpeechBubbleUI>();

            // Act
            fishTalker.ResetTalker();
            yield return null;

            // Assert
            mockGameObjectFactory.Received().Destroy(activeBubble);
            Assert.That(fishTalker.HasActiveBubble, Is.False);
        }
        #endregion

        #region tests de carga de datos

        /// <summary>
        /// carga por dias
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_LoadsCorrectDialogue_BasedOnPassedDays()
        {
            // Arrange
            int testDay = 5;
            mockDependencies = new FishTalkerDependencies(mockEvaluator,
                mockPathResolver, mockResourceLoader, mockGameObjectFactory,
                mockTimeService, mockGlobal, mockEventBus, fishConfig, testDay);

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                playerPath,
                npcPath
            );
            yield return null;

            // Assert
            mockResourceLoader.Received().LoadText(Arg.Is<string>(path => path.Contains($"_day{testDay}")));
        }

        /// <summary>
        /// fallback  fichero base
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FishTalker_FallsBackToDefault_WhenDaySpecificNotFound()
        {
            // Arrange
            mockResourceLoader.LoadText(Arg.Is<string>(path => path.Contains("_day"))).Returns((TextAsset)null);

            var defaultCsv = "Id;Text;Condition\n1;Default dialogue;true";
            var mockDefaultAsset = new TextAsset(defaultCsv);
            mockResourceLoader.LoadText(Arg.Is<string>(path => !path.Contains("_day"))).Returns(mockDefaultAsset);

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                "fallback_player",
                "fallback_npc"
            );
            yield return null;

            // Assert
            mockResourceLoader.Received().LoadText(Arg.Is<string>(path => path.Contains("_day")));
            mockResourceLoader.Received().LoadText(Arg.Is<string>(path => !path.Contains("_day")));
            Assert.That(fishTalker.DialogueLineCount, Is.GreaterThan(0));
        }

        #endregion

        #region tests hibridos
        /// <summary>
        /// verificacion logica update cuando no cumple intervalo
        /// </summary>
        [Test]
        public void FishTalker_Tick_ReturnsFalse_WhenTimeNotElapsed()
        {
            // Arrange
            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor
            );
            float shortDelta = 0.001f;

            // Act
            bool result = fishTalker.Tick(shortDelta);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// verificacion logica update cuando SI cumple intervalo
        /// </summary>
        [Test]
        public void FishTalker_Tick_ReturnsTrue_WhenTimeElapsedAndSpeakingOccurs()
        {
            // Arrange
            mockEvaluator.Evaluate(Arg.Any<string>()).Returns(true);
            mockRandomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(0.1f);

            var mockBubbleInstance = new GameObject("MockBubble");
            mockBubbleInstance.AddComponent<SpeechBubbleUI>().InitForTests();
            mockGameObjectFactory.Instantiate(Arg.Any<GameObject>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>())
                .Returns(mockBubbleInstance);

            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
               playerPath,
                npcPath,
               minTimeInterval,
               maxTimeInterval
            );
            // Act
            bool result = fishTalker.Tick(maxTimeInterval); // Tiempo suficiente
            // Assert
            Assert.That(result, Is.True);
            Object.DestroyImmediate(mockBubbleInstance);
        }

        #endregion

        #region test de validaciones

        [Test]
        public void FishTalker_IsValidConfiguration_ReturnsFalse_WhenMissingComponents()
        {
            // Arrange - NO inicializar el FishTalker
            // Act
            bool result = fishTalker.IsValidConfiguration();
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void FishTalker_IsValidConfiguration_ReturnsTrue_WhenProperlyConfigured()
        {
            // Arrange
            fishTalker.InitForTesting(
                mockDependencies,
                mockSpeechBubblePrefab,
                bubbleAnchor,
                "valid_player",
                "valid_npc"
            );

            // Act
            bool result = fishTalker.IsValidConfiguration();

            // Assert
            Assert.That(result, Is.True);
        }

        #endregion
    }
}
