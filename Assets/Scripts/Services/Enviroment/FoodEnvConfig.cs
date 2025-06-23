using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    [CreateAssetMenu(fileName = "FoodEnvConfig", menuName = "Food/Food config", order = 1)]
    public class FoodEnvConfig : ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
    }
}
