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
        [SerializeField] private int minAmountLifeTime;
        [SerializeField] private int maxAmountLifeTime;

        private Queue<NPCFishController> availableFish = new Queue<NPCFishController>();
        private List<NPCFishController> activeFish = new List<NPCFishController>();

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
                fish.Init(this, boundsService, Random.Range(minAmountLifeTime, maxAmountLifeTime));
                fish.ResetFish(); // reinicia estados y tiempo de vida
                activeFish.Add(fish);
            }
        }

        public void RecycleFish(NPCFishController fish)
        {
            fish.gameObject.SetActive(false);
            activeFish.Remove(fish);
            availableFish.Enqueue(fish);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            // puedes ajustar esto según tu escena
            return new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
        }
    }

}
