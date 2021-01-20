using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class CrossRoadSide
    {
        public bool status;
        public RectHitbox hitbox;
        public int priorityLevel;

        public List<BetterAI> aiOnSide= new List<BetterAI>();

        public CrossRoadSide(RectHitbox _hitbox)
        {
            this.priorityLevel = 1;
            this.status = false;
            this.hitbox = _hitbox;
        }
    }
}
