using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class StopSign : AbstractSign
    {
        int x, y;

        public StopSign(AbstractSignController _controller) : base()
        {
            controller = _controller;
        }

        public override void Read(BetterAI _ai)
        {
            //_ai.needToBrake(x, y);
        }

        public void editLocation(Point _location)
        {
            this.x = _location.X;
            this.y = _location.Y;
        }
        public Point getSignLocation()
        {
            return new Point(x, y);
        }
    }

    
}
