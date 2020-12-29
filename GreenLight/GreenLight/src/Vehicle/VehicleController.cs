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
    class VehicleController : EntityController
    {

        public List<Vehicle> vehicleList = new List<Vehicle>();
        private static List<VehicleStats> vehicles = new List<VehicleStats>();

        public override void Initialize()
        {
            Log.Write("Initializing the VehicleController");
        }

        public VehicleController()
        {
        }

        public void AddVehicle(int _x, int _y, VehicleStats _stats = null)
        {
            if (_stats == null)
            {
                _stats = getRandomStats();
            }
        }

        private VehicleStats getRandomStats()
        {
            return null;
        }

        static private void initVehicleStats()
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

        static public void addVehicleStats(string _name, int _weight, float _length, int _topspeed, int _motorpwr, int _surface, float _cw)
        {
            VehicleStats _temp = new VehicleStats(_name, _weight, _length, _topspeed, _motorpwr, _surface, _cw);
            if (vehicles.Find(x => x == _temp) == null)
            {
                vehicles.Add(_temp);
            }

            General_Form.Main.UserInterface.SimSVM.Selection_box.Add_Element(_temp.Name);
        }

        static public VehicleStats getVehicleStat(string _name)
        {
            VehicleStats _temp = vehicles.Find(x => x.Name == _name);

            if (_temp == null)
            {
                try
                {
                    _temp = vehicles[0];
                }
                catch (Exception)
                {
                    _temp = new VehicleStats("", 1, 1, 1, 1, 1, 1);
                }
            }

            return _temp;
        }

        static public List<string> getStringVehicleStats()
        {
            if (!vehicles.Any())
            {   
                initVehicleStats();
            }
            Console.WriteLine(vehicles.Count());

            List<string> _temp = new List<string>();
            vehicles.ForEach(x => _temp.Add(x.Name));
            return _temp;
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
