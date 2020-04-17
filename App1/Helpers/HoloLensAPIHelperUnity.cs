using SynergizDiag;
using SynergizDiag.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SynergizDiag
{

    public class HoloLensAPIHelperUnity:MonoBehaviour 

    {
        #region const
        public const String HTTP = "http://";
        public const String EMPTY_IP = "0.0.0.0";
        public const String API_CREDENTIAL_NAME = "ident";
        public const String API_LOGIN = "portal";
        public const String API_PASSWORD = "hololSyn2*";
        public const String API_COMMAND_PREFIX = "/api/";
        public const String API_PARAMETER_PREFIX = "?";
        public const String API_LOCALHOST_ADRESS = "localhost:10080";
        public const String API_WIFI_PORT = ":80";
        #region API commands
        public const String API_COMMAND_WIFI_INTERFACES = "wifi/interfaces";
        public const String API_COMMAND_WIFI_NETWORKS = "wifi/networks";
        public const String API_COMMAND_WIFI_NETWORKS_PARAMETER = "interface=";
        public const String API_COMMAND_IPCONFIG = "networking/ipconfig";
        #endregion
        #endregion
        private static string wifiAdress;
        private static HoloLensConnectedBy connectedBy = HoloLensConnectedBy.USB;
        private static String result = String.Empty;

        public static string Adress
        {
            get
            {
                string returnedAdress = EMPTY_IP;
                switch (connectedBy)
                {
                    case HoloLensConnectedBy.USB:
                        returnedAdress = API_LOCALHOST_ADRESS;
                        break;
                    case HoloLensConnectedBy.WiFi:
                        returnedAdress = wifiAdress + API_WIFI_PORT;
                        break;
                    default:
                        break;
                }
                return returnedAdress;
            }
            set
            {
                wifiAdress = value;
            }
        }

        public static void SetConnectivity(HoloLensConnectedBy connectivity, String adress = EMPTY_IP)
        {
            DiagInfos.WriteDebug("Changing Connectivity", connectivity.ToString() + " " + adress);
            connectedBy = connectivity;
            Adress = adress;
            DiagInfos.WriteDebug("Connectivity changed", connectedBy.ToString() + " " + Adress);
        }



        public static IEnumerator GetInfos(string command, string parameter = "")
        {
            DiagInfos.WriteDebug("UnityWebRequest", "accessing");
            WaitForSeconds waitTime = new WaitForSeconds(2f); //Do the memory allocation once

            string authorization = authenticate(API_LOGIN, API_PASSWORD);

            StringBuilder sb = new StringBuilder(HTTP).Append(Adress).Append(API_COMMAND_PREFIX).Append(command).Append((String.IsNullOrWhiteSpace(parameter) ? String.Empty : API_PARAMETER_PREFIX + parameter));
            DiagInfos.WriteDebug("UnityWebRequest - Url", sb.ToString());

            UnityWebRequest request = UnityWebRequest.Get(sb.ToString());
            request.SetRequestHeader("AUTHORIZATION", authorization);
            UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
             result = request.downloadHandler.text;
            DiagInfos.WriteDebug("Result", result);
            yield return result;


        }

        private static string authenticate(string username, string password)
        {
            string auth = username + ":" + password;
            auth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        public  async Task<String> GetInfosASync(string command, string parameter = "")
        {
            try
            {
                StartCoroutine(GetInfos(command, parameter));
                while (result == String.Empty)
                {
                    await Task.Delay(1000);
                }

            }
            catch (Exception e)
            {

                DiagInfos.WriteDebug("exception", e.StackTrace);

            }
            return result;

        }

    }
}
