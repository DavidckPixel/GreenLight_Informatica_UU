using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;


namespace GreenLight
{
    class AI
    {
        //This is the AI class, every AI has a vehicle that it takes care of
        //This class holds variables for driver behaviour and will let the vehicle move accordingly.
        //For now this class can only make vehicles accelarate or brake to a certain speed,
        //but in the future the AI will be able to choose where and how fast to drive, depending on the cars around them.

        public Vehicle v;
        int reactionSpeed;
        float followInterval;
        float speedRelativeToLimit;
        float ruleBreakingChance;
        int speedlimit = 28; //tijdelijk
        Thread moveVehicle;
        public int targetspeed;
        Point destinationpoint = new Point(1900, 1000); //Ingegeven door road?
        int framesbuffered = 625;
        public List<Point> location = new List<Point>();
        public List<Point> location2 = new List<Point>();
        List<Point> locationlocal = new List<Point>();

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
            while (true)
            {
                if (v.listcounter % 2 == 0)
                {
                    location.Clear();
                    calculateList();
                    foreach (Point p in locationlocal)
                    {
                        location.Add(p);
                    }
                    Thread.Sleep(9000);
                    locationlocal.Clear();
                    location2.Clear();
                    framesbuffered = 625;
                    calculateList();
                    foreach (Point p in locationlocal)
                    {
                        location2.Add(p);
                    }
                    Console.WriteLine("Lijst 1: " + location.Count + "   Lijst 2: " + location2.Count + "  Listnummer: " + v.listcounter);
                    locationlocal.Clear();
                    framesbuffered = 625;
                    Thread.Sleep(9000);
                }
                Thread.Sleep(100);
            }
        }

        void calculateList()
        {
            v.calculateAngle(destinationpoint.X, destinationpoint.Y);
            while (framesbuffered > 0)
            {
                locationlocal.Add(v.move(destinationpoint.X, destinationpoint.Y));
                if (v.isBraking)
                {
                    v.brakeToSpeed(0);
                }
                if (v.isAccelerating)
                {
                    v.accelerate(targetspeed);
                }
                needToBrake(destinationpoint.X, destinationpoint.Y);
                //Console.WriteLine("braking: " + v.isBraking + "    -    accelarating:" + v.isAccelerating + "    -    speed:" + v.speed + " Frames: " + framesbuffered);
                framesbuffered--;
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
            //Console.WriteLine(brakedistance + "    -    " + distancefromend);
            if (brakedistance >= distancefromend)
            {
                v.isBraking = true;
                v.isAccelerating = false;
            }
        }
    }
}