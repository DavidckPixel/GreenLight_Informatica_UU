using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Drawing;
using GreenLight.src.Driver.GPS;

namespace GreenLight
{
    public class VehicleController : EntityController
    {

        public List<BetterVehicle> vehicleList = new List<BetterVehicle>();
        public List<BetterVehicle> toDelete = new List<BetterVehicle>();
        
        public VehicleStats selectedVehicle;

        public List<VehicleStats> availableVehicleStats = new List<VehicleStats>();

        private SimulationController simController;

        public override void Initialize()
        {
            Log.Write("Initializing the VehicleController");
        }

        public VehicleController(SimulationController _simController)
        {
            this.simController = _simController;
        }

        public void getVehicle(Node _node, bool _collectData, VehicleStats _stats = null)
        {
            if (_stats == null)
            {
                Random ran = new Random();

                int _totalOccurance = (int)this.availableVehicleStats.Sum(x => x.Occurance);
                int _ranNumber = ran.Next(0, _totalOccurance);

                foreach (VehicleStats _stat in this.availableVehicleStats)
                {
                    _totalOccurance =- (int)_stat.Occurance;

                    if (_totalOccurance <= 0)
                    {
                        _stats = _stat;
                    }
                }

                VehicleTypeConfig.ReadJson();
                _stats = VehicleTypeConfig.vehicles.First();
            }

            BetterVehicle _vehicle = new BetterVehicle(_stats, _node, this.simController.aiController.GetDriver());

            if (true)
            {
                General_Form.Main.DataScreen.dataController.collector.addVehicleToCollect(_vehicle);
            }

            vehicleList.Add(_vehicle);
        }

        public void initvehList()
        {
            this.availableVehicleStats.Clear();

            List<string> availableVehicleStatsString = General_Form.Main.UserInterface.SimSVM.selectionBox.elementsAvailable;
            availableVehicleStatsString.ForEach(x => this.availableVehicleStats.Add(getVehicleStat(x)));
            this.availableVehicleStats.RemoveAll(x => x == null);

            Console.WriteLine("AMOUNT OF STATS VEHICLE: LODED: {0}", this.availableVehicleStats.Count());

            if (!this.availableVehicleStats.Any())
            {
                this.availableVehicleStats = VehicleTypeConfig.vehicles;
            }
        }

        private VehicleStats getRandomStats()
        {
            return null;
        }

        static public void addVehicleStats(string _name, int _weight, float _length, int _topspeed, int _motorpwr, int _surface, float _cw, float _occurance)
        {
            VehicleStats _temp = new VehicleStats(_name, _weight, _length, _topspeed, _motorpwr, _surface, _cw, false, _occurance);
            if (VehicleTypeConfig.vehicles.Find(x => x == _temp) == null)
            {
                VehicleTypeConfig.vehicles.Add(_temp);
            }

            General_Form.Main.UserInterface.SimSVM.selectionBox.AddElement(_temp.Name);
        }

        static public VehicleStats getVehicleStat(string _name)
        {
            VehicleStats _temp = VehicleTypeConfig.vehicles.Find(x => x.Name == _name);

            if (_temp == null)
            {
                try
                {
                    _temp = VehicleTypeConfig.vehicles[0];
                }
                catch (Exception)
                {
                    _temp = new VehicleStats("", 1, 1, 1, 1, 1, 1, false,1);
                }
            }

            return _temp;
        }

        static public List<string> getStringVehicleStats()
        {
            List<string> _temp = new List<string>();
            VehicleTypeConfig.vehicles.ForEach(x => _temp.Add(x.Name));
            return _temp;
        }

        public void DeleteVehicle(VehicleStats _stats)
        {
            VehicleTypeConfig.vehicles.Remove(_stats);
        }

        public void SelectVehicle(VehicleStats _stats)
        {
            this.selectedVehicle = _stats;
        }

        private bool AllowEdit()
        {
            if (this.selectedVehicle.canEdit)
            {
                //ERROR MESSAGE HERE!

                return (true);
            }
            return (false);
        }

        public void ChangeWeight(int _weight, Slider o)
        {
            if(this.selectedVehicle  == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            if (AllowEdit())
            {
                o.Value = this.selectedVehicle.Weight;
                return;
            }
            this.selectedVehicle.Weight = _weight;
        }

        public void ChangeLength(float _length, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }
            if (AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Length * 10;
                return;
            }
            this.selectedVehicle.Length = _length / 10;
        }

        public void ChangeTopspeed(int _topSpeed, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }
            if (AllowEdit())
            {
                o.Value = this.selectedVehicle.Topspeed;
                return;
            }
            this.selectedVehicle.Topspeed = _topSpeed;
        }

        public void ChangeMotorpwr(int _motorpwr, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }
            if (AllowEdit())
            {
                o.Value = this.selectedVehicle.Motorpwr;
                return;
            }
            this.selectedVehicle.Motorpwr = _motorpwr;
        }

        public void ChangeSurface(float _surface, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }
            if (AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Surface * 10;
                return;
            }
            this.selectedVehicle.Surface = _surface / 10;
        }

        public void ChangeCw(float _cw, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }
            if (AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Cw * 10;
                return;
            }
            this.selectedVehicle.Cw = _cw / 10;
        }


        public void ChangeOccurance(float _occurance, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            this.selectedVehicle.Occurance = _occurance;
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
 