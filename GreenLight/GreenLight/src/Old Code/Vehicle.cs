using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;


namespace GreenLight
{
    //This is the old Vehicle class, which used to create a car.
    //This class also contains methods that calculate how the car moves, accelarates and brakes.
    //All calculations are based on real-life physics

    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project and cannot be used for tests anymore. 

    public class Vehicle : ScreenObject     
    {
        public float x, y; 
        public float speed = 0; 
        public float a, abrake;

        string name; 

        int weight;
        float length;
        int motorpwr;
        int topspeed;
        float cw;
        float surface;
        
        float airResistance;
        float rollingResistance;
        
        float crw = 0.012f;
        World physics = WorldConfig.physics[0]; 

        public bool isAccelerating = true;
        public bool isBraking = false;
        
        Bitmap Car;
        int angle;
        public int frame = 0;
        public int listcounter = 0;


        public Vehicle(VehicleStats _stat, int x, int y) : base(new Point(x,y))
        {
            this.weight = _stat.Weight;
            this.length = _stat.Length;
            this.topspeed = _stat.Topspeed;
            this.name = _stat.Name;
            this.motorpwr = _stat.Motorpwr;
            this.x = x;
            this.y = y;
            this.cw = _stat.Cw;
            this.surface = _stat.Surface;      
            
            a = this.motorpwr / this.weight;
            abrake = physics.Brakepwr / this.weight;
            Car = new Bitmap(Properties.Resources.Car);
        }

        public float Slipperiness {
            get{ return crw; }
            set { crw = value; }
        }
        
        public void drawVehicle(Graphics g, List<Point> location)
        {
            int xtemp = 0;
            int ytemp = 0;
            try
            {
                xtemp = location[frame].X;
                ytemp = location[frame].Y;
            }
            catch
            { Console.WriteLine("Not Av." + location.Count);;
            }
            g.DrawImage(RotateImage(Car, angle), xtemp-Car.Width/2, ytemp - Car.Height / 2, Car.Width, Car.Height);
            if (frame == 300)
            {
                listcounter++;
            }
            frame++;
        }

        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }

        void brakeInTime(float braketime)
        {
            while (braketime > 0 && speed > 0)
            {
                airResistance = (float) (0.5f * physics.Density * cw * surface * speed * speed);
                abrake = (physics.Brakepwr + airResistance ) / this.weight;
                speed -= abrake / 100;
                braketime -= 0.01f;
            }
            if (speed < 0)
            {
                speed = 0;
            }
        }

        public void brakeToSpeed (float targetspeed)
        {
            airResistance = (float)(0.5f * physics.Density * cw * surface * speed * speed);
            abrake = (physics.Brakepwr + airResistance) / this.weight;
            speed -= (abrake * (0.016f));
            if (speed <= targetspeed)
            {
                speed = targetspeed;
                isBraking = false;
            }
        }
        
        public float brkdistance()
        {
            airResistance = (float)(0.5f * physics.Density * cw * surface * speed * speed);
            abrake = (physics.Brakepwr + airResistance) / this.weight;

            float brkdistance = weight * speed * speed / (physics.Brakepwr*2);
            return brkdistance;
        }

        public void calculateAngle(int xt, int yt)
        {
            float xDiff = xt - x;
            float yDiff = yt - y;
            angle = (int) (Math.Atan2(yDiff, xDiff) * (180 / Math.PI));

        }


        public Point move(int xt, int yt)
        {
            if (Math.Abs(x - xt) > 1 || Math.Abs(y - yt) > 1)
            {
                float xmove = Math.Abs(xt - x) / (Math.Abs(xt - x) + Math.Abs(yt - y));
                float ymove = 1.0f - xmove;
                if (x < xt)
                {
                    x = x + xmove * speed * 0.08f;    
                }
                if (x > xt)
                {
                    x = x - xmove * speed * 0.08f;
                }
                if (y < yt)
                {
                    y = y + ymove * speed * 0.08f;
                }
                if (y > yt)
                {
                    y = y - ymove * speed * 0.08f;
                }            
            }
            else
            {
                speed = 0;
                isAccelerating = false;
                isBraking = false;
            }
            return new Point((int)x, (int)y);
        }

        public void accelerate(float targetspeed)
        {
            airResistance = (float)(0.5f * physics.Density * cw * surface * speed * speed);
            rollingResistance = (float)(crw * this.weight * physics.Gravity);
            a = (this.motorpwr - (airResistance + rollingResistance)) / this.weight;
            if (airResistance + rollingResistance < this.motorpwr)
            {
                speed += a * 0.016f;
            }
            
            if (speed >= targetspeed)
            {
                speed = targetspeed;
                isAccelerating = false;
            }
        }
    }
}