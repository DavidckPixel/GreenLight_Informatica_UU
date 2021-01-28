using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GreenLight.src.Driver.GPS;

namespace GreenLight
{
    public class BetterVehicle : ScreenObject
    {
        public BetterAI vehicleAI;
        public World physics;

        string name;
        int weight;
        int motorpwr;
        int topspeed;
        float length;
        float cw; //Drag coefficient
        float surface; //Surface area of the front of the vehicle

        public double speed;
        public double brakeDistance; //Distance until car is completely still;

        public float currentAngle = 90;

        public AbstractRoad currentRoad;
        public Lane currentLane;

        public double locationX, locationY;

        public float drawDegree;
        public bool hardStop;
        public bool toDelete;

        public RectHitbox hitbox;

        public BetterVehicle(VehicleStats _stat, Node _startPoint, BetterAI _ai) : base(_startPoint.knot.Cord)
        {
            this.physics = General_Form.Main.SimulationScreen.Simulator.worldController.SimulationWorld;

            this.weight = _stat.Weight;
            this.length = _stat.Length;
            this.topspeed = _stat.Topspeed;
            this.name = _stat.Name;
            this.motorpwr = _stat.Motorpwr;
            this.cw = _stat.Cw;
            this.surface = _stat.Surface;

            this.locationX = _startPoint.knot.Cord.X;
            this.locationY = _startPoint.knot.Cord.Y;

            this.vehicleAI = _ai;
            this.vehicleAI.setVehicle(this, _startPoint);
        }

        public void Update()
        {
            if (toDelete)
            {
                return;
            }

            if (hardStop)
            {
                return;
            }

            ChangeSpeed();
            ChangeLocation(speed);
        }

        public void ChangeSpeed()
        {
            double airResistance = (float)(0.5f * physics.Density * cw * surface * Math.Pow(this.speed,2));
            double abrake = physics.Brakepwr / this.weight;
            if (vehicleAI.isBraking)
            {
                this.speed -= abrake * vehicleAI.accelerate;
                this.speed = this.speed < 0 ? 0 : this.speed;
                if (speed == 0)
                {
                    vehicleAI.isBraking = false;
                }
            }
            else if (vehicleAI.isAccelerating == true && !vehicleAI.handBreakOn && !this.hardStop)
            { 
                double rollingResistance = (float)(physics.slip * this.weight * physics.Gravity);
                double a = (this.motorpwr - (airResistance + rollingResistance)) / this.weight;
                if (airResistance + rollingResistance < this.motorpwr)
                {
                    speed += a * vehicleAI.accelerate;
                }
            }
            brakeDistance = weight * speed * speed / (physics.Brakepwr);
            this.vehicleAI.lanePointsMovePerTick = RoadMath.LanePointsInDistance(this.speed * this.vehicleAI.followInterval, vehicleAI.currentLanePointIndex, this.currentLane.points);
        }

        public void ChangeLocation(double _speed)
        {
            double radAngle = (this.currentAngle + 270) % 360 * (Math.PI / 180);
            double moveX = Math.Cos(radAngle) * _speed / 5;
            double moveY = Math.Sin(radAngle) * _speed / 5;

            double distance = Math.Sqrt(Math.Abs(moveX * moveX) + Math.Abs(moveY * moveY));

            double locationmoved = 0;
            int _indexdiff = 0;
            int _index = this.vehicleAI.currentLanePointIndex;

            while (distance > locationmoved && _index + _indexdiff + 2 < this.currentLane.points.Count)
            {
                locationmoved += RoadMath.Distance(this.currentLane.points[_index + _indexdiff].cord, this.currentLane.points[_index + _indexdiff + 1].cord);
                _indexdiff++;
            }
            while (_indexdiff > 0)
            {
                this.vehicleAI.switchLanePoints();
                _indexdiff--;
            }
            this.currentAngle = this.vehicleAI.goal.degree;
            this.locationX = this.vehicleAI.goal.cord.X;
            this.locationY = this.vehicleAI.goal.cord.Y;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, new Rectangle(new Point((int)this.locationX, (int)this.locationY), new Size(5, 5)));

            Image _image = Image.FromFile("../../Images/BetterCarStraight.png");
            Bitmap _bitmap = new Bitmap(_image);

            _image = DrawData.BetterRotateImage(_image, this.currentAngle);

            g.DrawImage(_image, new Point((int)this.locationX - 10, (int)this.locationY - 10));
        }

        public void SwitchRoad(AbstractRoad _road, int _laneIndex)
        {
            this.currentRoad = _road;
            this.currentLane = this.currentRoad.Drivinglanes[_laneIndex - 1];
        }

        public void CreateHitbox()
        {
            hitbox = new RectHitbox(new Point((int)this.locationX - 10, (int)this.locationY - 10), new Point((int)this.locationX + 10, (int)this.locationY - 10), new Point((int)this.locationX - 10, (int)this.locationY + 10),new Point((int)this.locationX + 10, (int)this.locationY + 10), Color.Pink);
        }

        public void DeleteVehicle(bool _dumpData)
        {
            this.toDelete = true;
            General_Form.Main.SimulationScreen.Simulator.vehicleController.toDelete.Add(this);
        }
    }
}