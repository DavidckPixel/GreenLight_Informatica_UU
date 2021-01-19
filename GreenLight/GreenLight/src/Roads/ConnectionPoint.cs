using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class ConnectionPoint
    {
        public Point Location;
        public RectHitbox Hitbox;
        public string Side;
        public bool Active;

        public ConnectionPoint(Point _loc, string _side, double _scale)
        {
            this.Location = _loc;
            this.Side = _side;
            this.Active = true;
            double _unscaledSize = Math.Max(Math.Min(Roads.Config.laneWidth / 4, Roads.Config.crossroadExtra), Math.Min(Roads.Config.laneWidth, Roads.Config.crossroadExtra/4));

            int _size = (int)(_unscaledSize * _scale);

            Point[] _points = new Point[4];

            _points[0] = new Point(this.Location.X - _size, this.Location.Y - _size);
            _points[1] = new Point(this.Location.X + _size, this.Location.Y - _size);
            _points[2] = new Point(this.Location.X - _size, this.Location.Y + _size);
            _points[3] = new Point(this.Location.X + _size, this.Location.Y + _size);

            Hitbox = new RectHitbox(_points[0], _points[1], _points[2], _points[3], Color.Green);
        }

        public void setActive(bool _active)
        {
            this.Active = _active;
        }

        public void Draw(Graphics g)
        {
            Hitbox.Draw(g);
        }

    }
}
