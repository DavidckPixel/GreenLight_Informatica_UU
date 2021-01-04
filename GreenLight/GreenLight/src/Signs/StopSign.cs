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

        public StopSign(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public override void Read(AI _ai)
        {
            _ai.needToBrake(x, y);
        }

        public Point getSignLocation()
        {
            return new Point(x, y);
        }
    }

    
}
