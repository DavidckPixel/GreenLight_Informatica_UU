using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace GreenLight
{
    class VehicleTypeConfig
    {
        //This class initializes and holds the Config data for Vehicles
        //It uses the newtonsoft json deserializer.

        public static List<Vehicle> types;

        static VehicleTypeConfig()
        {
            ReadJson();
        }

        private static void ReadJson()
        {
            string _file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Vehicle\\VehicleType.json";

            try
            {
                using (StreamReader r = new StreamReader(_file))
                {
                    string Json = r.ReadToEnd();
                    types = JsonConvert.DeserializeObject<List<Vehicle>>(Json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
