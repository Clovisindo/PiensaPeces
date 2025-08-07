using System;
using System.IO;
using UnityEngine;

namespace Game.Core
{
    public class SaveSystem
    {
        private readonly IFileStorage fileStorage;
        private readonly string localMarkerPath;

        public SaveSystem(IFileStorage fileStorage, string customPath = null)
        {
            this.fileStorage = fileStorage;

            if (string.IsNullOrEmpty(customPath))
            {
                string folderPath = Path.Combine(Application.dataPath, "../GameFiles");
                Directory.CreateDirectory(folderPath);
                localMarkerPath = Path.Combine(folderPath, "first_launch.marker");
            }
            else
            {
                localMarkerPath = customPath;
            }
        }

        public bool IsFirstLaunch()
        {
            if (!fileStorage.Exists(localMarkerPath))
            {
                fileStorage.WriteAllText(localMarkerPath, DateTime.Now.ToBinary().ToString());
                return true;
            }
            return false;
        }

        public DateTime GetFirstLaunchDate()
        {
            if (!fileStorage.Exists(localMarkerPath))
            {
                var now = DateTime.Now;
                fileStorage.WriteAllText(localMarkerPath, now.ToBinary().ToString());
                return now;
            }

            var content = fileStorage.ReadAllText(localMarkerPath);
            return DateTime.FromBinary(Convert.ToInt64(content));
        }

        public void ResetFirstLaunch()
        {
            if (fileStorage.Exists(localMarkerPath))
                fileStorage.Delete(localMarkerPath);
        }

        /// <summary>
        /// Permite setear manualmente la fecha de primera ejecución (para pruebas/debug).
        /// </summary>
        public void SetFirstLaunchDate(DateTime date)
        {
            fileStorage.WriteAllText(localMarkerPath, date.ToBinary().ToString());
        }

        /// <summary>
        /// Devuelve la cantidad de días desde la primera ejecución. El primer día cuenta como día 1.
        /// </summary>
        public int GetDaysSinceFirstLaunch()
        {
            var firstLaunch = GetFirstLaunchDate();
            var days = (DateTime.Now - firstLaunch).Days + 1;
            return days;
        }
    }
}
