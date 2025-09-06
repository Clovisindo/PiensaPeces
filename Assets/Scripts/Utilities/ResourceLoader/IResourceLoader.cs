using UnityEngine;

namespace Game.Utilities
{
    public interface IResourceLoader
    {
        TextAsset LoadText(string path);
    }
}
