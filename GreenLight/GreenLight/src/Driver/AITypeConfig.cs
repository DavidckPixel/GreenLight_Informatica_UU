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
    public static class AITypeConfig
    {
        public static List<DriverStats> aiTypes = new List<DriverStats>();

        static AITypeConfig()
        {
            ReadJson();
        }

        static public void ReadJson()
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Driver\\DriverType.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    aiTypes = JsonConvert.DeserializeObject<List<DriverStats>>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static public void SaveJson()
        {
            string json = JsonConvert.SerializeObject(aiTypes);
            Console.WriteLine(json);

            string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Driver\\DriverType.json";

            try
            {
                using (StreamWriter sr = new StreamWriter(file))
                {
                    sr.Write(json);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }

}
