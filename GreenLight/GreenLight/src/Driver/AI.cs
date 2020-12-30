using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace GreenLight
{
    class AI
    {
        //This is the AI class, every AI has a vehicle that it takes care of
        //This class holds variables for driver behaviour and will let the vehicle move accordingly.
        //For now this class can only make vehicles accelarate or brake to a certain speed,
        //but in the future the AI will be able to choose where and how fast to drive, depending on the cars around them.

        public Vehicle v;
        float reactionSpeed;
        float followInterval;
        int speedRelativeToLimit;
        float ruleBreakingChance;
        int speedlimit = 28; //tijdelijk
        Thread run;
        Thread moveVehicle;
        public int targetspeed;
        Point destinationpoint = new Point(4000, 980); //Ingegeven door road?

        
        public AI(Vehicle v, DriverStats _stats)
        {
            this.v = v;


            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;
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