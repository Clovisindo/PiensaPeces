using Game.Utilities;

namespace Game.Fishes
{
    public class NPCFishAI : IFishAI
    {
        private readonly float _swimProbability;
        private IRandomService _randomService;
        private IMathClamp _mathClamp;

        public NPCFishAI( float swimProbability = 0.5f, IMathClamp mathClamp = null, IRandomService randomService = null)
        {

            _randomService = randomService ?? new UnityRandomService();
            _mathClamp = mathClamp ?? new UnityMathClamp();
            _swimProbability = _mathClamp.MathfClamp(swimProbability);
        }

        public FishIntent EvaluateIntent()
        {
            return _randomService.Value < _swimProbability ? FishIntent.SwimRandomly : FishIntent.Idle;
        }

    }
}
