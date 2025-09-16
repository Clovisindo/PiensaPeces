using UnityEngine;

namespace Game.Fishes
{
    public class UnityYieldInstruction : IYieldInstruction
    {
        public YieldInstruction WaitForSeconds(float seconds) => new WaitForSeconds(seconds);
    }
}
