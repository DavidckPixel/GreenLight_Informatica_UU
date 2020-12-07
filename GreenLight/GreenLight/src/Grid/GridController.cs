using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    class GridController
    {
        public List<Gridpoint> Gridpoints = new List<Gridpoint>();
        protected List<DrivingLane> drivinglanes;
        public GridConfig config;
        bool firstClick;
        int lanes = 1;
        string Selected;
        string RoadType;
        AbstractRoad roadType;
        int drivingLaneDistance = 40;
        int roadAmount;

        public GridController()
        {
            GridConfig.Init(ref this.config);
        }

        public void CreateGridPoints()
        {
            for(int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Gridpoints.Add(new Gridpoint(new Point(x * config.SpacingWidth, y * config.SpacingHeight), new Size(5,5)));
                }
            }
        }

        public void OnClick(Object o, MouseEventArgs mea)
        {
            Gridpoint _firstPoint;
            Gridpoint _secondPoint;


            if (firstClick)
            {
                _firstPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_firstPoint = !null)
                    firstClick = false;
            }
            else
            {
                _secondPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_secondPoint = !null && _secondPoint =! _fisrtPoint)
                {
                    firstClick = true;
                    if (Selected == "Roads")
                        CreateRoad(_firstPoint, _secondPoint);
                }
            }

            //Console.WriteLine(_selectedPoint);
        }

        public void CreateRoad(Gridpoint FirstPoint, Gridpoint SecondPoint)
        {
            AbstractRoad newRoad;

            switch (RoadType)
            {
                case "CurvedRoad":
                    {
                        for (int t = 1; t <= lanes; t++)
                        {
                            string Direction = Direction(_firstPoint, _secondPoint);
                            Gridpoint _firstPoint = FirstPoint;
                            Gridpoints _secondPoint = SecondPoint;
                            if (Direction == "NE" || Direction == "SW")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Y += t / 2 * drivingLaneDistance / 2; 
                                        _secondPoint.X -= t / 2 * drivingLaneDistance / 2
                                    }
                                    else 
                                    {
                                        _firstPoint.Y -= (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.X += (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Y += t / 2 * drivingLaneDistance;
                                        _secondPoint.Y -= t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t = !1)
                                    {
                                        _firstPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }
                            else if (Direction == "SE" || Direction == "NW")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Y += t / 2 * drivingLaneDistance / 2;
                                        _secondPoint.X += t / 2 * drivingLaneDistance / 2
                                    }
                                    else
                                    {
                                        _firstPoint.Y -= (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.X -= (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.Y += t / 2 * drivingLaneDistance;
                                        _secondPoint.Y += t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t = !1)
                                    {
                                        _firstPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Y -= (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }

                            newRoad = new CurvedRoad(_firstPoint, _secondPoint, lanes, Direction);
                            drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount+1));
                        }
                        roadAmount++;
                    }
                    break;
                case "DiagonalRoad":
                    for (int t = 1; t <= lanes; t++)
                    {
                        Gridpoint _firstPoint = FirstPoint;
                        Gridpoints _secondPoint = SecondPoint;

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
                            else if (t % 2 == 1 && t = !1)
                            {
                                _firstPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                                _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                            }
                        }
                        newRoad = new DiagonalRoad(_firstPoint, _secondPoint, lanes, Direction(_firstPoint, _secondPoint);
                        drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount + 1));
                    }
                    roadAmount++;
                    break;
                case "StraightRoad":
                    if (_firstPoint.X == _secondPoint.X || _firstPoint.Y == _secondPoint.Y)
                    {
                        for (int t = 1; t <= lanes; t++)
                        {
                            string Direction = Direction(_firstPoint, _secondPoint, RoadType);
                            Gridpoint _firstPoint = FirstPoint;
                            Gridpoints _secondPoint = SecondPoint;
                            if (Direction == "E" || Direction == "W")
                            {
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
                                    else if (t % 2 == 1 && t =! 1)
                                    {
                                        _firstPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }
                            else if (Direction == "N" || Direction == "Z")
                            {
                                if (lanes % 2 == 0)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.X -= t / 2 * drivingLaneDistance / 2;
                                        _secondPoint.X -= t / 2 * drivingLaneDistance;
                                    }
                                    else
                                    {
                                        _firstPoint.X += (t + 1) / 2 * drivingLaneDistance / 2;
                                        _secondPoint.X += (t + 1) / 2 * drivingLaneDistance / 2;
                                    }
                                }
                                else // (lanes % 2 == 1)
                                {
                                    if (t % 2 == 0)
                                    {
                                        _firstPoint.X -= t / 2 * drivingLaneDistance;
                                        _secondPoint.X -= t / 2 * drivingLaneDistance;
                                    }
                                    else if (t % 2 == 1 && t = !1)
                                    {
                                        _firstPoint.X += (t - 1) / 2 * drivingLaneDistance;
                                        _secondPoint.X += (t - 1) / 2 * drivingLaneDistance;
                                    }
                                }
                            }
                            newRoad = new CurvedRoad(_firstPoint, _secondPoint, lanes, Direction, RoadType);
                            drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount + 1));
                        }
                        roadAmount++;
                    }
                    break;
            }
        }

        public String Direction(Point _firstPoint, Point _secondPoint)
        {
            string RoadDirection;
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
            return RoadDirection
        }

        

        public void DrawGridPoints(Object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            foreach(Gridpoint x in Gridpoints)
            {
                x.Draw(g);
            }
        }

        //CLICK
    }
}
