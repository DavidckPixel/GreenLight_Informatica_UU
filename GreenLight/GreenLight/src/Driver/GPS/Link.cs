using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    class Link
    {
        int laneIndex;
        Knot begin;
        Knot end;
        double length;

        public Link(Knot _begin, Knot _end, int _laneIndex = 1)
        {
            this.begin = _begin;
            this.end = _end;
            this.laneIndex = _laneIndex;
        }
    }
}
