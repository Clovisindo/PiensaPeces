using UnityEngine;

public class FollowTargetState : IState
{
    private readonly BaseFishController fish;
    private readonly StateMachine stateMachine;
    private readonly float speed;
    private readonly Transform target;
    private SpriteRenderer spriteRenderer;

    public FollowTargetState(BaseFishController fish, StateMachine stateMachine, float speed, Transform target)
    {
        this.fish = fish;
        this.stateMachine = stateMachine;
        this.speed = speed;
        this.target = target;
        this.spriteRenderer = fish.GetComponent<SpriteRenderer>();
    }

    public void Enter() 
    {
        Debug.Log("Entering Follow Target state.");
    }

    public void Update()
    {
        if (target == null) return;
        // Flip en eje X según dirección
        if (target.position.x < 0)
            spriteRenderer.flipX = true;
        else if (target.position.x > 0)
            spriteRenderer.flipX = false;

        var pos = fish.GetTransform().position;
        fish.GetTransform().position = Vector3.MoveTowards(pos, target.position, speed * Time.deltaTime);
    }

    public void Exit() 
    {
        Debug.Log("Exiting Follow Target state.");
    }
}

