using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    class Gridpoint
    {
        Point Cords;
        Rectangle Hitbox;
        Size Size;

        public Gridpoint(Point _Cords, Size _Size)
        {
            this.Cords = _Cords;
            this.Size = _Size;

            this.Hitbox = new Rectangle(this.Cords, this.Size);
        }

        public bool Collision(Point _p)
        {
            return this.Hitbox.Contains(_p);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Black, this.Hitbox);
        }

        public override string ToString()
        {
            return this.Cords.ToString();
        }

    }
}
