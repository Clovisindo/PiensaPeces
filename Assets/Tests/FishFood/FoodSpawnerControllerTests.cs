using Game.Data;
using Game.Events;
using Game.FishFood;
using Game.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;

namespace Game.FishFood.Tests
{
    public class FoodSpawnerControllerTests
    {
        private FoodSpawnerController spawner;
        private GameObject fakeFood;
        private IBoundsService bounds;
        private FoodManagerService foodManager;
        private FoodEnvConfig foodConfig;
        private IEventBus<FoodEaten> eatenBus;
        private IEventBus<FoodSpawned> spawnedBus;
        Func<GameObject, Vector3, Quaternion, GameObject> fakeFactory;


        [SetUp]
        public void Setup()
        {
            var texture = new Texture2D(32, 32);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.zero);
            fakeFood = new GameObject("FakeFood");
            fakeFood.AddComponent<Food>();
            fakeFood.AddComponent<SpriteRenderer>();
            fakeFood.AddComponent<FoodFallBehaviour>();


            fakeFactory = (prefab, pos, rot) => fakeFood;
            spawner = new GameObject().AddComponent<FoodSpawnerController>();

            bounds = Substitute.For<IBoundsService>();
            bounds.GetMinBounds().Returns(Vector2.zero);
            bounds.GetMaxBounds().Returns(new Vector2(10, 10));

            foodManager = Substitute.For<FoodManagerService>();
            eatenBus = Substitute.For<IEventBus<FoodEaten>>();
            spawnedBus = Substitute.For<IEventBus<FoodSpawned>>();

            foodConfig = ScriptableObject.CreateInstance<FoodEnvConfig>();
            foodConfig.prefab = new GameObject("Prefab");
        }

        [Test]
        public void SpawnFood_ShouldRiseFoodSpawnedEvent()
        {
            spawner.Init(bounds, foodManager, new[] { foodConfig }, eatenBus, spawnedBus, fakeFactory);

            spawner.SpawnFood();

            spawnedBus.Received().Raise(Arg.Is<FoodSpawned>(e => e.food == fakeFood));

        }
    }
}
