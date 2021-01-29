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
        //This class is used to control and draw one vehicle. A vehicle gets directions from the AI in the BetterAI class.
        //The vehicle gets its data from the VehicleType.json
        //In this class, the current speed of the vehicle is being calculated, after which the location is being updated based on its speed.

        public BetterAI vehicleAI;
        public World physics;

        string name;
        int weight;
        int motorpwr;
        public int topspeed;
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

        //Initializes variables, mainly from the VehicleType.json
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
            this.vehicleAI.SetVehicle(this, _startPoint);
        }

        //Updates the speed as well as the location of the vehicle.
        //This usually happens every 16 milliseconds, in order to achieve 62.5 FPS.
        //This method will be called from the SimulationController
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

        //Here, the speed of the vehicle will be adjusted when braking or accelarating.
        //The change in speed is based on the airresistance, rollingresistance and motorpower when accelerating.
        //The change in speed is based on the brakepower when braking
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

        //Here, the vehicle will update its location based on its speed and the driving direction.
        //If the distance traveled in one tick is a few points in the driving lane, this method will call the switchLanePoints method a few times, allowing the coordinates to be updated
        //The vehicle will then be set to those coordinates, updating its location to where it should be.
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
                this.vehicleAI.SwitchLanePoints();
                _indexdiff--;
            }
            this.currentAngle = this.vehicleAI.goal.degree;

            this.locationX = this.vehicleAI.origin.cord.X;
            this.locationY = this.vehicleAI.origin.cord.Y;
        }


        //The method draws the vehicle based on its location and direction
        public override void Draw(Graphics g)
        {
            Image _image = Image.FromFile("../../Images/BetterCarStraight.png");
            Bitmap _bitmap = new Bitmap(_image);

            _image = DrawData.BetterRotateImage(_image, this.currentAngle);
            if (!this.toDelete)
            {
                g.DrawImage(_image, new Point((int)this.locationX - 10, (int)this.locationY - 10));
            }

        }

        //This method switches to the next road if the vehicle enters a connection between two roads
        public void SwitchRoad(AbstractRoad _road, int _laneIndex)
        {
            
            this.currentRoad = _road;
            this.currentLane = this.currentRoad.Drivinglanes[_laneIndex - 1];
        }

        //This defines the hitbox of the vehicle
        public void CreateHitbox()
        {
            hitbox = new RectHitbox(new Point((int)this.locationX - 10, (int)this.locationY - 10), new Point((int)this.locationX + 10, (int)this.locationY - 10), new Point((int)this.locationX - 10, (int)this.locationY + 10),new Point((int)this.locationX + 10, (int)this.locationY + 10), Color.Pink);
        }

        //Here, the vehicle will be set to delete so it won't be included in the simulation anymore
        public void DeleteVehicle(bool _dumpData)
        {
            this.toDelete = true;
            General_Form.Main.SimulationScreen.Simulator.vehicleController.toDelete.Add(this);
        }
    }
}