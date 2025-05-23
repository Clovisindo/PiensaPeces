using UnityEngine;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Services.Bounds;

namespace Assets.Scripts.Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Collider2D fishTankCollider;
        [SerializeField] private PlayerFishController fishPlayer;
        [SerializeField] private FoodSpawnerController spawnerController;

        [SerializeField] private NPCFishController[] npcFish;//todo implementar

        public EventBus<FoodEaten> foodEatentEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();

        void Awake()
        {
            //var boundsService = new CameraBoundsService(Camera.main);
            var boundsService = new FishTankBoundsService(fishTankCollider);

            fishPlayer.Init(boundsService, foodEatentEventBus, FoodSpawnedEventBus);
            spawnerController.Init(foodEatentEventBus, FoodSpawnedEventBus);

            //foreach (var npc in npcFish)
            //{
            //    npc.Init(boundsService);
            //}
        }
    }
}
