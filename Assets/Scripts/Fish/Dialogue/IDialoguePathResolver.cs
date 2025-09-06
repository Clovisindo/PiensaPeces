namespace Game.Fishes
{ 
    public interface IDialoguePathResolver
    {
        string Resolver(IDialogueEvaluator evaluator, string playerPath, string npcPatch);
    }
}
