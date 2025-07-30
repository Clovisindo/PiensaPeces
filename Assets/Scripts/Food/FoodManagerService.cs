using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.FishFood
{
    public class FoodManagerService
    {
        private readonly List<GameObject> activeFoods = new();
        public bool HasAnyFood() => activeFoods.Any();

        public void RegisterFood(GameObject food)
        {
            if (!activeFoods.Contains(food))
                activeFoods.Add(food);
        }

        public void UnregisterFood(GameObject food)
        {
            if (activeFoods.Contains(food))
                activeFoods.Remove(food);
        }

        public GameObject GetClosestFood(Vector3 position)
        {
            if (activeFoods.Count == 0) return null;

            return activeFoods
                .OrderBy(f => Vector3.Distance(f.transform.position, position))
                .FirstOrDefault();
        }

        
    }

}
