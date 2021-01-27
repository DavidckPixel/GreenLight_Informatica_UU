using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//This is the main hitbox class from which the other hitboxes inherit, it holds all the basic function and has some extra features
//used to find the topcord and the square size of the hitbox.
//It also has 2 static functions which simply is the Math.max function but then upgraded to take 4 integers instead of the normal 2

namespace GreenLight
{
    abstract public class Hitbox
    {
        public Point Topcord;
        public string Type;
        public Size Size;
        public bool Visible = true;

        public int SizeX, SizeY;

        public Color color;
        public int _lanes = 0;

        public List<LanePoints> lanepoints = new List<LanePoints>();

        public Hitbox(Point _one, Point _two, Point _three, Point _four)
        {
            Topcord = new Point(BetterMin(_one.X, _two.X, _three.X, _four.X), BetterMin(_one.Y, _two.Y, _three.Y, _four.Y));

            Size = new Size(BetterMax(_one.X, _two.X, _three.X, _four.X) - Topcord.X, BetterMax(_one.Y, _two.Y, _three.Y, _four.Y) - Topcord.Y);
        }

        public abstract bool Contains(Point _p);
        //public abstract bool Contains(RectHitbox _h);

        public abstract bool Collide(Hitbox _h, Graphics g);

        public abstract void Draw(Graphics g);

        public abstract void ShowOverlap(Graphics g);

        public static int BetterMin(int _one, int _two, int _three, int _four)
        {
            int _firstround = Math.Min(_one, _two);
            int _secondround = Math.Min(_three, _four);

            return Math.Min(_firstround, _secondround);
        }

        public static int BetterMax(int _one, int _two, int _three, int _four)
        {
            int _firstround = Math.Max(_one, _two);
            int _secondround = Math.Max(_three, _four);

            return Math.Max(_firstround, _secondround);
        }



    }
}
