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
        // DiagonalRoad is a roadtype for Vertical, horizontal and diagonal roads. 
        // The CalculateLanes function calculates a straight line between two points, and uses this to create drivinglane objects for the road..
        // The corners of the road are calculated and used to contruct a Recthitbox for the road
        // In this class we mainly use a lot of math, but the simple idea is: when the constructor is called, it will calculate a straight line between 2 points.


        public DiagonalRoad(Point _point1, Point _point2, int _lanes, string _dir, string _type, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo) : base(_point1, _point2, _lanes, "DiagonalRoad", _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo)
        {
            this.Dir = _dir;
            this.Type = _type;

            double _slp = (double)(_point2.Y - _point1.Y) / (double)(_point2.X - _point1.X);
            if (_point2.X - _point1.X == 0)
            {
                _slp = 0;
            }

            this.slp = _slp;

            Point[] _points = RoadMath.hitBoxPointsDiagonal(_point1, _point2, lanes, this.laneWidth, true, this.slp, false);
            this.hitbox = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Yellow);

            for (int x = 1; x <= this.lanes; x++)
            {
                this.Drivinglanes.Add(CalculateLanes(point1, point2, x));
            }

            Lane.OrderDrivingLanes(this);
        }

        private DrivingLane CreateDrivingLane(Point _point1, Point _point2, int _thisLane)
        {
            double _slp = (double)(_point2.Y - _point1.Y) / (double)(_point2.X - _point1.X);
            if (_point2.X - _point1.X == 0)
            {
                _slp = 0;
            }

            this.slp = _slp;
        
            Point[] _points = RoadMath.hitBoxPointsDiagonal(_point1, _point2, 1, this.laneWidth, false, this.slp, false);
            Hitbox _temp = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Green);
            return new DrivingLane(LanePoints.CalculateDiagonalLane(_point1,_point2), this.Dir, this.lanes, _thisLane, _temp);
        }

        // This method moves the begin- and/or endpoint of different lanes in a DiagonalRoad, so lanes are not overlapping
        public DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
            int drivingLaneDistance = this.laneWidth;
            double slp;

            if (_firstPoint.X != _secondPoint.X && _firstPoint.Y != _secondPoint.Y)
            {
                slp = (double)(_firstPoint.Y - _secondPoint.Y) / (double)(_secondPoint.X - _firstPoint.X);

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

        public override Hitbox CreateHitbox(Point[] _points)
        {
            return new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);
        }
    }   
}
