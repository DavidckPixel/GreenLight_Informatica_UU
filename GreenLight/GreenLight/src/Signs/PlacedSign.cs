using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public struct PlacedSign
    {
        public RectHitbox Hitbox;
        public Point Location;
        public string Direction;
        public AbstractSign Sign;
        public Image Sign_image;
        public AbstractRoad Road;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign, Image _Sign_image, AbstractRoad _road)
        {
            this.Location = _location;
            this.Direction = _direction;
            this.Sign = _sign;
            this.Hitbox = new RectHitbox(new Point(Location.X - 5, Location.Y - 5), new Point(Location.X + 15, Location.Y - 5), new Point(Location.X - 5, Location.Y + 15), new Point(Location.X + 15, Location.Y + 15), Color.Red);
            this.Sign_image = _Sign_image;
            this.Road = _road;
        }

        public void draw(Graphics g)
        {
            Console.WriteLine(Sign.ToString());
            int _dir = Road.Drivinglanes[0].AngleDir;
            
            if (_dir >= 0 && _dir < 180)
            {
                g.DrawImage(Sign_image, Location.X - 5, Location.Y - 5, 20, 20);
            }
            else if (_dir >= 180 && _dir < 360)
            {
                g.DrawImage(Sign_image, Location.X + 5, Location.Y + 5, 20, 20);
            }
            this.Hitbox.Draw(g);
            //g.FillRectangle(Notsolid, this.Hitbox);
        }
    }
}
