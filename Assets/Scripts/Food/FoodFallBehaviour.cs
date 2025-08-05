using UnityEngine;

namespace Game.FishFood
{
    public class FoodFallBehaviour : MonoBehaviour
    {
        private float fallSpeed;
        private float minY;

        public void Init(float fallSpeed, float minY)
        {
            this.fallSpeed = fallSpeed;
            this.minY = minY;
        }

        private void Update()
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if(transform.position.y <= minY)
            {
                Destroy(gameObject);
            }
        }
    }
}
