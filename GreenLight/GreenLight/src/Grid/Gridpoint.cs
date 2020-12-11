using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    class Gridpoint : ScreenObject
    {
        //An actual gridpoint as drawn on the screen
        //It inheriteges form the ScreenObject, this is a master class for all things that eventually need to be drawn
        //on the field.


        Size Size;

        public Gridpoint(Point _Cords, Size _Size) : base(_Cords, _Size)
        {
            this.Size = _Size;
        }

        public bool Collision(Point _p)
        {
            return this.Hitbox.Contains(_p);
        }

        public void DrawGrid(Graphics g)
        {
            g.FillEllipse(Brushes.Black, this.Hitbox);
        }

        public override string ToString()
        {
            return this.Cords.ToString();
        }

    }
}
