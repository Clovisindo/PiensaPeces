using Game.Services;
using UnityEngine;

namespace Game.FishLogic
{
    public interface IFish
    {
        Transform GetTransform();
        IBoundsService GetBoundsService();
        SpriteRenderer GetSpriteRenderer();
    }
}
