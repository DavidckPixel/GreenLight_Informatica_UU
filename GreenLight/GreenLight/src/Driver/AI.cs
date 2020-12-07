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
        int speedlimit = 10; //tijdelijk
        Thread run;
        public bool isAccelerating;
        public int targetspeed;

        
        public AI(Vehicle v, int reactionSpeed, float followInterval, int speedRelativeToLimit, float ruleBreakingChance)
        {
            this.v = v;
            

            this.reactionSpeed = reactionSpeed;
            this.followInterval = followInterval;
            this.speedRelativeToLimit = speedRelativeToLimit;
            this.ruleBreakingChance = ruleBreakingChance;
            targetspeed = speedlimit + speedRelativeToLimit;
            isAccelerating = true;
            run = new Thread(test);
            run.Start();

        }

        public void test()
        {
            while (true)
            {
                if (v.speed < speedlimit && !isAccelerating)
                {
                    Thread.Sleep(reactionSpeed);
                    v.tryAccelerate(speedlimit);
                    isAccelerating = true;                }
                else if (v.speed >= speedlimit && isAccelerating)
                {
                    Thread.Sleep(reactionSpeed);
                    v.tryBrake(0);
                }
                else if (v.speed == 0 && isAccelerating)
                {
                    Thread.Sleep(1000);
                    isAccelerating = false;
                }
                Thread.Sleep(16);
            }
        }
    }
}