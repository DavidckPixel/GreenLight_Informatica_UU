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
    //This loads the different DriverProfile faces presets from the faces.json

    public static class DriverProfileData
    {
        public static List<DriverProfileFace> faces;

        static DriverProfileData()
        {
            initFaces();
        }

        private static void initFaces()
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Driver\\DriverProfile\\faces.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    faces = JsonConvert.DeserializeObject<List<DriverProfileFace>>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static List<string> FacesToString()
        {
            List<string> _temp = new List<string>();
            foreach(DriverProfileFace _face in faces)
            {
                _temp.Add(_face.name);
            }

            return _temp;
        }
    }
}
