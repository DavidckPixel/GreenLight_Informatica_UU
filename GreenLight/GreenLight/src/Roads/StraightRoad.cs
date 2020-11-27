using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    class StraightRoad : AbstractRoad
    {
        private char dir;

        private int roadwidth = 10; // HARDCODED WAARDE AANPASSEN

        public StraightRoad(Point _point1, Point _point2, int _lanes, char _dir) : base(_point1, _point2, _lanes)
        {
            //CalculateDrivingPath
            this.dir = _dir;
            this.CalculateDrivingLane();
        }

        protected override void CalculateDrivingLane()
        {
            List<LanePoints> _lanePoints = new List<LanePoints>();
            Point _normpoint1 = point1; Point _normpoint2 = point2;

            if (this.dir == 'N') // SWITCH CASE?
            {
                _normpoint1 = new Point(this.point1.X - this.roadwidth, this.point1.Y);
                _normpoint2 = new Point(this.point2.X + this.roadwidth, this.point2.Y);
            }
            else if (dir == 'E') 
            {
                //
                _normpoint1 = new Point(this.point1.X + this.roadwidth, this.point2.Y - this.roadwidth);
            }
            else if (dir == 'S')
            {
                //
                _normpoint1 = new Point(this.point1.X + this.roadwidth, this.point2.Y - this.roadwidth);
            }
            else if (dir == 'W')
            {
                //
                _normpoint1 = new Point(this.point1.X + this.roadwidth, this.point2.Y - this.roadwidth);
            }

            Tuple<int, int> _dir = GetDirection(_normpoint1, _normpoint2);

            Console.WriteLine("NORM points: {0} -- {1}", _normpoint1, _normpoint2);
            Console.WriteLine("DIR: {0} -- {1}", _dir.Item1, _dir.Item2);
            Point _prev = _normpoint1;
            int temp = 0;

            while (Math.Abs(_normpoint1.X - _normpoint2.X) > 0 || Math.Abs(_normpoint1.Y - _normpoint2.Y) > 0)
            {
                temp++;

                Console.WriteLine("{0} -- {1}", _normpoint1.Y, _normpoint2.Y);
                _normpoint1 = new Point(_normpoint1.X - _dir.Item1, _normpoint1.Y - _dir.Item2);

                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;

                if (temp > 500)
                {
                    break;
                }
            }

            foreach(LanePoints x in _lanePoints)
            {
                Log.Write(x.ToString());
            }

        }

        private Tuple<int,int> GetDirection(Point _point1, Point _point2)
        {
            int dirx = 0; int diry = 0;

            if (_point1.X < _point2.X)
            {
                dirx = -1;
            }
            else if (_point1.X > _point2.X)
            {
                dirx = 1;
            }
            if (_point1.Y < _point2.Y)
            {
                diry = -1;
            }
            else if (_point1.Y > _point2.Y)
            {
                diry = 1;
            }

            return Tuple.Create(dirx, diry);
        }
    }
}
