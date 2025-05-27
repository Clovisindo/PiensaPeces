using Assets.Scripts.Core;
using Assets.Scripts.Fish;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class ExitScreenState : IState
    {
        private readonly ExitScreenContext context;
        private Vector2 exitDirection;

        public ExitScreenState(ExitScreenContext exitContext)
        {
            this.context = exitContext;
        }

        public void Enter()
        {
            // Salir en dirección aleatoria (por defecto hacia fuera de pantalla)
            exitDirection = GetRandomExitDirection();
        }

        public void Update()
        {
            context.Fish.MoveInDirection(context.Transform ,exitDirection, context.Speed);

            if (context.Fish.IsOutOfBounds(context.Transform, context.BoundsService))
            {
                context.Fish.NotifyExited(); // Notifica al pool
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
