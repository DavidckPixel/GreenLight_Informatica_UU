using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    // This is the OriginPoints class, which we created to be the spawning point of vehicles.
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project.

    public class OriginPoints
    {
        public int Weight, X, Y;
        public List<Point> lanes;
        public bool isConnection;

        public OriginPoints(int _Weight, int _X, int _Y, List<Point> _lanes, bool _isConnection)
        {
            this.Weight = _Weight;
            this.X = _X;
            this.Y = _Y;
            this.lanes = _lanes;
            this.isConnection = _isConnection;
        }
    }
}