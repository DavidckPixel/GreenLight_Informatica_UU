using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace GreenLight.src.Data_Collection
{
    //The DataCollector is the object that knows what data to collect, and from which objects to collect it
    //It holds a list of AI & Vehicles (something that can be expended on incase more data needs to be collected)
    //Every Update cycle it then collects all the data from those instances in the list

    public class DataCollector
    {
        public Data data;
        public DataController controller;

        private List<BetterAI> aiCollection = new List<BetterAI>();
        private List<BetterVehicle> vehicleCollection = new List<BetterVehicle>();

        public DataCollector(DataController _controller)
        {
            this.data = new Data();
            this.controller = _controller;
        }

        public void AddBrakeData(int _data)
        {
            data.AddBrakeTick(_data);

            controller.SmallUpdateBrakeChart(_data);
        }

        public void AddBrakeData(List<int> _data)
        {
            _data.ForEach(x => data.AddBrakeTick(x));
        }

        //This function is called every Update Cycle and works the same as the CollectAIStats(), it increments through the list
        //Looking at every instance in there and looks/ collects its values
        //In this case for the vehicle it only looks at the speed, which it then averages among all cars

        public void CollectVehicleStats()
        {
            if (!vehicleCollection.Any())
            {
                return;
            }

            double _totalspeed = 0;

            foreach(BetterVehicle _vehicle in vehicleCollection)
            {
                _totalspeed += _vehicle.speed;
            }

            //Console.WriteLine("Collecting vehicle data!");

            double _averageSpeed = _totalspeed / vehicleCollection.Count;
            data.AddAverageSpeedPerTick(_averageSpeed);

            //Console.WriteLine("Added to data!");

            controller.SmallUpdateSpeedPerTickChart(data.GetAverageSpeedPerTickIndex(), _averageSpeed);
        }


        public void CollectAIStats()
        {
            if (!aiCollection.Any())
            {
                return;
            }

            int _amoutBraking = 0;

            foreach(BetterAI _ai in aiCollection)
            {
                if (_ai.isBraking)
                {
                    _amoutBraking++;
                }
            }
            data.AddPercentageOnBraking(_amoutBraking / aiCollection.Count() * 100);
        }

        //This is an easier access method to update both the AIdata and the VehicleData

        public void CollectAllData()
        {
            CollectVehicleStats();
            CollectAIStats();
        }

        //In the simulation, when a vehicle is created, it also needs to be added to the data collector (atleast, incase the data from that vehicle is important). 
        //so this method can be called to add it, the AddVehicleCollection method works exactly the same, only then takes a list instead of an instance

        public void addVehicleToCollect(BetterVehicle _vehicle)
        {
            vehicleCollection.Add(_vehicle);
            aiCollection.Add(_vehicle.vehicleAI);
        }

        public void addVehicleToCollect(List<BetterVehicle> _vehicles)
        {
            foreach (BetterVehicle _vehicle in _vehicles)
            {
                addVehicleToCollect(_vehicle);
            }
        }

        //------Add AI

        public void addAIToCollect(BetterAI _ai)
        {
            aiCollection.Add(_ai);
        }

        public void addAIToCollect(List<BetterAI> _ai)
        {
            _ai.ForEach(x => aiCollection.Add(x));
        }

        //When a vehicle has completed its journey and it removed, its also important that the data stops being collected, so when a vehicle is done the
        //RemoveVehicle method is called, There is certain data that cna only be collected after the vehicle/ AI is done, for this the _dump parameter is important
        //if this parameter is true (which is its standard value) it will dump its important valuables to the Data instance

        public void RemoveVehicle(BetterVehicle _vehicle, bool _dump = true)
        {
            vehicleCollection.Remove(_vehicle);
            RemoveAI(_vehicle.vehicleAI, _dump);

            if (_dump)
            {
                //DUMP DATA HERE
            }
        }

        public void RemoveAI(BetterAI _ai, bool _dump = true)
        {
            aiCollection.Remove(_ai);

            if (_dump)
            {
                this.AddBrakeData(_ai.profile.ticksOnBrake);
            }
        }





    }
}
