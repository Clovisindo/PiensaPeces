using UnityEngine;

namespace Game.Fishes
{
    public abstract class BaseFishController : MonoBehaviour
    {
        protected StateMachine stateMachine;
        protected StateManager stateManager;
        [SerializeField] protected float speed;

        protected virtual void Update()
        {
            stateMachine.Update();
        }

        public Transform GetTransform() => transform;
    }
}