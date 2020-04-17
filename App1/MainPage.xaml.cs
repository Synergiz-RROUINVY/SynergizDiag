using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Web.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http.Filters;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Certificates;
using Newtonsoft.Json;
using static SynergizDiag.DevicePortal;
using System.Text;
using SynergizDiag.Services;
using System.Net;
using SynergizDiag.Enums;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SynergizDiag
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
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

        public MainPage()
        {
            this.InitializeComponent();
            if (AnalyticsInfo.VersionInfo.DeviceFamily.Contains("Desktop"))
            {
                TgSwConnection.IsEnabled = true;
            }
            else
            {
                TgSwConnection.IsEnabled = false;
            }
        }

        #region Attributes
        public Dictionary<String, String> Infos = new Dictionary<String, String>();

        private Dictionary<CollectingType, List<DiagInfoServiceBase>> service;

        private WiFiAdapter firstAdapter;
        #endregion


        #region Button click
        public async void BtnPlouf_ClickAsync(object sender, RoutedEventArgs e)
        {
            await GetInfo(CollectingType.ByLocalClass);
            //await FindInfosAsync();

        }

        private async void BtnPloufAPI_Click(object sender, RoutedEventArgs e)
        {
            await GetInfo(CollectingType.ByAPI);
        }

        private async Task GetInfo(CollectingType collectingType)
        {
            StkInfo.Children.Clear();
            Infos.Clear();
            //Waiting Message
            SetTitle("Loading Infos...");
            try
            {
                DiagManager manager = new DiagManager();
                manager.CollectingBy = collectingType;
                if (collectingType == CollectingType.ByAPI)
                {
                    manager.SetConnectivity(TgSwConnection.IsOn ? DeviceConnectedBy.WiFi : DeviceConnectedBy.USB, TxBxAdress.Text);
                }
                Infos = await manager.GetAllInfos();
                StkInfo.Children.Clear();
                SetTitle("Infos Loaded");
                WriteInfos();
            }
            catch (Exception ex)
            {
                SetTitle("Error : " + ex.Message);
            }
        }
        #endregion
        #region UI
        /// <summary>
        /// Add a TextBlock to the content StackPanel
        /// </summary>
        /// <param name="Title"></param>
        private void SetTitle(String Title)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = Title;
            textBlock.FontSize = 30;
            textBlock.TextAlignment = TextAlignment.Center;
            StkInfo.Children.Add(textBlock);
        }

        /// <summary>
        /// Display data contained in Infos Dictionnary 
        /// </summary>
        private void WriteInfos()
        {
            foreach (String item in Infos.Keys)
            {
                TextBlockPair plouf = new TextBlockPair();
                plouf.Text1 = item;
                plouf.Text2 = Infos[item];
                StkInfo.Children.Add(plouf);
            }
            /*if (DiagInfos.Infos.Count > 0)
            {
                foreach (String item in DiagInfos.Infos.Keys)
                {
                    TextBlockPair plouf = new TextBlockPair();
                    plouf.Text1 = item;
                    plouf.Text2 = Infos[item];
                    StkInfo.Children.Add(plouf);
                }

            }*/
        }

        /// <summary>
        /// Enable/disable the adress entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TgSwConnection_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (TxBxAdress != null)
            {
                if (toggleSwitch.IsOn)
                {
                    TxBxAdress.IsEnabled = true;
                }
                else
                {
                    TxBxAdress.IsEnabled = false;
                }


            }
        }


        #endregion

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
