using UnityEngine;

namespace Game.Utilities
{
    public class UnityGameObjectFactory : IGameObjectFactory
    {
        public void Destroy(GameObject go) => UnityEngine.GameObject.Destroy(go);

        public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            => UnityEngine.GameObject.Instantiate(prefab, position, rotation, parent);
    }
}
