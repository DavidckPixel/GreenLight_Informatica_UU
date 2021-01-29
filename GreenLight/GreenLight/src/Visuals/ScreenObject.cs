using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GreenLight
{
    //This is the ScreenObject class from which each object that needs to be drawn (Vehicle, Road, Gridpoint) inherits basic features.
    //It gives each ScreenObject its cords and a Hitbox.

    public abstract class ScreenObject
    {
        public Point topLeft;
        public Hitbox hitbox;

        public ScreenObject(Point _topleft)
        {
            topLeft = _topleft;
        }

        public virtual void Draw(Graphics g)
        {
            hitbox.Draw(g);
        }
    }


}
