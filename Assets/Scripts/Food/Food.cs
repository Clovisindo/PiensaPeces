using Game.Events;
using UnityEngine;

namespace Game.FishFood
{
    public class Food : MonoBehaviour
    {
        [SerializeField] FoodFallBehaviour foodFallBehaviour;

        private FoodManagerService foodManager;
        private IEventBus<FoodEaten> foodEatentBus;

        public void Init(FoodManagerService foodManager, Sprite foodSprite, float foodFallSpeed, float minY, IEventBus<FoodEaten> foodEatentBus)
        {
            this.foodManager = foodManager;
            this.foodEatentBus = foodEatentBus;

            this.GetComponent<SpriteRenderer>().sprite = foodSprite;

            foodManager.RegisterFood(gameObject);
            foodFallBehaviour.Init(foodFallSpeed, minY);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Colisión con: " + other.name);
            if (other.CompareTag("PlayerFish"))
            {
                foodEatentBus.Raise(new FoodEaten());
                foodManager.UnregisterFood(gameObject);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            foodManager?.UnregisterFood(gameObject);
        }
    }
}