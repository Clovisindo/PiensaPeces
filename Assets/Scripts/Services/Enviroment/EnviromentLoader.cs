using Game.Data;
using Game.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Game.Services
{
    public class EnviromentLoader : IEnviromentLoader
    {
        private readonly ILogger _logger;

        public EnviromentLoader(ILogger logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Carga los configs para el dia concreto, si no hay, carga los del ultimo dia
        /// </summary>
        /// <param name="groundConfigs"></param>
        /// <param name="fishConfigs"></param>
        /// <param name="foodConfigs"></param>
        /// <param name="audioConfigs"></param>
        /// <param name="daysPassed"></param>
        /// <returns></returns>
        public EnvironmentLoadResult Load(List<GroundEnvironmentDayConfig> groundConfigs, List<FishEnvDayConfig> fishConfigs, List<FoodEnvDayConfig> foodConfigs, List<AudioEnvDayConfig> audioConfigs, int daysPassed)
        {
            var builder = new LoadDataContext.Builder();

            var selectedGround = GetConfigForDay(groundConfigs,daysPassed);
            var selectedFish = GetConfigForDay(fishConfigs, daysPassed);
            var selectedFood = GetConfigForDay(foodConfigs, daysPassed);
            var selectedAudio = GetConfigForDay(audioConfigs, daysPassed);

            if (selectedFish != null)
                builder.WithFishConfigs(selectedFish?.fishEnvDayConfigs.ToArray());
            if (selectedFood != null)
                builder.WithFoodConfigs(selectedFood?.foodEnvConfigs.ToArray());
            if (selectedAudio != null)
                builder.WithAudioConfigs(selectedAudio?.audioConfigs.ToArray());

            return new EnvironmentLoadResult
            {
                Context = builder.Build(),
                SelectedGroundConfig = selectedGround,
                SelectedFishConfig = selectedFish,
                SelectedFoodConfig = selectedFood,
                SelectedAudioConfig = selectedAudio
            };
        }


        private T GetConfigForDay<T>(List<T> configs, int daysPassed) where T : EnvDayConfigBase
        {
            if (configs == null || configs.Count == 0)
            {
                _logger.LogWarning($"No {typeof(T).Name} Config found for day {daysPassed}, load last day data.");
                return null;
            }

            var found = configs.Find(c => c.dayNumber == daysPassed);
            if (found != null) return found;

            // si no hay, devolvemos el ultimo de los elementos.
            return configs.OrderByDescending(c => c.dayNumber).FirstOrDefault();
        }
    }
}
