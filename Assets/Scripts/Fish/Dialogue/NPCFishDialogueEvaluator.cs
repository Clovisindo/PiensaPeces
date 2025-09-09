using Game.Utilities;

namespace Game.Fishes
{
    public class NPCFishDialogueEvaluator : IDialogueEvaluator
    {
        private readonly ITimeService _timeService;
        private readonly IRandomService _randomService;
        public float currentTime => _timeService.Time;

        public NPCFishDialogueEvaluator(ITimeService timeService = null, IRandomService randomService = null)
        {
            _timeService = timeService ?? new UnityTimeService();
            _randomService = randomService ?? new UnityRandomService();
        }

        public bool Evaluate(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition) || condition == "Always")
                return true;

            if (condition.StartsWith("Time >"))
            {
                var parts = condition.Split('>');
                if (float.TryParse(parts[1].Trim(), out float minTime))
                    return currentTime > minTime;
            }
            if (condition == "Random")
                return _randomService.Value < 0.1f; // 10% chance
            return false;
        }
    }
}
