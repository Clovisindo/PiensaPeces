using Assets.Scripts.Core;
using Assets.Scripts.Services.Bounds;
using UnityEngine;


namespace Assets.Scripts.Fish
{
    internal class ExitableFish : IExitableFish
    {

        public void MoveInDirection(Transform transform, Vector2 direction, float speed)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        public bool IsOutOfBounds(Transform transform, IBoundsService boundsService)
        {
            // Aquí tu lógica de detección de salida (ej: usando BoundsService)
            return !boundsService.IsInsideBounds(transform.position);
        }

        public void NotifyExited(NPCFishController fish, NPCFishPool pool)
        {
            pool.RecycleFish(fish);
        }
    }
}
