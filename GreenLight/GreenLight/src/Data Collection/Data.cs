using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Data_Collection
{
    public class Data
    {
        //string dataName = "test";

        List<int> seriesBrakeTicks;
        List<int> pointsBrakeTick;

        List<Tuple<int, int>> percBrakePerTick;
        List<Tuple<int, double>> averageSpeedPerTick;

        public Data()
        {
            Console.WriteLine("Initialized a new Data Collection!");

            seriesBrakeTicks = new List<int>();
            pointsBrakeTick = new List<int>();
            percBrakePerTick = new List<Tuple<int, int>>();
            averageSpeedPerTick = new List<Tuple<int, double>>();
        }

        public void AddBrakeTick(int Ticks)
        {
            int _100Ticks = Ticks / 100;
            int _index = seriesBrakeTicks.IndexOf(_100Ticks);

            if (!seriesBrakeTicks.Contains(_100Ticks)) 
            {
                //SERIES IS NOT YET IN THE LIST, SO ADD IT
                seriesBrakeTicks.Add(_100Ticks);
                seriesBrakeTicks.Sort();
                _index = seriesBrakeTicks.IndexOf(_100Ticks);
                pointsBrakeTick.Insert(_index, 0);
            }

            //THE DATA IS IN THE SERIELIST

            //ADD DATA POINT ON THE CORRECT PLACE;
            pointsBrakeTick[_index]++;
        }

        public void GetBrakeData(out List<int> serie, out List<int> points)
        {
            serie = this.seriesBrakeTicks;
            points = this.pointsBrakeTick;

            return;
        }

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

        }

        public List<Tuple<int,double>> GetAverageSpeedPerTick()
        {
            return averageSpeedPerTick;
        }

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
