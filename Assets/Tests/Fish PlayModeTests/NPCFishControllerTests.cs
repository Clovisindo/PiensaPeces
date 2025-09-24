using Game.Components;
using Game.Data;
using Game.Events;
using Game.FishLogic;
using Game.Services;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Fishes.Tests
{
    public class NPCFishControllerTests
    {
        private GameObject fishGO;
        private NPCFishController controller;
        private NPCFishPool pool;
        private IFishStateFactory fishStateFactory;
        private IBoundsService boundsService;
        private NPCFishDependencies dependencies;
        private FishConfig config;
        private IGlobal globalMock;

        [SetUp]
        public void Setup()
        {
            fishGO = new GameObject("NPCFish");
            fishGO.AddComponent<SpriteRenderer>();
            fishGO.AddComponent<TransformLimiter>();
            fishGO.AddComponent<FishTalker>();

            controller = fishGO.AddComponent<NPCFishController>();

            pool = Substitute.For<NPCFishPool>();
            fishStateFactory = Substitute.For<IFishStateFactory>();
            boundsService = Substitute.For<IBoundsService>();
            globalMock = Substitute.For<IGlobal>();
            globalMock.GameSpeed.Returns(1f);

            dependencies = TestHelpers.CreateDefaultDependencies(
                monoBehaviour: controller,
                fishStateFactory: fishStateFactory,
                boundsService: boundsService,
                global: globalMock);

            var tex = new Texture2D(10, 10);
            var sprite = Sprite.Create(tex, new Rect(0, 0, 10, 10), Vector2.zero);
            config = TestHelpers.CreateConfig(maxLifetime: 5f, speed: 2f, fishSprite: sprite);

            controller.Init(config, pool, dependencies, 0);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(fishGO);
        }

        [UnityTest]
        public IEnumerator Init_AssignsSpriteAndDependencies()
        {
            var renderer = fishGO.GetComponent<SpriteRenderer>();
            Assert.AreEqual(renderer.sprite, config.fishSprite);
            var limiter = fishGO.GetComponent<TransformLimiter>();
            Assert.IsTrue(limiter.enabled);
            yield return null;
        }

        [UnityTest]
        public IEnumerator NotifyExit_CallsPoolRecyle()
        {
            controller.NotifyExit();

            pool.Received(1).RecycleFish(controller);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ResetFish_ResetsLifeTime()
        {
            controller.ResetFish();

            Assert.That(controller.GetLifeTime(), Is.EqualTo(0));
            yield return null;
        }

        [UnityTest]
        public IEnumerator Update_IncrementsLifeTime()
        {
            float before = controller.GetLifeTime();
            yield return null;
            float after = controller.GetLifeTime();

            Assert.That(after, Is.GreaterThan(before));
        }
    }



    /// <summary>
    /// Helpers para construir configs y dependencias falsas.
    /// </summary>
    public static class TestHelpers
    {
        public static FishConfig CreateConfig(float maxLifetime = 5f, float speed = 1f, Sprite fishSprite = null)
        {
            return new FishConfig
            {
                maxLifetime = maxLifetime,
                speed = speed,
                fishSprite = fishSprite,
                intervalEvaluateIntent = 0.5f
            };
        }
        public static NPCFishDependencies CreateDefaultDependencies(
            MonoBehaviour monoBehaviour,
            IFishStateFactory fishStateFactory = null,
            IBoundsService boundsService = null,
            IGlobal global = null)
        {
            return new NPCFishDependencies(
                monoBehaviour,
                fishStateFactory ?? Substitute.For<IFishStateFactory>(),
                boundsService ?? Substitute.For<IBoundsService>(),
                resourceLoader: Substitute.For<IResourceLoader>(),
                global: Substitute.For<IGlobal>(),
                sfxEventBus: Substitute.For<EventBus<SFXEvent>>()
            );
        }
    }
}
