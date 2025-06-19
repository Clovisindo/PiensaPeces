using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
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
        [SerializeField] private int daysPassed;

        [Header("Fish Configs")]
        [SerializeField] private FishConfig[] fishConfigs;

        private Queue<NPCFishController> availableFish = new();
        private List<NPCFishController> activeFish = new();
        private EventBus<SFXEvent> sfxEventBus;

        public void Init(IBoundsService boundservice, FishConfig[] fishConfigs, int daysPassed, EventBus<SFXEvent> sfxEventBus)
        {
            this.boundsService = boundservice;
            this.fishConfigs = fishConfigs;
            this.daysPassed = daysPassed;
            this.sfxEventBus = sfxEventBus;
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
                config.Init();
                fish.Init(config, this, boundsService, daysPassed, sfxEventBus);
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
            fish.ResetFish();
            fish.gameObject.SetActive(false);
            activeFish.Remove(fish);
            availableFish.Enqueue(fish);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector2 min = boundsService.GetMinBounds();
            Vector2 max = boundsService.GetMaxBounds();

            Vector3 spawnPos = Vector3.zero;

            int edge = Random.Range(0, 4); // 0=left, 1=right, 2=top, 3=bottom

            switch (edge)
            {
                case 0: // left
                    spawnPos = new Vector3(min.x, Random.Range(min.y, max.y), 0f);
                    break;
                case 1: // right
                    spawnPos = new Vector3(max.x, Random.Range(min.y, max.y), 0f);
                    break;
                case 2: // top
                    spawnPos = new Vector3(Random.Range(min.x, max.x), max.y, 0f);
                    break;
                case 3: // bottom
                    spawnPos = new Vector3(Random.Range(min.x, max.x), min.y, 0f);
                    break;
            }

            return spawnPos;
        }
    }


}
