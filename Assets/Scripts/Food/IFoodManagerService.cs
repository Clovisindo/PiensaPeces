using UnityEngine;

namespace Game.FishFood
{
    public interface IFoodManagerService
    {
        GameObject GetClosestFood(Vector3 position);
        bool HasAnyFood();
        void RegisterFood(GameObject food);
        void UnregisterFood(GameObject food);
    }
}