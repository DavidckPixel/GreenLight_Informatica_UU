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
    public class GridConfig
    {
        //This class initializes and holds the Config data for the Grid
        //It uses the newtonsoft json deserializer.

        public int SpacingWidth;
        public int SpacingHeight;
        public int BoxSize;
        public int HitSize;

        public static void Init(ref GridConfig _temp)
        {
            GridConfig.ReadJson(ref _temp);
        }

        private static void ReadJson(ref GridConfig _temp)
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Grid\\GridConfig.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    _temp = JsonConvert.DeserializeObject<GridConfig>(json);
                }

            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
