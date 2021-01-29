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
        //This class is in control of all the data of the vehicles, including the data in the VehicleType.json
        //It allows the user to change data of a certain vehicletype in the SimulationSubVehicleMenu which is visible on the right side of the simulationscreen
        //It is also used to create vehicles

        public List<BetterVehicle> vehicleList = new List<BetterVehicle>();
        public List<BetterVehicle> toDelete = new List<BetterVehicle>();
        
        public VehicleStats selectedVehicle;

        public List<VehicleStats> availableVehicleStats = new List<VehicleStats>();

        private SimulationController simController;

        public override void Initialize()
        {
        }

        //This method initializes the simulationcontroller, which gives this class access to the BetterAI
        public VehicleController(SimulationController _simController)
        {
            this.simController = _simController;
        }

        //This creates a vehicle based on a startingpoint (_node) and adds a driver/AI to it.
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

                if (this.availableVehicleStats.Any() && _stats == null)
                {
                    _stats = this.availableVehicleStats.First();
                }

                if (_stats == null) {

                    Console.WriteLine("Better not");
                    VehicleTypeConfig.ReadJson();
                    this.availableVehicleStats = VehicleTypeConfig.vehicles;
                    _stats = VehicleTypeConfig.vehicles.First();
                }
            }

            BetterVehicle _vehicle = new BetterVehicle(_stats, _node, this.simController.aiController.GetDriver());

            if (true)
            {
                General_Form.Main.DataScreen.dataController.collector.addVehicleToCollect(_vehicle);
            }

            vehicleList.Add(_vehicle);
        }


        //This method initializes all the data for the different vehicletypes in VehicleType.json
        public void initvehList()
        {
            this.availableVehicleStats.Clear();

            List<string> availableVehicleStatsString = General_Form.Main.UserInterface.SimSVM.selectionBox.elementsAvailable;
            availableVehicleStatsString.ForEach(x => this.availableVehicleStats.Add(getVehicleStat(x)));
            this.availableVehicleStats.RemoveAll(x => x == null);

            if (!this.availableVehicleStats.Any())
            {
                this.availableVehicleStats = VehicleTypeConfig.vehicles;
            }
        }

        //This method adds a new vehicletype to the VehicleType.json, the user can do this from the menu on the right in the simulation [SimulationSubVehicleMenu]
        static public void addVehicleStats(string _name, int _weight, float _length, int _topspeed, int _motorpwr, int _surface, float _cw, float _occurance)
        {
            VehicleStats _temp = new VehicleStats(_name, _weight, _length, _topspeed, _motorpwr, _surface, _cw, false, _occurance);
            if (VehicleTypeConfig.vehicles.Find(x => x == _temp) == null)
            {
                VehicleTypeConfig.vehicles.Add(_temp);
            }

            General_Form.Main.UserInterface.SimSVM.selectionBox.AddElement(_temp.Name);
        }

        //This method returns the vehiclestats of a certain vehicletype from the JSON
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

        //This method returns the current vehiclestats as a string
        static public List<string> getStringVehicleStats()
        {
            List<string> _temp = new List<string>();
            VehicleTypeConfig.vehicles.ForEach(x => _temp.Add(x.Name));
            return _temp;
        }

        //This method reletes a vehicletype from the JSON
        public void DeleteVehicle(VehicleStats _stats)
        {
            VehicleTypeConfig.vehicles.Remove(_stats);
        }

        //This method changes the current vehicletype
        public void SelectVehicle(VehicleStats _stats)
        {
            this.selectedVehicle = _stats;
        }

        //This method returns whether or not the current vehicletype is editable
        private bool AllowEdit()
        {
            if (this.selectedVehicle.canEdit)
            {
                return (true);
            }
            return (false);
        }

        //This method changes the weight in kg of the current vehicletype
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

        //This method changes the length in m of the current vehicletype
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

        //This method changes the maximum speed in m/s of the current vehicletype
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

        //This method changes the motorpower in watts of the current vehicletype
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

        //This method changes the surface area in m^2 of the current vehicletype
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

        //This method changes the cw value of the current vehicletype
        public void ChangeCw(float _cw, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                return;
            }
            if (AllowEdit())
            {
                o.Value = (int)this.selectedVehicle.Cw * 10;
                return;
            }
            this.selectedVehicle.Cw = _cw / 10;
        }

        //This method changes how frequently the current vehicletype will be spawned
        public void ChangeOccurance(float _occurance, Slider o)
        {
            if (this.selectedVehicle == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            this.selectedVehicle.Occurance = _occurance;
        }
    }
}
 