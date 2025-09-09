namespace Game.Fishes
{
    public interface IDialogueEvaluator
    {
        float currentTime { get; }
        public bool Evaluate(string condition);

    }
}
