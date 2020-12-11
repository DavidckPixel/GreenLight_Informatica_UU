using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace GreenLight
{
    class AI
    {
        public Vehicle v;
        int reactionSpeed;
        float followInterval;
        float speedRelativeToLimit;
        float ruleBreakingChance;
        int speedlimit = 28; //tijdelijk
        Thread run;
        public int targetspeed;

        
        public AI(Vehicle v, int reactionSpeed, float followInterval, int speedRelativeToLimit, float ruleBreakingChance)
        {
            this.v = v;
            

            this.reactionSpeed = reactionSpeed;
            this.followInterval = followInterval;
            this.speedRelativeToLimit = speedRelativeToLimit;
            this.ruleBreakingChance = ruleBreakingChance;
            targetspeed = speedlimit + speedRelativeToLimit;
            run = new Thread(test);
            run.Start();

        }

        public void test()
        {
            while (true)
            {
                if (v.speed < targetspeed && !v.isAccelerating && !v.isBraking)
                {
                    Thread.Sleep(reactionSpeed);
                    v.tryAccelerate(targetspeed);
                }               
                Thread.Sleep(16);
            }
        }
    }
}