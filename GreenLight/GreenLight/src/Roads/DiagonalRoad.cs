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
        //Similar to CurvedRoad, but now math for diagnol

        private string dir;

        public DiagonalRoad(Point _point1, Point _point2, int _lanes, string _dir) : base(_point1, _point2, _lanes)
        {
            this.dir = _dir;

            for (int x = 1; x <= this.lanes; x++)
            {
                this.Drivinglanes.Add(CalculateLanes(point1, point2, x));
            }
        }

        protected override DrivingLane CalculateDrivingLane(Point _point1, Point _point2)
        {
            List<LanePoints>_lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;

            double _rc = (_point2.Y - _point1.Y) / (_point2.X - _point1.X);
            int _dir = GetDirection(_point1, _point2);
            Point _prev = _normpoint1;

            for (int x = 0; x <= Math.Abs(_point1.X - _point2.X); x++)
            {
                _normpoint1 = new Point(_point1.X + x * _dir, (int)(_point1.Y + x * _rc * _dir));
                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;
            }

            return new DrivingLane(_lanePoints, 0);

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

        private DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
            int drivingLaneDistance = 40;

            if (lanes % 2 == 0)
            {
                if (t % 2 == 0)
                {
                    _firstPoint.Y -= t / 2 * drivingLaneDistance / 2;
                    _secondPoint.Y -= t / 2 * drivingLaneDistance / 2;
                }
                else
                {
                    _firstPoint.Y += (t + 1) / 2 * drivingLaneDistance / 2;
                    _secondPoint.Y += (t + 1) / 2 * drivingLaneDistance / 2;
                }
            }
            else // (lanes % 2 == 1)
            {
                if (t % 2 == 0)
                {
                    _firstPoint.Y -= t / 2 * drivingLaneDistance;
                    _secondPoint.Y -= t / 2 * drivingLaneDistance;
                }
                else if (t % 2 == 1 && t != 1)
                {
                    _firstPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                    _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                }
            }

            return CalculateDrivingLane(_firstPoint, _secondPoint);
        }

    }   
}
