using Game.Data;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using ILogger = Game.Utilities.ILogger;

namespace Game.Services.Tests
{

    public class EnviromentSystemTests
    {
        private GameObject go;
        private EnviromentSystem envSystem;
        private IEnviromentLoader loaderMock;
        private ILogger loggerMock;
        private Sprite dummySprite;

        private class EnvSetupData
        {
            public GameObject SpawnGO { get; }
            public GameObject PrefabGO { get; }
            public GroundEnvironmentDayConfig GroundConfig { get; }
            public FishEnvDayConfig FishConfig { get; }
            public FoodEnvDayConfig FoodConfig { get; }
            public AudioEnvDayConfig AudioConfig { get; }

            public EnvSetupData(GameObject spawnGO, GameObject prefabGO,
                GroundEnvironmentDayConfig groundConfig, FishEnvDayConfig fishConfig,
                FoodEnvDayConfig foodConfig, AudioEnvDayConfig audioConfig)
            {
                SpawnGO = spawnGO;
                PrefabGO = prefabGO;
                GroundConfig = groundConfig;
                FishConfig = fishConfig;
                FoodConfig = foodConfig;
                AudioConfig = audioConfig;
            }
        }

        [SetUp]
        public void SetUp()
        {
            go = new GameObject();
            envSystem = go.AddComponent<EnviromentSystem>();

            loggerMock = Substitute.For<ILogger>();
            loaderMock = Substitute.For<IEnviromentLoader>();

            dummySprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero);

            envSystem.Initialize(
                new List<Transform>(),
                new List<GroundEnvironmentDayConfig>(),
                new List<FishEnvDayConfig>(),
                new List<FoodEnvDayConfig>(),
                new List<AudioEnvDayConfig>(),
                loaderMock,
                loggerMock
            );
            envSystem.SetLoaderService(loaderMock);
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(go);
        }


        [UnityTest]
        public System.Collections.IEnumerator LoadEnviromentData_InstantiatesGroundPrefabsAndAssignsSprites()
        {
            var currentDayConfig = 1;
            var setup = CreateDayConfigsAndSpawnPos(currentDayConfig);
            var expectedContext = SetupLoaderMock(setup, currentDayConfig);

            var context = envSystem.LoadEnviromentData(currentDayConfig);
            yield return null;

            var instantiated = GameObject.Find("Prefab(Clone)");
            Assert.IsNotNull(instantiated);
            Assert.AreEqual(setup.SpawnGO.transform.position, instantiated.transform.position);
            Assert.AreEqual(dummySprite, instantiated.GetComponent<SpriteRenderer>().sprite);
            Assert.AreEqual(expectedContext, context);

        }

        [UnityTest]
        public System.Collections.IEnumerator LoadEnviromentData_LoadsFoodConfigAndAssignsSprites()
        {
            var currentDayConfig = 2;
            var setup = CreateDayConfigsAndSpawnPos(currentDayConfig);
            var expectedContext = SetupLoaderMock(setup, currentDayConfig);

            var context = envSystem.LoadEnviromentData(currentDayConfig);
            yield return null;

            Assert.AreEqual(dummySprite, setup.PrefabGO.GetComponent<SpriteRenderer>().sprite);
        }

        [UnityTest]
        public System.Collections.IEnumerator LoadEnviromentData_LoadsFishConfigWithoutErrors()
        {
            var currentDayConfig = 3;
            var setup = CreateDayConfigsAndSpawnPos(currentDayConfig);
            var expectedContext = SetupLoaderMock(setup, currentDayConfig);

            var context = envSystem.LoadEnviromentData(currentDayConfig);
            yield return null;

            Assert.AreEqual(expectedContext, context);
        }

        [UnityTest]
        public System.Collections.IEnumerator LoadEnviromentData_LoadsAudioConfigWithoutErrors()
        {
            var currentDayConfig = 4;
            var setup = CreateDayConfigsAndSpawnPos(currentDayConfig);
            var expectedContext = SetupLoaderMock(setup, currentDayConfig);

            var context = envSystem.LoadEnviromentData(currentDayConfig);
            yield return null;

            Assert.AreEqual(expectedContext, context);
        }

