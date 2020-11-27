using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace GreenLight
{
    class Vehicle : Form        //tijdelijk
    {
        int weight;
        double length;
        int maxspeed;
        const int brakepwr = 12000; //In Newton
        int motorpwr;
        public double speed = 0;    //tijdelijke waarde in m/s
        string name;
        public double a, abrake; //versnelling & vertraging
        Thread start, stop;
        double x, y;
        double cw; // Weerstandscoëfficient, normaal tussen de 0.25 en 0.35
        double surface;
        double density = 1.293; //in Kg/m3
        double airResistance;
        double rollingResistance;
        double gravity = 9.81; //tijdeljk
        double crw = 0.012; // rolweerstandcoëfficient, 0.15 voor ijs, 0.9 voor beton, 0.67 voor droog asfalt, 0.53 voor nat asfalt
        Bitmap Car;
        int angle;
        bool isAccelerating = false;
        bool isBraking = false;


        public Vehicle(string name, int weight, double length, int maxspeed, int motorpwr, int x, int y, double cw, double surface)
        {
            this.weight = weight;
            this.length = length;
            this.maxspeed = maxspeed;
            this.name = name;
            this.motorpwr = motorpwr;
            this.x = x;
            this.y = y;
            this.cw = cw;
            this.surface = surface;
            a = this.motorpwr / this.weight;
            abrake = brakepwr / this.weight;
            Car = new Bitmap(Properties.Resources.Car);
            this.Paint += tekenAuto;
            Thread beweeg = new Thread(() => move(100800, 980));
            beweeg.Start();
            this.MouseClick += klik;
            this.DoubleBuffered = true;
        }

        public double Slipperiness {
            get{ return crw; }
            set { crw = value; }
        }
        
        //Tijdelijke tekenmethode
        void tekenAuto(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            int xtemp = (int) x;
            int ytemp = (int) y;
            g.DrawImage(RotateImage(Car, angle), xtemp, ytemp, Car.Width/25, Car.Width/25);
            g.DrawString((speed * 3.6).ToString(), new Font("Calibri", 10), Brushes.Black, 100, 130);
            g.DrawString(a.ToString(), new Font("Calibri", 10), Brushes.Black, 100, 100);
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

        void brakeInTime(double braketime)
        {
            while (braketime > 0 && speed > 0)
            {
                airResistance = 0.5 * density * cw * surface * speed * speed;
                abrake = (brakepwr + airResistance ) / this.weight;
                speed -= abrake / 100;
                braketime -= 0.01;
                Thread.Sleep(10);
            }
            if (speed < 0)
            {
                speed = 0;
            }
        }

        void brakeToSpeed(double targetspeed)
        {
            
            while (speed > targetspeed && isBraking)
            {
                airResistance = 0.5 * density * cw * surface * speed * speed;
                abrake = (brakepwr + airResistance) / this.weight;
                speed -= abrake / 100;
                Thread.Sleep(10);
            }
            if (speed < targetspeed)
            {
                speed = targetspeed;
            }
        }

        void calculateAngle(int xt, int yt)
        {

            double xDiff = xt - x;
            double yDiff = yt - y;
            angle = (int) (Math.Atan2(yDiff, xDiff) * (180 / Math.PI));

        }


        void move(int xt, int yt) //xt and yt are the targetcoordinates
        {
            calculateAngle(xt, yt);
            double xmove = Math.Abs(xt - x) / (Math.Abs(xt - x) + Math.Abs(yt - y));
            double ymove = 1.0 - xmove;
            while (true)
            {
                while (Math.Abs(x - xt) > 1 && Math.Abs(y - yt) > 1)
                {
                    if (x < xt)
                    {
                        x = x + xmove * speed / 10;       //10 pixels per meter
                    }
                    if (x > xt)
                    {
                        x = x - xmove * speed / 10;
                    }
                    if (y < yt)
                    {
                        y = y + ymove * speed / 10;
                    }
                    if (y > yt)
                    {
                        y = y - ymove * speed / 10;
                    }
                    Thread.Sleep(10);
                    this.Invalidate();
                }
                speed = 0;
                a = 0;
                isAccelerating = false;
            }
        }

        void klik(object o, EventArgs ea)
        {
            if (!isAccelerating)
            {
                start = new Thread(() => accelerate(maxspeed));
                start.Start();
                isAccelerating = true;
                isBraking = false;
            }
            else
            {
                stop = new Thread(() => brakeToSpeed(0));
                stop.Start();
                isBraking = true;
                isAccelerating = false;
            }
        }

        void accelerate(double targetspeed)
        {
            while (speed < targetspeed && isAccelerating)
            {
                airResistance = 0.5 * density * cw * surface * speed * speed;
                rollingResistance = crw * this.weight * gravity;
                a = (this.motorpwr - (airResistance + rollingResistance)) / this.weight;
                speed += a * 0.01;
                Thread.Sleep(10);
            }
            if (speed > targetspeed)
            {
                speed = targetspeed;
            }
        }
    }
}
