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

        public Rectangle Hitbox;
        public Point Cords;

        public ScreenObject(Point _p1, Point _p2)
        {
            int left, top, right, bottom;

            left = Math.Min(_p1.X, _p2.X);
            right = Math.Max(_p1.X, _p2.X);

            top = Math.Min(_p1.Y, _p2.Y);
            bottom = Math.Max(_p1.Y, _p2.Y);

            Hitbox = Rectangle.FromLTRB(left, top, right, bottom);
        }

        public virtual void Draw(Graphics g)
        {

        }
    }
}