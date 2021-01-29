 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GreenLight
{
    // CurvedRoad is a road type that calculates a dynamic curve inbetween 2 points to draw a road.
    // The corners of the road are calculated and used to contruct a Curvedhitbox for the road.
    // There are two kinds of CurvedRoads, one has it curve at the top side, the other at the bottom.
    // In this class we mainly use a lot of math, but the simple idea is: when the constructor is called, it will calculate a curved road between 2 points

    public class CurvedRoad : AbstractRoad
    {
        public CurvedRoad(Point _point1, Point _point2, int _lanes, string _dir, string _type, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo) : base(_point1, _point2, _lanes, "Curved", _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo)
        {
            this.Dir = _dir;
            this.Type = _type;

            if (Dir == "SE" || Dir == "SW" || Dir == "NE" || Dir == "NW")
            {
                Point[] _points = RoadMath.hitBoxPointsCurved(_point1, _point2, this.lanes, this.laneWidth, true, _dir);
                this.hitbox = new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], _dir, Color.Yellow)
                {
                    _lanes = _lanes
                };

                for (int x = 1; x <= lanes; x++)
                {
                    Drivinglanes.Add(this.CalculateLanes(point1, point2, x));
                }

                Lane.OrderDrivingLanes(this);
            }
        }
        private DrivingLane CreateDrivingLane(Point _point1, Point _point2, int _thisLane)
        {
            Point[] _points = RoadMath.hitBoxPointsCurved(_point1, _point2, 1, this.laneWidth, false, Dir);
            Hitbox _temp = new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], Dir, Color.Green);
            return new DrivingLane(LanePoints.CalculateCurveLane(_point1, _point2, this.Dir), this.Dir, this.lanes, _thisLane, _temp);
        }
        
        private DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
        int drivingLaneDistance = this.laneWidth;
        string _Direction = this.Dir;

            if (_Direction == "SE" || _Direction == "NW")
            {
                if (lanes % 2 == 0)
                {
                    if (t % 2 == 0)
                    {
                        _firstPoint.X += (t / 2) * drivingLaneDistance;
                        _secondPoint.Y += (t / 2) * drivingLaneDistance;
                    }
                    else
                    {
                        _firstPoint.X -= (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
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
                        _firstPoint.X -= (t / 2) * drivingLaneDistance;
                        _secondPoint.Y += (t / 2) * drivingLaneDistance;
                    }
                    else
                    {
                        _firstPoint.X += (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
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



            return CreateDrivingLane(_firstPoint, _secondPoint, t);
        }
        
        public override Hitbox CreateHitbox(Point[] _points)
        {
            return new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], Dir, Color.Yellow);
        }
    }
}
