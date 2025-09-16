using UnityEngine;

namespace Game.Fishes
{
    public interface IYieldInstruction
    {
        YieldInstruction WaitForSeconds(float seconds);
    }
}
