using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class OriginPointController
    {
        public List<OriginPoints> OriginPointsList = new List<OriginPoints> ();
        List<VehicleOriginPoint<Point>> converted;
        VehicleOriginPoint<Point> selected;
        int sum;
        Random rnd;

        public OriginPointController()
        {
            //Console.WriteLine("new OPC");
        }
        public void AddOriginPoint(int weight, Point location) //Adds an OriginPoint to the list of OriginPoints
        {
            Console.WriteLine("making an originpoint");
            foreach (OriginPoints _op in OriginPointsList)
            {
                if (_op.X < location.X + 4 && _op.X > location.X - 4 && _op.Y < location.Y + 4 && _op.Y > location.Y - 4)
                {
                    Console.WriteLine("returned on point" + _op);
                    return;
                }
            }
            OriginPoints _temp = new OriginPoints(weight, location.X, location.Y, null, false);
            if (OriginPointsList.Find(x => x == _temp) == null)
            {
                OriginPointsList.Add(_temp);
            }
            SaveJson();
        }

        public void draw(Graphics g)        //Has to be called
        {
            foreach (OriginPoints _op in OriginPointsList)
                g.FillEllipse(Brushes.White, _op.X, _op.Y, 20, 20);
        }


        public Point GetSpawnPoint //Returns a random OriginPoint based on its weight
        {
            get
            {
                try
                {
                    ConvertList();
                    var probability = rnd.NextDouble() * sum;   //gets a number between 0 and the sum of all the weights
                    selected = converted.SkipWhile(i => i.Weight < probability).First();
                    return selected.Location;
                }
                catch
                {
                    Console.WriteLine("No spawnpoints available.");
                }
                return new Point (0,0);
            }
        }

        public void ConvertList()   //list where every item gets it's own weight plus the sum of the weight of the previous items
        {
            converted = new List<VehicleOriginPoint<Point>>(OriginPointsList.Count);
            sum = 0;
            foreach (var item in OriginPointsList.Take(OriginPointsList.Count))
            {
                if (!item.isConnection)
                {
                    sum += item.Weight;
                    converted.Add(new VehicleOriginPoint<Point> { Weight = sum, Location = new Point(item.X, item.Y) });
                }
            }
            rnd = new Random();
        }

        public OriginPoints getOriginPoint(int _weight)
        {
            OriginPoints _temp = OriginPointsList.Find(x => x.Weight == _weight);

            if (_temp == null)
            {
                try
                {
                    _temp = OriginPointsList[0];
                }
                catch (Exception)
                {
                    _temp = new OriginPoints(80, 0, 0, null, false);
                }
            }
            return _temp;
        }

        private void initOriginPoints()
        {
            try
            {
                string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\Vehicle\\VehicleType.json";

                using (StreamReader sr = new StreamReader(file))
                {
                    string json = sr.ReadToEnd();
                    OriginPointsList = JsonConvert.DeserializeObject<List<OriginPoints>>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public List<int> getWeightOriginPoints()
        {
            if (!OriginPointsList.Any())
            {
                initOriginPoints();
            }

            List<int> _temp = new List<int>();
            OriginPointsList.ForEach(x => _temp.Add(x.Weight));
            return _temp;
        }
        public void SaveJson()
        {
            string json = JsonConvert.SerializeObject(OriginPointsList);
            string file = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\GreenLight\\src\\OriginPoints\\OriginPoints.json";
            using (StreamWriter sr = new StreamWriter(file))
            {
                sr.Write(json);
            }
        }
    }
}