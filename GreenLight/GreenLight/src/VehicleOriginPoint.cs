using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace GreenLight
{
    public class VehicleOriginPoint<Point>
    {
        public int Weight { get; set; }
        public Point Location { get; set; }
    }
}