using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Tools.WindowsDevicePortal.DevicePortal;

namespace SynergizDiag.Services
{
    class HolographicOsInfoServiceAPI:DiagInfoServiceAPIBase
    {
        #region API Commands
        public const string API_HOLOGRAPHIC_IPD = "holographic/os/settings/ipd";
        public const string API_HOLOGRAPHIC_SERVICES = "holographic/os/services";
        public const string API_HOLOGRAPHIC_WEBMANAGEMENT_HTTP_SETTINGS = "holographic/os/webmanagement/settings/https";
        #endregion

        #region Delegates Converter
        private static async Task<Dictionary<String, String>> IPDToDico(Dictionary<string, string> returnedInfo, InterPupilaryDistance ipd)
        {
            returnedInfo.Add("Inter Pupilary Distance", ipd.Ipd.ToString());
            returnedInfo.Add("Inter Pupilary Distance Raw", ipd.IpdRaw.ToString());

            return returnedInfo;
        }

        private static async Task<Dictionary<String, String>> HTTPSSettingsToDico(Dictionary<string, string> returnedInfo, WebManagementHttpSettings httpsSettings)
        {
            returnedInfo.Add("HTTPS Required for WebManagement", httpsSettings.IsHttpsRequired.ToString());

            return returnedInfo;
        }

        private static async Task<Dictionary<String, String>> ServicesToDico(Dictionary<string, string> returnedInfo, HolographicServices holoSrv)
        {
            returnedInfo.Add("--- Services ---","Observed / Expected");
            returnedInfo.Add("-- DWM.exe", new StringBuilder(holoSrv.Status.Dwm.Observed.ToString()).Append(" / ").Append(holoSrv.Status.Dwm.Expected.ToString()).ToString() );
            returnedInfo.Add("-- HoloShellApp.exe", new StringBuilder(holoSrv.Status.HoloShellApp.Observed.ToString()).Append(" / ").Append(holoSrv.Status.HoloShellApp.Expected.ToString()).ToString());
            returnedInfo.Add("-- HoloSi.exe", new StringBuilder(holoSrv.Status.HoloSi.Observed.ToString()).Append(" / ").Append(holoSrv.Status.HoloSi.Expected.ToString()).ToString());
            returnedInfo.Add("-- MixedRealitytCapture.exe", new StringBuilder(holoSrv.Status.MixedRealitytCapture.Observed.ToString()).Append(" / ").Append(holoSrv.Status.MixedRealitytCapture.Expected.ToString()).ToString());
            returnedInfo.Add("-- SiHost.exe", new StringBuilder(holoSrv.Status.SiHost.Observed.ToString()).Append(" / ").Append(holoSrv.Status.SiHost.Expected.ToString()).ToString());
            returnedInfo.Add("-- Spectrum.exe", new StringBuilder(holoSrv.Status.Spectrum.Observed.ToString()).Append(" / ").Append(holoSrv.Status.Spectrum.Expected.ToString()).ToString());


            return returnedInfo;
        }



        #endregion
        public const string LABEL = "Holographic OS";

        public override async Task<Dictionary<string, string>> GetInfos(Dictionary<string, string> returnedInfo)
        {
            await base.GetInfos(returnedInfo);
            returnedInfo.Add("--------- " + LABEL + " ---------", "----------------");

            returnedInfo = await GetInfo<InterPupilaryDistance>(returnedInfo, IPDToDico, API_HOLOGRAPHIC_IPD);
            returnedInfo = await GetInfo<WebManagementHttpSettings>(returnedInfo, HTTPSSettingsToDico, API_HOLOGRAPHIC_WEBMANAGEMENT_HTTP_SETTINGS);
            returnedInfo = await GetInfo<HolographicServices>(returnedInfo, ServicesToDico, API_HOLOGRAPHIC_SERVICES);

            return returnedInfo;
        }
    }
}
