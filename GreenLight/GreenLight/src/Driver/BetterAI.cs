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
        public bool[] status = new bool[4] { false, false, false, false };

        public List<AbstractRoad> drivingRoads = new List<AbstractRoad>();
        public int index;

        public bool wantsToSwitch;
        public bool closeToCars;
        public bool crossRoadOccupied;
        private BetterGPS navigator;

        public int lanePointsMovePerTick;

        public int currentLanePointIndex;
        public int CurrentLaneIndex;

        RectHitbox foundHitbox = null;
        public CrossRoad currentCrossRoad = null;
        public CrossRoadSide currentCrossRoadSide = null;
        int crossRoadTimer = 0;

        bool needsToStop = false;
        List<PlacedSign> signsOnRoadRead = new List<PlacedSign>();

        public bool isSwitchingLanes;
        public int switchCount;

        public BetterAI(DriverStats _stats)
        {
            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;

            ChangeTargetSpeed(3);
            ChangePriority(2);
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

            Random ran = new Random();
            int _mood = 1; //can be 0 / 1 / 2
            if(this.ruleBreakingChance > 10 && this.speedRelativeToLimit > 9)
            {
                _mood = 2;
            }
            else if(this.speedRelativeToLimit < -9)
            {
                _mood = 0;
            }
            profile.mood = profile.faceType.speech[_mood][ran.Next(0, profile.faceType.speech[_mood].Count)];

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
            this.targetspeed = Math.Abs(_speed + this.speedRelativeToLimit / 10);
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

            List<BetterVehicle> allVehicles = General_Form.Main.SimulationScreen.Simulator.vehicleController.vehicleList;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll((x => x.currentLane == this.vehicle.currentLane && x.currentRoad == this.vehicle.currentRoad));

            int _distance = RoadMath.LanePointsInDistance(this.vehicle.brakeDistance + 8, this.currentLanePointIndex, this.vehicle.currentLane.points);

            _distance = _distance == 0 ? 1 : _distance;

            //x.vehicleAI.currentLaneIndex - x.vehicleAI.lanePointsMovePerTick - 5 <= this.currentLaneIndex && x.vehicleAI.currentLaneIndex + x.vehicleAI.lanePointsMovePerTick + 5 > this.currentLaneIndex
            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLanePointIndex > this.currentLanePointIndex && x.vehicleAI.currentLanePointIndex < this.currentLanePointIndex  + _distance))
            {
                this.wantsToSwitch = true;
                this.closeToCars = true;

                if (this.nextRoad.roadtype == "Cross" && this.vehicle.currentLane.points.Count - this.currentLanePointIndex < 10) ;
                {
                    this.wantsToSwitch = false;
                }
            }
            else
            {
                this.isBraking = false;
            }
        }

        private void crossRoadRules()
        {
            if (this.nextRoad.roadtype != "Cross" || this.currentCrossRoadSide == null)
            {
                if (this.crossRoadOccupied)
                {
                    Console.WriteLine("WARNING - you should never be here!");
                }
                this.crossRoadOccupied = false;
                return; //NEXT ROAD IS NOT A CROSSROAD, WE DO NOT CARE HERE;
            }

            int Num = RoadMath.LanePointsInDistance(vehicle.brakeDistance, this.currentLanePointIndex, this.vehicle.currentLane.points);

            if (vehicle.currentLane.points.Count() - this.currentLanePointIndex > Num + 2 && this.vehicle.speed > 1) 
            {
                return; //ENOUGH TIME TO BRAKE, NO NEED OT CHEKC NOW
            }

            CrossRoad _nextRoad = (CrossRoad)this.nextRoad;
           

            int _index = _nextRoad.sides.ToList().IndexOf(this.currentCrossRoadSide);

            string dir = ""; //STRAIGHT - LEFT - RIGHT
            int _indexEnd = _nextRoad.sides.ToList().FindIndex(x => x.hitbox.Contains(navigator.nextRoad.Drivinglanes[navigator.currentPath.laneIndex.First()].points.Last().cord));

            int _indexdiff = _index - _indexEnd;
            int _checkIndex1 = 0;
            int _checkIndex2 = 0;
            // 0 = top // 1 = left // 2 = bottom // 3 = right
            bool[] status = new bool[4] { false, false, false, false };

            if (Math.Abs(_indexdiff) == 2)
            {
                dir = "STRAIGHT";
                _index = _index + 1 > 3 ? 0 : _index + 1; //Right

                if (_nextRoad.sides[_index].priorityLevel >= this.priority) // If same Priority or lower priority
                {

                    status[_index] = true; // Look at right


                    _index = _index + 2 > 3 ? _index - 2 : _index + 2; // Left
                    if (_nextRoad.sides[_index].priorityLevel > this.priority) // If lower priority
                    {
                        status[_index] = true; //Look at left
                    }
                }
            }
            else if( _indexdiff == 1 || _indexdiff == -3)
            {
                dir = "LEFT";
                _checkIndex1 = _index + 1 > 3 ? 0 : _index + 1; // Right
                _checkIndex2 = _index + 2 > 3 ? _index - 2 : _index + 2; //Top
                status[_checkIndex2] = true; //Look at top

                if (_nextRoad.sides[_checkIndex1].priorityLevel >= this.priority) // If same Priority or lower priority
                {

                    status[_checkIndex1] = true; //Look at right

                    _index = _index - 1 < 0 ? 0 : _index - 1; // Left
                    if (_nextRoad.sides[_index].priorityLevel > this.priority)
                    {
                        status[_index] = true; //Look at left
                    }
                }
            }
            else if(_indexdiff == -1 || _indexdiff == 3)
            {
                dir = "RIGHT";
                _index = _index - 1 < 0 ? 0 : _index - 1; //Left
                if (_nextRoad.sides[_index].priorityLevel > this.priority)
                {
                    status[_index] = true; //Look at left
                }

            }

            //if ((_side1.status && _side1.priorityLevel > this.priority)|| (_side2.status && _side2.priorityLevel > this.priority) || )
            if (((_nextRoad.sides[0].status && status[0])
                || (_nextRoad.sides[1].status && status[1])
                || (_nextRoad.sides[2].status && status[2])
               || (_nextRoad.sides[3].status && status[3])))
                {
                this.crossRoadOccupied = true; //THERE IS A CAR IN THE WAY;
                    //this.handBreakOn = true;
                }
            else
            {
                this.crossRoadOccupied = false;
            }

            this.status = status;
            //CHECK FIRST AND LAST LANEPOINT, SEE IF THEY ARE ON ONE LINE = STRAIGHT AHEAD;
            //
        }

        private void InCrossRoadRange()
        {
            if (this.nextRoad.roadtype != "Cross" || this.currentCrossRoadSide != null)
            {
                return;

            }
            int pointsTillEnd = this.vehicle.currentLane.points.Count() - 1 - this.currentLanePointIndex;

            if(pointsTillEnd >= 60)
            {
                return;
            }

            if(this.nextRoad.roadtype == "Cross")
            {
                CrossRoad _temproad = (CrossRoad)nextRoad;
                CrossRoadSide _crossRoadSide = _temproad.sides.ToList().Find(x => x.hitbox.Contains(this.vehicle.currentLane.points.Last().cord));

                _crossRoadSide.status = true;
                _crossRoadSide.aiOnSide.Add(this);

                this.currentCrossRoad = _temproad;

                this.currentCrossRoadSide = _crossRoadSide;
            }
        }

        private void LeavingCrossRoadSide()
        {

            this.currentCrossRoadSide.aiOnSide.Remove(this);

            if (!this.currentCrossRoadSide.aiOnSide.Any())
            {
                this.currentCrossRoadSide.status = false;
                this.currentCrossRoadSide.priorityLevel = 0;
            }
            else
            {
                this.currentCrossRoadSide.priorityLevel = this.currentCrossRoadSide.aiOnSide.Max(x => x.priority);
            }
            this.currentCrossRoadSide.driving = false;
            this.currentCrossRoadSide = null;
        }
        

        private void CheckForSigns()
        {
            List<PlacedSign> _signsOnRoad = this.vehicle.currentRoad.Signs;
            List<PlacedSign> _inHitbox = _signsOnRoad.FindAll(x => x.Hitbox.Contains(goal.cord));

            foreach(PlacedSign _selectedSign in _inHitbox)
            {
                if (_selectedSign.flipped == this.vehicle.currentLane.flipped)
                {
                    if (!this.signsOnRoadRead.Contains(_selectedSign))
                    {
                        _selectedSign.Sign.Read(this);
                        this.signsOnRoadRead.Add(_selectedSign);
                    }
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
            if (this.crossRoadOccupied)
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

            int _laneNum = _drivinglane.thisLane;

            if (navigator.currentPath.NextLaneIndex != null)
            {
                if (navigator.currentPath.NextLaneIndex.Any())
                {
                    if (!navigator.currentPath.NextLaneIndex.Any(x => x == _laneNum))
                    {
                        this.wantsToSwitch = true;
                    }
                }
            }

            if (this.isSwitchingLanes)
            {
                return null;
            }

            if (_road.getLanes() == 1)
            {
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


            if (navigator.currentPath.NextLaneIndex.TrueForAll(x => x < (this.CurrentLaneIndex)))
            {
                if (_side == 1)
                {
                    return false;
                }
            }

            //navigator.currentPath.NextLaneIndex.ForEach(x => Console.WriteLine("ALL THE LANE INDEX: " + x));

            if (navigator.currentPath.NextLaneIndex.TrueForAll(x => x > (this.CurrentLaneIndex)))
            {
                if (_side == -1)
                {
                    return false;
                }
            }

            if (!navigator.currentPath.NextLaneIndex.Contains(_goalLaneIndex) && navigator.currentPath.NextLaneIndex.Contains(this.currentLanePointIndex))
            {
                Console.WriteLine("YOU ARE ON THE CORRECT LANE, DO NOT SWITCH!");
                return false; //YOU ARE NOT MOVING CORRECTLY
            }

            if (_goalLaneIndex < 0 || _goalLaneIndex > this.vehicle.currentRoad.Drivinglanes.Count() - 1)
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
            this.wantsToSwitch = false;

            this.currentLanePointIndex = this.vehicle.currentLane.points.FindIndex(x => x.cord == this.goal.cord);
            Lane _laneToGo = CheckSwitchLane();

            if(_laneToGo == null)
            {
                //Nolanes to switch too
                return;
            }

            this.wantsToSwitch = false;

            if (navigator.currentPath.NextLaneIndex.Any())
            {
                if (!navigator.currentPath.NextLaneIndex.Any(x => x == _laneToGo.thisLane))
                {
                    wantsToSwitch = true;
                }
            }

            this.origin = this.goal;
            this.currentLanePointIndex++;
            if(this.currentLanePointIndex >= _laneToGo.points.Count())
            {
                this.currentLanePointIndex = _laneToGo.points.Count() - 1;
            }

            this.isSwitchingLanes = true;

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

            this.isSwitchingLanes = false;

            if (_index >= _points.Count() - 2)
            {
                this.SwitchRoad();
                return;
            }

            this.origin = this.goal;
            //Console.WriteLine("INDEX: {0} -- AMOUNT OF POINTS: {1}",)
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
            List<Tuple<int, int>> _laneSwapList = navigator.currentPath.LaneSwap.FindAll(x => x.Item1 == this.CurrentLaneIndex); // GAAT FOUT
            Tuple<int, int> _laneSwap;

            if (_laneSwapList.Any())
            {
                Random ran = new Random();
                _laneSwap = _laneSwapList[ran.Next(0, _laneSwapList.Count())];
            }
            else
            {
                _laneSwap = navigator.currentPath.LaneSwap.First();
            }

            if (this.vehicle.currentRoad.roadtype == "Cross")
            {
                Console.WriteLine("Leaving Crossroad NOW!");
                this.LeavingCrossRoadSide();
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
    }
}
