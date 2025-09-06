using UnityEngine;

namespace Game.Utilities
{
    public interface IGameObjectFactory
    {
        GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null);
        void Destroy(GameObject go);
    }
}
