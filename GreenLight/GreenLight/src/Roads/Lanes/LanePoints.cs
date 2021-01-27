using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class LanePoints
    {
        //lanepoints are very simple put, just a point with a corresponding degree, so every car can drive from points to point
        //and angle itself accordingly between the 2 to create a smooth illusion of driving.

        public Point cord { get; set; }
        public float degree;
        public Tuple<double, double> distance;

        public LanePoints(Point _cord, float _degree)
        {
            this.cord = _cord;
            this.degree = _degree;
            this.distance = new Tuple<double, double>(0, 0);
        }

        public override string ToString()
        {
            string _temp = "CORDS: "+ this.cord + "  -  DEGREE: " + this.degree + " - distance: " + this.distance.Item1 ;
            return _temp;
        }
        
        public void setDistance(double _first, double _second)
        {
            this.distance = new Tuple<double, double>(_first, _second);
        }

        
        public void Flip()
        {
            FlipDegree();
            FlipTuple();
        }
        
        private void FlipDegree()
        {
            degree = ((degree + 180) % 360);
        }
        
        private void FlipTuple()
        {
            double _item1 = distance.Item1;
            double _item2 = distance.Item2;
            distance = new Tuple<double, double>(_item2, _item1);
        }

        //The following Functions are some General purpose static functions that calculated the LanePoints based on 2 points
        //a function for both curves and Diagonal / (straight) lines

        public static List<LanePoints> CalculateCurveLane(Point _point1, Point _point2, string Dir)
        {
            List<LanePoints> _lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;

            Tuple<int, int> _dir = GetCurveDirection(_point1, _point2);

            Point _prev = _normpoint1;
            Point _nulpoint;

            double _prevXtemp = _prev.X;
            double _prevYtemp = _prev.Y;

            if (Dir == "NE")
            {
                _nulpoint = new Point(Math.Max(_point1.X, _point2.X), Math.Min(_point1.Y, _point2.Y));
            }
            else if (Dir == "NW")
            {
                _nulpoint = new Point(Math.Min(_point1.X, _point2.X), Math.Min(_point1.Y, _point2.Y));
            }
            else if (Dir == "SW")
            {
                _nulpoint = new Point(Math.Min(_point1.X, _point2.X), Math.Max(_point1.Y, _point2.Y));
            }
            else // (dir == "SE")
            {
                _nulpoint = new Point(Math.Max(_point1.X, _point2.X), Math.Max(_point1.Y, _point2.Y)); //Aangepast
            }

            double _deltaX = Math.Abs(_point1.X - _point2.X);
            double _deltaY = Math.Abs(_point1.Y - _point2.Y);
            double _Ytemp = 0;
            double _Xtemp = 0;
            double _ytemp = 0;
            double _xtemp = 0;


            for (double x = 0, y = 0; x <= _deltaX || y <= _deltaY; x += 0.50, y += 0.50)
            {
                
                if ((x > _deltaX && y > _deltaY) || _prev == _point2)
                    break;

                _Xtemp = _point1.X + x * _dir.Item1;
                _ytemp = _point1.Y + y * _dir.Item2;

                


                if ((Dir == "NE" || Dir == "NW") && x <= _deltaX)
                {
                    _Ytemp = (double)_nulpoint.Y + Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - (double)_nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }
                else if ((Dir == "SE" || Dir == "SW") && x <= _deltaX)
                {
                    _Ytemp = (double)_nulpoint.Y - Math.Sqrt(Math.Pow(_deltaY, 2) * (1 - (Math.Pow(_Xtemp - (double)_nulpoint.X, 2) / Math.Pow(_deltaX, 2))));
                }

                if ((Dir == "NE" || Dir == "NW") && y <= _deltaY)
                {
                    _xtemp = (double)_nulpoint.X + Math.Sqrt(Math.Pow(_deltaX, 2) * (1 - (Math.Pow(_ytemp - (double)_nulpoint.Y, 2) / Math.Pow(_deltaY, 2))));
                }
                else if ((Dir == "SE" || Dir == "SW") && y <= _deltaY)
                {
                    _xtemp = (double)_nulpoint.X - Math.Sqrt(Math.Pow(_deltaX, 2) * (1 - (Math.Pow(_ytemp - (double)_nulpoint.Y, 2) / Math.Pow(_deltaY, 2))));
                }

                if (Math.Sqrt(Math.Pow(Math.Abs(_prev.X - _Xtemp), 2) + Math.Pow(Math.Abs(_prev.Y - _Ytemp), 2)) <= Math.Sqrt(Math.Pow(Math.Abs(_prev.X - _xtemp), 2) + Math.Pow(Math.Abs(_prev.Y - _ytemp), 2)))
                {
                    _normpoint1 = new Point((int)_Xtemp, (int)_Ytemp);
                }
                else
                {
                    _normpoint1 = new Point((int)_xtemp, (int)_ytemp);
                }
                //Console.WriteLine("normpoint: " + _normpoint1 + " prev: " + _prev);


                
                _lanePoints.Add(new LanePoints(_normpoint1, RoadMath.CalculateAngle((float)_prevXtemp,(float)_prevYtemp, (float)_Xtemp,(float) _Ytemp)));


                _prev = _normpoint1;
                _prevXtemp = _Xtemp;
                _prevYtemp = _Ytemp;
            }
            //Console.WriteLine("test for first point: "+_lanePoints.First());
            RoadMath.CalculateDistanceLanePoints(ref _lanePoints);

            return _lanePoints;
        }

        private static Tuple<int, int> GetCurveDirection(Point _point1, Point _point2)
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

        public static List<LanePoints> CalculateDiagonalLane(Point _point1, Point _point2)
        {
            List<LanePoints> _lanePoints = new List<LanePoints>();
            Point _normpoint1 = _point1; Point _normpoint2 = _point2;
            double _slp;
            Point _prev = _normpoint1;
            bool divByZero = false;

            _slp = (double)(_point2.Y - _point1.Y) / (double)(_point2.X - _point1.X);
            if (_point2.X - _point1.X == 0)
            {
                _slp = 0;
                int _vertical = _point1.Y > _point2.Y ? -1 : 1; //ADDED THIS INSTEAD OF IF-STATEMENT
                divByZero = true;

                for (int y = 0; y <= Math.Abs(_point1.Y - _point2.Y); y++)
                {
                    _normpoint1 = new Point(_point1.X, (int)(_point1.Y + y * _vertical));
                    _lanePoints.Add(new LanePoints(_normpoint1, RoadMath.CalculateAngle(_prev, _normpoint1)));

                    _prev = _normpoint1;
                }
            }
            
            //_road.slp = _slp; Moved this to be done in Class with same code, just different place

            int _dir = _point2.X >= _point1.X ? 1 : -1; //Removed GetDiagonalDirection here

            for (int x = 0; x <= Math.Abs(_point1.X - _point2.X) && !divByZero; x++)
            {
                _normpoint1 = new Point(_point1.X + x * _dir, (int)(_point1.Y + x * _slp * _dir));
                _lanePoints.Add(new LanePoints(_normpoint1, RoadMath.CalculateAngle(_point1, _point2)));

                _prev = _normpoint1;
            }
            RoadMath.CalculateDistanceLanePoints(ref _lanePoints);
            return _lanePoints;
        }
    }
}
