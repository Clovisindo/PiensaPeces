using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    [CreateAssetMenu(fileName = "GroundEnvConfig", menuName = "Ground/ground config", order = 1)]
    public class GroundEnvConfig: ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
    }
}
