using Assets.Scripts.Fish.NPC;
using Assets.Scripts.Services.Enviroment;
using System;

namespace Assets.Scripts.Utilities
{
    public class LoadDataContext
    {
        public FishConfig[] FishConfigsCurrentDay { get; }
        public FoodEnvConfig[] FoodConfigsCurrentDay { get; }

        private LoadDataContext(FishConfig[] fishConfigsCurrentDay, FoodEnvConfig[] foodConfigsCurrentDay)
        {
            FishConfigsCurrentDay = fishConfigsCurrentDay;
            FoodConfigsCurrentDay = foodConfigsCurrentDay;
        }

        // Builder interno
        public class Builder
        {
            private FishConfig[] _fishConfigsCurrentDay = Array.Empty<FishConfig>();
            private FoodEnvConfig[] _foodConfigsCurrentDay = Array.Empty<FoodEnvConfig>();

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

            public LoadDataContext Build()
            {
                return new LoadDataContext(_fishConfigsCurrentDay, _foodConfigsCurrentDay);
            }
        }
    }
}
