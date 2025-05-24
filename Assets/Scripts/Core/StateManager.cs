using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class StateManager
    {
        private readonly StateMachine stateMachine;

        public StateManager(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void ApplyState( IState newState)
        {
            if (IsSameState(newState))
            {
                Debug.Log($"[StateManager] Estado {newState.GetType().Name} ya está activo.");
                return;
            }
            Debug.Log($"[StateManager] Transición: {stateMachine.currentState?.GetType().Name} ➜ {newState.GetType().Name}");
            stateMachine.ChangeState(newState);
        }

        private bool IsSameState(IState newState)
        {
            var current = stateMachine.currentState;
            return current != null && current.GetType() == newState.GetType();
        }
    }
}
