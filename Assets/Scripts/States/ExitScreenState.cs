using Assets.Scripts.Core;
using Assets.Scripts.Fish;
using Assets.Scripts.Services.Bounds;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class ExitScreenState : IState
    {
        private readonly IExitableFish fish;
        private readonly IBoundsService boundsService;
        private readonly NPCFishController npcFishController;
        private readonly NPCFishPool pool;
        private readonly Transform transform;
        private readonly float speed;
        private Vector2 exitDirection;

        public ExitScreenState(Transform transform, NPCFishController fishController,IBoundsService boundsService, 
            IExitableFish fish, NPCFishPool pool, float speed)
        {
            this.transform = transform;
            this.boundsService = boundsService;
            this.npcFishController = fishController;
            this.pool = pool;
            this.fish = fish;
            this.speed = speed;
        }

        public void Enter()
        {
            // Salir en dirección aleatoria (por defecto hacia fuera de pantalla)
            exitDirection = GetRandomExitDirection();
        }

        public void Update()
        {
            fish.MoveInDirection(transform ,exitDirection, speed);

            if (fish.IsOutOfBounds(transform,boundsService))
            {
                fish.NotifyExited(npcFishController, pool); // Notifica al pool
            }
        }

        public void Exit()
        {
            // Nada que limpiar
        }

        private Vector2 GetRandomExitDirection()
        {
            // Por ejemplo: diagonal hacia afuera
            var directions = new Vector2[]
            {
            Vector2.up + Vector2.left,
            Vector2.up + Vector2.right,
            Vector2.down + Vector2.left,
            Vector2.down + Vector2.right,
            Vector2.left,
            Vector2.right,
            Vector2.down
            };

            return directions[Random.Range(0, directions.Length)].normalized;
        }
    }
}
