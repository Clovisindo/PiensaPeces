using Assets.Scripts.Core;
using Assets.Scripts.Services.Bounds;
using UnityEngine;


namespace Assets.Scripts.Fish
{
    internal class ExitableFish : IExitableFish
    {
        private NPCFishController controller;
        private NPCFishPool pool;


        public void Init(NPCFishController controller, NPCFishPool pool)
        {
            this.controller = controller;
            this.pool = pool;
        }

        public void MoveInDirection(Transform transform, Vector2 direction, float speed)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        public bool IsOutOfBounds(Transform transform, IBoundsService boundsService)
        {
            // Aquí tu lógica de detección de salida (ej: usando BoundsService)
            return !boundsService.IsInsideBounds(transform.position);
        }

        public void NotifyExited()
        {
            pool.RecycleFish(controller);
        }
    }
}