        [UnityTest]
        public System.Collections.IEnumerator LoadEnviromentData_UsesLastDayConfigIfNotFound()
        {
            var currentDayConfig = 1;
            var loaderDayConfig = 5;
            var setup = CreateDayConfigsAndSpawnPos(currentDayConfig);
            var expectedContext = SetupLoaderMock(setup, loaderDayConfig);

            var context = envSystem.LoadEnviromentData(loaderDayConfig);
            yield return null;

            Assert.AreEqual(expectedContext, context);
        }

        private EnvSetupData CreateDayConfigsAndSpawnPos(int dayNumberConfig)
        {
            var spawnGO = new GameObject("SpawnPos");
            var prefabGO = new GameObject("Prefab");
            prefabGO.AddComponent<SpriteRenderer>();

            var groundConfig = ScriptableObject.CreateInstance<GroundEnvConfig>();
            groundConfig.prefab = prefabGO;
            groundConfig.sprite = dummySprite;

            var fishConfig = ScriptableObject.CreateInstance<FishConfig>();
            fishConfig.fishSprite = dummySprite;

            var foodConfig = ScriptableObject.CreateInstance<FoodEnvConfig>();
            foodConfig.sprite = dummySprite;
            foodConfig.prefab = prefabGO;

            var audioConfig = ScriptableObject.CreateInstance<AudioEnvConfig>();

            var groundDayConfig = ScriptableObject.CreateInstance<GroundEnvironmentDayConfig>();
            groundDayConfig.dayNumber = dayNumberConfig;
            groundDayConfig.groundEnvConfigs = new List<GroundEnvConfig> { groundConfig };

            var fishDayConfig = ScriptableObject.CreateInstance<FishEnvDayConfig>();
            fishDayConfig.dayNumber = dayNumberConfig;
            fishDayConfig.fishEnvDayConfigs = new List<FishConfig> { fishConfig };

            var audioDayConfig = ScriptableObject.CreateInstance<AudioEnvDayConfig>();
            audioDayConfig.dayNumber = dayNumberConfig;
            audioDayConfig.audioConfigs = new List<AudioEnvConfig> { audioConfig };

            var foodDayConfig = ScriptableObject.CreateInstance<FoodEnvDayConfig>();
            foodDayConfig.dayNumber = dayNumberConfig;
            foodDayConfig.foodEnvConfigs = new List<FoodEnvConfig> { foodConfig };

            return new EnvSetupData(spawnGO,prefabGO,groundDayConfig, fishDayConfig,foodDayConfig,audioDayConfig);
        }

        private LoadDataContext SetupLoaderMock(EnvSetupData setup, int dayNumber)
        {
            envSystem.Initialize(
                new List<Transform> { setup.SpawnGO.transform },
                new List<GroundEnvironmentDayConfig> { setup.GroundConfig },
                new List<FishEnvDayConfig> { setup.FishConfig },
                new List<FoodEnvDayConfig> { setup.FoodConfig },
                new List<AudioEnvDayConfig> { setup.AudioConfig },
                loaderMock,
                loggerMock
            );

            var loadDataContext = new LoadDataContext.Builder().Build();

            var loadResult = new EnvironmentLoadResult
            {
                SelectedGroundConfig = setup.GroundConfig,
                SelectedAudioConfig = setup.AudioConfig,
                SelectedFishConfig = setup.FishConfig,
                SelectedFoodConfig = setup.FoodConfig,
                Context = loadDataContext
            };

            loaderMock.Load(
                Arg.Any<List<GroundEnvironmentDayConfig>>(),
                Arg.Any<List<FishEnvDayConfig>>(),
                Arg.Any<List<FoodEnvDayConfig>>(),
                Arg.Any<List<AudioEnvDayConfig>>(),
                dayNumber
            ).Returns(loadResult);

            return loadDataContext;
        }
    }
}


