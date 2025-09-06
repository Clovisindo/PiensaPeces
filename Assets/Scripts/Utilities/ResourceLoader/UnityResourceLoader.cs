using UnityEngine;

namespace Game.Utilities
{
    public class UnityResourceLoader : IResourceLoader
    {
        public TextAsset LoadText(string path) => Resources.Load<TextAsset>(path);
    }
}
