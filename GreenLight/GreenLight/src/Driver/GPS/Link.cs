using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //A link has a begin knot and an end knot and a laneIndex. There are as many links between two knots as there are lanes. 
    //A link can be contructed with a Begin Knot and an End Knot. A laneIndex should be given when it's not the first lane.
    public class Link
    {
        public int laneIndex;
        public Knot begin;
        public Knot end;

        public Link(Knot _begin, Knot _end, int _laneIndex)
        {
            this.begin = _begin;
            this.end = _end;
            this.laneIndex = _laneIndex;
        }
    }
}
