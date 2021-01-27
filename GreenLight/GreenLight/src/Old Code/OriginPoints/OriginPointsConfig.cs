using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GreenLight
{
    static class OriginPointsConfig
    {
        public static List<OriginPoints> originpoints;

        static OriginPointsConfig()
        {
            ReadJson();
        }

        private static void ReadJson()
        {
            string _file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\OriginPoints\\OriginPoints.json";

            try
            {
                using (StreamReader r = new StreamReader(_file))
                {
                    string Json = r.ReadToEnd();
                    originpoints = JsonConvert.DeserializeObject<List<OriginPoints>>(Json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
