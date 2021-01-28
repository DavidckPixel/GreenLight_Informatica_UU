using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;



namespace GreenLight
{
    //This is the old AI class, created to hold variables for driver behaviour and to make vehicles move.
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project. 

    public class AI
    {
        public Vehicle v;
        float reactionSpeed;
        float followInterval;
        int speedRelativeToLimit;
        float ruleBreakingChance;
        int speedlimit = 28; 

        public int prioritylevel = 2;
        
        Thread moveVehicle;
        public int targetspeed;
        Point destinationpoint = new Point(1900, 10); 
        Point endpoint; 
		int framesbuffered = 625;
        public List<Point> location = new List<Point>();
        public List<Point> location2 = new List<Point>();
        List<Point> locationlocal = new List<Point>();
        public int maxSpeed;

        
        public AI(Vehicle v, DriverStats _stats)
        {
            this.v = v;


            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;
            targetspeed = speedlimit + speedRelativeToLimit;
            
            moveVehicle = new Thread(vehiclemovement);
            moveVehicle.Start();
        }

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
                    locationlocal.Clear();
                    framesbuffered = 625;
                    Thread.Sleep(9000);
                    calculateList();
                    location2.Clear();
                    foreach (Point p in locationlocal)
                    {
                        location2.Add(p);
                    }
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
                framesbuffered--;
            }
        }

        public void changeDestination(int xt, int yt)
        {            
            destinationpoint = new Point(xt, yt);
            v.calculateAngle(xt, yt);
            v.isBraking = false;
            v.isAccelerating = true;            
        }
        
        public void needToBrake(int xt, int yt)
        {
            float distancefromend = (float) (Math.Sqrt((v.x - xt) * (v.x - xt) + (v.y - yt) * (v.y - yt))/5);
            float brakedistance = v.brkdistance();
            if (brakedistance >= distancefromend)
            {
                v.isBraking = true;
                v.isAccelerating = false;
            }
        }

        public void Draw(Graphics g)
        {
            v.drawVehicle(g, locationlocal);
        }
    }
}