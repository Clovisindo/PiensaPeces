using Game.Core;
using Game.Data;
using Game.Events;
using Game.Fishes;
using Game.FishFood;
using Game.Services;
using Game.States;
using Game.UI;
using Game.Utilities;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Runtime
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
            saveSystem = new SaveSystem(new WriteFileStorage());
            saveSystem.SetFirstLaunchDate(Convert.ToDateTime(firstGameLaunch));// borrar debug
            //if (saveSystem.IsFirstLaunch())//ToDo: se puede borrar esto cuando terminemos pruebas
            //{
            //    Debug.Log($"Primera vez que se abre el juego.");
            //}
            //else
            //{
            //    Debug.Log($"YA SE HABIA ABIERTO EL JUEGO.");
            //}
            int daysPassed = saveSystem.GetDaysSinceFirstLaunch();
            Debug.Log($"Días desde la primera vez que se abrió el juego: {daysPassed}");

            enviromentSystem.SetLoaderService(new EnviromentLoader(new UnityLogger()));
            var loadContextData = enviromentSystem.LoadEnviromentData(daysPassed);
            sfxManager.Init(sfxEventBus);

            audioEnvSystem.Initialize(loadContextData.AudioConfigsCurrentDay.ToList(), daysPassed, sfxEventBus);

            var foodForCurrentDay = loadContextData.FoodConfigsCurrentDay.ToArray();
            fishTankScaler.Init();
            var boundsService = new FishTankBoundsService(fishTankScaler.GetCollider());
            foodManagerService = new FoodManagerService();

            npcFishPool.Init(boundsService, new FishStateFactory(), loadContextData.FishConfigsCurrentDay, daysPassed, sfxEventBus);
            playerConfig.Init();
            fishPlayer.Init(playerConfig, new FishStateFactory(), boundsService, foodManagerService, sfxManager, daysPassed, foodEatentEventBus, FoodSpawnedEventBus, hungryEventBus, sfxEventBus);
            spawnerController.Init(boundsService, foodManagerService, foodForCurrentDay, foodEatentEventBus, FoodSpawnedEventBus);
        }
    }
}
