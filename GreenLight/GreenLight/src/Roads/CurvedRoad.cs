using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GreenLight
{
    public class CurvedRoad : AbstractRoad
    {

        //A roadtype for Curved roads, In here the CalculateDrivingLane function calculates a dynamic curve inbetween 2 points
        //It gets a dir in the constructor, which stands for direction, this is to determin which way the cars should drive.
        //In this class we mainly use a lot of math, but the simple idea is: when the constructor is called, it will calculate a curved line between 2 points
        //For how many lanes you told the constructor to have.


        private string dir;

        public CurvedRoad(Point _point1, Point _point2, int _lanes, string _dir) : base(_point1, _point2, _lanes)
        {
            this.dir = _dir;
            
            for (int x = 1; x <= lanes; x++)
            {
                Drivinglanes.Add(this.CalculateLanes(point1, point2, x));
            }
        }

        protected override DrivingLane CalculateDrivingLane(Point _point1, Point _point2, int _thisLane)
        {
            Console.WriteLine("{0} --- {1}", _point1, _point2);
            

            List<LanePoints> _lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;

            Tuple<int, int> _dir = GetDirection(_point1, _point2);
            Console.WriteLine(dir);

            Point _prev = _normpoint1;
            Point _nulpoint;

            if (dir == "NE")
            {
               _nulpoint = new Point(Math.Max(_point1.X, _point2.X), Math.Min(_point1.Y, _point2.Y));
            }
            else if (dir == "NW")
            {
               _nulpoint = new Point(Math.Min(_point1.X, _point2.X), Math.Min(_point1.Y, _point2.Y));
            }
            else if (dir == "SW")
            {
               _nulpoint = new Point(Math.Min(_point1.X, _point2.X), Math.Max(_point1.Y, _point2.Y));
            }
            else // (dir == "SE")
            {
               _nulpoint = new Point(Math.Max(_point1.X, point2.X), Math.Max(_point1.Y, point2.Y));
            }

            int _deltaX = Math.Abs(_point1.X - _point2.X);
            int _deltaY = Math.Abs(_point1.Y - _point2.Y);
            int _Ytemp = 0;
            int _Xtemp = 0;
            int _ytemp = 0;
            int _xtemp = 0;

            Console.WriteLine(_nulpoint);

            for (int x = 0, y = 0; x <= _deltaX || y <= _deltaY; x++, y++)
            {
                if ((x >= _deltaX && y >= _deltaY) || _prev == _point2)
                    break;

                _Xtemp = _point1.X + x * _dir.Item1;
                _ytemp = _point1.Y + y * _dir.Item2;

                if ((dir == "NE" || dir == "NW") && x <= _deltaX)
                {
                    _Ytemp = _nulpoint.Y + (int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }
                else if ((dir == "SE" || dir == "SW") && x <= _deltaX)
                {
                    _Ytemp = _nulpoint.Y - (int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }
                if ((dir == "NE" || dir == "NW") && y <= _deltaY)
                {
                    _xtemp = _nulpoint.X + (int)Math.Sqrt(Math.Pow(_deltaX, 2) * (1 - (Math.Pow(_ytemp - _nulpoint.Y, 2) / Math.Pow(_deltaY, 2))));
                }
                else if ((dir == "SE" || dir == "SW") && y <= _deltaY)
                {
                    _xtemp = _nulpoint.X - (int)Math.Sqrt(Math.Pow(_deltaX, 2) * (1 - (Math.Pow(_ytemp - _nulpoint.Y, 2) / Math.Pow(_deltaY, 2))));
                }

                if (Math.Sqrt(Math.Pow(Math.Abs(_prev.X - _Xtemp), 2) + Math.Pow(Math.Abs(_prev.Y - _Ytemp), 2)) <= Math.Sqrt(Math.Pow(Math.Abs(_prev.X - _xtemp), 2) + Math.Pow(Math.Abs(_prev.Y - _ytemp), 2)))
                {
                    _normpoint1 = new Point(_Xtemp, _Ytemp);
                }
                else
                {
                    _normpoint1 = new Point(_xtemp, _ytemp);
                }


                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;
            }

            foreach (LanePoints x in _lanePoints)
            {
                Console.WriteLine(x.ToString());
            }

            return new DrivingLane(_lanePoints, this.dir, lanes, _thisLane);
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

        private DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
            int drivingLaneDistance = 40;

            Console.WriteLine("TEST: {0} -- {1}", _firstPoint, _secondPoint);

            string _Direction = this.dir;

                if (_Direction == "SE" || _Direction == "NW")
                {
                    if (lanes % 2 == 0)
                    {
                        if (t % 2 == 0)
                        {
                            _firstPoint.X -= (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                            _secondPoint.Y -= (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                        }
                        else
                        {
                            _firstPoint.X += (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
                            _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
                        }
                    }
                    else // (lanes % 2 == 1)
                    {
                        if (t % 2 == 0)
                        {
                            _firstPoint.X += t / 2 * drivingLaneDistance;
                            _secondPoint.Y += t / 2 * drivingLaneDistance;
                        }
                        else if (t % 2 == 1 && t != 1)
                        {
                            _firstPoint.X -= (t - 1) / 2 * drivingLaneDistance;
                            _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
                        }
                    }
                }
                else if (_Direction == "NE" || _Direction == "SW")
                {
                    if (lanes % 2 == 0)
                    {
                        if (t % 2 == 0)
                        {
                            _firstPoint.X -= (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                            _secondPoint.Y += (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                        }
                        else
                        {
                            _firstPoint.X += (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
                            _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
                        }
                    }
                    else // (lanes % 2 == 1)
                    {
                        if (t % 2 == 0)
                        {
                            _firstPoint.X += t / 2 * drivingLaneDistance;
                            _secondPoint.Y -= t / 2 * drivingLaneDistance;
                        }
                        else if (t % 2 == 1 && t != 1)
                        {
                            _firstPoint.X -= (t - 1) / 2 * drivingLaneDistance;
                            _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                        }
                    }
                }

            Console.WriteLine("STARTPOINTS : {0} -- {1}", _firstPoint, _secondPoint);

                return CalculateDrivingLane(_firstPoint, _secondPoint, t);
            }
        
    }
}
