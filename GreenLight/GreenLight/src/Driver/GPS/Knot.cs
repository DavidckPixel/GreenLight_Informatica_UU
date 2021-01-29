using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{
    //A class that holds the Point of connection between to roads, and the Point where they are connected. 
    //A knot is constructed with 2 roads and a point.
    //It has an equals method that can be used to determine if knots are the same, but with switched roads.
    public class Knot
    {
        public AbstractRoad Road1;
        public AbstractRoad Road2;
        public Point Cord;

        public Knot(AbstractRoad _road1, AbstractRoad _road2, Point _cord)
        {
            Road1 = _road1;
            Road2 = _road2;
            Cord = _cord;
        }

        public void Draw(Graphics g)
        {
            //Console.WriteLine("Drawing a knot!");
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
        }
    }
}
