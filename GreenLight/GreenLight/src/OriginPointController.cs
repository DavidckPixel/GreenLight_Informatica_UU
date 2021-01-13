using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace GreenLight
{
    public class OriginPointController
    {
        List<VehicleOriginPoint<Point>> OriginPoints = new List<VehicleOriginPoint<Point>>{};
        List<VehicleOriginPoint<Point>> converted;
        VehicleOriginPoint<Point> selected;
        int sum;
        Random rnd;

        public OriginPointController()
        {
        }

        public void AddOriginPoint (int weight, Point location) //Adds an OriginPoint to the list of OriginPoints
        {
            OriginPoints.Add(new VehicleOriginPoint<Point> { Weight = weight, Location = location });
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
            converted = new List<VehicleOriginPoint<Point>>(OriginPoints.Count);
            sum = 0;
            foreach (var item in OriginPoints.Take(OriginPoints.Count))
            {
                sum += item.Weight;
                converted.Add(new VehicleOriginPoint<Point> { Weight = sum, Location = item.Location });
            }
            Console.WriteLine(sum);
            rnd = new Random();
        }
    }
}