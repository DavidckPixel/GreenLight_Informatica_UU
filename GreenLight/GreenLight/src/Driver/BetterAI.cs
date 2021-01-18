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

        public AbstractRoad nextRoad = null;
        public Lane nextLane = null; 

        public List<AbstractRoad> drivingRoads = new List<AbstractRoad>();
        public int index;

        public bool wantsToSwitch;
        public bool closeToCars;
        private GPS navigator;

        public int lanePointsMovePerTick;

        public int currentLaneIndex;

        RectHitbox foundHitbox = null;
        CrossRoad currentCrossRoad = null;
        int crossRoadTimer = 0;

        bool crossroadPriority = false;
        bool needsToStop = false;

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

            InCrossRoadRange();
            crossRoadRules();
            NeedToBrake();
            CalculateAcceleration();
        }

        public void DistanceToCars()
        {
            closeToCars = false;

            List<BetterVehicle> allVehicles = BetterVehicleTest.vehiclelist;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll(x => x.currentLane == this.vehicle.currentLane);

            int _distance = RoadMath.LanePointsInDistance(this.vehicle.brakeDistance + 8, this.currentLaneIndex, this.vehicle.currentLane.points);

            _distance = _distance == 0 ? 1 : _distance;

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

        private void crossRoadRules()
        {
            int Num = RoadMath.LanePointsInDistance(10, 0, this.vehicle.currentLane.points);

            AbstractRoad _currentRoad = vehicle.currentRoad;
            if (!(_currentRoad.Type == "Cross" || this.nextRoad.Type == "Cross"))
            {
                return; //THIS OR NEXT ROAD IS NOT CROSSROAD
            }

            int _indexfromOther = (int)Math.Ceiling(vehicle.brakeDistance) > Num ? (int)Math.Ceiling(vehicle.brakeDistance) - Num : 0;

            if (vehicle.currentLane.points.Count() - this.currentLaneIndex > _indexfromOther && this.nextRoad.Type == "Cross") 
            {
                return; //TOO FAR AWAY
            }            

            if(Num - this.currentLaneIndex >= Math.Ceiling(vehicle.brakeDistance) && _currentRoad.Type == "Cross")
            {        
                return;
            }

            int _index = currentCrossRoad.sideHitboxes.ToList().FindIndex(x => x.Contains(this.vehicle.currentLane.points.First().cord));

            string dir = ""; //STRAIGHT - LEFT - RIGHT
            int _indexEnd = 0;

            if (_currentRoad.Type == "Cross")
            {
                 _indexEnd = currentCrossRoad.sideHitboxes.ToList().FindIndex(x => x.Contains(this.vehicle.currentLane.points.Last().cord));
            }

            int _indexdiff = _index - _indexEnd;
            int _checkIndex1 = 0;
            int _checkIndex2 = 0;

            if (Math.Abs(_indexdiff) == 2)
            {
                dir = "STRAIGHT";
                _index = _index + 1 > 3 ? 0 : _index + 1;
                _checkIndex1 = _index;
                _checkIndex2 = _index;
            }
            else if( _indexdiff == 1 || _indexdiff == -3)
            {
                dir = "LEFT";
                _index = _index + 1 > 3 ? 0 : _index + 1;
                _checkIndex1 = _index;

                _checkIndex2 = _index + 2 > 3 ? _index - 2 : _index + 2;
            }
            else if(_indexdiff == -1 || _indexdiff == 3)
            {
                dir = "RIGHT";
                _checkIndex1 = -1;
                _checkIndex2 = -1;
            }

            if (dir == "LEFT" || dir == "STRAIGHT")
            {
                if (currentCrossRoad.sideStatus[_checkIndex1] || currentCrossRoad.sideStatus[_checkIndex2])
                {
                    this.isBraking = true; //THERE IS A CAR IN THE WAY;
                }
            }

            //CHECK FIRST AND LAST LANEPOINT, SEE IF THEY ARE ON ONE LINE = STRAIGHT AHEAD;
            //
        }

        private void InCrossRoadRange()
        {
            //IF CURRENTROAD OR NEXTROAD == CROSSROAD
            AbstractRoad _currentRoad = vehicle.currentRoad;

            if (!(_currentRoad.Type == "Cross" || this.nextRoad.Type == "Cross") || this.crossRoadTimer > 0)
            {
                //Console.WriteLine("THERE WAS NO CROSSROAD IN SIGHT");
                return;

            }
            int pointsTillEnd = this.vehicle.currentLane.points.Count() - 1 - this.currentLaneIndex;
            int checkPointsAhead = 30;
            int pointsdiff = 30 - pointsTillEnd;

            if(pointsTillEnd >= 30 && _currentRoad.Type != "Cross")
            {
                return;
            }

            RectHitbox _currentFoundHitbox = null;
            Point _location = new Point((int)vehicle.locationX, (int)vehicle.locationY);
            try
            {
                if (pointsTillEnd >= checkPointsAhead)
                {
                    Console.WriteLine("WE ARE CURRENTLY ON THE CROSSROAD!");

                    currentCrossRoad = (CrossRoad)_currentRoad;
                    for (int x = 0; x < 30; x++)
                    {
                        _location = this.vehicle.currentLane.points[this.currentLaneIndex + x].cord;

                        int y = 0;
                        foreach (RectHitbox _hitbox in currentCrossRoad.sideHitboxes)
                        {
                            if (_hitbox.Contains(_location))
                            {
                                _currentFoundHitbox = _hitbox;
                                currentCrossRoad.sideStatus[y] = true;

                            }
                            y++;
                        }
                        if (_currentFoundHitbox != null)
                        {
                            this.crossRoadTimer = 45;
                            break;
                        }
                    }

                    Console.WriteLine("The SIDES: LINKS: {0}, BOTTOM: {1}, RIGHT: {2}, TOP: {3} ", currentCrossRoad.sideStatus[0], currentCrossRoad.sideStatus[1], currentCrossRoad.sideStatus[2], currentCrossRoad.sideStatus[3]);
                }
                else
                {
                    Console.WriteLine("THE CROSSROAD IS THE NEXT ROAD!");
                    Console.WriteLine(pointsdiff);
                    currentCrossRoad = (CrossRoad)this.nextRoad;
                    for (int x = 0; x < Math.Abs(pointsdiff); x++)
                    {
                        _location = this.nextRoad.Drivinglanes.First().points[x].cord;

                        Console.WriteLine(_location);

                        int y = 0;
                        foreach (RectHitbox _hitbox in currentCrossRoad.sideHitboxes)
                        {
                            if (_hitbox.Contains(_location))
                            {
                                _currentFoundHitbox = _hitbox;
                                currentCrossRoad.sideStatus[y] = true;
                                Console.WriteLine("STATUS SET!");
                            }
                            y++;
                        }
                        if (_currentFoundHitbox != null)
                        {
                            this.crossRoadTimer = 45;
                            break;
                        }
                    }

                    Console.WriteLine("The SIDES: LINKS: {0}, BOTTOM: {1}, RIGHT: {2}, TOP: {3} ", currentCrossRoad.sideStatus[0], currentCrossRoad.sideStatus[1], currentCrossRoad.sideStatus[2], currentCrossRoad.sideStatus[3]);
                }
            }catch(Exception e) {  }

            if(_currentFoundHitbox != null)
            {
                this.foundHitbox = _currentFoundHitbox;
            }
            else if(this.currentCrossRoad != null && this.foundHitbox != null)
            {
                int Index = this.currentCrossRoad.sideHitboxes.ToList().IndexOf(this.foundHitbox);
                this.currentCrossRoad.sideStatus[Index] = false;
                this.foundHitbox = null;
                Console.WriteLine("WE LEFT THE CROSSROAD");
            }
            
            //FORALL CROSSROADS  WHERE I WILL BE IN THE HITBOX IN 30 LANEPOINTS (OR LESS) ,SET SIDE TO TRUE
        }

        public void NeedToBrake()
        {
            double _distance = RoadMath.Distance(this.locationGoal.X, this.locationGoal.Y, this.vehicle.locationX, this.vehicle.locationY);

           
            double _distanceToRoadSwitch = RoadMath.DistanceToLastLanePoint(this.currentLaneIndex, vehicle.currentLane.points);

            double _brakeDistance = vehicle.brakeDistance + vehicle.speed;

            if (this.needsToStop)
            {
                //this.isBraking = true;
            }

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

        private Lane CheckSwitchLane()
        {
            AbstractRoad _road = vehicle.currentRoad;
            Lane _drivinglane = vehicle.currentLane;

            Lane _selectedLane = null;

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
                _selectedLane = _road.Drivinglanes[_laneNum];
            }
            else if (_laneNum != 1 && _leftAvailable)
            {
                _selectedLane = _road.Drivinglanes[_laneNum - 2];
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

            if(this.vehicle.currentRoad.Type == "Curved")
            {
                return false; //CARS CANNOT SWITCH WHILE ON CURVED ROADS
            }

            Lane _goalLane = vehicle.currentRoad.Drivinglanes[_goalLaneIndex];

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
            Lane _laneToGo = CheckSwitchLane();

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
            else
            {
                this.isAccelerating = true;
            }
        }

        public void switchLanePoints()
        {
            List<LanePoints> _points = this.vehicle.currentLane.points;
            this.currentLaneIndex++;
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

            if(crossRoadTimer > 0)
            {
                crossRoadTimer--;
            }

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
            this.currentLaneIndex = 0;

            this.SteerWheel(RoadMath.TranslateDegree(goal.degree));

            this.locationGoal = new Point(-1000, -1000); //TEMP

            //Console.WriteLine("Origin Degreee: {0}", RoadMath.TranslateDegree(origin.degree));
            //Console.WriteLine("LanePointDistance: {0}", this.lanePointDistance);
        }

        public void SwitchRoad()
        {

            this.index += 1;

            if(this.drivingRoads.Count() - 1 <= index)
            {
                this.index = 0;
            }


            this.currentLaneIndex = 0;
            this.vehicle.SwitchRoad();
            this.nextRoad = this.drivingRoads[this.index];

            this.origin = this.goal;
            this.goal = this.vehicle.currentLane.points.First();

            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.SteerWheel(RoadMath.TranslateDegree(RoadMath.CalculateAngle(origin.cord,goal.cord)));
        }
    }
}
