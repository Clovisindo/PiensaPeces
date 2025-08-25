using Game.Events;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("FishFood.Tests")]
namespace Game.FishFood
{
    public class Food : MonoBehaviour
    {
        FoodFallBehaviour foodFallBehaviour;

        private FoodManagerService foodManager;
        private IEventBus<FoodEaten> foodEatentBus;

        public void Init(FoodManagerService foodManager, Sprite foodSprite, float foodFallSpeed, float minY, IEventBus<FoodEaten> foodEatentBus)
        {
            this.foodManager = foodManager;
            this.foodEatentBus = foodEatentBus;

            this.GetComponent<SpriteRenderer>().sprite = foodSprite;

            foodManager.RegisterFood(gameObject);
            foodFallBehaviour = this.gameObject.GetComponent<FoodFallBehaviour>();
            foodFallBehaviour.Init(foodFallSpeed, minY);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Colisión con: " + other.name);
            if (other.CompareTag("PlayerFish"))
            {
                HandleCollisionWithPlayer();
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Ejemplo de como se haria "mal" aplicando internal y visibilidad en assembly, en vez de extraer todo
        /// a una clase con solo logica y testear, pero como aqui solo es un metodo, dejamos esto
        /// de ejemplo para comparar
        /// </summary>
        internal void HandleCollisionWithPlayer()
        {
            foodEatentBus.Raise(new FoodEaten());
            foodManager.UnregisterFood(gameObject);
        }

        private void OnDestroy()
        {
            foodManager?.UnregisterFood(gameObject);
        }
    }
}