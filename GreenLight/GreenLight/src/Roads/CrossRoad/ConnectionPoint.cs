using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    // Each CrossRoads has an amount of ConnectionPoints, equal to 4 times the amount of lanes (one time on each side).
    // When ConnectionPoints are active, they are connected to at least one other ConnectionPoint

    public class ConnectionPoint
    {
        public Point Location;
        public Point transLocation;
        public RectHitbox Hitbox;
        public string Side;
        public bool Active;
        public bool end;
        public Bitmap arrowImg = new Bitmap(1, 1);
        public int Place;

        public ConnectionPoint(Point _loc, string _side, double _scale, int _place)
        {
            this.Location = _loc;
            this.Side = _side;
            this.Active = true;
            double _unscaledSize = Math.Max(Math.Min(Roads.Config.laneWidth / 4, Roads.Config.crossroadExtra), Math.Min(Roads.Config.laneWidth, Roads.Config.crossroadExtra/4));

            int _size = (int)(_unscaledSize * _scale);
            this.Place = _place;

            if (_scale != 0)
            {
                Point[] _points = new Point[4];

                _points[0] = new Point(this.Location.X - _size, this.Location.Y - _size);
                _points[1] = new Point(this.Location.X + _size, this.Location.Y - _size);
                _points[2] = new Point(this.Location.X - _size, this.Location.Y + _size);
                _points[3] = new Point(this.Location.X + _size, this.Location.Y + _size);

                Hitbox = new RectHitbox(_points[0], _points[1], _points[2], _points[3], Color.Green);
            }
            else
                Hitbox = null;
        }

        public void setActive(bool _active)
        {
            this.Active = _active;
        }

        public virtual void Draw(Graphics g)
        {
            if (Hitbox != null) 
                 Hitbox.Draw(g);
        }

    }
}
