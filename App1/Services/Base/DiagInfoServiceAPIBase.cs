using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynergizDiag.Services
{
    public abstract class DiagInfoServiceAPIBase : DiagInfoServiceBase
    {

        protected static async Task<Dictionary<string, string>> GetInfo<T>(Dictionary<string, string> returnedInfo, ObjectToDico<T> converter, string command, string parameter = "")
        {
            var rawResult = await DevicePortalAPIHelper.GetInfosAsync(command, parameter) ;
            T result = JsonConvert.DeserializeObject<T>(rawResult);

            return await converter(returnedInfo, result);
        }

        protected static async Task<T> GetInfo<T>(string command, string parameter = "")
        {
            var rawResult = await DevicePortalAPIHelper.GetInfosAsync(command, parameter);
            T result = JsonConvert.DeserializeObject<T>(rawResult);

            return result;
        }


        public delegate Task<Dictionary<string, string>> ObjectToDico<T>(Dictionary<string, string> returnedInfo, T objectToConvert);
    }
}
