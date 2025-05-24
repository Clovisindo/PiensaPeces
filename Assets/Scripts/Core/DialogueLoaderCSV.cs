using Assets.Scripts.Fish.Dialogue;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.Core
{
    public class DialogueLoaderCsv
    {
        public static List<FishDialogueLine> Load(string path)
        {
            var lines = File.ReadAllLines(path);
            var dialogueLines = new List<FishDialogueLine>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var split = line.Split(';'); // formato: ID;Texto;Condición

                dialogueLines.Add(new FishDialogueLine
                {
                    Id = split[0],
                    Text = split[1],
                    Condition = split.Length > 2 ? split[2] : "Always"
                });
            }

            return dialogueLines;
        }
    }
}
