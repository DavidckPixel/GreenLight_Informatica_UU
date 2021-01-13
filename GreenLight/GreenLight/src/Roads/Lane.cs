using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public abstract class Lane
    {

        public ConnectionLink link;

        public List<LanePoints> points;
        public Point begin, end;
        public string dir;
        public Hitbox offsetHitbox;

        public abstract void Draw(Graphics g);
        public abstract void DrawoffsetHitbox(Graphics g);
    }
}
