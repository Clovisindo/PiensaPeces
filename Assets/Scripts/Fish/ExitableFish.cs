using Game.FishLogic;
using Game.Services;
using UnityEngine;


namespace Game.Fishes
{
    public class ExitableFish : IExitable
    {
        private NPCFishController controller;
        private NPCFishPool pool;
        private SpriteRenderer spriteRenderer;


        public void Init(NPCFishController controller, NPCFishPool pool)
        {
            this.controller = controller;
            this.pool = pool;
            this.spriteRenderer = controller.GetComponent<SpriteRenderer>();
        }

        public void MoveInDirection(Transform transform, Vector2 direction, float speed)
        {
            // Flip en eje X según dirección
            if (direction.x < 0)
                spriteRenderer.flipX = true;
            else if (direction.x > 0)
                spriteRenderer.flipX = false;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        public bool IsOutOfBounds(Transform transform, IBoundsService boundsService)
        {
            return !boundsService.IsInsideBounds(transform.position);
        }

        public void NotifyExited()
        {
            pool.RecycleFish(controller);
        }
    }
}
