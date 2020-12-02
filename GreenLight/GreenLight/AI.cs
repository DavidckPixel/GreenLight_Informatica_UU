using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace GreenLight
{
    class AI
    {
        Vehicle v;
        double reactionSpeed;
        double followInterval;
        double speedRelativeToLimit;
        double ruleBreakingChance;
        int speedlimit = 10; //tijdelijk
        Thread run, stop;
        bool isAccelerating;
        
        public AI(Vehicle v, double reactionSpeed, double followInterval, double speedRelativeToLimit, double ruleBreakingChance)
        {
            this.v = v;
            

            this.reactionSpeed = reactionSpeed;
            this.followInterval = followInterval;
            this.speedRelativeToLimit = speedRelativeToLimit;
            this.ruleBreakingChance = ruleBreakingChance;
            isAccelerating = false;
            Thread run = new Thread(test);
            run.Start();  

        }

        public void test()
        {
            
            while (true)
            {
                if (v.speed < speedlimit && !isAccelerating)
                {
                    v.tryAccelerate(speedlimit);
                    isAccelerating = true;
                }
                else if (v.speed >= speedlimit && isAccelerating)
                {
                    v.tryBrake(0);

                }
                else if (v.speed == 0 && isAccelerating)
                {
                    Thread.Sleep(2000);
                    isAccelerating = false;
                    
                    
                    
                }
            }
            
            
        }

     
      
    }
}