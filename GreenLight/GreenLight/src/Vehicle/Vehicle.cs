using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace GreenLight
{
    class Vehicle : ScreenObject     
    {
        float x, y; //Location of the vehicle
        public float speed = 0;    //Speed of vehicle
        public float a, abrake; //acceleration and braking speed

        //Properties of the vehicle
        string name; 
        int weight;
        float length;
        int motorpwr;
        int topspeed; //Topspeed of vehicle
        float cw; //Drag coefficient
        float surface; //Surface area of the front of the vehicle
        
        
        //Resistances
        float airResistance;
        float rollingResistance;
        
        //fixed values of the world
        float crw = 0.012f; // Rolling Resistance coëfficient --> Temporary value, should be able to set this in weather settings
        World physics = WorldConfig.physics[0]; // 

        public bool isAccelerating = false;
        public bool isBraking = false;
        Thread acc, brk;
        Thread startmove;

        Bitmap Car; //Image of the vehicle
        int angle; //Angle at which the image/vehicle is rotated


        public Vehicle(string name, int weight, float length, int topspeed, int motorpwr, int x, int y, float cw, float surface) : base(new Point(x,y), new Size(40,40))
        {
            this.weight = weight;
            this.length = length;
            this.topspeed = topspeed;
            this.name = name;
            this.motorpwr = motorpwr;
            this.x = x;
            this.y = y;
            this.cw = cw;
            this.surface = surface;
            this.Cords = new Point(x, y); //Ignore this
            
            
            a = this.motorpwr / this.weight;
            abrake = physics.Brakepwr / this.weight;
            Car = new Bitmap(Properties.Resources.Car);
            startmove = new Thread(() => move(100800, 980));
            startmove.Start();
        }

        public void klik(int xt, int yt)
        {
            isBraking = false;
            isAccelerating = false;
            startmove.Abort();
            startmove = new Thread(() => move(xt, yt));
            startmove.Start();
        }

        public float Slipperiness {
            get{ return crw; }
            set { crw = value; }
        }
        
        //Tekenmethode
        public void tekenAuto(Graphics g)
        {
            int xtemp = (int) x;
            int ytemp = (int) y;
            g.DrawImage(RotateImage(Car, angle), xtemp-Car.Width/2, ytemp - Car.Height / 2, Car.Width, Car.Height);
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
                Thread.Sleep(16);
            }
            if (speed < 0)
            {
                speed = 0;
            }
        }

        public void brakeToSpeed(float targetspeed)
        {
            while (speed > targetspeed && isBraking)
            {
                airResistance = (float) (0.5f * physics.Density * cw * surface * speed * speed);
                abrake = (physics.Brakepwr + airResistance) / this.weight;
                speed -= abrake / 100;
                Thread.Sleep(16);
            }
            if (speed < targetspeed)
            {
                speed = targetspeed;
                

            }
            isBraking = false;
        }

        public void brkdistance(int xt, int yt)
        {
            airResistance = (float)(0.5f * physics.Density * cw * surface * speed * speed);
            abrake = (physics.Brakepwr + airResistance) / this.weight;
            float brktime = speed / abrake;
            float brkdistance = brktime * speed;
            float distancefromend = (float) Math.Sqrt((x - xt) * (x - xt) + (y - yt) * (y - yt));
            if (brkdistance > distancefromend && !isBraking)
            {
                tryBrake(0);

            }
        }

        void calculateAngle(int xt, int yt)
        {
            float xDiff = xt - x;
            float yDiff = yt - y;
            angle = (int) (Math.Atan2(yDiff, xDiff) * (180 / Math.PI));

        }


/*        private void Vehicle_Load(object sender, EventArgs e)
        {

        }
*/
        public void move(int xt, int yt) //xt and yt are the targetcoordinates
        {
            calculateAngle(xt, yt);
            float xmove = Math.Abs(xt - x) / (Math.Abs(xt - x) + Math.Abs(yt - y));
            float ymove = 1.0f - xmove;
            while (true)
            {
                while (Math.Abs(x - xt) > 1 && Math.Abs(y - yt) > 1)
                {
                    if (x < xt)
                    {
                        x = x + xmove * speed / 6.25f;       //10 pixels per meter
                    }
                    if (x > xt)
                    {
                        x = x - xmove * speed / 6.25f;
                    }
                    if (y < yt)
                    {
                        y = y + ymove * speed / 6.25f;
                    }
                    if (y > yt)
                    {
                        y = y - ymove * speed / 6.25f;
                    }
                    brkdistance(xt, yt);
                    Thread.Sleep(16);
                }
                speed = 0;
                a = 0;
                isAccelerating = false;
                Thread.Sleep(32);
            }
        }
        

        public void tryBrake(float targetspeed)
        {
            isAccelerating = false;
            isBraking = true;
            try
            {                
                if (brk != null)
                {
                    brk = null;
                }

                brk = new Thread(() => brakeToSpeed(targetspeed));
                brk.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void tryAccelerate(float targetspeed)
        {

            isAccelerating = true;
            isBraking = false;

            if (targetspeed > topspeed)
            {
                targetspeed = topspeed;
            }

            try
            {
                if (acc == null)
                {
                    acc = new Thread(() => accelerate(targetspeed));
                    acc.Start();
                }
                else
                {
                    acc = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void accelerate(float targetspeed)
        {
            while (speed < targetspeed && isAccelerating)
            {
                airResistance = (float) (0.5f * physics.Density * cw * surface * speed * speed);
                rollingResistance = (float) (crw * this.weight * physics.Gravity);
                a = (this.motorpwr - (airResistance + rollingResistance)) / this.weight;
                if (airResistance + rollingResistance < this.motorpwr)
                {
                    speed += a * 0.016f;
                }                
                Thread.Sleep(16);
            }            
            if (speed > targetspeed)
            {
                speed = targetspeed;
                isAccelerating = false;
            }
        }
    }
}
