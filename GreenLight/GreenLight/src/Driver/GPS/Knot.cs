using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{

    //A knot is a place at the end/ beginnin of a road often connecting 2 roads

    public class Knot
    {
        public AbstractRoad Road1;
        public AbstractRoad Road2;
        public Point Cord;

        public Knot(AbstractRoad _road1 = null, AbstractRoad _road2 = null, Point _cord = new Point())
        {
            Log.Write("Knot created at cord: "+ _cord);
            Road1 = _road1;
            Road2 = _road2;
            Cord = _cord;
        }

        public void Draw(Graphics g)
        {
            Console.WriteLine("Drawing a knot!");
            g.FillEllipse(Brushes.Blue, new Rectangle(this.Cord, new Size(5, 5)));
        }

        //Since the existance of a road in road1 or road2 is not consistent/ important, for example:
        // road1 = "Diagonal" road2 = "Straight" is equal to road1 = "Straight" road2 = "Diagonal".
        //so additional logic is needed to see if 2 knots are equal.

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
        }
    }
}
