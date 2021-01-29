using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace GreenLight
{
    // This class initializes RoadsConfig and fills it with some base values for all roads
    static class Roads
    {
        public static RoadsConfig Config;

        static Roads()
        {
            ReadJson();

            if (Config != null)
            {
                
            }
        }

        private static void ReadJson()
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Roads\\RoadsConfig.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    Roads.Config = JsonConvert.DeserializeObject<RoadsConfig>(json);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
