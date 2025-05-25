using UnityEngine;

namespace Assets.Scripts.Fish.Dialogue
{
    public class NPCFishDialogueEvaluator : IDialogueEvaluator
    {
        public float currentTime => Time.time;

        public NPCFishDialogueEvaluator() { }

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
                return Random.value < 0.1f; // 10% chance

            return false;
        }
    }
}
