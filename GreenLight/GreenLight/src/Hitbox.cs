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

        public abstract bool Collide(Hitbox _h);

        
    }
}
