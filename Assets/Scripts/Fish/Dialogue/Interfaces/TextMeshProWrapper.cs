using TMPro;
using UnityEngine;

namespace Game.Fishes
{
    public class TextMeshProWrapper : ITextMeshProWrapper
    {
        private readonly TextMeshProUGUI _textMesh;
        public TextMeshProWrapper(TextMeshProUGUI textMesh) => _textMesh = textMesh;
        public float FontSize { get => _textMesh.fontSize; set => _textMesh.fontSize = value; }
        public void SetText(string text) => _textMesh.SetText(text);
        public void ForceMeshUpdate() => _textMesh.ForceMeshUpdate();
        public Vector2 GetRenderedValues(bool onlyVisibleCharacters) => _textMesh.GetRenderedValues(onlyVisibleCharacters);
    }
}
