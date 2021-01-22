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
    public class VehicleController : EntityController
    {

        public List<Vehicle> vehicleList = new List<Vehicle>();
        public static List<VehicleStats> vehicles = new List<VehicleStats>();

        public VehicleStats selectedVehicle;

        public override void Initialize()
        {
            Log.Write("Initializing the VehicleController");
        }

        public VehicleController()
        {
        }

        public Vehicle getVehicle(int _x, int _y, VehicleStats _stats = null)
        {
            if (_stats == null)
            {
                Random ran = new Random();
                int _index = ran.Next(0, vehicles.Count() - 1);
                _stats = vehicles[_index];
            }

            return new Vehicle(_stats, _x, _y);
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

        static public void addVehicleStats(string _name, int _weight, float _length, int _topspeed, int _motorpwr, int _surface, float _cw, float _occurance)
        {
            VehicleStats _temp = new VehicleStats(_name, _weight, _length, _topspeed, _motorpwr, _surface, _cw, true, _occurance);
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
                    _temp = new VehicleStats("", 1, 1, 1, 1, 1, 1, true,1);
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

        public void CreateNewVehicle()
        {

        }

        public void SelectVehicle(VehicleStats _stats)
        {
            this.selectedVehicle = _stats;


            
        }

        private bool AllowEdit()
        {
            if (!this.selectedVehicle.canEdit)
            {
                //ERROR MESSAGE HERE!

                return (false);
            }
            return (true);
        }

        public void ChangeWeight(int _weight, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = this.selectedVehicle.Weight;
            }
            this.selectedVehicle.Weight = _weight;
        }

        public void ChangeLength(float _length, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Length * 10;
            }
            this.selectedVehicle.Length = _length / 10;
        }

        public void ChangeTopspeed(int _topSpeed, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = this.selectedVehicle.Topspeed;
            }
            this.selectedVehicle.Topspeed = _topSpeed;
        }

        public void ChangeMotorpwr(int _motorpwr, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = this.selectedVehicle.Motorpwr;
            }
            this.selectedVehicle.Motorpwr = _motorpwr;
        }

        public void ChangeSurface(float _surface, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Surface * 10;
            }
            this.selectedVehicle.Surface = _surface / 10;
        }

        public void ChangeCw(float _cw, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Cw * 10;
            }
            this.selectedVehicle.Cw = _cw / 10;
        }


        public void ChangeOccurance(float _occurance, Slider o)
        {
            if (!AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Occurance;
            }
            this.selectedVehicle.Occurance = _occurance / 10;
        }

        /*     public int Weight;
    public float Length;
    public int Topspeed;
    public int Motorpwr;
    public float Surface;
    public float Cw;

    "Weight": 1353,
    "Length": 4.77,
    "Topspeed": 61,
    "Motorpwr": 111900,
    "Surface": 2.65,
    "Cw": 0.3,
    */
    }
}
 