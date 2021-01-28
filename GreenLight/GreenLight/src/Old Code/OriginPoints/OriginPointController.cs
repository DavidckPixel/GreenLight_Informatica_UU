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
    // This is the OriginPointController class, which we created to control everything that has to do with OriginPoints
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project.

    public class OriginPointController
    {
        public List<OriginPoints> OriginPointsList = new List<OriginPoints> ();
        List<VehicleOriginPoint<Point>> converted;
        VehicleOriginPoint<Point> selected;
        int sum;
        Random rnd;

        public OriginPointController()
        { }

        //Adds an OriginPoint to the list of OriginPoints
        public void AddOriginPoint(int weight, Point location) 
        {
            foreach (OriginPoints _op in OriginPointsList)
            {
                if (_op.X < location.X + 4 && _op.X > location.X - 4 && _op.Y < location.Y + 4 && _op.Y > location.Y - 4)
                {
                    return;
                }
            }

            OriginPoints _temp = new OriginPoints(weight, location.X, location.Y, null, false);
            if (OriginPointsList.Find(x => x == _temp) == null)
            {
                OriginPointsList.Add(_temp);
            }
        }

        //Draws all OriginPoint in the list and has to be called from another part of the project
        public void draw(Graphics g)        
        {
            foreach (OriginPoints _op in OriginPointsList)
                g.FillEllipse(Brushes.White, _op.X, _op.Y, 20, 20);
        }

        //Returns a random OriginPoint based on its weight
        public Point GetSpawnPoint 
        {
            get
            {
                try
                {
                    ConvertList();
                    var probability = rnd.NextDouble() * sum;
                    selected = converted.SkipWhile(i => i.Weight < probability).First();
                    return selected.Location;
                }
                catch
                { }
                return new Point (0,0);
            }
        }

        public void ConvertList()   
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