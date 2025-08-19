using Game.StateMachineManager;
using Game.Services;
using Game.Core;
using Game.FishLogic;
using Assets.Scripts.Services.TimeService;

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

        private void ApplySwimState()
        {
            stateManager.ApplyState(new SwimState(fish, boundsService, new UnityTimeService(), speed));
        }
    }
}
