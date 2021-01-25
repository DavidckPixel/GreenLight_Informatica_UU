using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

//Base class for driving lane

namespace GreenLight
{
    public abstract class Lane
    {
        public ConnectionLink link;

        public List<LanePoints> points;
        public string dir;
        public Hitbox offsetHitbox;
        public List<Lane> beginConnectedTo = new List<Lane>();
        public List<Lane> endConnectedTo = new List<Lane>();
        public float AngleDir;

        public abstract void Draw(Graphics g);
        public abstract void DrawoffsetHitbox(Graphics g);

        public abstract void FlipPoints();

        public int thisLane = 0;
        public bool flipped = false;

        public void DrawLine(Graphics g)
        {
            Point _old = points.First().cord;
                foreach (LanePoints _point in points)
                {
                    g.DrawLine(Pens.DarkGoldenrod, _point.cord, _old);
                    _old = _point.cord;
                }
            
        }
    }
}
