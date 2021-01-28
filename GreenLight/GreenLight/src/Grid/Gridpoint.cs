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
        //An actual gridpoint as drawn on the screen
        //It inheriteges form the ScreenObject, this is a master class for all things that eventually need to be drawn
        //on the field.


        Size Size;
        public Point Cords;
        Rectangle Visual_hitbox;
        Rectangle Hitbox;
        public bool Used;

        public Gridpoint(Point _Cords, int _Size, int HitSize)
        {
            this.Cords = _Cords;
            Visual_hitbox = new Rectangle(this.Cords, new Size(_Size, _Size));
            Hitbox = new Rectangle(new Point(this.Cords.X+(int)(_Size*0.5)-(int)(0.5*HitSize), this.Cords.Y+(int)(_Size*0.5)-(int)(0.5*HitSize)), new Size(HitSize, HitSize));
        }

        public bool Collision(Point _p)
        {
            return this.Hitbox.Contains(_p);
        }

        public void DrawGrid(Graphics g)
        {
            g.FillEllipse(Brushes.Black, this.Visual_hitbox);
        }

        public override string ToString()
        {
            return this.Cords.ToString();
        }

    }
}
