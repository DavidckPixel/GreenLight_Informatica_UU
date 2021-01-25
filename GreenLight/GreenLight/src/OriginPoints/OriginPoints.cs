using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class OriginPoints
    {
        //This class stores all current OriginPoints

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