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
        //Every road has a list of these DrivingLanes, a driving lane consists of a list of LanePoints
        //And for now an int that determines which type of road it is.
        //Each object from this class also has its own Draw feature, this draw feature
        //Draws a straight lane between all the points in the LanePoints list in order.
        //This is used for testing to see if our algorithm created a smooth road -- This will not be used in final release.


        List<LanePoints> points;
        int Road; //Needs to be removed

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
