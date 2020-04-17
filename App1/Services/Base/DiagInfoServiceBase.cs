using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynergizDiag.Services
{
    public abstract class DiagInfoServiceBase
    {
        public const string LABEL = "";
        public string GetLabel()
        {
            return LABEL;
        }

        public virtual async Task<Dictionary<string, string>> GetInfos(Dictionary<String, String> returnedInfo)
        {
            if (returnedInfo == null)
            {
                returnedInfo = new Dictionary<string, string>();
            }

            return returnedInfo;
        }

    }
}
