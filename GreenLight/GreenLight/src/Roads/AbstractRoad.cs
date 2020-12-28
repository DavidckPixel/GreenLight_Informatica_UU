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
        protected Point point1;
        protected Point point2;

        protected int lanes;
        public List<DrivingLane> Drivinglanes=  new List<DrivingLane>();

        //Basic Road Constructor, every road calls this constructor during initialzation
        public AbstractRoad(Point _point1, Point _point2, int _lanes) : base(_point1, new Size(Math.Abs(_point1.X - _point2.X), Math.Abs(_point1.Y - _point2.Y)))
        {
            this.point1 = _point1;
            this.point2 = _point2;
            this.lanes = _lanes;
            this.Cords = _point1;
        }

        protected abstract DrivingLane CalculateDrivingLane(Point _point1, Point _point2);

        public static int CalculateAngle(Point _point1, Point _point2)
        {
            //calculateAngle
            int _deltaX = _point1.X - _point2.X ;
            int _deltaY = _point1.Y - _point2.Y;

            double _raddegree = Math.Atan2(_deltaX , _deltaY);

            int _degree = (int)(_raddegree * (180 / Math.PI));
            if (_degree < 0)
            {
                _degree = 360 + _degree;
            }

            return _degree % 360;
        }

        public void Draw(Graphics g)
        {
            foreach(DrivingLane _lane in Drivinglanes)
            {
                _lane.Draw(g);
            }
        }

        public Point getPoint1() { return point1; }
        //public void setPoint1(Point _value) { point1 = _value; }
        public Point getPoint2() { return point2; }
        //public void setPoint2(Point _value) { point2 = _value; }

        public int getLanes() { return lanes; }

        public override string ToString()
        {
            string _temp = point1.ToString() + " - " + point2.ToString();
            return _temp;
        }


    }
}
