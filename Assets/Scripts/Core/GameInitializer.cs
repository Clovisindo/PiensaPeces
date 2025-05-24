using UnityEngine;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;

namespace Assets.Scripts.Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Collider2D fishTankCollider;
        [SerializeField] private PlayerFishController fishPlayer;
        [SerializeField] private FoodSpawnerController spawnerController;

        private FoodManagerService foodManagerService;

        [SerializeField] private NPCFishController[] npcFish;//todo implementar

        public EventBus<FoodEaten> foodEatentEventBus = new();
        public EventBus<FoodSpawned> FoodSpawnedEventBus = new();
        public EventBus<HungryEvent> hungryEventBus = new();

        void Awake()
        {
            //var boundsService = new CameraBoundsService(Camera.main);
            var boundsService = new FishTankBoundsService(fishTankCollider);
            foodManagerService = new FoodManagerService();

            fishPlayer.Init(boundsService, foodManagerService, foodEatentEventBus, FoodSpawnedEventBus, hungryEventBus);
            spawnerController.Init(foodManagerService, foodEatentEventBus, FoodSpawnedEventBus);

            //foreach (var npc in npcFish)
            //{
            //    npc.Init(boundsService);
            //}
        }
    }
}
