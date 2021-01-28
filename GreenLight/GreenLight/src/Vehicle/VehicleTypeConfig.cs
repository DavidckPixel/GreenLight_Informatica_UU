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

        public static List<VehicleStats> vehicles = new List<VehicleStats>();


        static VehicleTypeConfig()
        {
            ReadJson();
        }

        public static void ReadJson()
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Vehicle\\VehicleType.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    vehicles = JsonConvert.DeserializeObject<List<VehicleStats>>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static public void SaveJson()
        {
            string json = JsonConvert.SerializeObject(vehicles);
            Console.WriteLine(json);

            string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Vehicle\\VehicleType.json";

            using (StreamWriter sr = new StreamWriter(file))
            {
                sr.Write(json);
            }
        }

    }
}
