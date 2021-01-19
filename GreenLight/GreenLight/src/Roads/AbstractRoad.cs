using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    //This is the abstract road class from which each roadtype inherits some basic features.
    //This basic features include: CalculateDrivingLane, Every road needs to have its own function of CalculateDrivingLane
    //This class also has a static function that takes 2 points as input, and calculates the angle in degrees between them.
    //Every Road has 4 base variables: 2 points inbetween which the road lays, the amount of lanes the road has, and a list of the drivingLanes
    //Drivinglanes are a list of points that the car follows to drive on the road.

    public abstract class AbstractRoad : ScreenObject
    {
        public Point point1;
        public Point point2;

        public int lanes;
        public List<Lane> Drivinglanes=  new List<Lane>();
        public List<PlacedSign> Signs = new List<PlacedSign>();
        public string roadtype;
        public double slp;
        public bool beginconnection, endconnection;
        
        public string Type;
        public string Dir;
        public AbstractRoad beginConnectedTo, endConnectedTo;
        public int laneWidth = Roads.Config.laneWidth;

        //Basic Road Constructor, every road calls this constructor during initialzation
        public AbstractRoad(Point _point1, Point _point2, int _lanes, string _roadtype, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo) : base(new Point(Math.Min(_point1.X, _point2.X),Math.Min(_point1.Y, _point2.Y)))
        {
            this.point1 = _point1;
            this.point2 = _point2;
            this.lanes = _lanes;
            this.roadtype = _roadtype;
            this.beginconnection = _beginconnection;
            this.endconnection = _endconnection;
            this.beginConnectedTo = _beginConnectedTo;
            this.endConnectedTo = _endConnectedTo; 
        }

        //protected abstract DrivingLane CalculateDrivingLane(Point _point1, Point _point2, int _thislane);
        

        public abstract Point[] hitBoxPoints(Point one, Point two, int _lanes, int _laneWidth);

        

        public abstract Hitbox CreateHitbox(Point[] _array);


        public virtual void Draw(Graphics g)
        {
            foreach(Lane _lane in Drivinglanes)
            {
                _lane.Draw(g);
            }
            foreach(PlacedSign _sign in Signs)
            {
                _sign.draw(g);
            }

            DrawLine(g);

            this.hitbox.Draw(g);
        }

        public void DrawLine(Graphics g)
        {
            if (!General_Form.Main.BuildScreen.builder.roadBuilder.visualizeLanePoints)
            {
                return;
            }

            foreach(Lane _lane in Drivinglanes)
            {
                _lane.DrawLine(g);
            }
        }

        public Point getPoint1() { return point1; }
        public Point getPoint2() { return point2; }

        public int getLanes() { return lanes; }

        public string getRoadtype() { return roadtype; }

        public override string ToString()
        {
            string _temp = point1.ToString() + " - " + point2.ToString();
            return _temp;
        }
    }
}
