using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace GreenLight
{
    static class Grid
    {
        public static GridConfig Config;

        static Grid()
        {
            ReadJson();
        }

        private static void ReadJson()
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Grid\\GridConfig.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    Grid.Config = JsonConvert.DeserializeObject<GridConfig>(json);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
