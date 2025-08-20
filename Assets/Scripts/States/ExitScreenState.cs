using Game.Context;
using Game.StateMachineManager;
using System;
using UnityEngine;

namespace Game.States
{
    public class ExitScreenState : IState
    {
        private readonly ExitScreenContext context;
        private readonly Func<int, int, int> randomRange;
        private Vector2 exitDirection;

        public ExitScreenState(ExitScreenContext exitContext, Func<int, int, int> randomRange = null)
        {
            this.context = exitContext;
            this.randomRange = randomRange ?? UnityEngine.Random.Range;
        }

        public void Enter()
        {
            exitDirection = GetRandomExitDirection();
        }

        public void Update()
        {
            context.ExitBehavior.MoveInDirection(context.Transform ,exitDirection, context.Speed);

            if (context.ExitBehavior.IsOutOfBounds(context.Transform, context.BoundsService))
            {
                context.ExitBehavior.NotifyExited();
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

            return directions[randomRange(0, directions.Length)].normalized;
        }
    }
}
