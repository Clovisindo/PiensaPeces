using Game.FishLogic;
using Game.StateMachineManager;

namespace Game.States
{ 
    public class IdleState : IState
    {
        private readonly IFish fish;

        public IdleState(IFish fish)
        {
            this.fish = fish;
        }

        public void Enter()
        {
            //Debug.Log("Entering idle state.");
        }

        public void Update()
        {
        }

        public void Exit() 
        {
            //Debug.Log("Exiting idle state.");
        }
    }
}
