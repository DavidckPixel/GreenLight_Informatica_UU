using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    class GPSData
    {
        List<Node> nodes;
        List<AbstractRoad> roads;

        public HashSet<Knot> _allKnots;

        public GPSData(List<AbstractRoad> _roads)
        {
            nodes = new List<Node>();
            this.roads = _roads;

            FindAllKnots();
        }

        private void FindAllKnots()
        {
            _allKnots = new HashSet<Knot>();

            foreach (AbstractRoad _road in roads)
            {
                Knot _TempKnot = new Knot(_road, _road.endConnectedTo, _road.point2);
                if (!TestDuplicateKnot(_TempKnot))
                {
                    _allKnots.Add(_TempKnot);
                }
                _TempKnot = new Knot(_road.beginConnectedTo, _road, _road.point1);
                if (!TestDuplicateKnot(_TempKnot))
                {
                    _allKnots.Add(_TempKnot);
                }
            }

            Console.WriteLine("Knot count: " + _allKnots.Count());
        }

        private bool TestDuplicateKnot(Knot _knot)
        {
            foreach(Knot _testKnot in _allKnots)
            {
                if (_knot.Equals(_testKnot))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
