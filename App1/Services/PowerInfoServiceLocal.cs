using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Devices.Enumeration;
using Windows.Devices.Power;
#endif
namespace SynergizDiag.Services
{
    class PowerInfoServiceLocal : DiagInfoServiceBase
    {
        public const string LABEL = "Battery";
#if WINDOWS_UWP
        private BatteryReport batteryReport;

        async private void RequestIndividualBatteryReports()
        {
            // Find batteries 
            var deviceInfo = await DeviceInformation.FindAllAsync(Battery.GetDeviceSelector());
            foreach (DeviceInformation device in deviceInfo)
            {
                try
                {
                    // Create battery object
                    var battery = await Battery.FromIdAsync(device.Id);

                    // Get report
                    batteryReport = battery.GetReport();
                    
                }
                catch { /* Add error handling, as applicable */ }
            }
        }
#endif
        public async override Task<Dictionary<string, string>> GetInfos(Dictionary<string, string> returnedInfo)
        {
            returnedInfo = await base.GetInfos(returnedInfo);
            returnedInfo.Add("------- " + LABEL + " -------", "----------------");
#if WINDOWS_UWP

            RequestIndividualBatteryReports();
            returnedInfo.Add("Battery Level", ((int)( (float)batteryReport.RemainingCapacityInMilliwattHours / (float)batteryReport.FullChargeCapacityInMilliwattHours * 100)).ToString() + "%");
            returnedInfo.Add("Battery State", batteryReport.Status.ToString());
#endif
            return returnedInfo;
        }
    }

}
