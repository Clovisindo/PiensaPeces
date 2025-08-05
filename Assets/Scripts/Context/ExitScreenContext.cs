using Game.FishLogic;
using Game.Services;
using UnityEngine;

namespace Game.Context 
{
    public class ExitScreenContext
    {
        public Transform Transform { get; }
        public IBoundsService BoundsService { get; }
        public IExitable ExitBehavior { get; }
        public float Speed { get; }

        public ExitScreenContext(Transform transform, IBoundsService boundsService, IExitable exitBehavior, float speed)
        {
            Transform = transform;
            BoundsService = boundsService;
            ExitBehavior = exitBehavior;
            Speed = speed;
        }
    }

}
