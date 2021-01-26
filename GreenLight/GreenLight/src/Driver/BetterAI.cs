using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GreenLight.src.Data_Collection;
using GreenLight.src.Driver.GPS;

namespace GreenLight
{
    public class BetterAI
    {
        public DriverProfile profile;

        float reactionSpeed;
        public float followInterval;
        int speedRelativeToLimit;
        float ruleBreakingChance;

        int roadSpeedLimit;
        public double targetspeed = 2; //TEMPERALY PUBLIC
        public int priority = 2;

        public bool isBraking = false;
        public bool isAccelerating = true;
        public float accelerate = 0.016f;
        public bool handBreakOn = false;
        public bool brakeToZero = false;
        public bool toDelete = false;

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
        private BetterGPS navigator;

        public int lanePointsMovePerTick;

        public int currentLanePointIndex;
        public int CurrentLaneIndex;

        RectHitbox foundHitbox = null;
        CrossRoad currentCrossRoad = null;
        int crossRoadTimer = 0;

        bool needsToStop = false;
        List<PlacedSign> signsOnRoadRead = new List<PlacedSign>();

        

        public BetterAI(DriverStats _stats)
        {
            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;
        }

        public void initGPS(Node _startNode)
        {
            navigator = new BetterGPS(this, _startNode);
            this.SetPath();
        }

        public void setVehicle(BetterVehicle _vehicle, Node _startNode)
        {
            this.vehicle = _vehicle;
            profile = new DriverProfile(this.vehicle.physics);

            initGPS(_startNode);
        }

        public void Update()
        {
            if (toDelete)
            {
                return;
            }

            if (vehicle.hardStop)
            {
                return;
            }
            DistanceToCars();

            if (wantsToSwitch)
            {
                SwitchLanes();
            }

            InCrossRoadRange();
            crossRoadRules();

            NeedToBrake();
            CalculateAcceleration();

            UpdateProfile();
        }

        public void ChangeTargetSpeed(double _speed)
        {
            this.targetspeed = _speed + this.speedRelativeToLimit;
        }

        public void ChangePriority(int priority)
        {
            if (BrakeRule())
            {
                priority++;
            }
            if (BrakeRule(2))
            {
                priority++;
            }

            this.priority = priority;
        }

        public bool BrakeRule(double multiplier = 1)
        {
            Random ran = new Random();
            int ranValue = ran.Next(0, 100);

            if(multiplier == 0)
            {
                return false;
            }

            if(ranValue / multiplier < this.ruleBreakingChance)
            {
                return true;
            }
            return false;
        }

        private void UpdateProfile()
        {
            if (this.isBraking)
            {
                profile.AddBreakTick();
            }
            profile.CalculateFuel(this.vehicle.speed);
        }

        public void DistanceToCars()
        {
            closeToCars = false;

            List<BetterVehicle> allVehicles = BetterVehicleTest.vehiclelist;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll(x => x.currentLane == this.vehicle.currentLane && x.currentRoad == this.vehicle.currentRoad);

            int _distance = RoadMath.LanePointsInDistance(this.vehicle.brakeDistance + 8, this.currentLanePointIndex, this.vehicle.currentLane.points);

            _distance = _distance == 0 ? 1 : _distance;

            //x.vehicleAI.currentLaneIndex - x.vehicleAI.lanePointsMovePerTick - 5 <= this.currentLaneIndex && x.vehicleAI.currentLaneIndex + x.vehicleAI.lanePointsMovePerTick + 5 > this.currentLaneIndex
            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLanePointIndex > this.currentLanePointIndex && x.vehicleAI.currentLanePointIndex < this.currentLanePointIndex  + _distance))
            {
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

            if (vehicle.currentLane.points.Count() - this.currentLanePointIndex > _indexfromOther && this.nextRoad.Type == "Cross") 
            {
                return; //TOO FAR AWAY
            }            

            if(Num - this.currentLanePointIndex >= Math.Ceiling(vehicle.brakeDistance) && _currentRoad.Type == "Cross")
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
                return;

            }
            int pointsTillEnd = this.vehicle.currentLane.points.Count() - 1 - this.currentLanePointIndex;
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
                    //Console.WriteLine("WE ARE CURRENTLY ON THE CROSSROAD!");

