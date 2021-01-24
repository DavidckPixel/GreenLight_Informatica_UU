using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class DiagonalRoad : AbstractRoad
    {
        //A roadtype for Vertical, horizontal and diagonal roads. 
        //The CalculateLanes function calculates a straight line between 2 points, and uses this to create drivinglane objects for the road.
        //It gets a dir in the constructor, which stands for direction, this is to deterin which way the cars should drive.
        //The corners of the road are calculated and used to contruct a Recthitbox for the road
        //In this class we mainly use a lot of math, but the simple idea is: when the constructor is called, it will calculate a curved line between 2 points
        //For how many lanes you told the constructor to have.


        public DiagonalRoad(Point _point1, Point _point2, int _lanes, string _dir, string _type, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo) : base(_point1, _point2, _lanes, "DiagonalRoad", _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo)
        {
            Console.WriteLine("Diagonal Road");
            this.Dir = _dir;
            this.Type = _type;

            Point[] _points = hitBoxPoints(_point1, _point2, lanes, 20, true);
            this.hitbox = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Yellow);

            for (int x = 1; x <= this.lanes; x++)
            {
                this.Drivinglanes.Add(CalculateLanes(point1, point2, x));
            }
        }

        private DrivingLane CreateDrivingLane(Point _point1, Point _point2, int _thisLane)
        {
            double _slp = (double)(_point2.Y - _point1.Y) / (double)(_point2.X - _point1.X); //This code was borrow from the CalculateLanePoints since apperantly the slp is calculated in there
            if (_point2.X - _point1.X == 0)
            {
                _slp = 0;
            }

            this.slp = _slp;
        
            Point[] _points = hitBoxPoints(_point1, _point2, 1, 20, false);
            Hitbox _temp = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Green);
            return new DrivingLane(LanePoints.CalculateDiagonalLane(_point1,_point2), this.Dir, this.lanes, _thisLane, _temp);
        }

        public DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
            int drivingLaneDistance = 20;
            double slp;

            if (_firstPoint.X != _secondPoint.X && _firstPoint.Y != _secondPoint.Y)
            {
                slp = (double)(_firstPoint.Y - _secondPoint.Y) / (double)(_secondPoint.X - _firstPoint.X);

                //slpPer = -1 / slp;
                //oneX = Math.Abs(Math.Sqrt(1 + Math.Pow(slpPer, 2)));
                //amountX = drivingLaneDistance / oneX;

                if (lanes % 2 == 0)
                {
                    if (t % 2 == 0)
                    {
                        if (slp <= -1 || slp >= 1)
                        {
                            _firstPoint.X += (int)( drivingLaneDistance * (t / 2));
                            _secondPoint.X += (int)(drivingLaneDistance * (t / 2));
                        }
                        else
                        {
                            _firstPoint.Y += (int)(drivingLaneDistance * (t / 2));
                            _secondPoint.Y += (int)(drivingLaneDistance * (t / 2));
                        }
                    }
                    else
                    {
                        if (slp <= -1 || slp >= 1)
                        {
                            _firstPoint.X -= (int)(drivingLaneDistance * (t - 1) / 2);
                            _secondPoint.X -= (int)(drivingLaneDistance * (t - 1) / 2);
                        }
                        else
                        {
                            _firstPoint.Y -= (int)(drivingLaneDistance * (t - 1) / 2);
                            _secondPoint.Y -= (int)(drivingLaneDistance * (t - 1) / 2);
                        }
                    }
                }
                else // (lanes % 2 == 1)
                {
                    if (t % 2 == 0)
                    {
                        if (slp <= -1 || slp >= 1)
                        {
                            _firstPoint.X -= (int)(drivingLaneDistance * t / 2);
                            _secondPoint.X -= (int)(drivingLaneDistance * t / 2);
                        }
                        else
                        {
                            _firstPoint.Y -= (int)(drivingLaneDistance * t / 2);
                            _secondPoint.Y -= (int)(drivingLaneDistance * t / 2);
                        }
                    }
                    else if (t % 2 == 1 && t != 1)
                    {
                        if (slp <= -1 || slp >= 1)
                        {
                            _firstPoint.X += (int)(drivingLaneDistance * (t - 1) / 2);
                            _secondPoint.X += (int)(drivingLaneDistance * (t - 1) / 2);
                        }
                        else
                        {
                            _firstPoint.Y += (int)(drivingLaneDistance * (t - 1) / 2);
                            _secondPoint.Y += (int)(drivingLaneDistance * (t - 1) / 2);
                        }

                    }
                }
            }
            else if (_firstPoint.X == _secondPoint.X)
            {
                if (lanes % 2 == 0)
                {
                    if (t % 2 == 0)
                    {
                        _firstPoint.X += (t / 2) * drivingLaneDistance;
                        _secondPoint.X += (t / 2) * drivingLaneDistance;
                    }
                    else
                    {
                        _firstPoint.X -= (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.X -= (t - 1) / 2 * drivingLaneDistance;
                    }
                }
                else //if (lanes%2 !=0)
                {
                    if (t % 2 == 0)
                    {
                        _firstPoint.X -= t / 2 * drivingLaneDistance;
                        _secondPoint.X -= t / 2 * drivingLaneDistance;
                    }
                    else if (t % 2 == 1 && t != 1)
                    {
                        _firstPoint.X += (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.X += (t - 1) / 2 * drivingLaneDistance;
                    }
                }
            }
            else //if(_fistrPoint.Y == _secondPoint.Y)
            {
                if (lanes % 2 == 0)
                {
                    if (t % 2 == 0)
                    {
                        _firstPoint.Y += (t / 2) * drivingLaneDistance;
                        _secondPoint.Y += (t / 2) * drivingLaneDistance;
                    }
                    else
                    {
                        _firstPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
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
            }

            return CreateDrivingLane(_firstPoint, _secondPoint, t);
        }

        public override Point[] hitBoxPoints(Point one, Point two, int _lanes, int _laneWidth, bool _Roadhitbox)
        {
            Point _one, _two;
            int _roadWidth = (_laneWidth * _lanes) / 2;

            if (lanes % 2 == 0 && _Roadhitbox)
            {
                if (this.slp != 0)
                {
                    if (this.slp <= -1 || this.slp >= 1)
                    {
                       one.X += 10;
                       two.X += 10;
                    }
                    else
                    {
                        one.Y += 10;
                        two.Y += 10;
                    }
                }
                else
                {
                    if (one.X == two.X)
                    {
                        one.X += 10;
                        two.X += 10;
                    }
                    else
                    {
                        one.Y += 10;
                        two.Y += 10;
                    }
                }
            }

            if (one.Y <= two.Y)
            {
                _one = one;
                _two = two;
            }
            else
            {
                _two = one;
                _one = two;
            }

            Point[] _points = new Point[4];
            int _angle;

            float xDiff = _one.X - _two.X;
            float yDiff = _one.Y - _two.Y;
            _angle = (int)(Math.Atan2(yDiff, xDiff) * (180 / Math.PI));
            _angle = Math.Abs(_angle);

            //Console.WriteLine("Angle: {0}", _angle);

            if (_angle >= 45 && (_angle < 135 || _angle > 180)) 
            {
                _points[0] = new Point(_one.X + _roadWidth, _one.Y);
                _points[1] = new Point(_one.X - _roadWidth, _one.Y); //Hoogste punt Altijd
                _points[2] = new Point(_two.X + _roadWidth, _two.Y); //Laagste Punt
                _points[3] = new Point(_two.X - _roadWidth, _two.Y); 
            }
            else if(_angle >= 0 && _angle < 45)
            {
                _points[1] = new Point(_two.X, _two.Y - _roadWidth);
                _points[2] = new Point(_one.X, _one.Y + _roadWidth);
                _points[0] = new Point(_one.X, _one.Y - _roadWidth);
                _points[3] = new Point(_two.X, _two.Y + _roadWidth);
            }
            else
            {

                _points[1] = new Point(_one.X, _one.Y - _roadWidth); //Hoogste punt, altijd
                _points[2] = new Point(_two.X, _two.Y + _roadWidth); // Laagste Punt

                if (_one.Y + _roadWidth <= _two.Y - _roadWidth)
                {
                    _points[0] = new Point(_one.X, _one.Y + _roadWidth);
                    _points[3] = new Point(_two.X, _two.Y - _roadWidth);
                }
                else
                {
                    _points[3] = new Point(_one.X, _one.Y + _roadWidth);
                    _points[0] = new Point(_two.X, _two.Y - _roadWidth);
                }
            }

            return _points;
        }


        public override Hitbox CreateHitbox(Point[] _points)
        {
            return new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);
        }
    }   
}
