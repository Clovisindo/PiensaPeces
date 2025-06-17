using System;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class SaveSystem
    {
        private const string FirstLaunchKey = "FirstLaunchDate";

        public DateTime GetFirstLaunchDate()
        {
            if(PlayerPrefs.HasKey(FirstLaunchKey))
            {
                long binary = Convert.ToInt64(PlayerPrefs.GetString(FirstLaunchKey));
                return DateTime.FromBinary(binary);
            }
            else 
            {
                DateTime actualDate = DateTime.Now;
                PlayerPrefs.SetString(FirstLaunchKey, actualDate.ToBinary().ToString());
                PlayerPrefs.Save();
                return actualDate;
            }
        }
        //ToDo: borrar, debug
        public void SetFirstLaunchDate( DateTime actualDate)
        {
            PlayerPrefs.SetString(FirstLaunchKey, actualDate.ToBinary().ToString());
            PlayerPrefs.Save();
        }

        public int GetDaysSinceFirstLaunch()
        {
            DateTime firstLaunch = GetFirstLaunchDate();
            return (DateTime.Now - firstLaunch).Days;
        }

        public void ResetFirstLaunch()
        {
            PlayerPrefs.DeleteKey(FirstLaunchKey);
            PlayerPrefs.Save();
        }

    }
}
