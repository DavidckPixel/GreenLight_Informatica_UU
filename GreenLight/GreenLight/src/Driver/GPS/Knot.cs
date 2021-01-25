using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{
    class Knot
    {
        AbstractRoad Road1;
        AbstractRoad Road2;
        Point Cord;

        public Knot(AbstractRoad _road1 = null, AbstractRoad _road2 = null, Point _cord = new Point())
        {
            Console.WriteLine("Knot Created!");
            Road1 = _road1;
            Road2 = _road2;
            Cord = _cord;
        }

        public void Draw(Graphics g)
        {
            Console.WriteLine("Drawing a knot!");
            g.FillEllipse(Brushes.Blue, new Rectangle(this.Cord, new Size(5, 5)));
        }

        public override bool Equals(object obj)
        {
            try
            {
                Knot _knot = (Knot)obj;
                if ((_knot.Road1 == this.Road1 || _knot.Road1 == this.Road2) &&
                    (_knot.Road2 == this.Road1 || _knot.Road2 == this.Road2))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            return base.Equals(obj);
        }
    }
}
