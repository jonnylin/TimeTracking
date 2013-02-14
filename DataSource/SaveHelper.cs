using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataSource
{
    class Utils
    {
        public static ApplicationData ApplicationData = null;
        public static ApplicationDataContainer RoamingSettings = null;
        //public static void SaveSettingToRoaming(string containerName, string keyName, object itemToSave)
        //{
        //    // Load local and roamingSettings
        //    RoamingSettings = Utils.ApplicationData.Current.RoamingSettings;
        //    if (RoamingSettings.Containers.ContainsKey(containerName))
        //    {
        //        RoamingSettings.Containers[containerName].Values[keyName] = itemToSave;
        //    }
        //    else
        //    {
        //        RoamingSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        //        RoamingSettings.Containers[containerName].Values[keyName] = itemToSave;
        //    }
        //}
        //public static object GetSettingFromRoaming(string containerName, string keyName)
        //{
        //    // Load local and roamingSettings
        //    RoamingSettings = Utils.ApplicationData.Current.RoamingSettings;
        //    if (RoamingSettings.Containers.ContainsKey(containerName))
        //    {
        //        if (RoamingSettings.Containers[containerName].Values.ContainsKey(keyName))
        //        {
        //            return RoamingSettings.Containers[containerName].Values[keyName];
        //        }
        //    }
        //    return null;
        //}
    }
}
