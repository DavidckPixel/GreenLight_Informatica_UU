using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public struct PlacedSign
    {
        public RectHitbox Hitbox;
        public Point Location;
        public string Direction;
        public AbstractSign Sign;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign, AbstractRoad _read = null)
        {
            this.Location = _location;
            this.Direction = _direction;
            this.Sign = _sign;

            this.Hitbox = new RectHitbox(new Point(_location.X - 3, _location.Y - 3), new Point(_location.X + 3, _location.Y - 3), new Point(_location.X - 3, _location.Y + 3), new Point(_location.X + 3, _location.Y + 3));
        }

        public void draw(Graphics g)
        {
            //Brush Notsolid = new SolidBrush(Color.FromArgb(100, Color.Red));
            this.Hitbox.Draw(g, Color.Red);
            //g.FillRectangle(Notsolid, this.Hitbox);
        }
    }
}
