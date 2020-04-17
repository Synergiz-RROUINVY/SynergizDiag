using SynergizDiag.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
#else
using static System.Net.WebRequestMethods;
#endif

namespace SynergizDiag
{
    public class DevicePortalAPIHelper
    {
        #region const
        public const String HTTP = "http://";
        public const String EMPTY_IP = "0.0.0.0";
        public const String API_CREDENTIAL_NAME = "ident";
        public const String API_LOGIN = "Airfoils";
        public const String API_PASSWORD = "b1j@m11987";
        public const String API_COMMAND_PREFIX = "/api/";
        public const String API_PARAMETER_PREFIX = "?";
        public const String API_LOCALHOST_ADRESS = "localhost:10080";
        public const String API_WIFI_PORT = ":80";
        #endregion
        #region attributes
        private static string wifiAdress;
        private static DeviceConnectedBy connectedBy = DeviceConnectedBy.USB;
        private static string login = API_LOGIN;
        private static string password = API_PASSWORD;
        #endregion
        #region Property
        public static string Adress
        {
            get
            {
                string returnedAdress = EMPTY_IP;
                switch (connectedBy)
                {
                    case DeviceConnectedBy.USB:
                        returnedAdress = API_LOCALHOST_ADRESS;
                        break;
                    case DeviceConnectedBy.WiFi:
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

        public static DeviceConnectedBy ConnectedBy
        {
            get
            {
                return connectedBy;
            }
        }
        #endregion

        /// <summary>
        /// Set the connection mode and adress (if needed)
        /// </summary>
        /// <param name="connectivity"></param>
        /// <param name="adress"></param>
        public static void SetConnectivity(DeviceConnectedBy connectivity, String adress = EMPTY_IP)
        {
            connectedBy = connectivity;
            Adress = adress;
        }


        public static void SetCredentials(string newLogin, string newPassword)
        {
            login = newLogin;
            password = newPassword;
        }

        /// <summary>
        /// Gather data from the HoloLens's DevicePortal API 
        /// </summary>
        /// <param name="adress">network adress of the HoloLens</param>
        /// <param name="command">API Command</param>
        /// <param name="parameter">Optionnal parameter</param>
        /// <returns></returns>
        public static async Task<string> GetInfosAsync(string command, string parameter = "")
        {
            //Build URI
            StringBuilder sb = new StringBuilder(HTTP).Append(Adress).Append(API_COMMAND_PREFIX).Append(command).Append((String.IsNullOrWhiteSpace(parameter) ? String.Empty : API_PARAMETER_PREFIX + parameter));

#if WINDOWS_UWP

            HttpBaseProtocolFilter filters = new HttpBaseProtocolFilter();
            //Set credentials for accessing API
            filters.ServerCredential = new PasswordCredential(API_CREDENTIAL_NAME, login, password);
            filters.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            filters.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filters.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);


            using (var client = new HttpClient(filters))
            {
                var response = await client.GetStringAsync(new Uri(sb.ToString()));
                return response;
            }
#else
            HttpWebRequest request = HttpWebRequest.CreateHttp(sb.ToString());
            request.Credentials = new NetworkCredential(login, password);
            WebResponse response = await request.GetResponseAsync();
            // Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            return await readStream.ReadToEndAsync();
#endif
        }


    }
}
