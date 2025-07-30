using UnityEngine;
using Game.Events;
using Game.Services;

namespace Game.FishFood
{
    public class FoodSpawnerController : MonoBehaviour
    {
        [SerializeField] private FoodEnvConfig[] foodConfigs;
        [SerializeField] float foodFallSpeed = 1.0f;
        private IBoundsService boundsService;
        private FoodManagerService foodManagerService;
        private IEventBus<FoodEaten> foodEatentBus;
        private IEventBus<FoodSpawned> foodSpawnedBus;


        public void Init(IBoundsService boundsService, FoodManagerService foodManagerService, FoodEnvConfig[] foodPrefabs, EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus)
        {
            this.boundsService = boundsService;
            this.foodManagerService = foodManagerService;
            this.foodConfigs = foodPrefabs;
            this.foodEatentBus = foodEatentEventBus;
            this.foodSpawnedBus = foodSpawnedEventBus;
        }

        public void SpawnFood()
        {
            var min = boundsService.GetMinBounds();
            var max = boundsService.GetMaxBounds();

            float randomX = UnityEngine.Random.Range(min.x, max.x);
            Vector2 spawnPosition = new Vector2(randomX, max.y);

            var actualFoodConfig = foodConfigs[UnityEngine.Random.Range(0, foodConfigs.Length)];
            var food = Instantiate(actualFoodConfig.prefab, spawnPosition, Quaternion.identity);
            food.GetComponent<Food>().Init(foodManagerService, actualFoodConfig.sprite, foodFallSpeed, min.y, foodEatentBus);

            foodSpawnedBus.Raise(new FoodSpawned { food = food });
        }
    }
}
