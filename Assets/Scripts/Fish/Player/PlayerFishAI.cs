using Game.FishFood;
using System;
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
        private readonly Transform _fishTransform;
        private readonly IHungerComponent _hungerComponent;
        private readonly IFoodManagerService _foodManagerService;

        public PlayerFishAI(Transform fishTransform, IHungerComponent hungerComponent, IFoodManagerService foodManagerService)
        {
            _fishTransform = fishTransform ?? throw new ArgumentNullException(nameof(fishTransform));
            _hungerComponent = hungerComponent ?? throw new ArgumentNullException(nameof(hungerComponent));
            _foodManagerService = foodManagerService ?? throw new ArgumentNullException(nameof(foodManagerService));
        }

        public FishIntent EvaluateIntent()
        {
            if (!_hungerComponent.IsHungry)
                return FishIntent.SwimRandomly;

            var closestFood = _foodManagerService.GetClosestFood(_fishTransform.position);
            return closestFood != null ? FishIntent.FollowFood : FishIntent.SwimRandomly;
        }

        public Transform GetTargetFood()
        {
            return _foodManagerService.GetClosestFood(_fishTransform.position)?.transform;
        }
    }

}
