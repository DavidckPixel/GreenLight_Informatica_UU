using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    class Link
    {
        public int laneIndex;
        public Knot begin;
        public Knot end;
        double length;

        public Link(Knot _begin, Knot _end, int _laneIndex = 1)
        {
            this.begin = _begin;
            this.end = _end;
            this.laneIndex = _laneIndex;
        }

        public void ConsolePrint()
        {
            Console.WriteLine("Begin Knot: {0}, End Knot: {1}, LaneIndex {2}", begin.Cord, end.Cord, laneIndex);
        }
    }
}
