using UnityEngine;

namespace Game.Fishes
{
    public interface ITextMeshProWrapper
    {
        float FontSize { get; set; }
        void SetText(string text);
        void ForceMeshUpdate();
        Vector2 GetRenderedValues(bool onlyVisibleCharacters);
    }
}
