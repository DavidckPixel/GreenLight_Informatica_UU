using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace GreenLight
{
    public abstract class ScreenObject
    {
        //This is the ScreenObject class from which each object that needs to be drawn (Vehicle, Road, Gridpoint) inherits basic features.
        //This class will draw these objects on the bitmap via the BitmapController with the Draw method.
        //It also gives each ScreenObject its cords and a hitbox, which is a rectangle that contains the whole ScreenObject.

       // public Rectangle Hitbox;
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
