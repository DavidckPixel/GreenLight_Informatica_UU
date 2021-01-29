using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace GreenLight
{
    // This is the base class for DrivingLanes and CrossLanes.
    // It consists of a few abstract functions and a few important variables for all lanes.
    public abstract class Lane
    {
        public ConnectionLink link;

        public List<LanePoints> points;
        public string dir;
        public Hitbox offsetHitbox;
        public List<Lane> beginConnectedTo = new List<Lane>();
        public List<Lane> endConnectedTo = new List<Lane>();
        public float AngleDir;

        public abstract void Draw(Graphics g);
        public abstract void DrawoffsetHitbox(Graphics g);

        public abstract void FlipPoints();

        public int thisLane = 0;
        public bool flipped = false;

        public void DrawLine(Graphics g, Pen pen)
        {
            g.DrawString(thisLane.ToString(), SystemFonts.DefaultFont, Brushes.Pink, this.points.First().cord);

            Point _old = points.First().cord;
            foreach (LanePoints _point in points)
            {
                g.DrawLine(pen, _point.cord, _old);
                _old = _point.cord;
            }

        }

        public static void OrderDrivingLanes(AbstractRoad _road)
        {
            int Xside = _road.Drivinglanes.First().points.First().cord.X;
            List<Lane> _orderd = new List<Lane>();
            int count = 1;

            if (_road.Drivinglanes.TrueForAll(x => RoadMath.Distance(Xside, x.points.First().cord.Y, x.points.First().cord.X, x.points.First().cord.Y) < 5))
            {
                Console.WriteLine("Ordering the driving Lanes: the lane is laiyng Horizontally");
                _road.Drivinglanes.Sort(delegate (Lane x, Lane y)
                {
                    return x.points.First().cord.Y.CompareTo(y.points.First().cord.Y);
                });
            }
            else
            {
                Console.WriteLine("Ordering the driving Lanes: the lane is laiyng Vertically");
                _road.Drivinglanes.Sort(delegate (Lane x, Lane y)
                {
                    return x.points.First().cord.X.CompareTo(y.points.First().cord.X);
                });
            }

            foreach (Lane _lane in _road.Drivinglanes)
            {
                _lane.thisLane = count;
                count++;
            }
        }
    }
}
