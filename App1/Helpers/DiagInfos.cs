using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynergizDiag
{
    public class DiagInfos
    {
        private static int number = 0;
        public static Dictionary<String, String> Infos = new Dictionary<string, string>();

        public static void WriteDebug(String key, String Value)
        {
            Infos.Add(number + " - " + key, Value);
            number++;
        }
    }
}
