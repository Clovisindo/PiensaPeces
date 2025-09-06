using System;

namespace Game.Fishes
{
    public class DialoguePathResolver : IDialoguePathResolver
    {
        public string Resolver(IDialogueEvaluator evaluator, string playerPath, string npcPath)
        {
            return evaluator switch
            {
                PlayerFishDialogueEvaluator => playerPath,
                NPCFishDialogueEvaluator => npcPath,
                _ => throw new ArgumentException("Unknown evaluator type")
            };
        }
    }
}
