using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace GreenLight.src.Data_Collection
{
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

            double _averageSpeed = _totalspeed / vehicleCollection.Count;
            data.AddAverageSpeedPerTick(_averageSpeed);
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


        public void CollectAllData()
        {
            CollectVehicleStats();
            CollectAIStats();
        }

        //------Add Vehicle

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

        //------Remove Things

        public void RemoveVehicle(BetterVehicle _vehicle, bool _dump = true)
        {
            vehicleCollection.Remove(_vehicle);

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
