using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    // The Arrows that are drawn on each ConnectionPoint to show in which directions a car can go.
    public class CrossArrow
    {
        public Point Location;
        public Bitmap bitmap;
        public CrossRoad crossroad;

        public CrossArrow(Point _location, Bitmap _bitmap, CrossRoad _crossroad)
        {
            this.Location = _location;
            this.bitmap = _bitmap;
            this.crossroad = _crossroad;
        }
    }
}
