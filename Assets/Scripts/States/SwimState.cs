using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using UnityEngine;


namespace Game.States
{
    public class SwimState : IState
    {
        private readonly IFish fish;
        private readonly IBoundsService boundsService;
        private readonly float speed;
        private readonly SpriteRenderer spriteRenderer;
        private Vector2 destination;

        private float timeSinceLastDirectionChange = 0f;
        private const float minDirectionChangeInterval = 1.0f;

        public SwimState(IFish fish, IBoundsService boundsService,  float speed)
        {
            this.fish = fish;
            this.boundsService = boundsService;
            this.speed = speed;
            this.spriteRenderer = fish.GetSpriteRenderer();
        }

        public void Enter()
        {
            //Debug.Log("Entering swim state.");
            SetNewDestination();
            timeSinceLastDirectionChange = 0f;
        }

        public void Update()
        {
            timeSinceLastDirectionChange += Time.deltaTime;
            SwimMovement();
        }

        public void Exit()
        {
            //Debug.Log("Exiting swim state.");
        }

        private void SwimMovement()
        {
            var t = fish.GetTransform();
            Vector2 currentPos = t.position;

            // Mover hacia el destino
            Vector2 newPos = Vector2.MoveTowards(currentPos, destination, speed * Time.deltaTime);
            t.position = new Vector3(newPos.x, newPos.y, t.position.z);

            // Flip en eje X según dirección
            if (destination.x < currentPos.x)
                spriteRenderer.flipX = true;
            else if (destination.x > currentPos.x)
                spriteRenderer.flipX = false;

            // Cambiar destino solo si ha pasado suficiente tiempo
            bool reachedDestination = Vector2.Distance(currentPos, destination) < 0.1f;
            bool nearBounds = IsNearBounds(currentPos);

            if ((reachedDestination || nearBounds) && timeSinceLastDirectionChange >= minDirectionChangeInterval)
            {
                SetNewDestination();
                timeSinceLastDirectionChange = 0f;
            }
        }

        private void SetNewDestination()
        {
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

            float margin = 0.5f;

            return position.x <= min.x + margin || position.x >= max.x - margin ||
                   position.y <= min.y + margin || position.y >= max.y - margin;
        }
    }
}