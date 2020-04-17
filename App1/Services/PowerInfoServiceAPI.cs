using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Tools.WindowsDevicePortal.DevicePortal;

namespace SynergizDiag.Services
{
    class PowerInfoServiceAPI:DiagInfoServiceAPIBase
    {
        public const string LABEL = "Holographic OS";


        public const string API_BATTERY_STATE = "power/battery";

        private static async Task<Dictionary<String, String>> BatteryStateToDico(Dictionary<string, string> returnedInfo, BatteryState BtSte)
        {
            returnedInfo.Add("Battery Level", ((int)BtSte.Level).ToString() + "%");
            returnedInfo.Add("Battery" ,(BtSte.IsBatteryPresent? "is present": "no battery detected") + (BtSte.IsCharging? " and charging" : " and not charging"));
            returnedInfo.Add("AC", BtSte.IsOnAcPower ? "plugged" : "not plugged");


            return returnedInfo;
        }

        public override async Task<Dictionary<string, string>> GetInfos(Dictionary<string, string> returnedInfo)
        {
            await base.GetInfos(returnedInfo);
            returnedInfo.Add("------- " + LABEL + " -------", "----------------");

            returnedInfo = await GetInfo<BatteryState>(returnedInfo, BatteryStateToDico, API_BATTERY_STATE);

            return returnedInfo;
        }

    }
}
