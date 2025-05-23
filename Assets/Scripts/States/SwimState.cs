using Assets.Scripts.Services.Bounds;
using UnityEngine;

public class SwimState : IState
{
    private readonly BaseFishController fish;
    private readonly IBoundsService boundsService;
    private readonly StateMachine stateMachine;
    private readonly float speed;
    private Vector2 destination;

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
        SwimMovement();
    }

    public void Exit() 
    {
        Debug.Log("Exiting swim state.");
    }

    private void SwimMovement()
    {
        var t = fish.GetTransform();
        Vector2 currentPos = t.position;

        // Mover hacia el destino
        Vector2 newPos = Vector2.MoveTowards(currentPos, destination, speed * Time.deltaTime);
        t.position = new Vector3(newPos.x, newPos.y, t.position.z); // mantén Z fijo en 2D

        // Rotar hacia la dirección del movimiento
        Vector2 direction = destination - currentPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        t.rotation = Quaternion.Slerp(t.rotation, Quaternion.Euler(0, 0, angle), 5f * Time.deltaTime);

        // Cambiar de destino si está cerca o fuera de límites
        if (Vector2.Distance(currentPos, destination) < 0.1f || IsNearBounds(currentPos))
        {
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        var currentPos = fish.GetTransform().position;
        var min = boundsService.GetMinBounds();
        var max = boundsService.GetMaxBounds();

        float x = Random.Range(min.x + 0.5f, max.x - 0.5f);
        float y = Random.Range(min.y + 0.5f, max.y - 0.5f);

        destination = new Vector2(x, y);
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

