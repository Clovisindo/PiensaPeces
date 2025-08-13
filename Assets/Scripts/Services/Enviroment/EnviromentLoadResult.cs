using Game.Data;

namespace Game.Services
{
    public class EnvironmentLoadResult
    {
        public LoadDataContext Context { get; set; }
        public GroundEnvironmentDayConfig SelectedGroundConfig { get; set; }
        public FishEnvDayConfig SelectedFishConfig { get; set; }
        public FoodEnvDayConfig SelectedFoodConfig { get; set; }
        public AudioEnvDayConfig SelectedAudioConfig { get; set; }
    }
}
