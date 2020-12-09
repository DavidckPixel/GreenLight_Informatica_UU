using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

        public void Draw(Graphics g)
        {
            try
            {
                Point _pointtemp = points[0].cord;

                foreach (LanePoints x in points)
                {
                    g.DrawLine(Pens.Black, _pointtemp, x.cord);
                    _pointtemp = x.cord;
                }
            }catch(Exception e) { }
        }
    }
}
