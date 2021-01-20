using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public static class RoadMath
    {

        public static double Distance(Point _one, Point _two)
        {
            return Math.Sqrt(Math.Pow(_one.X - _two.X, 2) + Math.Pow(_one.Y - _two.Y, 2));
        }

        public static double Distance(double _oneX, double _oneY, double _twoX, double _twoY)
        {
            return Math.Sqrt(Math.Pow(_oneX - _twoX, 2) + Math.Pow(_oneY - _twoY, 2));
        }

        /*
        public static float TranslateDegree(float _degree)
        {
            _degree = _degree - 90 < 0 ? 360 : _degree - 90;
            return _degree % 360;
        } */

        public static float CalculateAngle(Point _point1, Point _point2) //Copied van roads
        {
            //calculateAngle
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

        public static float CalculateAngle(float _point1X, float _point1Y, float _point2X, float _point2Y) //Copied van roads
        {
            //calculateAngle
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

        public static float OldCalculateAngle(Point _point1, Point _point2) //Copied van roads
        {
            //calculateAngle
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

        public static double AddDistanceLanePoints(int _index1, int _index2, List<LanePoints> _points) //NOT TESTED
        {
            if(_index2 < _index1)
            {
                return 0;
            }

            List<LanePoints> _partialpoints = _points.GetRange(_index1, _index2 - _index1);
            return _partialpoints.Sum(x => x.distance.Item1);
        }

        public static double DistanceToLastLanePoint(int _index, List<LanePoints> _points)
        {
            int _index2 = _points.Count() - 1;
            return AddDistanceLanePoints(_index, _index2, _points);
        }

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
    }
}
