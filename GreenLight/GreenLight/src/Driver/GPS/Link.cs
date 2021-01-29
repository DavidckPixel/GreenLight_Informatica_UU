using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{

    //A link is a connection between 2 knots, it also contains the laneindex of the road with which lane it is connected

    public class Link
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
            Log.Write("Begin Knot: " + begin.Cord + ", End Knot: " + end.Cord + ", LaneIndex: " + laneIndex);
        }
    }
}
