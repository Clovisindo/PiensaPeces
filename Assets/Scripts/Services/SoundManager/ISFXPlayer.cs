using Game.Data;

namespace Game.Services
{
    public interface ISFXPlayer
    {
        void PlayPitched(AudioEmitterData[] effects, AudioEmitterData requested, float pitchMin, float pitchMax);
    }
}
