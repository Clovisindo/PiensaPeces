using UnityEngine;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;
using Assets.Scripts.Fish.NPC;

namespace Assets.Scripts.Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private FishTankMeshScaler fishTankScaler;
        [SerializeField] private PlayerFishController fishPlayer;
        [SerializeField] private FoodSpawnerController spawnerController;
        [SerializeField] private NPCFishPool npcFishPool;
        [SerializeField] private SFXManager sfxManager;
        [SerializeField] private FishConfig playerConfig;
        private FoodManagerService foodManagerService;

        public EventBus<FoodEaten> foodEatentEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();
        public EventBus<HungryEvent> hungryEventBus = new();
        public EventBus<SFXEvent> sfxEventBus = new();

        void Awake()
        {
            fishTankScaler.Init();
            var boundsService = new FishTankBoundsService(fishTankScaler.GetCollider());
            foodManagerService = new FoodManagerService();

            npcFishPool.Init(boundsService, sfxEventBus);
            playerConfig.Init();
            fishPlayer.Init(playerConfig,boundsService, foodManagerService, sfxManager, foodEatentEventBus, FoodSpawnedEventBus, hungryEventBus, sfxEventBus);
            spawnerController.Init(foodManagerService, foodEatentEventBus, FoodSpawnedEventBus);
        }
    }
}
