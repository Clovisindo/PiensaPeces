

using UnityEngine;

namespace Assets.Scripts.States
{
    public class IdleState : IState
    {
        private readonly BaseFishController fish;
        private readonly StateMachine stateMachine;

        public IdleState(BaseFishController fish, StateMachine stateMachine)
        {
            this.fish = fish;
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            Debug.Log("Entering idle state.");
        }

        public void Update()
        {
        }

        public void Exit() 
        {
            Debug.Log("Exiting idle state.");
        }
    }
}
