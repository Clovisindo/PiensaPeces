using Assets.Scripts.Core;
using Assets.Scripts.States;
using UnityEngine;

public abstract class BaseFishController : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected StateManager stateManager;
    [SerializeField] protected float speed;

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        stateManager = new StateManager(stateMachine);
        stateManager.ApplyState(new IdleState(this,stateMachine));
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public Transform GetTransform() => transform;
}