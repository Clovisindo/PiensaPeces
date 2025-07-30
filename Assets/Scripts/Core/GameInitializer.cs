using UnityEngine;
using System;
using System.Linq;
using Game.Services;
using Game.UI;
using Game.Fishes;
using Game.FishFood;
using Game.Events;

namespace Game.Core
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
        [SerializeField] private AudioEnviromentSystem audioEnvSystem;
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
            sfxManager.Init(sfxEventBus);

            audioEnvSystem.Initialize(loadContextData.AudioConfigsCurrentDay.ToList(), daysPassed, sfxEventBus);

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
