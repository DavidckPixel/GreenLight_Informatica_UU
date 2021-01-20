using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class BetterVehicle : ScreenObject
    {
        public BetterAI vehicleAI;
        private World physics = WorldConfig.physics.First();

        string name;
        int weight;
        float length;
        int motorpwr;
        int topspeed; //Topspeed of vehicle
        float cw; //Drag coefficient
        float surface; //Surface area of the front of the vehicle

        public double speed;
        public double brakeDistance; //Distance until car is completely still;

        public float currentAngel = 90;

        public AbstractRoad currentRoad;
        public Lane currentLane;

        public double locationX, locationY;

        public float drawDegree;

        public BetterVehicle(VehicleStats _stat, Point _startPoint) : base(_startPoint)
        {
            this.weight = _stat.Weight;
            this.length = _stat.Length;
            this.topspeed = _stat.Topspeed;
            this.name = _stat.Name;
            this.motorpwr = _stat.Motorpwr;
            this.cw = _stat.Cw;
            this.surface = _stat.Surface;

            this.locationX = _startPoint.X;
            this.locationY = _startPoint.Y;
        }

        public void Update()
        {
            ChangeSpeed();
            StayOnLane(this.speed);
            //WriteCarData();

            //Console.WriteLine("---------------------------------------------------------");
        }

        public void ChangeSpeed()
        {
            double airResistance = (float)(0.5f * physics.Density * cw * surface * Math.Pow(this.speed,2));
            double abrake = (physics.Brakepwr + airResistance) / this.weight;

            if (vehicleAI.isBraking) //The AI is braking
            {
                this.speed -= abrake;
                this.speed = this.speed < 0 ? 0 : this.speed;
            }
            else if (vehicleAI.isAccelerating == true && !vehicleAI.handBreakOn)
            { 
                double rollingResistance = (float)(physics.slip * this.weight * physics.Gravity);
                double a = (this.motorpwr - (airResistance + rollingResistance)) / this.weight;

                if (airResistance + rollingResistance < this.motorpwr)
                {
                    speed += a * vehicleAI.accelerate;
                }

                //Console.WriteLine("Accelerating!!! - {0}", this.speed);
            }

            brakeDistance = weight * speed * speed / (physics.Brakepwr * 2);

            this.vehicleAI.lanePointsMovePerTick = RoadMath.LanePointsInDistance(this.speed * this.vehicleAI.followInterval, vehicleAI.currentLaneIndex, this.currentLane.points);
        }

        public void ChangeLocation(double _speed)
        {
            double radAngel =( (this.currentAngel + 270) % 360) * (Math.PI / 180);

            double moveX = Math.Cos(radAngel) * _speed;
            double moveY = Math.Sin(radAngel) * _speed;

            double newLocationX = this.locationX + moveX;
            double newLocationY = this.locationY + moveY;

            this.locationX = newLocationX;
            this.locationY = newLocationY;

            this.vehicleAI.vehiclePointDistance -= _speed;
       
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, new Rectangle(new Point((int)this.locationX, (int)this.locationY), new Size(5, 5)));

            Image _image = Image.FromFile("../../Images/BetterCarStraight.png");
            Bitmap _bitmap = new Bitmap(_image);

            _image = DrawData.BetterRotateImage(_image, this.currentAngel);  //HIER MOET NOG NAAR GEKEKEN WORDEN!!!!

            g.DrawImage(_image, new Point((int)this.locationX - 10, (int)this.locationY - 10));
        }

        private void WriteCarData()
        {
            Console.WriteLine("CAR: {0}", this.name);
            Console.WriteLine("CAR LOCATION: {0}", new Point((int)this.locationX, (int)this.locationY));
            Console.WriteLine("CURRENT ANGEL: {0}", this.currentAngel);
            Console.WriteLine("SPEED: {0}", this.speed);
            Console.WriteLine("isBraking: {0}", vehicleAI.isBraking);
            Console.WriteLine("Distance to Point: {0}", vehicleAI.vehiclePointDistance);
        }

        public void SwitchRoad(AbstractRoad _road, int _laneIndex)
        {
            this.currentRoad = _road;
            this.currentLane = this.currentRoad.Drivinglanes[_laneIndex];
        }

        private void StayOnLane(double _localspeed)
        {
            if (_localspeed > this.vehicleAI.vehiclePointDistance)
            {
                _localspeed -= this.vehicleAI.vehiclePointDistance;

                this.locationX = vehicleAI.goal.cord.X;
                this.locationY = vehicleAI.goal.cord.Y;
                vehicleAI.switchLanePoints();
                StayOnLane(_localspeed);
            }
            else
            {
                ChangeLocation(_localspeed);
            }
        }
    }
}
