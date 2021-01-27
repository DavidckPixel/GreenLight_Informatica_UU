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
    static class UserControls
    {
        public static UserControlsConfig Config;

        static UserControls()
        {
            ReadJson();

            if (Config != null)
            {
               // Console.WriteLine(Config.projectName);
               // Console.WriteLine(Config.standardSubMenu["X"]);
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
