using Game.Context;
using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using UnityEngine;

namespace Game.States
{
    public class FishStateFactory : IFishStateFactory
    {
        public IState CreateSwimState(IFish fish, IBoundsService boundsService, float speed) => new SwimState(fish, boundsService, speed);

        public IState CreateIdleState(IFish fish) => new IdleState(fish);

        public IState CreateExitState(Transform transform, IBoundsService bounds, IExitable fishExitable, float speed) =>
            new ExitScreenState(new ExitScreenContext(transform, bounds, fishExitable, speed));
        public IState CreateFollowState(IFish fish, float speed, Transform target) =>
            new FollowTargetState(fish,speed,target);
    }
}
