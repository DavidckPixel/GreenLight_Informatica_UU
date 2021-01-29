using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{

    // This is the RoadMath class, which, as it's name suggests, deals with all math problems that come up in road classes 
    /// <include file='.../.../Documentation/XML-items/RoadMath.xml' path='docs/members[@name="roadmath"]/RoadMath/*'/>
    public static class RoadMath
    {
        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/Distance/*'/>
        public static double Distance(Point _one, Point _two)
        {
            return Math.Sqrt(Math.Pow(_one.X - _two.X, 2) + Math.Pow(_one.Y - _two.Y, 2));
        }


        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/Distance2/*'/>
        public static double Distance(double _oneX, double _oneY, double _twoX, double _twoY)
        {
            return Math.Sqrt(Math.Pow(_oneX - _twoX, 2) + Math.Pow(_oneY - _twoY, 2));
        }


        
        // The following three methods calculate at which angle a car has to position itsself on a certain LanePoint
        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/CalculateAngle/*'/>
        public static float CalculateAngle(Point _point1, Point _point2)
        {
            float _deltaX = _point1.X - _point2.X;
            float _deltaY = _point1.Y - _point2.Y;

            double _raddegree = Math.Atan2(_deltaY, _deltaX);

            float _degree = (float)(_raddegree * (180 / Math.PI)) - 90;
            if (_degree < 0)
            {
                _degree = 360 + _degree;
            }

            return _degree;
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/CalculateAngle2/*'/>
        public static float CalculateAngle(float _point1X, float _point1Y, float _point2X, float _point2Y)
        {
            float _deltaX = _point1X - _point2X;
            float _deltaY = _point1Y - _point2Y;

            double _raddegree = Math.Atan2(_deltaY, _deltaX);

            float _degree = (float)(_raddegree * (180 / Math.PI)) - 90;
            if (_degree < 0)
            {
                _degree = 360 + _degree;
            }

            return _degree;
        }

        public static float OldCalculateAngle(Point _point1, Point _point2)
        {
            float _deltaX = _point1.X - _point2.X;
            float _deltaY = _point1.Y - _point2.Y;

            double _raddegree = Math.Atan2(_deltaX, _deltaY);

            float _degree = (float)(_raddegree * (180 / Math.PI));
            if (_degree < 0)
            {
                _degree = 360 + _degree;
            }

            return _degree % 360;
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/CalculateDistanceLanePoints/*'/>
        public static void CalculateDistanceLanePoints(ref List<LanePoints> _points)
        {
            LanePoints _before = _points.First();
            LanePoints _after;
            int x = 0;

            foreach(LanePoints _point in _points)
            {
                if (x == _points.Count() - 1)
                {
                     _after = _point;
                }
                else
                {
                    _after = _points[x + 1];
                }
                double distance = Distance(_point.cord, _after.cord);
                double distance2 = Distance(_point.cord, _before.cord);
                _point.setDistance(distance, distance2);
                _before = _point;

                x++;
            }
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/AddDistanceLanePoints/*'/>
        public static double AddDistanceLanePoints(int _index1, int _index2, List<LanePoints> _points)
        {
            if(_index2 < _index1)
            {
                return 0;
            }

            List<LanePoints> _partialpoints = _points.GetRange(_index1, Math.Abs(_index2 - _index1));
            return _partialpoints.Sum(x => x.distance.Item1);
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/DistanceToLastLanePoint/*'/>
        public static double DistanceToLastLanePoint(int _index, List<LanePoints> _points)
        {
            int _index2 = _points.Count() - 1;
            return AddDistanceLanePoints(_index, _index2, _points);
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/AddDistanceMultiLanes/*'/>
        public static double AddDistanceMultiLanes(int _index, int _count, List<List<LanePoints>> _lanes) //NOT TESTED
        {
            int _added;
            double sum = 0;
            List<LanePoints> _partialpoints;

            for(int x = 0; x < _lanes.Count(); x++)
            {
                _added = _lanes[x].Count() - _index - 1;
                _count -= _added;

                if (_count > 0)
                {
                    _partialpoints = _lanes[x].GetRange(_index, _added);
                    sum += _partialpoints.Sum(y => y.distance.Item1);
                }
                else
                {
                    _partialpoints = _lanes[x].GetRange(_index, _added - _count);
                    sum += _partialpoints.Sum(y => y.distance.Item1);
                    return sum;
                }

                _index = 0;
            }

            return sum;
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/LanePointsInDistance/*'/>
        public static int LanePointsInDistance(double _distance, int _index, List<LanePoints> _points)
        {
            double _currentDistance = 0;
            int _addIndex = 0;

            while(_currentDistance < _distance)
            {
                if (_addIndex + _index >= _points.Count())
                {
                    return _addIndex;
                }

                _currentDistance += _points[_addIndex + _index].distance.Item1;
                _addIndex++;
            }

            return _addIndex;
        }


       
        // The following three methods create an array of all locations of LanePoints that are inside a Hitbox
        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/hitBoxPointsCurved/*'/>
        public static Point[] hitBoxPointsCurved(Point one, Point two, int _lanes, int _laneWidth, bool _Roadhitbox, string _dir)
        {
            Point _one, _two;
            int _roadWidth = (_laneWidth * _lanes) / 2;
            Point[] _points = new Point[4];

            if (_lanes % 2 == 0 && _Roadhitbox)
            {
                if (_dir == "SE" || _dir == "NW")
                {
                    one.X -= 10;
                    two.Y -= 10;
                }
                else if (_dir == "SW" || _dir == "NE")
                {
                    one.X -= 10;
                    two.Y += 10;
                }
            }

            if (_dir == "NW" || _dir == "NE")
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

                if (_dir == "NW")
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

                if (_dir == "SE")
                {
                    _points[0] = new Point(_one.X - _roadWidth, _one.Y);
                    _points[1] = new Point(_one.X + _roadWidth, _one.Y);
                    _points[2] = new Point(_two.X, _two.Y - _roadWidth);
                    _points[3] = new Point(_two.X, _two.Y + _roadWidth);

                }
                else
                {
                    _points[0] = new Point(_one.X, _one.Y - _roadWidth);
                    _points[1] = new Point(_one.X, _one.Y + _roadWidth);
                    _points[2] = new Point(_two.X + _roadWidth, _two.Y);
                    _points[3] = new Point(_two.X - _roadWidth, _two.Y);
                }
            }

            return _points;
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/hitBoxPointsDiagonal/*'/>
        public static Point[] hitBoxPointsDiagonal(Point one, Point two, int _lanes, int _laneWidth, bool _Roadhitbox, double _slp, bool _checklegal)
        {
            Point _one, _two;
            int _roadWidth;
            if (_checklegal && _lanes == 1)
            {
                _roadWidth = ((_laneWidth * _lanes) + 20) / 2;
            }
            else
            {
                _roadWidth = (_laneWidth * _lanes) / 2;
            }
          

            if (_lanes % 2 == 0 && _Roadhitbox)
            {
                if (_slp != 0)
                {
                    if (_slp <= -1 || _slp >= 1)
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

            if (_angle >= 45 && (_angle <= 135 || _angle > 180))
            {
                _points[0] = new Point(_one.X + _roadWidth, _one.Y);
                _points[1] = new Point(_one.X - _roadWidth, _one.Y); 
                _points[2] = new Point(_two.X + _roadWidth, _two.Y); 
                _points[3] = new Point(_two.X - _roadWidth, _two.Y);
            }
            else if (_angle >= 0 && _angle < 45)
            {
                _points[1] = new Point(_two.X, _two.Y - _roadWidth);
                _points[2] = new Point(_one.X, _one.Y + _roadWidth);
                _points[0] = new Point(_one.X, _one.Y - _roadWidth);
                _points[3] = new Point(_two.X, _two.Y + _roadWidth);
            }
            else
            {

                _points[1] = new Point(_one.X, _one.Y - _roadWidth); 
                _points[2] = new Point(_two.X, _two.Y + _roadWidth); 

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

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/hitBoxPointsCross/*'/>
        public static Point[] hitBoxPointsCross(Point one, Point two, int _lanes, int _laneWidth, bool _RoadhitBox)
        {
            int Extra = Roads.Config.crossroadExtra;
            Point[] _points = new Point[4];

            Rectangle _rec = new Rectangle(one, new Size(1, 1));
            int _inflate = _lanes * _laneWidth / 2 + Extra;

            _rec.Inflate(_inflate, _inflate);

            _points[0] = _rec.Location;
            _points[1] = new Point(_rec.Right, _rec.Top);
            _points[2] = new Point(_rec.Left, _rec.Bottom);
            _points[3] = new Point(_rec.Right, _rec.Bottom);

            return _points;
        }


        // Decides the direction of a CurvedRoad
        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/Direction/*'/>
        public static string Direction(Point _firstPoint, Point _secondPoint, string _Roadtype)
        {
            string RoadDirection = "x";
            string RoadType = _Roadtype;
            switch (RoadType)
            {
                case "Curved":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NE";
                            else
                                RoadDirection = "SE";
                        }
                        else
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NW";
                            else
                                RoadDirection = "SW";
                        }
                        
                    }
                    break;
                case "Curved2":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NE";
                            else
                                RoadDirection = "SE";
                        }
                        else
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NW";
                            else
                                RoadDirection = "SW";
                        }
                        


                    }
                    break;
                case "DiagonalRoad":
                    {
                        RoadDirection = "D";
                    }
                    break;
            }
            return RoadDirection;
        }

        /// <include file=".../.../Documentation/XML-items/RoadMath.xml" path='docs/members[@name="roadmath"]/calculateSlope/*'/>
        public static double calculateSlope(Point one, Point two)
        {
            double _slp = (double)(two.Y - one.Y) / (double)(two.X - one.X); 
            if (two.X - one.X == 0)
            {
                _slp = 0;
            }

            return _slp;
        }

    }
}
