using Game.Fishes;
using Game.Services;
using UnityEngine;

namespace Game.States 
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
