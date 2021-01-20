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
        public int priority = 4;

        public bool isBraking = false;
        public bool isAccelerating = true;
        public float accelerate = 0.016f;
        public bool handBreakOn = false;

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
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll(x => x.currentLane == this.vehicle.currentLane && x.currentRoad == this.vehicle.currentRoad);

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

            this.handBreakOn = false;

            int _index = currentCrossRoad.sides.ToList().FindIndex(x => x.hitbox.Contains(this.vehicle.currentLane.points.First().cord));

            string dir = ""; //STRAIGHT - LEFT - RIGHT
            int _indexEnd = 0;

            if (_currentRoad.Type == "Cross")
            {
                 _indexEnd = currentCrossRoad.sides.ToList().FindIndex(x => x.hitbox.Contains(this.vehicle.currentLane.points.Last().cord));
            }

            int _indexdiff = _index - _indexEnd;
            int _checkIndex1 = 0;
            int _checkIndex2 = 0;

            bool[] status = new bool[4] { false, false, false, false };

            if (Math.Abs(_indexdiff) == 2)
            {
                dir = "STRAIGHT";
                _index = _index + 1 > 3 ? 0 : _index + 1;

                if (this.currentCrossRoad.sides[_index].priorityLevel >= this.priority)
                {
                    status[_index] = true;

                    _index = _index - 1 < 0 ? 0 : _index - 1;
                    if (this.currentCrossRoad.sides[_index].priorityLevel > this.priority)
                    {
                        status[_index] = true;
                    }
                }
            }
            else if( _indexdiff == 1 || _indexdiff == -3)
            {
                dir = "LEFT";
                _checkIndex1 = _index + 1 > 3 ? 0 : _index + 1;
                _checkIndex2 = _index + 2 > 3 ? _index - 2 : _index + 2;
                status[_checkIndex2] = true;

                if (this.currentCrossRoad.sides[_index].priorityLevel >= this.priority)
                {
                    status[_checkIndex1] = true;

                    _index = _index - 1 < 0 ? 0 : _index - 1;
                    if (this.currentCrossRoad.sides[_index].priorityLevel > this.priority)
                    {
                        status[_index] = true;
                    }
                }
            }
            else if(_indexdiff == -1 || _indexdiff == 3)
            {
                dir = "RIGHT";
                _index = _index - 1 < 0 ? 0 : _index - 1;
                if (this.currentCrossRoad.sides[_index].priorityLevel > this.priority)
                {
                    status[_index] = true;
                }

            }

            CrossRoadSide _side1 = currentCrossRoad.sides[_checkIndex1];
            CrossRoadSide _side2 = currentCrossRoad.sides[_checkIndex2];

                //if ((_side1.status && _side1.priorityLevel > this.priority)|| (_side2.status && _side2.priorityLevel > this.priority) || )
                if((this.currentCrossRoad.sides[0].status && status[0])
                    || (this.currentCrossRoad.sides[1].status && status[1])
                    || (this.currentCrossRoad.sides[2].status && status[2])
                   || (this.currentCrossRoad.sides[3].status && status[3])) 
                {
                    this.isBraking = true; //THERE IS A CAR IN THE WAY;
                this.handBreakOn = true;
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
                        foreach (CrossRoadSide _side in currentCrossRoad.sides)
                        {
                            if (_side.hitbox.Contains(_location))
                            {
                                _currentFoundHitbox = _side.hitbox;
                                _side.aiOnSide.Add(this);
                                _side.status = true;

                                _side.priorityLevel = this.priority > _side.priorityLevel ? this.priority : _side.priorityLevel;
                            }
                            y++;
                        }
                        if (_currentFoundHitbox != null)
                        {
                            this.crossRoadTimer = 45;
                            break;
                        }
                    }

                    //Console.WriteLine("The SIDES: LINKS: {0}, BOTTOM: {1}, RIGHT: {2}, TOP: {3} ", currentCrossRoad.sideStatus[0], currentCrossRoad.sideStatus[1], currentCrossRoad.sideStatus[2], currentCrossRoad.sideStatus[3]);
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
                        foreach (CrossRoadSide _side in currentCrossRoad.sides)
                        {
                            if (_side.hitbox.Contains(_location))
                            {
                                _currentFoundHitbox = _side.hitbox;
                                _side.aiOnSide.Add(this);
                                _side.status = true;
                                Console.WriteLine("STATUS SET!");

                                _side.priorityLevel = this.priority > _side.priorityLevel ? this.priority : _side.priorityLevel;
                            }
                            y++;
                        }
                        if (_currentFoundHitbox != null)
                        {
                            this.crossRoadTimer = 45;
                            break;
                        }
                    }

                    Console.WriteLine("The SIDES: LINKS: {0}, BOTTOM: {1}, RIGHT: {2}, TOP: {3} ", currentCrossRoad.sides[0].status, currentCrossRoad.sides[1].status, currentCrossRoad.sides[2].status, currentCrossRoad.sides[3].status);
                }
            }catch(Exception e) {  }

            if(_currentFoundHitbox != null)
            {
                this.foundHitbox = _currentFoundHitbox;
            }
            else if(this.currentCrossRoad != null && this.foundHitbox != null)
            {
                int Index = this.currentCrossRoad.sides.ToList().FindIndex(x => x.hitbox == this.foundHitbox);

                if (this.currentCrossRoad.sides[Index].aiOnSide.Contains(this))
                {
                    return;
                }

                this.currentCrossRoad.sides[Index].aiOnSide.Remove(this);
                this.foundHitbox = null;

                if(this.currentCrossRoad.sides[Index].aiOnSide.Count <= 0)
                {
                    this.currentCrossRoad.sides[Index].status = false;
                }
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

            this.SteerWheel(origin.degree); // + 180 % 360 ?
        }

        private void SteerWheel(float _angel)
        {
            this.vehicle.currentAngel = _angel;
        }

        public void SetPath(List<AbstractRoad> _path, int _startIndex, int _startDrivingLaneIndex = 0, int _startDrivingPointsIndex = 0) //Naam weiziging
        {
            _startIndex = _startIndex > _path.Count() - 2 ? 0 : _startIndex; //The startindex cannot be higher then the amount of roads in the list;
            this.index = _startIndex; //Index is set to the start Index;
            this.drivingRoads = _path;

            vehicle.SwitchRoad(_path[_startIndex], _startDrivingLaneIndex); //Switch the active road
            this.nextRoad = _path[_startIndex + 1]; //Sets the active road to be the one after the current road;

            _startDrivingPointsIndex = _startIndex > this.vehicle.currentLane.points.Count() - 2 ? 0 : _startDrivingPointsIndex; //checks if the startDrivingPointsIndex will not get out of bounds

            this.origin = this.vehicle.currentLane.points[_startDrivingPointsIndex]; //sets the origin point
            this.goal = this.vehicle.currentLane.points[_startDrivingPointsIndex + 1]; //Dit kan een error krijgen;
            ForceCarLocation(this.origin.cord);

            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.currentLaneIndex = 0;
            this.SteerWheel(RoadMath.TranslateDegree(goal.degree));

            //this.locationGoal = _path.Last().Drivinglanes.First().points.Last().cord; //TEMP
            this.locationGoal = new Point(-1000, -1000);

            //Console.WriteLine("Origin Degreee: {0}", RoadMath.TranslateDegree(origin.degree));
            //Console.WriteLine("LanePointDistance: {0}", this.lanePointDistance);
        }

        private void ForceCarLocation(Point _p)
        {
            this.vehicle.locationX = _p.X;
            this.vehicle.locationY = _p.Y;
        }

        public void SwitchRoad()
        {
            this.index += 1;
            int _nextIndex = this.index + 1; ;

            if(this.drivingRoads.Count() <= index)
            {
                this.index = 0;
                _nextIndex = 1;
            }
            else if(this.drivingRoads.Count() <= _nextIndex)
            {
                _nextIndex = 0;
            }

            this.currentLaneIndex = 0;

            int _drivingLaneIndex = this.vehicle.currentLane.thisLane - 1;
            AbstractRoad _currentRoad = this.drivingRoads[this.index];

            if (_currentRoad.Type == "Cross")
            {
                _drivingLaneIndex = 0; //Select the correctDrivingLaneIndex
            }
            else if (this.vehicle.currentRoad.Type == "Cross")
            {
                _drivingLaneIndex = 0; //Select the correctDrivingLaneIndex
            }

            this.vehicle.SwitchRoad(_currentRoad, _drivingLaneIndex);
            this.nextRoad = this.drivingRoads[_nextIndex];

            this.origin = this.goal;
            this.goal = this.vehicle.currentLane.points.First();

            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.SteerWheel(RoadMath.TranslateDegree(RoadMath.CalculateAngle(origin.cord,goal.cord)));
        }
    }
}
