using Game.Data;
using System;

namespace Game.Data
{
    public class LoadDataContext
    {
        public FishConfig[] FishConfigsCurrentDay { get; }
        public FoodEnvConfig[] FoodConfigsCurrentDay { get; }
        public AudioEnvConfig[] AudioConfigsCurrentDay { get; }

        private LoadDataContext(FishConfig[] fishConfigsCurrentDay, FoodEnvConfig[] foodConfigsCurrentDay, AudioEnvConfig[] audioConfigsCurrentDay)
        {
            FishConfigsCurrentDay = fishConfigsCurrentDay;
            FoodConfigsCurrentDay = foodConfigsCurrentDay;
            AudioConfigsCurrentDay = audioConfigsCurrentDay;
        }

        // Builder interno
        public class Builder
        {
            private FishConfig[] _fishConfigsCurrentDay = Array.Empty<FishConfig>();
            private FoodEnvConfig[] _foodConfigsCurrentDay = Array.Empty<FoodEnvConfig>();
            private AudioEnvConfig[] _audioConfigsCurrentDay = Array.Empty<AudioEnvConfig>();

            public Builder WithFishConfigs(FishConfig[] fishConfigs)
            {
                _fishConfigsCurrentDay = fishConfigs ?? Array.Empty<FishConfig>();
                return this;
            }

            public Builder WithFoodConfigs(FoodEnvConfig[] foodConfigs)
            {
                _foodConfigsCurrentDay = foodConfigs ?? Array.Empty<FoodEnvConfig>();
                return this;
            }

            public Builder WithAudioConfigs(AudioEnvConfig[] audioConfigs)
            {
                _audioConfigsCurrentDay = audioConfigs ?? Array.Empty<AudioEnvConfig>();
                return this;
            }

            public LoadDataContext Build()
            {
                return new LoadDataContext(_fishConfigsCurrentDay, _foodConfigsCurrentDay, _audioConfigsCurrentDay);
            }
        }
    }
}
