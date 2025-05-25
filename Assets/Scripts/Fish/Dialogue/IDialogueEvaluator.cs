namespace Assets.Scripts.Fish.Dialogue
{
    public interface IDialogueEvaluator
    {
        float currentTime { get; }

        public bool Evaluate(string condition);

    }
}
