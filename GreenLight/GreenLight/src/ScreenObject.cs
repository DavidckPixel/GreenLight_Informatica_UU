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
        //This is the ScreenObject class from which each object that needs to be drawn (Vehicle, Road, Gridpoint) inherits basic features.
        //This class will draw these objects on the bitmap via the BitmapController with the Draw method.
        //It also gives each ScreenObject its cords and a hitbox, which is a rectangle that contains the whole ScreenObject.

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