                    currentCrossRoad = (CrossRoad)_currentRoad;
                    for (int x = 0; x < 10; x++)
                    {
                        _location = this.vehicle.currentLane.points[this.currentLanePointIndex + x].cord;

                        foreach (CrossRoadSide _side in currentCrossRoad.sides)
                        {
                            if (_side.hitbox.Contains(_location))
                            {
                                _currentFoundHitbox = _side.hitbox;
                                _side.aiOnSide.Add(this);
                                _side.status = true;

                                _side.priorityLevel = this.priority > _side.priorityLevel ? this.priority : _side.priorityLevel;
                            }
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
                    //Console.WriteLine("THE CROSSROAD IS THE NEXT ROAD!");
                    //Console.WriteLine(pointsdiff);
                    currentCrossRoad = (CrossRoad)this.nextRoad;
                    for (int x = 0; x < Math.Abs(pointsdiff); x++)
                    {
                        _location = this.nextRoad.Drivinglanes.First().points[x].cord;

                        //Console.WriteLine(_location);

                        int y = 0;
                        foreach (CrossRoadSide _side in currentCrossRoad.sides)
                        {
                            if (_side.hitbox.Contains(_location))
                            {
                                _currentFoundHitbox = _side.hitbox;
                                _side.aiOnSide.Add(this);
                                _side.status = true;
                                //Console.WriteLine("STATUS SET!");

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

                    //Console.WriteLine("The SIDES: LINKS: {0}, BOTTOM: {1}, RIGHT: {2}, TOP: {3} ", currentCrossRoad.sides[0].status, currentCrossRoad.sides[1].status, currentCrossRoad.sides[2].status, currentCrossRoad.sides[3].status);
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
                    Console.WriteLine("HOEVAAK KWAM JE HIER DAN??");
                    //return;
                }

                this.currentCrossRoad.sides[Index].aiOnSide.Remove(this);
                this.foundHitbox = null;
                Console.WriteLine(this.currentCrossRoad.sides[Index].aiOnSide.Count + "AMOUNT OF VEHICLES ON THIS SIDE");

                if(!this.currentCrossRoad.sides[Index].aiOnSide.Any())
                {
                    this.currentCrossRoad.sides[Index].status = false;
                    this.currentCrossRoad.sides[Index].priorityLevel = 0;
                }
                else 
                {
                    this.currentCrossRoad.sides[Index].priorityLevel = this.currentCrossRoad.sides[Index].aiOnSide.Max(x => x.priority);
                }

                //Console.WriteLine("WE LEFT THE CROSSROAD");
            }
            
            //FORALL CROSSROADS  WHERE I WILL BE IN THE HITBOX IN 30 LANEPOINTS (OR LESS) ,SET SIDE TO TRUE
        }

        private void CheckForSigns()
        {
            List<PlacedSign> _signsOnRoad = this.vehicle.currentRoad.Signs;
            List<PlacedSign> _inHitbox = _signsOnRoad.FindAll(x => x.Hitbox.Contains(goal.cord));

            foreach(PlacedSign _selectedSign in _inHitbox)
            {
                if (!this.signsOnRoadRead.Contains(_selectedSign))
                {
                    _selectedSign.Sign.Read(this);
                    this.signsOnRoadRead.Add(_selectedSign);
                }
            }
        }

        public void NeedToBrake()
        {
            double _distance = RoadMath.Distance(this.locationGoal.X, this.locationGoal.Y, this.vehicle.locationX, this.vehicle.locationY);

            double _distanceToRoadSwitch = RoadMath.DistanceToLastLanePoint(this.currentLanePointIndex, vehicle.currentLane.points);

            double _brakeDistance = vehicle.brakeDistance + vehicle.speed;


            if (this.vehicle.speed == 0)
            {
                this.brakeToZero = false;
            }
            if (_distance < _brakeDistance)
            {
                this.isBraking = true;
            }
            if (this.brakeToZero)
            {
                this.isBraking = true;
            }
            if(_distanceToRoadSwitch < _brakeDistance && navigator.currentPath.NextLaneIndex != null && !navigator.currentPath.NextLaneIndex.Contains(vehicle.currentLane.thisLane))
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

            if (navigator.currentPath.NextLaneIndex != null)
            {
                if (navigator.currentPath.NextLaneIndex.Any())
                {
                    if (!navigator.currentPath.NextLaneIndex.Contains(_laneNum))
                    {
                        this.wantsToSwitch = true;
                    }
                }
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

            if (navigator.currentPath.NextLaneIndex != null)
            {

                if (navigator.currentPath.NextLaneIndex.Any())
                {
                    int _indexDiff = Math.Abs(this.currentLanePointIndex - navigator.currentPath.NextLaneIndex.First());
                    int _indexDiffSide = Math.Abs(this.currentLanePointIndex - navigator.currentPath.NextLaneIndex.First() + _side);

                    if (_indexDiff < _indexDiffSide)
                    {
                        return false;
                    }
                }

                if (navigator.currentPath.NextLaneIndex.Any())
                {
                    if (!navigator.currentPath.NextLaneIndex.Contains(_goalLaneIndex) && navigator.currentPath.NextLaneIndex.Contains(this.currentLanePointIndex))
                    {
                        return false; //YOU ARE NOT MOVING CORRECTLY
                    }
                }
            }

            if(_goalLaneIndex < 0 || _goalLaneIndex > this.vehicle.currentRoad.Drivinglanes.Count() - 1)
            {
                return false; //LANE IS OUT OF INDEX
            }

            if(this.vehicle.currentRoad.Type == "Curved" || this.vehicle.currentRoad.Type == "Cross")
            {
                return false; //CARS CANNOT SWITCH WHILE ON CURVED ROADS
            }

            Lane _goalLane = vehicle.currentRoad.Drivinglanes[_goalLaneIndex];

            if(_goalLane.flipped != this.vehicle.currentLane.flipped)
            {
                return false; //LANE GOES IN THE OPPOSITE DIRECTION
            }

            if(this.currentLanePointIndex < 50)
            {
                return false; //SWITCH IS HAPPENING TOO CLOSE TO BEGINNING OF THE ROAD
            }

            List<BetterVehicle> allVehicles = BetterVehicleTest.vehiclelist;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll(x => x.currentLane.thisLane == _goalLane.thisLane && x.currentRoad == this.vehicle.currentRoad);

            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLanePointIndex - x.vehicleAI.lanePointsMovePerTick - 5 <= this.currentLanePointIndex && x.vehicleAI.currentLanePointIndex + x.vehicleAI.lanePointsMovePerTick + 5> this.currentLanePointIndex))
            {
                //Console.WriteLine("there is a car in the way!"); 
                return false; //THERE IS A CAR IN THE WAY
            }

            return true;
        }

        private void SwitchLanes()
        {
            this.currentLanePointIndex = this.vehicle.currentLane.points.FindIndex(x => x.cord == this.goal.cord);
            this.wantsToSwitch = false;
            Lane _laneToGo = CheckSwitchLane();

            if(_laneToGo == null)
            {
                //Nolanes to switch too
                return;
            }

            this.origin = this.goal;
            this.currentLanePointIndex++;
            this.goal = _laneToGo.points[this.currentLanePointIndex];

            this.lanePointDistance = RoadMath.Distance(vehicle.locationX, vehicle.locationY, goal.cord.X, goal.cord.Y);
            this.vehiclePointDistance = this.lanePointDistance;

            this.CurrentLaneIndex = _laneToGo.thisLane;

            this.vehicle.currentLane = this.vehicle.currentRoad.Drivinglanes[this.CurrentLaneIndex - 1];

            this.SteerWheel(RoadMath.CalculateAngle(new Point((int)vehicle.locationX, (int)vehicle.locationY), goal.cord)); //Lanes get switched But wrong way!!!
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
            this.currentLanePointIndex++;
            int _index = this.currentLanePointIndex;

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

            this.CheckForSigns();
            this.SteerWheel(origin.degree); // + 180 % 360 ?
        }

        private void SteerWheel(float _angel)
        {
            this.vehicle.currentAngel = _angel;
        }

        public void SetPath() //Naam weiziging
        {
            Random ran = new Random();

            AbstractRoad _currentRoad = navigator.currentPath.road;
            this.CurrentLaneIndex = navigator.currentPath.laneIndex[ran.Next(0, navigator.currentPath.laneIndex.Count() - 1)];
            this.vehicle.SwitchRoad(_currentRoad, this.CurrentLaneIndex);
            this.nextRoad = navigator.nextRoad;

            this.origin = this.vehicle.currentLane.points[0]; //sets the origin point
            this.goal = this.vehicle.currentLane.points[1]; //Dit kan een error krijgen;
            ForceCarLocation(this.origin.cord);

            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.currentLanePointIndex = 0;

            this.SteerWheel(origin.degree);

            //this.locationGoal = _path.Last().Drivinglanes.First().points.Last().cord; //TEMP
            this.locationGoal = new Point(-5000, -5000);

            CheckSwitchLane();

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
            Tuple<int, int> _laneSwap = navigator.currentPath.LaneSwap.Find(x => x.Item1 == this.CurrentLaneIndex); // GAAT FOUT

            if(_laneSwap == null)
            {
                SignalDone();
                return;
            }

            this.CurrentLaneIndex = _laneSwap.Item2;

            navigator.NextPath();

            AbstractRoad _currentRoad = navigator.currentPath.road;

            this.vehicle.SwitchRoad(_currentRoad, this.CurrentLaneIndex);
            this.nextRoad = navigator.nextRoad;

            this.origin = this.goal;
            this.goal = this.vehicle.currentLane.points[1];
            this.currentLanePointIndex = 0;
            this.lanePointDistance = RoadMath.Distance(vehicle.locationX, vehicle.locationY, goal.cord.X, goal.cord.Y);
            this.vehiclePointDistance = this.lanePointDistance;

            float angle = RoadMath.CalculateAngle((float)vehicle.locationX, (float)vehicle.locationY, goal.cord.X, goal.cord.Y);

            //Console.WriteLine("LOCATION: {0} - {1}, GOAL: {2} - {3}", (float)vehicle.locationX, (float)vehicle.locationY, goal.cord.X, goal.cord.Y);
            //Console.WriteLine("THE ANGLE OF THE SWITCH IS: {0}", angle);

            this.signsOnRoadRead.Clear();
            this.SteerWheel(goal.degree);

            CheckSwitchLane();
        }

        public void SignalDone()
        {
            this.toDelete = true;
            this.vehicle.DeleteVehicle(false);
        }

        public void SwitchedPaths()
        {
            //TEMP
        }
    }
}
