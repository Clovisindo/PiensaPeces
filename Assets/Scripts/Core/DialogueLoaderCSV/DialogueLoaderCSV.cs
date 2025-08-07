using Game.Data;
using System.Collections.Generic;
using System.IO;

namespace Game.Core
{
    public class DialogueLoaderCsv
    {
        public static List<FishDialogueLine> Load(string csvText)
        {
            var lines = new List<FishDialogueLine>();
            using (StringReader reader = new StringReader(csvText))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                    {
                        lines.Add(new FishDialogueLine
                        {
                            Id = parts[0],
                            Text = parts[1],
                            Condition = parts.Length > 2 ? parts[2] : "Always"
                        });
                    }
                }
            }
            return lines;
        }
    }
}
