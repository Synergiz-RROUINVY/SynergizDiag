using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Tools.WindowsDevicePortal.DevicePortal;

namespace SynergizDiag.Services
{
    class SystemInfoServiceAPI : DiagInfoServiceAPIBase
    {
        #region API Commands
        public const string API_COMMAND_DEVICE_FAMILY = "os/devicefamily";
        public const string API_COMMAND_MACHINE_NAME = "os/machinename";
        public const string API_COMMAND_OS_INFO = "os/info";
        #endregion
        public const string LABEL = "System";


        public async override Task<Dictionary<string, string>> GetInfos(Dictionary<String, String> returnedInfo)
        {
            await base.GetInfos(returnedInfo);
            returnedInfo.Add("--------- " + LABEL + " ---------", "----------------");

            returnedInfo = await GetInfo<DeviceOsFamily>(returnedInfo, DeviceOsFamilyToDico, API_COMMAND_DEVICE_FAMILY);
            returnedInfo = await GetInfo<OperatingSystemInformation>(returnedInfo, OSInfoToDico, API_COMMAND_OS_INFO);
            returnedInfo = await GetInfo<DeviceName>(returnedInfo, DeviceNameToDico, API_COMMAND_MACHINE_NAME);

            return returnedInfo;
        }
        #region Delegates Converter

        private static async Task<Dictionary<String,String>> OSInfoToDico(Dictionary<string, string> returnedInfo, OperatingSystemInformation operatingSystem)
        {
            returnedInfo.Add("OS Edition", operatingSystem.OsEdition);
            returnedInfo.Add("OS Version", operatingSystem.OsVersionString);
            returnedInfo.Add("Platform", operatingSystem.PlatformName);
            returnedInfo.Add("Language", operatingSystem.Language);

            return returnedInfo;
        }

        private static async Task<Dictionary<String, String>> DeviceOsFamilyToDico(Dictionary<string, string> returnedInfo, DeviceOsFamily osFamily)
        {
            returnedInfo.Add("Device Os Family", osFamily.Family);
            return returnedInfo;

        }

        private static async Task<Dictionary<String, String>> DeviceNameToDico(Dictionary<string, string> returnedInfo, DeviceName machineName)
        {
            returnedInfo.Add("Machine Name", machineName.Name);
            return returnedInfo;

        }
        #endregion
    }
}
