using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SynergizDiag.DevicePortal;

namespace SynergizDiag.Services
{
    class NetworkingInfoServiceAPI : DiagInfoServiceAPIBase
    {

        #region API commands
        public const String API_COMMAND_WIFI_INTERFACES = "wifi/interfaces";
        public const String API_COMMAND_WIFI_NETWORKS = "wifi/networks";
        public const String API_COMMAND_WIFI_NETWORKS_PARAMETER = "interface=";
        public const String API_COMMAND_IPCONFIG = "networking/ipconfig";
        #endregion

        public const String LABEL = "Network";

        #region Delegates Converter
        private static async Task<Dictionary<string, string>> WifiNetworkToDico(Dictionary<string, string> returnedInfo, WifiInterfaces wifiInterfaces)
        {
            WifiNetworks wifiNetworkInfo = await GetInfo<WifiNetworks>(API_COMMAND_WIFI_NETWORKS, API_COMMAND_WIFI_NETWORKS_PARAMETER + wifiInterfaces.Interfaces[0].Guid);
            foreach (WifiNetworkInfo item in wifiNetworkInfo.AvailableNetworks)
            {
                if (item.IsConnected)
                {
                    returnedInfo.Add("Connected Wifi", item.Ssid);
                }
            }
            return returnedInfo;
        }

        private async static Task<Dictionary<string, string>> IPConfigToDico(Dictionary<string, string> returnedInfo, IpConfiguration ipconfigs)
        {
            int increment = 0;

            foreach (NetworkAdapterInfo item in ipconfigs.Adapters)
            {
                if (item.IpAddresses[0].Address != DevicePortalAPIHelper.EMPTY_IP)
                {

                    string name = "Network Adaptater " + increment;
                    returnedInfo.Add(name, item.Description);
                    returnedInfo.Add("-- Adaptater Type", item.AdapterType);
                    returnedInfo.Add("-- MAC Adress", item.MacAddress);
                    returnedInfo.Add("-- DHCP", item.Dhcp.ToString());
                    returnedInfo.Add("-- IP Adress", item.IpAddresses[0].ToString());
                    returnedInfo.Add("-- Gateways", item.Gateways[0].ToString());


                }
            }
            return returnedInfo;

        }
        #endregion

        /// <summary>
        /// Gather network informations by device portal API
        /// </summary>
        /// <param name="returnedInfo"></param>
        /// <returns></returns>
        public async override Task<Dictionary<string, string>> GetInfos(Dictionary<String,String> returnedInfo )
        {
            await base.GetInfos(returnedInfo);
            returnedInfo.Add("--------- " + LABEL + " ---------", "----------------");

            returnedInfo = await GetInfo<WifiInterfaces>(returnedInfo, WifiNetworkToDico, API_COMMAND_WIFI_INTERFACES);

            returnedInfo = await GetInfo<IpConfiguration>(returnedInfo, IPConfigToDico, API_COMMAND_IPCONFIG);

            return returnedInfo;
        }
    }
}
