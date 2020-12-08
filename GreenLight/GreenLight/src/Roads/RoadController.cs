using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    class RoadController
    {
        int drivingLaneDistance = 40;
        int roadAmount;
        protected List<DrivingLane> drivinglanes;
        
        string Selected;
        
        AbstractRoad roadType;
        public void CreateRoad(Gridpoint _firstPoint, Gridpoint _secondPoint, string _roadType, int _lanes)
        {
            AbstractRoad newRoad;
            string RoadType = _roadType;
            int lanes = _lanes;

            switch (RoadType)
            {
                case "CurvedRoad":
                    {
                        for (int t = 1; t <= lanes; t++)
                        {
                            string _Direction = Direction(_firstPoint.Cords, _secondPoint.Cords);

                            if (_Direction == "NE" || _Direction == "SW")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.Y += t / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.X -= t / 2 * drivingLaneDistance / 2;
                                    }
                                    else
                                    {
                                        _firstPoint.Cords.Y -= (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.X += (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.Y += t / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.Y -= t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t != 1)
                                    {
                                        _firstPoint.Cords.Y -= (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.Y += (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }
                            else if (_Direction == "SE" || _Direction == "NW")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.Y += t / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.X += t / 2 * drivingLaneDistance / 2;
                                    }
                                    else
                                    {
                                        _firstPoint.Cords.Y -= (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.X -= (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.Y += t / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.Y += t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t != 1)
                                    {
                                        _firstPoint.Cords.Y -= (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.Y -= (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }

                            newRoad = new CurvedRoad(_firstPoint.Cords, _secondPoint.Cords, lanes, _Direction);
                            drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount + 1));
                        }
                        roadAmount++;
                    }
                    break;

                case "DiagonalRoad":
                    for (int t = 1; t <= lanes; t++)
                    {
                        if (lanes % 2 == 0)
                        {
                            if (t % 2 == 0)
                            {
                                _firstPoint.Cords.Y -= t / 2 * drivingLaneDistance / 2;
                                _secondPoint.Cords.Y -= t / 2 * drivingLaneDistance / 2;
                            }
                            else
                            {
                                _firstPoint.Cords.Y += (t + 1) / 2 * drivingLaneDistance / 2;
                                _secondPoint.Cords.Y += (t + 1) / 2 * drivingLaneDistance / 2;
                            }
                        }
                        else // (lanes % 2 == 1)
                        {
                            if (t % 2 == 0)
                            {
                                _firstPoint.Cords.Y -= t / 2 * drivingLaneDistance;
                                _secondPoint.Cords.Y -= t / 2 * drivingLaneDistance;
                            }
                            else if (t % 2 == 1 && t != 1)
                            {
                                _firstPoint.Cords.Y += (t - 1) / 2 * drivingLaneDistance;
                                _secondPoint.Cords.Y += (t - 1) / 2 * drivingLaneDistance;
                            }
                        }
                        newRoad = new DiagonalRoad(_firstPoint.Cords, _secondPoint.Cords, lanes, Direction(_firstPoint.Cords, _secondPoint.Cords));
                        drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount + 1));
                    }
                    roadAmount++;
                    break;
                case "StraightRoad":
                    if (_firstPoint.Cords.X == _secondPoint.Cords.X || _firstPoint.Cords.Y == _secondPoint.Cords.Y)
                    {
                        for (int t = 1; t <= lanes; t++)
                        {
                            string _Direction = Direction(_firstPoint.Cords, _secondPoint.Cords);

                            if (_Direction == "E" || _Direction == "W")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.Y -= t / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.Y -= t / 2 * drivingLaneDistance / 2;
                                    }
                                    else
                                    {
                                        _firstPoint.Cords.Y += (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.Y += (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.Y -= t / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.Y -= t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t != 1)
                                    {
                                        _firstPoint.Cords.Y += (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.Y += (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }
                            else if (_Direction == "N" || _Direction == "Z")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.X -= t / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.X -= t / 2 * drivingLaneDistance;
                                    }
                                    else
                                    {
                                        _firstPoint.Cords.X += (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.Cords.X += (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Cords.X -= t / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.X -= t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t != 1)
                                    {
                                        _firstPoint.Cords.X += (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Cords.X += (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }
                            newRoad = new CurvedRoad(_firstPoint.Cords, _secondPoint.Cords, lanes, _Direction);
                            drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount + 1));
                        }
                        roadAmount++;
                    }
                    break;
            }
        }

        public String Direction(Point _firstPoint, Point _secondPoint)
        {
            string RoadDirection = "";
            switch (RoadType)
            {
                case "CurvedRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                        {
                            if (_firstPoint.Y < _secondPoint.X)
                                RoadDirection = "SW";
                            else
                                RoadDirection = "NW";
                        }
                        else
                        {
                            if (_firstPoint.Y < _secondPoint.X)
                                RoadDirection = "SE";
                            else
                                RoadDirection = "NE";
                        }
                    }
                    break;
                case "DiagonalRoad":
                    {
                        RoadDirection = "";
                    }
                    break;
                case "StraightRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                            RoadDirection = "E";
                        else if (_secondPoint.X < _firstPoint.X)
                            RoadDirection = "W";
                        else if (_firstPoint.Y < _secondPoint.Y)
                            RoadDirection = "Z";
                        else if (_firstPoint.Y > _secondPoint.Y)
                            RoadDirection = "N";
                    }
                    break;

            }
            return RoadDirection;
        }
    }
}
