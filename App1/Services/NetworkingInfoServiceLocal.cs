﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Networking;
using Windows.Networking.Connectivity;
#endif

namespace SynergizDiag.Services
{
    class NetworkingInfoServiceLocal : DiagInfoServiceBase
    {
#if WINDOWS_UWP
        private Dictionary<Guid, WiFiAdapter> WifiAdaptaters;
#else
#endif
        private int incrementEthernet = 0, incrementWifi = 0, incrementUnknow = 0, increment = 0;
        private char increm = 'a';

        public const string LABEL = "Network";

        /// <summary>
        /// Gather Network adaptater infos
        /// </summary>
        /// <param name="returnedInfo">returned Dictionnary filled with new info</param>
        /// <returns>Dictionnary<String-String> : network informations</returns>
        public async override Task<Dictionary<string, string>> GetInfos(Dictionary<String, String> returnedInfo)
        {
            returnedInfo = await base.GetInfos(returnedInfo);
            returnedInfo.Add("------- " + LABEL + " -------", "----------------");
            incrementEthernet = 0;
            incrementUnknow = 0;
            incrementWifi = 0;
            increm = 'a';
#if WINDOWS_UWP
            //Ask permission
            var access = await WiFiAdapter.RequestAccessAsync();

            if (access != WiFiAccessStatus.Allowed)
            {
                returnedInfo.Add("Error", "Wifi Acces denied");
            }
            else
            {
                //Get the list of Wifi Adaptaters
                var DeviceswifiAdaptaters = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                WifiAdaptaters = new Dictionary<Guid, WiFiAdapter>();
                //Transfer list in a more convenient Dictionnary
                foreach (WiFiAdapter item in await WiFiAdapter.FindAllAdaptersAsync())
                {
                    WifiAdaptaters.Add(item.NetworkAdapter.NetworkAdapterId, item);
                }
                //Get the list of network connection
                var connectedProfiles = NetworkInformation.GetConnectionProfiles().ToList();
                //Switch is irrelevant now but can be useful if more specific treatment is needed
                foreach (var item in NetworkInformation.GetHostNames())
                {
                    switch (item.Type)
                    {
                        case HostNameType.DomainName:
                            if (!item.DisplayName.Contains(".local"))
                            {
                                returnedInfo.Add("Machine Name", item.DisplayName);
                            }
                            break;
                        case HostNameType.Ipv4:
                        case HostNameType.Ipv6:
                        case HostNameType.Bluetooth:
                            returnedInfo = await getAdaptaterType(item,returnedInfo);
                            returnedInfo.Add(increm + "- Adress : ", item.DisplayName);
                            returnedInfo.Add(increm + "- Network Type : ", getNetworkType(item));
                            increment++;
                            increm++;
                            break;
                    }
                }
            }
#endif
            return returnedInfo;
        }

#if WINDOWS_UWP
        /// <summary>
        /// Retrieve SSID of connected wifi network
        /// </summary>
        /// <param name="wiFiAdapter"></param>
        /// <returns>String - SSID</returns>
        private static async Task<String> getConnectedWifi(WiFiAdapter wiFiAdapter)
        {

            await wiFiAdapter.ScanAsync();
            ConnectionProfile connectedProfile = await wiFiAdapter.NetworkAdapter.GetConnectedProfileAsync();
            return connectedProfile.GetNetworkNames()[0];
        }
        /// <summary>
        /// Gather adaptater informations associated to a hostname 
        /// </summary>
        /// <param name="item">hostname network adaptater to analyse</param>
        /// <param name="returnedInfo">returned Dictionnary filled with new info</param>
        /// <returns></returns>
        private async Task<Dictionary<String, String>> getAdaptaterType(HostName item, Dictionary<String, String> returnedInfo)
        {
            switch (item.IPInformation.NetworkAdapter.IanaInterfaceType)
            {
                case 6:
                    returnedInfo.Add("*** Network Adaptater " + increm + " ***", "Ethernet Adaptater " + incrementEthernet);
                    incrementEthernet++;
                    break;
                case 71:
                    returnedInfo.Add("*** Network Adaptater " + increm + " ***", "Wifi Adaptater " + incrementWifi);
                    incrementWifi++;
                    //In case of wifi network, add the connected network SSID
                    returnedInfo.Add(increm + "- Connected Wifi : ", await getConnectedWifi(WifiAdaptaters[item.IPInformation.NetworkAdapter.NetworkAdapterId]));
                    break;
                default:
                    returnedInfo.Add("*** Network Adaptater " + increm + " ***", "Unknown Type Adaptater " + incrementUnknow);
                    incrementUnknow++;
                    break;
            }
            
            return returnedInfo;
        }

        /// <summary>
        /// Get the type of network associated to a hostname
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static String getNetworkType(HostName item)
        {
            return item.IPInformation.NetworkAdapter.NetworkItem.GetNetworkTypes().ToString();
        }

#else
#endif
    }
}
