using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    class DiagonalRoad : AbstractRoad
    {
        private char dir;

        public DiagonalRoad(Point _point1, Point _point2, int _lanes, char _dir) : base(_point1, _point2, _lanes)
        {
            this.dir = _dir;
            this.CalculateDrivingLane();
        }

        protected override void CalculateDrivingLane()
        {
            _lanePoints = new List<LanePoints>();
            Point _normpoint1 = point1; Point _normpoint2 = point2;

            double _rc = (this.point2.Y - this.point1.Y) / (this.point2.X - this.point1.X);
            int _dir = GetDirection(point1, point2);
            Point _prev = _normpoint1;

            for (int x = 0; x <= Math.Abs(point1.X - point2.X); x++)
            {
                _normpoint1 = new Point(point1.X + x * _dir, (int)(point1.Y + x * _rc * _dir));
                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;
            }

            foreach (LanePoints x in _lanePoints)
            {
                Console.WriteLine(x.ToString());
            }
        }

        private int GetDirection(Point _point1, Point _point2)
        {
            if (_point2.X >= _point1.X)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

    }   
}
