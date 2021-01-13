﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class DiagonalRoad : AbstractRoad
    {
        //Similar to CurvedRoad, but now math for diagonal

        private string dir;

        public DiagonalRoad(Point _point1, Point _point2, int _lanes, string _dir, string _type, bool _beginconnection, bool _endconnection) : base(_point1, _point2, _lanes, "DiagonalRoad", _beginconnection, _endconnection)
        {
            this.Dir = _dir;
            this.Type = _type;

            Point[] _points = hitBoxPoints(_point1, _point2, lanes);
            //Console.WriteLine("{0},{1},{2},{3}", _points[1], _points[0], _points[3], _points[2]);
            this.Hitbox2 = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Yellow);

            for (int x = 1; x <= this.lanes; x++)
            {
                this.Drivinglanes.Add(CalculateLanes(point1, point2, x));
            }
        }

        protected override DrivingLane CalculateDrivingLane(Point _point1, Point _point2, int _thisLane)
        {
            List<LanePoints>_lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;
            double _slp;
            Point _prev = _normpoint1;
            bool divByZero = false;
           
            _slp = (double)(_point2.Y - _point1.Y) / (double)(_point2.X - _point1.X);
            if (_point2.X - _point1.X == 0)
            { 
                _slp = 0;
                int _vertical;
                divByZero = true;
                if (_point1.Y > _point2.Y)
                    _vertical = -1;
                else
                    _vertical = 1;

                for (int y = 0 ; y <= Math.Abs(_point1.Y - _point2.Y); y++)
                {
                    _normpoint1 = new Point(_point1.X, (int)(_point1.Y + y * _vertical));
                    _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                    _prev = _normpoint1;
                }
            }
            Console.WriteLine(_slp);
            this.slp = _slp;

            int _dir = GetDirection(_point1, _point2);

            /*if (_rc >= 0.5 || _point2.X - _point1.X == 0)
            {
                for (int y = 0; y <= Math.Abs(_point1.Y - _point2.Y); y++)
                {
                    _normpoint1 = new Point(_point1.X + y/_rc * _dir, (int)(_point1.Y + y * _rc * _dir));
                    _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                    _prev = _normpoint1;
                }
            }*/

            for (int x = 0; x <= Math.Abs(_point1.X - _point2.X) && !divByZero; x++)
            {
                _normpoint1 = new Point(_point1.X + x * _dir, (int)(_point1.Y + x * _slp * _dir));
                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;
            }
            Point[] _points = hitBoxPoints(_point1, _point2, 1);
            Hitbox _temp = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Green);
            return new DrivingLane(_lanePoints, this.Dir, lanes, _thisLane, _temp); 
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

        public DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
            int drivingLaneDistance = 20;
            int side;
            double slp, slpPer, oneX, amountX;

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
                            _firstPoint.X += (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t / 2 - 1));
                            _secondPoint.X += (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t / 2 - 1));
                        }
                        else
                        {
                            _firstPoint.Y += (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t / 2 - 1));
                            _secondPoint.Y += (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t / 2 - 1));
                        }
                    }
                    else
                    {
                        if (slp <= -1 || slp >= 1)
                        {
                            _firstPoint.X -= (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t - 1) / 2);
                            _secondPoint.X -= (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t - 1) / 2);
                        }
                        else
                        {
                            _firstPoint.Y -= (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t - 1) / 2);
                            _secondPoint.Y -= (int)(drivingLaneDistance / 2 + drivingLaneDistance * (t - 1) / 2);
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
                        _firstPoint.X += (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                        _secondPoint.X += (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                    }
                    else
                    {
                        _firstPoint.X -= (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
                        _secondPoint.X -= (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
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
                        _firstPoint.Y += (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                        _secondPoint.Y += (t / 2 - 1) * drivingLaneDistance + drivingLaneDistance / 2;
                    }
                    else
                    {
                        _firstPoint.Y -= (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
                        _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance + drivingLaneDistance / 2;
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

            return CalculateDrivingLane(_firstPoint, _secondPoint, t);
        }

        public override Point[] hitBoxPoints(Point one, Point two, int _lanes, int _laneWidth = 20)
        {
            Point _one, _two;
            int _roadWidth = (_laneWidth * _lanes) / 2;

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

                Console.WriteLine("Kom je hier??");

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