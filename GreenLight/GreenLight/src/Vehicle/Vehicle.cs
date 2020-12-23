using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace GreenLight
{
    class Vehicle : ScreenObject     
    {

        //This is the vehicle class, which creates a car.
        //This class also contains methods that calculate how the car moves, accelarates and brakes.
        //All calculations are based on real-life physics
        //The variables of which the car exists are stored in the VehicleType json file and read in with the VehicleTypeConfig class
        //World variables like gravity are stored in the Earth json file and read in with the WorldConfig class
        
        public float x, y; //Location of the vehicle
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

        public bool isAccelerating = true;
        public bool isBraking = false;

        //deze threads zijn niet in gebruik nu, acc, brk en move worden allemaal aangestuurd vanuit 1 Thread in de AI
        /*Thread acc, brk;
        Thread startmove;*/
        
        Bitmap Car; //Image of the vehicle
        int angle; //Angle at which the image/vehicle is rotated


        public Vehicle(string name, int weight, float length, int topspeed, int motorpwr, int x, int y, float cw, float surface) : base(new Point(x,y), new Size(20,20))
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
            
            Console.WriteLine("Created vehicle");
            
            
            a = this.motorpwr / this.weight;
            abrake = physics.Brakepwr / this.weight;
            Car = new Bitmap(Properties.Resources.Car);

        }

        //method to change destination in multi-threaded car system

        /*public void klik(int xt, int yt)
        {
            isBraking = false;
            isAccelerating = false;
            startmove.Abort();            
            startmove = new Thread(() => move(xt, yt));
            startmove.Start();
        }*/

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

        //Brake for set period of time method, this method is old, and isn't updated since the beginning of vehicles.
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

        //Brake to targetspeed method for single threaded car system
        public void brakeToSpeed (float targetspeed)
        {
            airResistance = (float)(0.5f * physics.Density * cw * surface * speed * speed);
            abrake = (physics.Brakepwr + airResistance) / this.weight;

            speed -= (abrake * (0.07f)); //Een waarde tussen 0.069 en 0.07 werkt hier het best, maar waar the f*** komt deze waarde vandaan???
            
            if (speed <= targetspeed)
            {
                speed = targetspeed;
                isBraking = false;
            }
        }
        
        //Brake to targetspeed method for multithreaded car system        
        /*public void brakeToSpeed(float targetspeed)
        {
            
            while (speed > targetspeed && isBraking)
            {                
                airResistance = (float) (0.5f * physics.Density * cw * surface * speed * speed);
                abrake = (physics.Brakepwr + airResistance) / this.weight;
                
                speed -= abrake * 0.016f;
                Thread.Sleep(16);
            }
            if (speed < targetspeed)
            {
                speed = targetspeed;                
            }
            //isBraking = false;
        }*/

        //Method to calculate the distance the car would need to brake to zero
        public float brkdistance(int xt, int yt)
        {
            airResistance = (float)(0.5f * physics.Density * cw * surface * speed * speed);
            abrake = (physics.Brakepwr + airResistance) / this.weight;

            float brktime = speed / abrake;
            float brkdistance = brktime * speed;
            
            //Console.WriteLine(brkdistance + "    -    " + distancefromend + "    -    " + speed);

            return brkdistance;
        }

        public void calculateAngle(int xt, int yt)
        {
            float xDiff = xt - x;
            float yDiff = yt - y;
            angle = (int) (Math.Atan2(yDiff, xDiff) * (180 / Math.PI));

        }


/*        private void Vehicle_Load(object sender, EventArgs e)
        {

        }
*/
        //method used to calculate new x and y for vehicle in single threaded car system
        public void move(int xt, int yt)
        {
            if (Math.Abs(x - xt) > 1 && Math.Abs(y - yt) > 1)
            {
                //calculateAngle(xt, yt);
                float xmove = Math.Abs(xt - x) / (Math.Abs(xt - x) + Math.Abs(yt - y));
                float ymove = 1.0f - xmove;
                if (x < xt)
                {
                    x = x + xmove * speed * 0.8f;       //5 pixels per meter
                }
                if (x > xt)
                {
                    x = x - xmove * speed * 0.8f;
                }
                if (y < yt)
                {
                    y = y + ymove * speed * 0.8f;
                }
                if (y > yt)
                {
                    y = y - ymove * speed * 0.8f;
                }            
                
            }
            else
            {
                speed = 0;
                isAccelerating = false;
                isBraking = false;
            }
        }

        //method used to calculate new x and y in multi treaded car system
        /*public void move(int xt, int yt) //xt and yt are the targetcoordinates
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
                        x = x + xmove * speed * 0.8f;       //5 pixels per meter
                    }
                    if (x > xt)
                    {
                        x = x - xmove * speed * 0.8f;
                    }
                    if (y < yt)
                    {
                        y = y + ymove * speed * 0.8f;
                    }
                    if (y > yt)
                    {
                        y = y - ymove * speed * 0.8f;
                    }
                    brkdistance(xt, yt);
                    //Console.WriteLine(speed);
                    Thread.Sleep(16);
                }
                //speed = 0;
                a = 0;
                isAccelerating = false;
                Thread.Sleep(32);
            }
        }*/

        //method used to call the brake to speed method in multi threaded car system;
        /*public void tryBrake(float targetspeed)
        {
            isBraking = true;
            isAccelerating = false;
            
            

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
        }*/


        //method used to call the accelerate to speed method in multi threaded car system;
        /*public void tryAccelerate(float targetspeed)
        {

            isAccelerating = true;
            isBraking = false;

            if (targetspeed > topspeed)
            {
                targetspeed = topspeed;
            }

            try
            {
                if (acc != null)
                {
                    acc = null;
                }
                acc = new Thread(() => accelerate(targetspeed));
                acc.Start();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }*/

        //method used to accelerate to target speed in the single threaded car system
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
        
        //method used to accelerate to target speed in the multi threaded car system
        /*public void accelerate(float targetspeed)
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
        }*/
    }
}
