using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    // The CrossLane class is only used for CrossRoads, it is a lane based on a ConnectionLink that the user makes

    public class CrossLane : Lane
    {
        public CrossLane(List<LanePoints> _points, ConnectionLink _link, int _index)
        {
            this.points = _points;
            this.link = _link;
            this.thisLane = _index;
        }

        public override void Draw(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void DrawoffsetHitbox(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void FlipPoints()
        {
            throw new NotImplementedException();
        }
    }
}
