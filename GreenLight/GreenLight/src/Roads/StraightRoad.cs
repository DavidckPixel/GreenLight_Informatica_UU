﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    class StraightRoad : AbstractRoad
    {

        //Similar to curved road, but now math for StraightRoads

        private string dir;

        private int roadwidth = 10; // HARDCODED WAARDE AANPASSEN

        public StraightRoad(Point _point1, Point _point2, int _lanes, string _dir) : base(_point1, _point2, _lanes)
        {
            
            this.dir = _dir;

            for(int x = 1; x <= this.lanes; x++)
            {
                this.Drivinglanes.Add(CalculateLanes(point1, point2, x));
            }
        }

        protected override DrivingLane CalculateDrivingLane(Point _point1, Point _point2)
        {
            Console.WriteLine("STARTPOINTS : {0} -- {1},   {2}", _point1, _point2, this.dir);

            List<LanePoints> _lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;

            /*
            if (this.dir == "N" || this.dir == "S")
            {
                _normpoint1 = new Point(_point1.X - this.roadwidth, _point1.Y);
                _normpoint2 = new Point(_point2.X + this.roadwidth, _point2.Y);
            }
            else if (dir == "E" || this.dir == "W") 
            {
                _normpoint1 = new Point(_point1.Y - this.roadwidth, _point1.X);
                _normpoint2 = new Point(_point2.Y + this.roadwidth, _point2.X);
            } */
           

            Tuple<int, int> _dir = GetDirection(_normpoint1, _normpoint2);
            Point _prev = _normpoint1;
            int temp = 0;

            while (Math.Abs(_normpoint1.X - _normpoint2.X) > 0 || Math.Abs(_normpoint1.Y - _normpoint2.Y) > 0)
            {
                temp++;

                Console.WriteLine("{0} -- {1}", _normpoint1, _normpoint2);
                _normpoint1 = new Point(_normpoint1.X - _dir.Item1, _normpoint1.Y - _dir.Item2);

                _lanePoints.Add(new LanePoints(_normpoint1, AbstractRoad.CalculateAngle(_prev, _normpoint1)));

                _prev = _normpoint1;

                if (temp > 500)
                {
                    break;
                }
            }

            return new DrivingLane(_lanePoints, 0);

            foreach (LanePoints x in _lanePoints)
            {
                Log.Write(x.ToString());
            }

              
        }

        private Tuple<int,int> GetDirection(Point _point1, Point _point2)
        {
            int dirx = 0; int diry = 0;

            if (_point1.X < _point2.X)
            {
                dirx = -1;
            }
            else if (_point1.X > _point2.X)
            {
                dirx = 1;
            }
            if (_point1.Y < _point2.Y)
            {
                diry = -1;
            }
            else if (_point1.Y > _point2.Y)
            {
                diry = 1;
            }

            return Tuple.Create(dirx, diry);
        }

        private DrivingLane CalculateLanes(Point _firstPoint, Point _secondPoint, int t)
        {
            string _Direction = this.dir;
            int drivingLaneDistance = 40;

            if (_Direction == "E" || _Direction == "W")
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
                    else if (t % 2 == 1 && t != 1)
                    {
                        _firstPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.Y += (t - 1) / 2 * drivingLaneDistance;
                    }
                }
            }
            else if (_Direction == "N" || _Direction == "S")
            {
                if (lanes % 2 == 0)
                {
                    if (t % 2 == 0)
                    {
                        _firstPoint.X -= t / 2 * drivingLaneDistance / 2;
                        _secondPoint.X -= t / 2 * drivingLaneDistance / 2;
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
                    else if (t % 2 == 1 && t != 1)
                    {
                        _firstPoint.X += (t - 1) / 2 * drivingLaneDistance;
                        _secondPoint.X += (t - 1) / 2 * drivingLaneDistance;
                    }
                }
            }
            return CalculateDrivingLane(_firstPoint, _secondPoint);
        }
    }
}
