using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    
    //A roadtype for Crossroads.
    //The corners of the crossroad are calculated and used to contruct a Recthitbox for the crossroad
    //A scale is calculated, and used to calculate connectionpoints for this road at the right place for when a settingsscreen is opened for the crossroad.


    public class CrossRoad : AbstractRoad
    {
        public List<ConnectionPoint> connectPoints = new List<ConnectionPoint>();
        public List<ConnectionLink> connectLinks = new List<ConnectionLink>();

        public ConnectionPoint selectedPoint;
        private double Scale;

        public CrossRoad(Point _point1, Point _point2, int _lanes, string _roadtype, bool _beginconnection, bool _endconnection) : base(_point1, _point2, _lanes, _roadtype, _beginconnection, _endconnection)
        {
            hitbox = CreateHitbox(hitBoxPoints(_point1, _point1, _lanes));

            int _width = (_lanes + 2) * 20 + 20;
            Scale = 500 / (double)_width;

            createConnectionPoints();
            SwitchSelectedPoint(connectPoints.First());
        }

        public override Hitbox CreateHitbox(Point[] _array)
        {
            return new RectHitbox(_array[0], _array[1], _array[2], _array[3], Color.Yellow);
        }

        public void SwitchSelectedPoint(ConnectionPoint _point)
        {
            if (selectedPoint != null)
            {
                selectedPoint.Hitbox.color = Color.Green;
            }

            if(selectedPoint == _point && selectedPoint != null)
            {
                selectedPoint = null;
                return;
            }
            else if(_point != null)
            {
                selectedPoint = _point;
                selectedPoint.Hitbox.color = Color.Blue;

            }
            else
            {
                selectedPoint = null;
            }

        }

        public override Point[] hitBoxPoints(Point one, Point two, int _lanes, int _laneWidth = 20)
        {
            Point[] _points = new Point[4];

            Rectangle _rec = new Rectangle(one, new Size(1, 1));
            int _inflate = _lanes * 20 / 2;
            _rec.Inflate(_inflate, _inflate);

            _points[0] = _rec.Location;
            _points[1] = new Point(_rec.Right, _rec.Top);
            _points[2] = new Point(_rec.Left, _rec.Bottom);
            _points[3] = new Point(_rec.Right, _rec.Bottom);

            return _points;
        }

        private void createConnectionPoints()
        {
            int Width = (int)((500 - ((this.lanes + 2) * 20) * this.Scale));

            createConnectionPointSide(new Point((int)(Width + 20 * this.Scale), (int)(Width)), 1, 0, "Top");
            createConnectionPointSide(new Point((int)(Width + 20 * this.Scale), (int)(Width+ ((lanes + 1) * 20 * this.Scale))), 1, 0, "Bottom");

            createConnectionPointSide(new Point((int)(Width), (int)(Width + 20 * this.Scale)), 0, 1, "Left");
            createConnectionPointSide(new Point((int)(Width + ((lanes + 1) * 20 * this.Scale)), (int)(Width + 20 * this.Scale)), 0, 1, "Right");
        }

        private void createConnectionPointSide(Point _loc, int _X, int _Y, string _side)
        {
            for(int x = 0; x < this.lanes; x++)
            {
                connectPoints.Add(new ConnectionPoint(new Point(_loc.X + (int)(20 * this.Scale) * x * _X, _loc.Y + (int)(20 * this.Scale) * x * _Y), _side, this.Scale));
            }
        }
    }
}
