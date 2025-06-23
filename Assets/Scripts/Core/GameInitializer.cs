using UnityEngine;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;
using Assets.Scripts.Fish.NPC;
using System;
using Assets.Scripts.Services.Enviroment;
using System.Linq;

namespace Assets.Scripts.Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private EnviromentSystem enviromentSystem;
        [SerializeField] private FishTankMeshScaler fishTankScaler;
        [SerializeField] private PlayerFishController fishPlayer;
        [SerializeField] private FoodSpawnerController spawnerController;
        [SerializeField] private NPCFishPool npcFishPool;
        [SerializeField] private SFXManager sfxManager;
        [SerializeField] private FishConfig playerConfig;
        [SerializeField] private String firstGameLaunch; //todo borrar
        private FoodManagerService foodManagerService;
        private SaveSystem saveSystem;

        public EventBus<FoodEaten> foodEatentEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();
        public EventBus<HungryEvent> hungryEventBus = new();
        public EventBus<SFXEvent> sfxEventBus = new();

        void Awake()
        {
            saveSystem = new SaveSystem();
            saveSystem.SetFirstLaunchDate(Convert.ToDateTime(firstGameLaunch));
            int daysPassed = saveSystem.GetDaysSinceFirstLaunch();
            

            Debug.Log($"Días desde la primera vez que se abrió el juego: {daysPassed}");

            var loadContextData = enviromentSystem.LoadGroundByGameData(daysPassed);
            var foodForCurrentDay = loadContextData.FoodConfigsCurrentDay.ToArray();
            fishTankScaler.Init();
            var boundsService = new FishTankBoundsService(fishTankScaler.GetCollider());
            foodManagerService = new FoodManagerService();

            npcFishPool.Init(boundsService, loadContextData.FishConfigsCurrentDay, daysPassed, sfxEventBus);
            playerConfig.Init();
            fishPlayer.Init(playerConfig,boundsService, foodManagerService, sfxManager, daysPassed, foodEatentEventBus, FoodSpawnedEventBus, hungryEventBus, sfxEventBus);
            spawnerController.Init( boundsService,foodManagerService, foodForCurrentDay, foodEatentEventBus, FoodSpawnedEventBus);
        }
    }
}
