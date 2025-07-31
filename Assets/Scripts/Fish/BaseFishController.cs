using Game.Core;
using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using UnityEngine;

namespace Game.Fishes
{
    public abstract class BaseFishController : MonoBehaviour, IFish
    {
        protected StateMachine stateMachine;
        protected StateManager stateManager;
        [SerializeField] protected float speed;

        protected virtual void Update()
        {
            stateMachine.Update();
        }

        public Transform GetTransform() => transform;

        public SpriteRenderer GetSpriteRenderer() => GetComponent<SpriteRenderer>();

        public IBoundsService GetBoundsService() => GetComponent<IBoundsService>();
    }
}