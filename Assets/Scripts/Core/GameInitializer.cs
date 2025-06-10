using UnityEngine;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;

namespace Assets.Scripts.Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private FishTankMeshScaler fishTankScaler;
        [SerializeField] private PlayerFishController fishPlayer;
        [SerializeField] private FoodSpawnerController spawnerController;
        [SerializeField] private NPCFishPool npcFishPool;
        private FoodManagerService foodManagerService;

        public EventBus<FoodEaten> foodEatentEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();
        public EventBus<HungryEvent> hungryEventBus = new();

        void Awake()
        {
            fishTankScaler.Init();
            var boundsService = new FishTankBoundsService(fishTankScaler.GetCollider());
            foodManagerService = new FoodManagerService();

            npcFishPool.Init(boundsService);
            fishPlayer.Init(boundsService, foodManagerService, foodEatentEventBus, FoodSpawnedEventBus, hungryEventBus);
            spawnerController.Init(foodManagerService, foodEatentEventBus, FoodSpawnedEventBus);
        }
    }
}
