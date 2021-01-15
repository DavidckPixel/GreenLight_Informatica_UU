using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    abstract public class Hitbox
    {
        public Point Topcord;
        public Size Size;

        public int SizeX, SizeY;

        public Color color;

        public Hitbox(Point _one, Point _two, Point _three, Point _four)
        {
            Topcord = new Point(BetterMin(_one.X, _two.X, _three.X, _four.X), BetterMin(_one.Y, _two.Y, _three.Y, _four.Y));

            Size = new Size(BetterMax(_one.X, _two.X, _three.X, _four.X) - Topcord.X, BetterMax(_one.Y, _two.Y, _three.Y, _four.Y) - Topcord.Y);
        }

        public abstract bool Contains(Point _p);
        //public abstract bool Contains(RectHitbox _h);

        public abstract bool Collide(RectHitbox _h);

        public abstract void Draw(Graphics g);

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
