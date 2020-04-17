using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynergizDiag.Services
{
    interface INetworkingInfoService
    {
        Task<Dictionary<string, string>> GetNetworkInfo();
    }
}