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

        public LoadDataContext LoadGroundByGameData(int daysPassed)
        {
            var config = allGroundDayConfigs.Find(c => c.dayNumber == daysPassed);

            if (config == null)
            {
                Debug.LogWarning($"No Ground Config found for day {daysPassed}, load last day data.");
                config = allGroundDayConfigs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }

            InstantiatePrefabsByConfig(config.groundEnvConfigs);
            var fishConfigCurrentDay = LoadFishConfigCurrentDay(allFishsDayConfigs, daysPassed);

            return fishConfigCurrentDay;
        }

        private LoadDataContext LoadFishConfigCurrentDay(List<FishEnvDayConfig> configs, int daysPassed)
        {
            var fishConfigData = configs.Find(c => c.dayNumber == daysPassed);
            if (fishConfigData == null)
            {
                Debug.LogWarning($"No Fish Config found for day {daysPassed}, load last day data.");
                fishConfigData = configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
            }
            return new LoadDataContext (fishConfigData.fishEnvDayConfigs.ToArray());
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

