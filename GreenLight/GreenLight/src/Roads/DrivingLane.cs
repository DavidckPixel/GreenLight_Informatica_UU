using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    class DrivingLane
    {
        //string connection;
        List<LanePoints> points;
        int Road;

        public DrivingLane(List<LanePoints> _points, int _Road)
        {
            this.points = _points;
            this.Road = _Road;
        }
    }
}
