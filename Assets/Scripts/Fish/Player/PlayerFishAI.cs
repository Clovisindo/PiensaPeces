using Game.FishFood;
using UnityEngine;

namespace Game.Fishes
{
    public enum FishIntent
    {
        Idle,
        SwimRandomly,
        FollowFood,
        EndLife
    }

    public class PlayerFishAI : IFishAI
    {
        private readonly Transform fishTransform;
        private readonly HungerComponent hungerComponent;
        private readonly FoodManagerService foodManagerService;

        public PlayerFishAI(Transform fishTransform, HungerComponent hungerComponent, FoodManagerService foodManagerService)
        {
            this.fishTransform = fishTransform;
            this.hungerComponent = hungerComponent;
            this.foodManagerService = foodManagerService;
        }

        public FishIntent EvaluateIntent()
        {
            if (!hungerComponent.IsHungry)
                return FishIntent.SwimRandomly;

            var closestFood = foodManagerService.GetClosestFood(fishTransform.position);
            return closestFood != null ? FishIntent.FollowFood : FishIntent.SwimRandomly;
        }

        public Transform GetTargetFood()
        {
            return foodManagerService.GetClosestFood(fishTransform.position)?.transform;
        }
    }

}
