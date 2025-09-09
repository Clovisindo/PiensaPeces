namespace Game.Utilities
{
    public interface IRandomService
    {
        float Value { get; }
        float Range(float min, float max);
        int Range( int min, int max);
    }
}
