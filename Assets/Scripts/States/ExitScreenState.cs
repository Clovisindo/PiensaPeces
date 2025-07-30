using Game.Core;
using Game.Utilities;
using UnityEngine;

namespace Game.States
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
            exitDirection = GetRandomExitDirection();
        }

        public void Update()
        {
            context.Fish.MoveInDirection(context.Transform ,exitDirection, context.Speed);

            if (context.Fish.IsOutOfBounds(context.Transform, context.BoundsService))
            {
                context.Fish.NotifyExited();
            }
        }

        public void Exit()
        {
        }

        private Vector2 GetRandomExitDirection()
        {
            var directions = new Vector2[]
            {
            Vector2.left,
            Vector2.right,
            };

            return directions[Random.Range(0, directions.Length)].normalized;
        }
    }
}
