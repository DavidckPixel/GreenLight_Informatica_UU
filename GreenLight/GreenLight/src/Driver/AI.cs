using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;



namespace GreenLight
{
    public class AI
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

        //Hoe the f** gaan we dit implementeren?
        public int prioritylevel = 2; //rijdt niet op yieldweg maar ook niet op priority
        
        Thread moveVehicle;
        public int targetspeed;
        Point destinationpoint = new Point(1900, 1060); //Ingegeven door road?
        Point endpoint; //Definitief eindpunt auto?
		int framesbuffered = 625;
        public List<Point> location = new List<Point>();
        public List<Point> location2 = new List<Point>();
        List<Point> locationlocal = new List<Point>();

        
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
        }

        //method used to drive the vehicle in the single threaded car System;
        public void vehiclemovement()
        {
            while (true)
            {
                if (v.listcounter % 2 == 0)
                {
                    calculateList();
                    location.Clear();
                    foreach (Point p in locationlocal)
                    {
                        location.Add(p);
                    }
                    /*Console.WriteLine("Lijst 1 berekend. Xend: " + location[624].X + "  -  Listnummer: " + v.listcounter);*/
                    locationlocal.Clear();
                    framesbuffered = 625;
                    Thread.Sleep(9000);
                    calculateList();
                    location2.Clear();
                    foreach (Point p in locationlocal)
                    {
                        location2.Add(p);
                    }
                    /*Console.WriteLine("Lijst 2 berekend. Xend: " + location2[624].X + "  -  Listnummer: " + v.listcounter);*/
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
                /*Console.WriteLine("braking: " + v.isBraking + "    -    accelarating:" + v.isAccelerating + "    -    speed:" + v.speed + " Frames: " + framesbuffered);*/
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
            
            if (brakedistance >= distancefromend)
            {
                v.isBraking = true;
                v.isAccelerating = false;
            }
        }
    }
}