using Game.Events;
using Game.FishFood;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.FishFood.Tests
{
    public class FoodTests
    {
        [UnityTest]
        public IEnumerator Food_OnTriggerEnter_WithPlayerFish_ShouldDestroyFood()
        {
            var go = new GameObject("Food");
            var sprite = go.AddComponent<SpriteRenderer>();
            go.AddComponent<FoodFallBehaviour>();
            var food = go.AddComponent<Food>();

            var foodManager = Substitute.For<FoodManagerService>();
            var bus = Substitute.For<IEventBus<FoodEaten>>();

            food.Init(foodManager, sprite.sprite, 1f, -5f, bus);

            var player = new GameObject("PlayerFish");
            player.tag = "PlayerFish";
            var collider = player.AddComponent<BoxCollider2D>();

            food.SendMessage("OnTriggerEnter2D", collider);

            yield return null; // dejar que Destroy se ejecute

            Assert.IsTrue(food == null);
        }

        [UnityTest]
        public IEnumerator FoodFallBehaviour_WhenReachMinY_ThenDestroy()
        {
            var go = new GameObject("Food");
            var sprite = go.AddComponent<SpriteRenderer>();
            var fallFood = go.AddComponent<FoodFallBehaviour>();

            fallFood.Init(1f, -10f);

            go.transform.position = new Vector3(0, -11f);
            yield return null;

            Assert.IsTrue(fallFood == null);
        }
    }
}