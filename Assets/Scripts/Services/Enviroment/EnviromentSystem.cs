using Assets.Scripts.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    public class EnviromentSystem : MonoBehaviour
    {
        [SerializeField] private List<Transform> groundSpawnPositions;
        [SerializeField] private List<GroundEnvironmentDayConfig> allGroundDayConfigs;
        [SerializeField] private List<FishEnvDayConfig> allFishsDayConfigs;
        [SerializeField] private List<FoodEnvDayConfig> allFoodDayConfigs;
        [SerializeField] private List<AudioEnvDayConfig> allAudioDayConfigs;
        private LoadDataContext.Builder loadDataContextBuilder;

        public LoadDataContext LoadGroundByGameData(int daysPassed)
        {
            loadDataContextBuilder = new LoadDataContext.Builder();
            var config = allGroundDayConfigs.Find(c => c.dayNumber == daysPassed);

            if (config == null)
            {
                Debug.LogWarning($"No Ground Config found for day {daysPassed}, load last day data.");
                config = allGroundDayConfigs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }


            InstantiatePrefabsByConfig(config.groundEnvConfigs);
            LoadFoodConfigCurrentDay(allFoodDayConfigs, daysPassed);
            LoadFishConfigCurrentDay(allFishsDayConfigs, daysPassed);
            LoadAudioConfigCurrentDay(allAudioDayConfigs, daysPassed);
            return loadDataContextBuilder.Build();
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
            loadDataContextBuilder.WithFoodConfigs(foodConfigData.foodEnvConfigs.ToArray());
        }

        private void LoadFishConfigCurrentDay(List<FishEnvDayConfig> configs, int daysPassed)
        {
            var fishConfigData = configs.Find(c => c.dayNumber == daysPassed);
            if (fishConfigData == null)
            {
                Debug.LogWarning($"No Fish Config found for day {daysPassed}, load last day data.");
                fishConfigData = configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }
            loadDataContextBuilder.WithFishConfigs(fishConfigData.fishEnvDayConfigs.ToArray());
        }

        private void LoadAudioConfigCurrentDay(List<AudioEnvDayConfig> configs, int daysPassed)
        {
            var audioConfigData = configs.Find(c => c.dayNumber == daysPassed);
            if (audioConfigData == null)
            {
                Debug.LogWarning($"No Audio Config found for day {daysPassed}, load last day data.");
                audioConfigData = configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }
            loadDataContextBuilder.WithAudioConfigs(audioConfigData.audioConfigs.ToArray());
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

