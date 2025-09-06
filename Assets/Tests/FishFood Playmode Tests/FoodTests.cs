using Game.Events;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.FishFood.Tests
{
    public class FoodTests
    {
        private GameObject go;
        private GameObject player;
        private BoxCollider2D collider;
        private SpriteRenderer sprite;
        private Food food;
        private FoodFallBehaviour fallFood;
        private FoodManagerService foodManager;
        private IEventBus<FoodEaten> bus;

        [TearDown]
        public void TearDown()
        {
            if (go != null)
                Object.DestroyImmediate(go);
            if (player != null)
                Object.DestroyImmediate(player);
        }

        [UnityTest]
        public IEnumerator Food_OnTriggerEnter_WithPlayerFish_ShouldDestroyFood()
        {
            go = new GameObject("Food");
            sprite = go.AddComponent<SpriteRenderer>();
            go.AddComponent<FoodFallBehaviour>();
            food = go.AddComponent<Food>();

            foodManager = Substitute.For<FoodManagerService>();
            bus = Substitute.For<IEventBus<FoodEaten>>();

            food.Init(foodManager, sprite.sprite, 1f, -5f, bus);

            player = new GameObject("PlayerFish");
            player.tag = "PlayerFish";
            collider = player.AddComponent<BoxCollider2D>();

            food.SendMessage("OnTriggerEnter2D", collider);

            yield return null; // dejar que Destroy se ejecute

            Assert.IsTrue(food == null);
        }

        [UnityTest]
        public IEnumerator FoodFallBehaviour_WhenReachMinY_ThenDestroy()
        {
            go = new GameObject("Food");
            sprite = go.AddComponent<SpriteRenderer>();
            fallFood = go.AddComponent<FoodFallBehaviour>();

            fallFood.Init(1f, -10f);

            go.transform.position = new Vector3(0, -11f);
            yield return null;

            Assert.IsTrue(fallFood == null);
        }
    }
}