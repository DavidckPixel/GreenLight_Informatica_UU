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


        

        public CurvedRoad(Point _point1, Point _point2, int _lanes, string _dir, string _type) : base(_point1, _point2, _lanes)
        {
            this.Dir = _dir;
            this.Type = _type;

            Point[] _points = hitBoxPoints(_point1, _point2, this.lanes);
            this.Hitbox2 = new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], _dir, Color.Yellow);

            if (Dir == "SE" || Dir == "SW"|| Dir == "NE" || Dir == "NW")
            {
                for (int x = 1; x <= lanes; x++)
                {
                    Drivinglanes.Add(this.CalculateLanes(point1, point2, x));
                }
            }
            else if(Dir == "SEccw" || Dir == "SWccw" || Dir == "NEccw" || Dir == "NWccw")
            {
                for (int x = 1; x <= lanes; x++)
                {
                    Drivinglanes.Add(this.CalculateLanes(point2, point1, x));
                }
            }
            
            
            
        }

        protected override DrivingLane CalculateDrivingLane(Point _point1, Point _point2, int _thisLane)
        {
            Console.WriteLine("{0} --- {1}", _point1, _point2);
            

            List<LanePoints> _lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;

            Tuple<int, int> _dir = GetDirection(_point1, _point2);
            Console.WriteLine(Dir);

            Point _prev = _normpoint1;
            Point _nulpoint;

            if (Dir == "NE" || Dir == "NEccw")
            {
               _nulpoint = new Point(Math.Max(_point1.X, _point2.X), Math.Min(_point1.Y, _point2.Y));
            }
            else if (Dir == "NW" || Dir == "NWccw")
            {
               _nulpoint = new Point(Math.Min(_point1.X, _point2.X), Math.Min(_point1.Y, _point2.Y));
            }
            else if (Dir == "SW" || Dir == "SWccw")
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

                if ((Dir == "NE" || Dir == "NW" || Dir == "NEccw" || Dir == "NWccw") && x <= _deltaX)
                {
                    _Ytemp = _nulpoint.Y + (int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }
                else if ((Dir == "SE" || Dir == "SW" || Dir == "SEccw" || Dir == "SWccw") && x <= _deltaX)
                {
                    _Ytemp = _nulpoint.Y - (int)Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - _nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }

                if ((Dir == "NE" || Dir == "NW" || Dir == "NEccw" || Dir == "NWccw") && y <= _deltaY)
                {
                    _xtemp = _nulpoint.X + (int)Math.Sqrt(Math.Pow(_deltaX, 2) * (1 - (Math.Pow(_ytemp - _nulpoint.Y, 2) / Math.Pow(_deltaY, 2))));
                }
                else if ((Dir == "SE" || Dir == "SW" || Dir == "SEccw" || Dir == "SWccw") && y <= _deltaY)
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

            Point[] _points = hitBoxPoints(_point1, _point2, 1);
            Hitbox _temp = new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], Dir, Color.Green);

            return new DrivingLane(_lanePoints, this.Dir, lanes, _thisLane, _temp);
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

            string _Direction = this.Dir;

                if (_Direction == "SE" || _Direction == "NW" || _Direction == "SEccw" || _Direction == "NWccw")
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
                else if (_Direction == "NE" || _Direction == "SW" || _Direction == "NEccw" || _Direction == "SWccw")
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

                return CalculateDrivingLane(_firstPoint, _secondPoint, t);
            }

        public override Point[] hitBoxPoints(Point one, Point two, int _lanes, int _laneWidth = 40)
        {
            Point _one, _two;
            int _roadWidth = (_laneWidth * _lanes) / 2;
            Point[] _points = new Point[4];

            if(Dir == "NW" || Dir == "NE")
            {
                if (one.X < two.X)
                {
                    _one = two;
                    _two = one;
                }
                else
                {
                    _one = one;
                    _two = two;
                }

                if(Dir == "NW")
                {
                    _points[0] = new Point(_one.X + _roadWidth, _one.Y);
                    _points[1] = new Point(_one.X - _roadWidth, _one.Y);
                    _points[2] = new Point(_two.X, _two.Y + _roadWidth);
                    _points[3] = new Point(_two.X, _two.Y - _roadWidth);
                }
                else
                {
                    _points[0] = new Point(_one.X, _one.Y + _roadWidth);
                    _points[1] = new Point(_one.X, _one.Y - _roadWidth);
                    _points[2] = new Point(_two.X - _roadWidth, _two.Y);
                    _points[3] = new Point(_two.X + _roadWidth, _two.Y);
                }
            }
            else
            {
                if (one.X < two.X)
                {
                    _one = one;
                    _two = two;
                }
                else
                {
                    _one = two;
                    _two = one;
                }

                if(Dir == "SE")
                {
                    //
                    _points[0] = new Point(_one.X - _roadWidth, _one.Y);
                    _points[1] = new Point(_one.X + _roadWidth, _one.Y);
                    _points[2] = new Point(_two.X, _two.Y - _roadWidth);
                    _points[3] = new Point(_two.X, _two.Y + _roadWidth);

                }
                else
                {
                    //
                    _points[0] = new Point(_one.X, _one.Y - _roadWidth);
                    _points[1] = new Point(_one.X, _one.Y + _roadWidth);
                    _points[2] = new Point(_two.X + _roadWidth, _two.Y);
                    _points[3] = new Point(_two.X - _roadWidth, _two.Y);
                }
            }

            return _points;
        }

        public override Hitbox CreateHitbox(Point[] _points)
        {
            return new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], Dir, Color.Yellow);
        }
    }
}
