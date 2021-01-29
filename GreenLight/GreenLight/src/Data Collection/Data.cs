using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Data_Collection
{
    //Data class is a class stat stores all the data that is collected, it consists of multiple lists and values that are changed during the course of a simulation
    //Incase the simulation is reset, this data  class is then thrown away and replaced by a new instance.

    public class Data
    {
        //string dataName = "test";

        List<int> seriesBrakeTicks;
        List<int> pointsBrakeTick;

        List<Tuple<int, int>> percBrakePerTick;
        List<Tuple<int, double>> averageSpeedPerTick;

        public Data()
        {
            Log.Write("Creating a new Data Instance");

            seriesBrakeTicks = new List<int>();
            pointsBrakeTick = new List<int>();
            percBrakePerTick = new List<Tuple<int, int>>();
            averageSpeedPerTick = new List<Tuple<int, double>>();
        }

        //The AddBrakeTick method is called by the data collector, the function takes a certain amount of ticks
        //these ticks correspond to how long the car was braking during the course of its lifetime. It then adds
        //this either adds this amount to a list, or it increments a value; depending whether or not we seen this brake value
        //before, this way it turfs how many cars brakes for how many ticks

        public void AddBrakeTick(int Ticks)
        {
            int _100Ticks = Ticks / 100;
            int _index = seriesBrakeTicks.IndexOf(_100Ticks);

            if (!seriesBrakeTicks.Contains(_100Ticks)) 
            {
                seriesBrakeTicks.Add(_100Ticks);
                seriesBrakeTicks.Sort();
                _index = seriesBrakeTicks.IndexOf(_100Ticks);
                pointsBrakeTick.Insert(_index, 0);
            }
            pointsBrakeTick[_index]++;
        }

        public void GetBrakeData(out List<int> serie, out List<int> points)
        {
            serie = this.seriesBrakeTicks;
            points = this.pointsBrakeTick;

            return;
        }

        //Adds a Tick value of speed

        public void AddAverageSpeedPerTick(double speed)
        {
            averageSpeedPerTick.Add(new Tuple<int, double>(averageSpeedPerTick.Count(), speed));
        }

        public int GetAverageSpeedPerTickIndex(double? _value = null)
        {
            if(_value == null)
            {
                return averageSpeedPerTick.Count() - 1;
            }
            else
            {
                return averageSpeedPerTick.FindIndex(x => x.Item2 == _value);
            }
        }

        public void AddPercentageOnBraking(int _perc)
        {
            //not yet implemented
        }

        public List<Tuple<int,double>> GetAverageSpeedPerTick()
        {
            return averageSpeedPerTick;
        }

        //The ToString() function is called when the data is exported, it nicely orders all the interesting collected data and adds it to a .txt file

        public override string ToString()
        {
            string _temp = " test ";
            
            foreach(Tuple<int,double> _speedData in averageSpeedPerTick)
            {
                _temp += _speedData.Item1 + " -- " + _speedData.Item2 + "\n";
            }

            return _temp;
        }
    }
}
