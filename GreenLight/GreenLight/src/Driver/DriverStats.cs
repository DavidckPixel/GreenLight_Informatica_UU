using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class DriverStats
    {
        public string Name;
        public float ReactionTime;
        public float FollowInterval;
        public int SpeedRelativeToLimit;
        public float RuleBreakingChance;
        public int Occurance;
        public bool Locked;

        public DriverStats(string Name, float ReactionTime, float FollowInterval, int SpeedRelativeToLimit, float RuleBreakingChance, int Occurance, bool Locked)
        {
            this.Name = Name;
            this.ReactionTime = ReactionTime;
            this.FollowInterval = FollowInterval;
            this.SpeedRelativeToLimit = SpeedRelativeToLimit;
            this.RuleBreakingChance = RuleBreakingChance;
            this.Occurance = Occurance;
            this.Locked = Locked;
        }
    }
}