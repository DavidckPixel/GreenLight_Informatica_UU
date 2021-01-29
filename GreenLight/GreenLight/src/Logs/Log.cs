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
    static class Log
    {

        //This is an additional system that deals with the Logging of all events in our program
        //It automaticly creates a file and writes it with whatever the programmer wants
        //Its similar to printing things in the console
        //Just use the function Log.Write("Text") and the "Text" is written in the file

        static string file;
        static bool save;
        static string basefile;

        static Log()
        {
            Log.readConfig();
            Log.InitLogFile();
        }

        private static void InitLogFile()
        {
            if (!save)
            {
                return;
            }

            string _filename = DateTime.Now.ToString().Replace(" ", "_").Replace(":", "-");
            file =  basefile + "/GreenLight/Logs/" + _filename;

            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine("DATE: " + _filename);
                sw.WriteLine();
            }

            Log.Write(file);
        }

        public static void Write(params string[] text)
        {
            if (!save)
            {
                return;
            }

            try
            {
                using (StreamWriter sw = File.AppendText(file))
                {
                    foreach (string x in text)
                    {
                        sw.WriteLine(x);
                    }
                }
            }catch(Exception e)
            {

            }
        }

        public static void Line()
        {
            Log.Write("----------------------------------------------------------------------------------", "");
        }

        private static void readConfig()
        {
            basefile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string _file = basefile + "\\GreenLight\\src\\Logs\\LogConfig.json";

            try
            {
                using (StreamReader r = new StreamReader(_file))
                {
                    string Json = r.ReadToEnd();
                    JObject jObject = JObject.Parse(Json);

                    save = (bool)jObject["Save"];
                }
            }
            catch (Exception e)
            {
                Log.Write(e.ToString());
            }
        }
    }
}
