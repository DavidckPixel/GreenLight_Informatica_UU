using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    class DriverStats
    {
        public string Name;
        public float ReactionTime;
        public float FollowInterval;
        public int SpeedRelativeToLimit;
        public float RuleBreakingChance;

        public DriverStats(string Name, float ReactionTime, float FollowInterval, int SpeedRelativeToLimit, float RuleBreakingChance)
        {
            this.Name = Name;
            this.ReactionTime = ReactionTime;
            this.FollowInterval = FollowInterval;
            this.SpeedRelativeToLimit = SpeedRelativeToLimit;
            this.RuleBreakingChance = RuleBreakingChance;

            Console.WriteLine("Constructed DriverStat");

        }

    }
}
