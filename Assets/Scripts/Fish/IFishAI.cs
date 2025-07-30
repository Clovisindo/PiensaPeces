using UnityEngine;

namespace Game.Fishes
{
    public interface IFishAI
    {
        public FishIntent EvaluateIntent();
    }
}
