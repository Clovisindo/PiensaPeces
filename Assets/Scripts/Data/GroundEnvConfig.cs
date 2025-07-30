using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "GroundEnvConfig", menuName = "Ground/ground config", order = 1)]
    public class GroundEnvConfig: ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
    }
}
