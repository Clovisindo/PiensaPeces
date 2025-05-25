using Assets.Scripts.Fish.Player;
using UnityEngine;

namespace Assets.Scripts.Fish
{
    public interface IFishAI
    {
        public FishIntent EvaluateIntent();
    }
}
