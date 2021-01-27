using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GreenLight
{
    static class WorldConfig
    {
        //This class initializes and holds the Config data for the World in which the simulation takes place
        //It uses the newtonsoft json deserializer.

        public static List<World> physics;

        static WorldConfig()
        {
            ReadJson();
        }

        private static void ReadJson()
        {
            string _file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Vehicle\\Worlds\\Earth.json";

            try
            {
                using (StreamReader r = new StreamReader(_file))
                {
                    string Json = r.ReadToEnd();
                    physics = JsonConvert.DeserializeObject<List<World>>(Json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void WriteJson()
        {
            string _file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Vehicle\\Worlds\\Earth.json";

            try
            {
                using (StreamWriter r = new StreamWriter(_file))
                {
                    string _datastream = JsonConvert.SerializeObject(physics);
                    r.Write(_datastream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static bool CheckDuplicateName(string _name)
        {
            if(physics.Any(x => x.name == _name))
            {
                return true;
            }
            return false;
        }
    }
}