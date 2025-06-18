namespace Assets.Scripts.Services.Enviroment
{
    using System.Collections.Generic;
    using UnityEngine;

    public class EnviromentSystem : MonoBehaviour
    {
        [SerializeField] private List<Transform> groundSpawnPositions;
        [SerializeField] private List<GroundEnvironmentDayConfig> allDayConfigs;

        public void LoadGroundByGameData(int daysPassed)
        {
            var config = allDayConfigs.Find(c => c.dayNumber == daysPassed);

            if (config == null)
            {
                Debug.LogWarning($"No Ground Config found for day {daysPassed}");
                return;
            }

            LoadPrefabsByConfig(config.groundEnvConfigs);
        }

        private void LoadPrefabsByConfig(List<GroundEnvConfig> configs)
        {
            for (int i = 0; i < configs.Count && i < groundSpawnPositions.Count; i++)
            {
                var instance = Instantiate(configs[i].prefab, groundSpawnPositions[i].position, Quaternion.identity);
                instance.GetComponent<SpriteRenderer>().sprite = configs[i].sprite;
            }
        }
    }
}

