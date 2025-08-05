using Game.Services;
using Game.StateMachineManager;
using UnityEngine;

namespace Game.FishLogic
{
    public interface IFishStateFactory
    {
        IState CreateSwimState(IFish fish, IBoundsService boundsService, float speed);
        IState CreateIdleState(IFish fish);
        IState CreateExitState(Transform transform, IBoundsService boundsService, IExitable fishExitable, float speed);
        IState CreateFollowState(IFish fish, float speed, Transform target);
    }
}
