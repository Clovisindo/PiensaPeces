using Assets.Scripts.Services.Bounds;
using UnityEngine;

public class SwimState : IState
{
    private readonly BaseFishController fish;
    private readonly IBoundsService boundsService;
    private readonly StateMachine stateMachine;
    private readonly float speed;
    private Vector3 destination;

    public SwimState(BaseFishController fish,IBoundsService boundsService, StateMachine stateMachine, float speed)
    {
        this.fish = fish;
        this.boundsService = boundsService;
        this.stateMachine = stateMachine;
        this.speed = speed;
    }

    public void Enter()
    {
        Debug.Log("Entering swim state.");
        SetNewDestination();
    }

    public void Update()
    {
        var t = fish.GetTransform();
        t.position = Vector3.MoveTowards(t.position, destination, speed * Time.deltaTime);

        if (Vector3.Distance(t.position, destination) < 0.1f || IsNearBounds(t.position))
        {
            SetNewDestination();
        }
    }

    public void Exit() 
    {
        Debug.Log("Exiting swim state.");
    }

    private void SetNewDestination()
    {
        var currentPos = fish.GetTransform().position;
        var min = boundsService.GetMinBounds();
        var max = boundsService.GetMaxBounds();

        // genera un punto aleatorio cercano, pero dentro de los límites
        var randomOffset = Random.insideUnitSphere * 3f;
        randomOffset.y = 0;

        var newDestination = currentPos + randomOffset;

        // clamp para no salirse de la pecera
        newDestination.x = Mathf.Clamp(newDestination.x, min.x + 0.5f, max.x - 0.5f);
        newDestination.y = currentPos.y;
        //newDestination.z = Mathf.Clamp(newDestination.z, min.y + 0.5f, max.y - 0.5f); 

        destination = newDestination;
    }

    private bool IsNearBounds(Vector3 position)
    {
        var min = boundsService.GetMinBounds();
        var max = boundsService.GetMaxBounds();

        float margin = 0.5f; // margen de seguridad para redireccionar antes de tocar

        return position.x <= min.x + margin || position.x >= max.x - margin ||
               position.y <= min.y + margin || position.y >= max.y - margin;
    }
}

