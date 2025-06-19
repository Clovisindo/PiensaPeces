using Assets.Scripts.Components;
using UnityEngine;

namespace Assets.Scripts.Fish.Dialogue
{
    public class PlayerFishDialogueEvaluator: IDialogueEvaluator
    {
        private readonly HungerComponent hungerComponent;
        private readonly int daysPassed;
        public float currentTime => Time.time;

        public PlayerFishDialogueEvaluator(HungerComponent hungerComponent, int daysPassed)
        {
            this.hungerComponent = hungerComponent;
            this.daysPassed = daysPassed;
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

            if (condition == "IsHungry")
                return hungerComponent.IsHungry;

            if (condition == "Random")
                return Random.value < 0.1f; // 10% chance
            if (condition == "Day1" && daysPassed == 1)
                return true;
            if (condition == "Day2" && daysPassed == 2)
                return true;

            return false;
        }
    }

}
