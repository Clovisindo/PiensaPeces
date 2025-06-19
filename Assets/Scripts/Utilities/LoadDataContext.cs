using Assets.Scripts.Fish.NPC;

namespace Assets.Scripts.Utilities
{
    public class LoadDataContext
    {
        public FishConfig[] FishConfigsCurrentDay { get; }


        public LoadDataContext(FishConfig[] fishConfigsCurrentDay)
        {
            FishConfigsCurrentDay = fishConfigsCurrentDay;
        }
    }
}
