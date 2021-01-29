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
    //A static class that reads the UserControlsConfig.Json and applies it to it's own UserControlsConfig named Config.
    static class UserControls
    {
        public static UserControlsConfig Config;

        static UserControls()
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
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\UserControls\\UserControlsConfig.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    UserControls.Config = JsonConvert.DeserializeObject<UserControlsConfig>(json);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
