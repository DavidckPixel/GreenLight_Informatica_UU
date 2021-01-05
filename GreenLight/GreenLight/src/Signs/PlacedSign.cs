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
        public Rectangle Hitbox;
        public Point Location;
        public string Direction;
        public AbstractSign Sign;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign)
        {
            this.Location = _location;
            this.Direction = _direction;
            this.Sign = _sign;

            this.Hitbox = new Rectangle(Location, new Size(1, 1));
            this.Hitbox.Inflate(6, 6);
        }

        public void draw(Graphics g)
        {
            Brush Notsolid = new SolidBrush(Color.FromArgb(100, Color.Red));
            g.FillRectangle(Notsolid, this.Hitbox);
        }
    }
}
