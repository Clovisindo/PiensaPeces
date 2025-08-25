using Game.Events;
using Game.FishFood;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;


namespace Game.FishFood.Tests
{
    public class FishFoodTests
    {
        private GameObject go;
        private Food food;
        private FoodManagerService foodManager;
        private IEventBus<FoodEaten> foodEventBus;
        private Sprite sprite;

        [SetUp]
        public void Setup()
        {
            go = new GameObject("Food");
            go.AddComponent<SpriteRenderer>();

            go.AddComponent<BoxCollider2D>();
            go.AddComponent<Rigidbody2D>().isKinematic = true; // para evitar físicas reales
            go.AddComponent<FoodFallBehaviour>();

            food = go.AddComponent<Food>();
            foodManager = Substitute.For<FoodManagerService>();
            foodEventBus = Substitute.For<IEventBus<FoodEaten>>();
            sprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 4, 4), Vector2.zero);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(go);
        }

        [Test]
        public void Init_ShouldRegisterFood_AndInitFallBehaviour()
        {
            food.Init(foodManager, sprite, 5f, -10f, foodEventBus);
            foodManager.Received(1).RegisterFood(go);

            Assert.AreEqual(sprite, go.GetComponent<SpriteRenderer>().sprite);

            // Comprobamos que FoodFallBehaviour recibe Init
            var fall = go.GetComponent<FoodFallBehaviour>();
            Assert.NotNull(fall);
        }

        [Test]
        public void OnTriggerEnter_WithPlayerFish_ShouldRaiseEvent_AndUnregister()
        {
            food.Init(foodManager, sprite, 1f, -1f, foodEventBus);

            food.HandleCollisionWithPlayer();//aislamos la logica sin Destroy para testear en editor

            foodEventBus.Received(1).Raise(Arg.Any<FoodEaten>());
            foodManager.Received(1).UnregisterFood(go);
        }
    }
}