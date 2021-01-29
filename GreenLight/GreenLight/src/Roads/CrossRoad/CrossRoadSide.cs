using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    // Contains information about one side of a CrossRoad
    public class CrossRoadSide
    {
        public bool status;
        public RectHitbox hitbox;
        public int priorityLevel;
        public string side;
        public bool driving;
        public int aiDriving;

        public List<BetterAI> aiOnSide= new List<BetterAI>();

        public CrossRoadSide(RectHitbox _hitbox, string _side)
        {
            this.priorityLevel = 2;
            this.status = false;
            this.hitbox = _hitbox;
            this.side = _side;
        }

        public void reset()
        {
            this.priorityLevel = 2;
            this.driving = false;
            this.aiDriving = 0;
            this.status = false;

        }

    }
}
