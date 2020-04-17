using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_UWP

using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

#endif
namespace SynergizDiag.Services
{
    class SystemInfoServiceLocal : DiagInfoServiceBase
    {
        public const string LABEL = "System";
        private static string parseSystemVersion(string sv)
        {
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);
            string SystemVersion = $"{v1}.{v2}.{v3}.{v4}";
            return SystemVersion;
        }
        /// <summary>
        /// Get System information by intern class
        /// </summary>
        /// <param name="returnedInfo">returned Dictionnary filled with new info</param>
        /// <returns></returns>
        public override async Task<Dictionary<string, string>> GetInfos(Dictionary<String, String> returnedInfo)
        {
            returnedInfo = await base.GetInfos(returnedInfo);
            returnedInfo.Add("--------- " + LABEL + " ---------", "----------------");

#if WINDOWS_UWP
            AnalyticsVersionInfo ai = AnalyticsInfo.VersionInfo;

            // get the system family information
            returnedInfo.Add("Device Family", ai.DeviceFamily);

            returnedInfo.Add("Device Family Version", ai.DeviceFamilyVersion);

            // get the system version number
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;

            returnedInfo.Add("System Version", parseSystemVersion(sv));
            // get the device manufacturer and model name
            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            returnedInfo.Add("Device Manufacturer", eas.SystemManufacturer);
            returnedInfo.Add("Device Model", eas.SystemProductName);

            // get the device manufacturer, model name, OS details etc.
            returnedInfo.Add("Operating System", eas.OperatingSystem);

            returnedInfo.Add("Friendly Name", eas.FriendlyName);

#endif
            return returnedInfo;
        }
    }
}
