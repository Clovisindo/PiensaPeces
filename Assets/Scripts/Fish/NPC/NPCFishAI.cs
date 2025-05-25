using Assets.Scripts.Fish.Player;
using UnityEngine;

namespace Assets.Scripts.Fish.NPC
{
    public class NPCFishAI : IFishAI
    {
        private readonly float swimProbability;

        public NPCFishAI( float swimProbability = 0.5f)
        {
            this.swimProbability = Mathf.Clamp01(swimProbability);
        }

        public FishIntent EvaluateIntent()
        {
            return Random.value < swimProbability ? FishIntent.SwimRandomly : FishIntent.Idle;
        }
    }
}
