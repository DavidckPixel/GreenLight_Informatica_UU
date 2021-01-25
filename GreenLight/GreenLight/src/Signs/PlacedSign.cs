using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

//this is the object that is actually placed on the Road that has a hitbox and says which specific sign is clicked

namespace GreenLight
{
    public struct PlacedSign
    {
        public RectHitbox Hitbox;
        public Point Location, Hitboxoffset;
        public string Direction;
        public AbstractSign Sign;
        public Image Sign_image;
        public AbstractRoad Road;
        public string signType;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign, Image _Sign_image, AbstractRoad _road, string _signType, Point _hitboxoffset)
        {
            this.Location = _location;
            this.Direction = _direction;
            this.Sign = _sign;
            this.Sign_image = _Sign_image;
            this.Road = _road;
            this.signType = _signType;
            int _dir = (int)Road.Drivinglanes[0].AngleDir;
            this.Hitboxoffset = _hitboxoffset;
            this.Hitbox = new RectHitbox(new Point(Location.X - 15, Location.Y - 15), new Point(Location.X + 15, Location.Y - 15), new Point(Location.X - 15, Location.Y + 15), new Point(Location.X + 15, Location.Y + 15), Color.Red);
        }

        public void draw(Graphics g)
        {
            int _dir = (int)Road.Drivinglanes[0].AngleDir;
            int offset = Math.Abs((_dir - 180) - 45) / 45 * 2;
            int X1 = Location.X - ((_dir - 90) / 9) * 2;
            int X2 = Location.X - (20 - (_dir - 180) / 9 * 2 * Math.Abs((_dir - 225) / 45 * 2));
            int Y1 = Location.Y + 3;
            int Y2 = Location.Y - (_dir - 180) / 9 * 2;
            int Y3 = Location.Y - (20 - (_dir - 270) / 9 * 2);

            if (_dir >= 0 && _dir <= 20)
            {
                this.Hitbox = new RectHitbox(new Point(Location.X - 15, Location.Y + 5), new Point(Location.X + 15, Location.Y + 5), new Point(Location.X - 15, Location.Y + 35), new Point(Location.X + 5, Location.Y + 35), Color.Red);
            }
            if (_dir > 20 && _dir <= 45 || _dir >= 270)
            {
                this.Hitbox = new RectHitbox(new Point(Hitboxoffset.X - 15, Hitboxoffset.Y - 15), new Point(Hitboxoffset.X + 15, Hitboxoffset.Y - 15), new Point(Hitboxoffset.X - 15, Hitboxoffset.Y + 15), new Point(Hitboxoffset.X + 15, Hitboxoffset.Y + 15), Color.Red);
            }
            if (_dir >= 0 && _dir <= 90)
            {
                g.DrawImage(Sign_image, Location.X, Location.Y, 20, 20);
            }
            else if (_dir > 90 && _dir < 180)
            {
                g.DrawImage(Sign_image, X1, Y1, 20, 20);
            }
            else if (_dir >= 180 && _dir < 270)
            {
                g.DrawImage(Sign_image, X2, Y2, 20, 20);
            }
            else
            {
                g.DrawImage(Sign_image, Location.X, Y3, 20, 20);
            }
            this.Hitbox.Draw(g);
            //g.FillRectangle(Notsolid, this.Hitbox);
        }

        public override string ToString()
        {
            string signString = "Sign" + " " + this.Location.X.ToString() + " " + this.Location.Y.ToString() + " " + this.signType + " " +  Hitboxoffset.X.ToString() + " " + Hitboxoffset.Y.ToString();
            if (this.signType == "speedSign")
            {
                signString += " " + Sign.speed.ToString(); 
            }
            return signString;
        }
    }
}