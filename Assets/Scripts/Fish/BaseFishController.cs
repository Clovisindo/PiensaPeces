using Assets.Scripts.States;
using UnityEngine;

public abstract class BaseFishController : MonoBehaviour
{
    protected StateMachine stateMachine;
    [SerializeField] protected float speed;

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(this,stateMachine));
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public Transform GetTransform() => transform;
}