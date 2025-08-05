using Game.FishFood;
using UnityEngine;

namespace Game.UI
{
    public class SpawnFoodButton : MonoBehaviour
    {
        [SerializeField] private FoodSpawnerController spawner;

        public void OnClick()
        {
            spawner.SpawnFood();
        }
    }
}