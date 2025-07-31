using Game.StateMachineManager;
using Game.Services;
using Game.Core;
using Game.FishLogic;

namespace Game.States
{
    public class NPCFishStateHandler
    {
        private readonly StateManager stateManager;
        private readonly StateMachine stateMachine;
        private readonly IFish fish;
        private readonly IBoundsService boundsService;
        private readonly float speed;

        public NPCFishStateHandler(IFish fish, IBoundsService boundsService, float speed)
        {
            this.stateMachine = new StateMachine();
            this.stateManager = new StateManager(stateMachine);
            this.fish = fish;
            this.boundsService = boundsService;
            this.speed = speed;

            ApplySwimState(); // Estado inicial
        }

        //public void ApplyIntent(FishIntent intent, bool forceExit)
        //{
        //    if (forceExit)
        //    {
        //        var exitContext = new ExitScreenContext(fish.GetTransform(), boundsService, fish.GetComponent<IExitableFish>(), speed);
        //        stateManager.ApplyState(new ExitScreenState(exitContext));
        //        return;
        //    }

        //    switch (intent)
        //    {
        //        case FishIntent.SwimRandomly:
        //            ApplySwimState();
        //            break;
        //        case FishIntent.Idle:
        //        default:
        //            stateManager.ApplyState(new IdleState(fish));
        //            break;
        //    }
        //}

        private void ApplySwimState()
        {
            stateManager.ApplyState(new SwimState(fish, boundsService, speed));
        }
    }
}
