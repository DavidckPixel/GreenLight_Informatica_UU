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
        Thread moveVehicle;
        public int targetspeed;
        Point destinationpoint = new Point(4000, 980); //Ingegeven door road?

        
        public AI(Vehicle v, int reactionSpeed, float followInterval, int speedRelativeToLimit, float ruleBreakingChance)
        {
            this.v = v;
            

            this.reactionSpeed = reactionSpeed;
            this.followInterval = followInterval;
            this.speedRelativeToLimit = speedRelativeToLimit;
            this.ruleBreakingChance = ruleBreakingChance;
            targetspeed = speedlimit + speedRelativeToLimit;
            
            //thread used to update vehicle speed and whereabouts in the single threaded car system
            moveVehicle = new Thread(vehiclemovement);
            moveVehicle.Start();
            
            //thread used to test the AI in the multi threaded car system
            /*run = new Thread(test);*/
            /*run.Start();*/


        }

        //method used to drive the vehicle in the single threaded car System;
        public void vehiclemovement()
        {
            v.calculateAngle(destinationpoint.X, destinationpoint.Y);

            while (true)
            {
                v.move(destinationpoint.X, destinationpoint.Y);

                if (v.isBraking)
                {
                    v.brakeToSpeed(0);
                }

                if (v.isAccelerating)
                {
                    v.accelerate(targetspeed);
                }
                
                needToBrake(destinationpoint.X, destinationpoint.Y);

                Console.WriteLine("braking: " + v.isBraking + "    -    accelarating:" + v.isAccelerating + "    -    speed:" + v.speed) ;
                Thread.Sleep(16);
            }
            
        }

        //Method used to change the destination of the vehicle in the single threaded car system
        public void changeDestination(int xt, int yt)
        {
            
            destinationpoint = new Point(xt, yt);
            v.calculateAngle(xt, yt);
            v.isBraking = false;
            v.isAccelerating = true;
            
        }

        //Method used to calculate if the car needs to start braking in the single threaded car system
        public void needToBrake(int xt, int yt)
        {
            float distancefromend = (float)Math.Sqrt((v.x - xt) * (v.x - xt) + (v.y - yt) * (v.y - yt)) / 5;
            float brakedistance = v.brkdistance(xt, yt);
            Console.WriteLine(brakedistance + "    -    " + distancefromend);
            if (brakedistance >= distancefromend)
            {
                v.isBraking = true;
                v.isAccelerating = false;
            }
        }


        //method used to test the AI in the multi threaded car sytem
        /*public void test()
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
        }*/
    }
}