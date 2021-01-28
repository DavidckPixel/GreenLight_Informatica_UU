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
    // This is the VehicleOriginPoint class, it was only used to save the Weight and Location of used OriginPoints
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project.

    class VehicleOriginPoint<Point>
    {
        public int Weight { get; set; }
        public Point Location { get; set; }
    }
}