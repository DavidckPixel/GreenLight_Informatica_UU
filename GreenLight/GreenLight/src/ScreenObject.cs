using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GreenLight
{
    abstract class ScreenObject
    {
        public Rectangle Hitbox;
        public Point Cords;

        public ScreenObject(Point _p1, Size _s)
        {
            this.Hitbox = new Rectangle(_p1, _s);
        }

        public virtual void Draw(Graphics g)
        {

        }
    }

    
}
