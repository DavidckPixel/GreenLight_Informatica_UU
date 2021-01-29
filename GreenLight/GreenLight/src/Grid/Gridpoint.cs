using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class Gridpoint
    {
        /* This is the Gridpoint class. 
           A gridpoint has a location, size, hitbox and bool to check if the gridpoint is already build on
           The method Collision is used to check if a certain point is on the gridpoint.
                                                                                                           */

        Size Size;
        public Point Cords;
        Rectangle visualHitbox;
        Rectangle Hitbox;
        public bool Used;

        public Gridpoint(Point _cords, int _size, int _hitsize)
        {
            this.Cords = new Point(_cords.X + 10, _cords.Y + 5);
            visualHitbox = new Rectangle(this.Cords, new Size(_size, _size));
            Hitbox = new Rectangle(new Point(this.Cords.X+(int)(_size*0.5)-(int)(0.5*_hitsize), this.Cords.Y+(int)(_size*0.5)-(int)(0.5*_hitsize)), new Size(_hitsize, _hitsize));
        }

        public bool Collision(Point _p)
        {
            return this.Hitbox.Contains(_p);
        }

        public void DrawGrid(Graphics g)
        {
            g.FillEllipse(Brushes.Black, this.visualHitbox);
        }

        public override string ToString()
        {
            return this.Cords.ToString();
        }

    }
}
