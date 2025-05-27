using Assets.Scripts.Fish;
using Assets.Scripts.Services.Bounds;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class ExitScreenContext
    {
        public Transform Transform { get; }
        public IBoundsService BoundsService { get; }
        public IExitableFish Fish { get; }
        public float Speed { get; }

        public ExitScreenContext(Transform transform, IBoundsService boundsService, IExitableFish fish, float speed)
        {
            Transform = transform;
            BoundsService = boundsService;
            Fish = fish;
            Speed = speed;
        }
    }

}
