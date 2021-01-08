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
        public abstract bool Contains(Point _p);
        public abstract bool Contains(RectHitbox _h);

        public abstract bool Collide(RectHitbox _h);

        public abstract void Draw(Graphics g, Color _color);

    }
}
