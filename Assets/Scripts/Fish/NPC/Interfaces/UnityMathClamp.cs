using UnityEngine;

namespace Game.Fishes
{
    public class UnityMathClamp : IMathClamp
    {
        public float MathfClamp(float x)=> Mathf.Clamp01(x);
    }
}
