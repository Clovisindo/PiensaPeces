using Assets.Scripts.Fish.NPC;
using Assets.Scripts.Services.Bounds;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Core
{
    public class NPCFishPool : MonoBehaviour
    {
        IBoundsService boundsService;

        [SerializeField] private NPCFishController npcFishPrefab;
        [SerializeField] private int poolSize = 1;

        [Header("Fish Configs")]
        [SerializeField] private FishConfig[] fishConfigs;

        private Queue<NPCFishController> availableFish = new();
        private List<NPCFishController> activeFish = new();

        public void Init(IBoundsService boundservice)
        {
            this.boundsService = boundservice;
        }

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var fish = Instantiate(npcFishPrefab, transform);
                fish.gameObject.SetActive(false);
                availableFish.Enqueue(fish);
            }

            SpawnFishIfNeeded();
        }

        private void Update()
        {
            SpawnFishIfNeeded();
        }

        private void SpawnFishIfNeeded()
        {
            while (activeFish.Count < poolSize && availableFish.Count > 0)
            {
                var fish = availableFish.Dequeue();
                fish.transform.position = GetRandomSpawnPosition();
                fish.gameObject.SetActive(true);

                var config = GetRandomFishConfig();
                fish.Init(config, this, boundsService);
                fish.ResetFish();

                activeFish.Add(fish);
            }
        }

        private FishConfig GetRandomFishConfig()
        {
            if (fishConfigs == null || fishConfigs.Length == 0)
            {
                Debug.LogWarning("No fish configs provided.");
                //return ScriptableObject.CreateInstance<FishConfig>();
            }

            return fishConfigs[Random.Range(0, fishConfigs.Length)];
        }

        public void RecycleFish(NPCFishController fish)
        {
            fish.gameObject.SetActive(false);
            activeFish.Remove(fish);
            availableFish.Enqueue(fish);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
        }
    }


}
