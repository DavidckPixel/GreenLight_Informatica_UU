using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class BetterAI
    {
        float reactionSpeed;
        public float followInterval;
        int speedRelativeToLimit;
        float ruleBreakingChance;

        int roadSpeedLimit;
        public double targetspeed = 4; //TEMPERALY PUBLIC

        public bool isBraking = false;
        public bool isAccelerating = true;
        public float accelerate = 0.016f;

        private BetterVehicle vehicle;

        public LanePoints goal;
        public LanePoints origin;
        public double lanePointDistance;
        public double vehiclePointDistance;

        public Point locationGoal;

        public AbstractRoad nextRoad; //
        public DrivingLane nextLane; //

        public List<AbstractRoad> drivingRoads = new List<AbstractRoad>();
        public int index;

        public bool wantsToSwitch;
        public bool closeToCars;
        private GPS navigator;

        public int lanePointsMovePerTick;

        public int currentLaneIndex;

        public BetterAI(DriverStats _stats, BetterVehicle _vehicle)
        {
            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;

            vehicle = _vehicle;
            vehicle.vehicleAI = this;

            navigator = new GPS();
        }

        public void Update()
        {
            DistanceToCars();

            if (wantsToSwitch)
            {
                SwitchLanes();
            }

            NeedToBrake();
            CalculateAcceleration();
        }

        public void DistanceToCars()
        {
            closeToCars = false;

            List<BetterVehicle> allVehicles = BetterVehicleTest.vehiclelist;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll(x => x.currentLane == this.vehicle.currentLane);

            int _distance = RoadMath.LanePointsInDistance(this.vehicle.brakeDistance, this.currentLaneIndex, this.vehicle.currentLane.points);

            //x.vehicleAI.currentLaneIndex - x.vehicleAI.lanePointsMovePerTick - 5 <= this.currentLaneIndex && x.vehicleAI.currentLaneIndex + x.vehicleAI.lanePointsMovePerTick + 5 > this.currentLaneIndex
            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLaneIndex > this.currentLaneIndex && x.vehicleAI.currentLaneIndex < this.currentLaneIndex  + _distance))
            {
                Console.WriteLine("TOO CLOSE!!! BRAKE!!!");

                this.wantsToSwitch = true;
                this.closeToCars = true;
            }
            else
            {
                this.isBraking = false;
            }
        }

        public void NeedToBrake()
        {
            double _distance = RoadMath.Distance(this.locationGoal.X, this.locationGoal.Y, this.vehicle.locationX, this.vehicle.locationY);

           
            double _distanceToRoadSwitch = RoadMath.DistanceToLastLanePoint(this.currentLaneIndex, vehicle.currentLane.points);

            double _brakeDistance = vehicle.brakeDistance + vehicle.speed;

            if (_distance < _brakeDistance)
            {
                this.isBraking = true;
            }

            if(_distanceToRoadSwitch < _brakeDistance && navigator.ForcedLane != null && vehicle.currentLane.thisLane != navigator.ForcedLane)
            {
                this.isBraking = true;
            }

            if (closeToCars)
            {
                this.isBraking = true;
            }
        }

        private DrivingLane CheckSwitchLane()
        {
            AbstractRoad _road = vehicle.currentRoad;
            DrivingLane _drivinglane = vehicle.currentLane;

            DrivingLane _selectedLane = null;

            if(_road.getLanes() == 1)
            {
                return null;
            }

            int _laneNum = _drivinglane.thisLane;

            if (navigator.ForcedLane == _laneNum)
            {
                //CAR must be on this lane
                return null;
            }

            bool _leftAvailable = LaneSideAvailable(-1);
            bool _rightAvailable = LaneSideAvailable(1);
               

            if (_laneNum != _road.getLanes() && _rightAvailable)
            {
                _selectedLane = (DrivingLane)_road.Drivinglanes[_laneNum];
            }
            else if (_laneNum != 1 && _leftAvailable)
            {
                _selectedLane = (DrivingLane)_road.Drivinglanes[_laneNum - 2];
            }

            return _selectedLane;
        }

        private bool LaneSideAvailable(int _side) //can be 1 or -1
        {
            int _goalLaneIndex = this.vehicle.currentLane.thisLane + _side - 1;

            if(_goalLaneIndex < 0 || _goalLaneIndex > this.vehicle.currentRoad.Drivinglanes.Count() - 1)
            {
                return false; //LANE IS OUT OF INDEX
            }

            DrivingLane _goalLane = (DrivingLane)vehicle.currentRoad.Drivinglanes[_goalLaneIndex];

            if(_goalLane.flipped != this.vehicle.currentLane.flipped)
            {
                return false; //LANE GOES IN THE OPPOSITE DIRECTION
            }

            if(this.currentLaneIndex < 50)
            {
                return false; //SWITCH IS HAPPENING TOO CLOSE TO BEGINNING OF THE ROAD
            }

            List<BetterVehicle> allVehicles = BetterVehicleTest.vehiclelist;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll(x => x.currentLane.thisLane == _goalLane.thisLane && x.currentRoad == this.vehicle.currentRoad);

            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLaneIndex - x.vehicleAI.lanePointsMovePerTick - 5 <= this.currentLaneIndex && x.vehicleAI.currentLaneIndex + x.vehicleAI.lanePointsMovePerTick + 5> this.currentLaneIndex))
            {
                Console.WriteLine("there is a car in the way!"); 
                return false; //THERE IS A CAR IN THE WAY
            }

            return true;
        }

        private void SwitchLanes()
        {
            this.currentLaneIndex = this.vehicle.currentLane.points.FindIndex(x => x.cord == this.goal.cord);
            this.wantsToSwitch = false;
            DrivingLane _laneToGo = CheckSwitchLane();

            if(_laneToGo == null)
            {
                //Nolanes to switch too
                return;
            }

            this.origin = this.goal;
            this.goal = _laneToGo.points[this.currentLaneIndex + 1];

            this.lanePointDistance = RoadMath.Distance(vehicle.locationX, vehicle.locationY, goal.cord.X, goal.cord.Y);
            this.vehiclePointDistance = this.lanePointDistance;

            this.SteerWheel(RoadMath.TranslateDegree(RoadMath.CalculateAngle(new Point((int)vehicle.locationX, (int)vehicle.locationY), goal.cord))); //Lanes get switched But wrong way!!!

            this.vehicle.currentLane = _laneToGo;
            DistanceToCars();
        }

        private void CalculateAcceleration()
        {
            if (this.vehicle.speed >= this.targetspeed)
            {
                this.isAccelerating = false;
            }
        }

        public void switchLanePoints()
        {
            List<LanePoints> _points = this.vehicle.currentLane.points;
            this.currentLaneIndex = _points.FindIndex(x => x.cord == this.goal.cord);
            int _index = this.currentLaneIndex;

            if (_index == _points.Count() - 1)
            {
                this.SwitchRoad();
                return;
            }

            this.origin = this.goal;
            this.goal = _points[_index + 1];
            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);

            this.vehiclePointDistance = this.lanePointDistance;

            this.SteerWheel(RoadMath.TranslateDegree(origin.degree)); // + 180 % 360 ?
        }

        private void SteerWheel(int _angel)
        {
            this.vehicle.currentAngel = _angel;
        }

        public void SetPath(int _startIndex) //Naam weiziging
        {

            this.index = _startIndex;

            this.origin = this.vehicle.currentLane.points.First();
            this.goal = this.vehicle.currentLane.points[1]; //Dit kan een error krijgen;
            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.SteerWheel(RoadMath.TranslateDegree(goal.degree));

            this.locationGoal = new Point(-1000, -1000); //TEMP

            //Console.WriteLine("Origin Degreee: {0}", RoadMath.TranslateDegree(origin.degree));
            //Console.WriteLine("LanePointDistance: {0}", this.lanePointDistance);
        }

        public void SwitchRoad()
        {

            this.index += 1;

            if(this.drivingRoads.Count() <= index)
            {
                this.index = 0;
            }

            this.vehicle.SwitchRoad();

            this.origin = this.goal;
            this.goal = this.vehicle.currentLane.points.First();

            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.SteerWheel(RoadMath.TranslateDegree(RoadMath.CalculateAngle(origin.cord,goal.cord)));
        }


    }
}
