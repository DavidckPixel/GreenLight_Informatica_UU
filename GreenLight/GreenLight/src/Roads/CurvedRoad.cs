using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GreenLight
{
    class CurvedRoad : AbstractRoad
    {
        private string dir;

        public CurvedRoad(Point _point1, Point _point2, int _lanes, string _dir) : base(_point1, _point2, _lanes)
        {
            this.dir = _dir;
            this.CalculateDrivingLane();
        }

        protected override void CalculateDrivingLane()
        {
            _lanePoints = new List<LanePoints>();
            Point _normpoint1 = point1; Point _normpoint2 = point2;

            Tuple<int, int> _dir = GetDirection(point1, point2);
            Point _prev = _normpoint1;
            Point _nulpoint;

            if (dir == "NE")
            {
               _nulpoint = new Point(Math.Max(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            }
            else if (dir == "NW")
            {
               _nulpoint = new Point(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            }
            else if (dir == "SW")
            {
               _nulpoint = new Point(Math.Min(point1.X, point2.X), Math.Max(point1.Y, point2.Y));
            }
            else // (dir == "SE")
            {
               _nulpoint = new Point(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y));
            }

            int _deltaX = Math.Abs(point1.X - point2.X);
            int _deltaY = Math.Abs(point1.Y - point2.Y);
            int _Ytemp = 0;
            int _Xtemp = 0;

            Console.WriteLine(_nulpoint);
            for (int x = 0; x <= _deltaX; x++)
            {
                
                _Xtemp = point1.X + x *_dir.Item1;

                if (dir == "NE" || dir == "NW")
                {
                    _Ytemp = _nulpoint.Y + (int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }
                else // if (dir == "SE" || dir == "SW")
                {
                    _Ytemp = _nulpoint.Y - (int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }

                _normpoint1 = new Point(_Xtemp, _Ytemp);
                Console.WriteLine((int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2)))));
                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;
            }

            foreach (LanePoints x in _lanePoints)
            {
                Console.WriteLine(x.ToString());
            }
        }

        private Tuple<int, int> GetDirection(Point _point1, Point _point2)
        {
            int dirx = 0; int diry = 0;

            if (_point1.X < _point2.X)
            {
                dirx = 1;
            }
            else if (_point1.X > _point2.X)
            {
                dirx = -1;
            }
            if (_point1.Y < _point2.Y)
            {
                diry = 1;
            }
            else if (_point1.Y > _point2.Y)
            {
                diry = -1;
            }

            return Tuple.Create(dirx, diry);
        }
    }
}
