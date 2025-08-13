using Game.Data;
using System.Collections.Generic;

namespace Game.Services
{
    public interface IEnviromentLoader
    {
        EnvironmentLoadResult Load(
            List<GroundEnvironmentDayConfig> groundConfigs,
            List<FishEnvDayConfig> fishConfigs,
            List<FoodEnvDayConfig> foodConfigs,
            List<AudioEnvDayConfig> audioConfigs,
            int daysPassed);
    }
}
