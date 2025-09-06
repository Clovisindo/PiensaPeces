namespace Game.Utilities
{
    public class UnityRandomService : IRandomService
    {
        public float Range(float min, float max) => UnityEngine.Random.Range(min, max);

        public int Range(int min, int max) => UnityEngine.Random.Range(min, max);
    }
}
