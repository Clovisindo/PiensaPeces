using Game.Data;
using Game.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ILogger = Game.Utilities.ILogger;

namespace Game.Services 
{
    public class EnviromentSystem : MonoBehaviour
    {
        [Header("Unity Editor Config")]
        [SerializeField] private List<Transform> groundSpawnPositions;
        [SerializeField] private List<GroundEnvironmentDayConfig> allGroundDayConfigs;
        [SerializeField] private List<FishEnvDayConfig> allFishsDayConfigs;
        [SerializeField] private List<FoodEnvDayConfig> allFoodDayConfigs;
        [SerializeField] private List<AudioEnvDayConfig> allAudioDayConfigs;
        private IEnviromentLoader enviromentLoader;
        private ILogger _logger;

        /// <summary>
        /// Inyección de dependencias para tests o inicializaciones manuales.
        /// </summary>
        public void Initialize(
            List<Transform> groundSpawnPositions,
            List<GroundEnvironmentDayConfig> groundConfigs,
            List<FishEnvDayConfig> fishConfigs,
            List<FoodEnvDayConfig> foodConfigs,
            List<AudioEnvDayConfig> audioConfigs,
            IEnviromentLoader loader,
            ILogger logger
        )
        {
            this.groundSpawnPositions = groundSpawnPositions;
            this.allGroundDayConfigs = groundConfigs;
            this.allFishsDayConfigs = fishConfigs;
            this.allFoodDayConfigs = foodConfigs;
            this.allAudioDayConfigs = audioConfigs;
            this.enviromentLoader = loader;
            this._logger = logger;
        }

        public void SetLoaderService(IEnviromentLoader loaderService)
        {
            _logger ??= new UnityLogger();
            this.enviromentLoader = loaderService;
        }

        public LoadDataContext LoadEnviromentData(int daysPassed)
        {
            var result = enviromentLoader.Load(allGroundDayConfigs, allFishsDayConfigs, allFoodDayConfigs, allAudioDayConfigs, daysPassed);

            if (result.SelectedGroundConfig?.groundEnvConfigs != null)
                InstantiatePrefabsByConfig(result.SelectedGroundConfig.groundEnvConfigs);
            LoadFoodConfigCurrentDay(allFoodDayConfigs, daysPassed);
            LoadFishConfigCurrentDay(allFishsDayConfigs, daysPassed);
            LoadAudioConfigCurrentDay(allAudioDayConfigs, daysPassed);
            return result.Context;
        }

        private void LoadFoodConfigCurrentDay( List<FoodEnvDayConfig> configs, int daysPassed)
        {
            var foodConfigData = configs.Find(c => c.dayNumber == daysPassed);
            if (foodConfigData == null)
            {
                Debug.LogWarning($"No foodConfigData Config found for day {daysPassed}, load last day data.");
                foodConfigData = configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }
            foreach (var config in foodConfigData.foodEnvConfigs)
            {
                config.prefab.GetComponent<SpriteRenderer>().sprite = config.sprite;
            }
        }

        private void LoadFishConfigCurrentDay(List<FishEnvDayConfig> configs, int daysPassed)
        {
            var fishConfigData = configs.Find(c => c.dayNumber == daysPassed);
            if (fishConfigData == null)
            {
                Debug.LogWarning($"No Fish Config found for day {daysPassed}, load last day data.");
                fishConfigData = configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }
        }

        private void LoadAudioConfigCurrentDay(List<AudioEnvDayConfig> configs, int daysPassed)
        {
            var audioConfigData = configs.Find(c => c.dayNumber == daysPassed);
            if (audioConfigData == null)
            {
                Debug.LogWarning($"No Audio Config found for day {daysPassed}, load last day data.");
                audioConfigData = configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }
        }

        private void InstantiatePrefabsByConfig(List<GroundEnvConfig> configs)
        {
            for (int i = 0; i < configs.Count && i < groundSpawnPositions.Count; i++)
            {
                var instance = Instantiate(configs[i].prefab, groundSpawnPositions[i].position, Quaternion.identity);
                instance.GetComponent<SpriteRenderer>().sprite = configs[i].sprite;
            }
        }

    }
}

