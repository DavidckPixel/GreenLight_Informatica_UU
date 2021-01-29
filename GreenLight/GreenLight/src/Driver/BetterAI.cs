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

        public double targetspeed = 3;
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
        public bool ignoreOccupied;
        
        BetterGPS navigator;
        public bool startedCrossing;

        public int lanePointsMovePerTick;

        public int currentLanePointIndex;
        public int CurrentLaneIndex;

        public CrossRoad currentCrossRoad = null;
        public CrossRoadSide currentCrossRoadSide = null;
        int crossRoadTimer = 0;

        public bool lightIsRed = false;
        public bool lightIsGreen = false;
      
        List<PlacedSign> signsOnRoadRead = new List<PlacedSign>();

        public bool isSwitchingLanes;
        public int switchCount;

        public string Goal = "";

        public BetterAI(DriverStats _stats)
        {
            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;

            ChangeTargetSpeed(targetspeed);
            ChangePriority(2);
        }

        public void InitGPS(Node _startNode)
        {
            navigator = new BetterGPS(this, _startNode);
            this.SetPath();
        }

        public void SetVehicle(BetterVehicle _vehicle, Node _startNode)
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

            InitGPS(_startNode);
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
            //crossRoadRules();
            ReworkedCrossRoadCheck();

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

            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLanePointIndex > this.currentLanePointIndex && x.vehicleAI.currentLanePointIndex < this.currentLanePointIndex  + _distance))
            {
                this.wantsToSwitch = true;
                this.closeToCars = true;

                if (this.nextRoad != null)
                {

                    if (this.nextRoad.roadtype == "Cross" && this.vehicle.currentLane.points.Count - this.currentLanePointIndex < 10) ;
                    {
                        this.wantsToSwitch = false;
                    }
                }
            }
        }

        private void CrossRoadRules()
        {
            if (this.nextRoad == null)
            {
                return;
            }
            if (this.nextRoad.roadtype != "Cross" || this.currentCrossRoadSide == null)
            {
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

            if (((_nextRoad.sides[0].status && status[0])
                || (_nextRoad.sides[1].status && status[1])
                || (_nextRoad.sides[2].status && status[2])
               || (_nextRoad.sides[3].status && status[3])))
                {
                this.crossRoadOccupied = true; //THERE IS A CAR IN THE WAY;
                }
            else
            {
                this.crossRoadOccupied = false;
            }

            this.status = status;
        }

        private void InCrossRoadRange()
        {
            if (this.nextRoad == null)
            {
                return;
            }
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
                List<CrossRoadSide> _crossRoadSideL = _temproad.sides.ToList().FindAll(x => x.hitbox.Contains(this.vehicle.currentLane.points.Last().cord));

                if(_crossRoadSideL.Count > 1)
                {
                    Console.WriteLine("WEL WE FOUND YOUR PROBLEM!!!!!!!!!!!!!!!!!!!!!!!!");
                }

                CrossRoadSide _crossRoadSide = _crossRoadSideL.First();

                _crossRoadSide.status = true;
                _crossRoadSide.aiOnSide.Add(this);

                this.currentCrossRoad = _temproad;

                this.currentCrossRoadSide = _crossRoadSide;
                this.startedCrossing = false;
            }
            
        }

        private void LeavingCrossRoadSide()
        {
            if(currentCrossRoadSide == null)
            {
                Console.WriteLine("THE FUCKKKKKKK!!! WAAROM?!?!");
                return;
            }
            
            this.currentCrossRoadSide.aiOnSide.Remove(this);

            if (!this.currentCrossRoadSide.aiOnSide.Any())
            {
                this.currentCrossRoadSide.status = false;
                this.currentCrossRoadSide.priorityLevel = 2;
            }
            else
            {
                this.currentCrossRoadSide.priorityLevel = this.currentCrossRoadSide.aiOnSide.First().priority;

                if(this.currentCrossRoadSide.aiOnSide.Any() && this.currentCrossRoadSide.priorityLevel == 0)
                {
                    Console.WriteLine("YOU SHOULD RELALY NEVER BE HERE PLEASE DONT COME HERE THIS IS SO WEIRD");

                    this.currentCrossRoadSide.priorityLevel = 2;
                }
            }

            this.currentCrossRoadSide.aiDriving--;

            if (this.currentCrossRoadSide.aiDriving <= 0)
            {
                Console.WriteLine("NO MORE CARS FROM THIS SIDE");
                this.currentCrossRoadSide.driving = false;
            }
            this.currentCrossRoadSide = null;
            this.currentCrossRoad = null;
            this.startedCrossing = false;
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
            double _distanceToRoadSwitch = RoadMath.DistanceToLastLanePoint(this.currentLanePointIndex, vehicle.currentLane.points);
            double _brakeDistance = vehicle.brakeDistance;

            if (this.vehicle.speed == 0)
            {
                this.brakeToZero = false;
            }
            if (_distanceToRoadSwitch < _brakeDistance && navigator.currentPath.NextLaneIndex != null && !navigator.currentPath.NextLaneIndex.Contains(vehicle.currentLane.thisLane))
            {
                this.isBraking = true;
            }
            if (this.lightIsRed)
            {
                this.isBraking = true;
                this.ignoreOccupied = false;
            }
            if (this.lightIsGreen)
            {
                this.isBraking = false;
                this.ignoreOccupied = true;
            }
            if (this.crossRoadOccupied || this.brakeToZero || closeToCars)
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

            if (navigator.currentPath.NextLaneIndex.TrueForAll(x => x > (this.CurrentLaneIndex)))
            {
                if (_side == -1)
                {
                    return false;
                }
            }

            if (!navigator.currentPath.NextLaneIndex.Contains(_goalLaneIndex) && navigator.currentPath.NextLaneIndex.Contains(this.currentLanePointIndex))
            {
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
            if (this.vehicle.speed >= this.targetspeed || this.vehicle.speed >= this.vehicle.topspeed)
            {
                this.isAccelerating = false;
            }
            else
            {
                this.isAccelerating = true;
            }
        }

        public void SwitchLanePoints()
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
            this.goal = _points[_index + 1];
            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);

            this.vehiclePointDistance = this.lanePointDistance;

            if(crossRoadTimer > 0)
            {
                crossRoadTimer--;
            }

            this.CheckForSigns();
        }

        public void SteerWheel(float _angel)
        {
            this.vehicle.currentAngle = _angel;
        }

        public void SetPath()
        {
            Random ran = new Random();

            AbstractRoad _currentRoad = navigator.currentPath.road;
            int _random = ran.Next(0, navigator.currentPath.laneIndex.Count());
            this.CurrentLaneIndex = navigator.currentPath.laneIndex[_random];
            this.vehicle.SwitchRoad(_currentRoad, this.CurrentLaneIndex);
            this.nextRoad = navigator.nextRoad;

            this.origin = this.vehicle.currentLane.points[0]; //sets the origin point
            this.goal = this.vehicle.currentLane.points[1];
            ForceCarLocation(this.origin.cord);

            this.lanePointDistance = RoadMath.Distance(origin.cord, goal.cord);
            this.vehiclePointDistance = this.lanePointDistance;

            this.currentLanePointIndex = 0;

            this.SteerWheel(origin.degree);
            this.locationGoal = new Point(-5000, -5000);

            CheckSwitchLane();
        }

        private void ForceCarLocation(Point _p)
        {
            this.vehicle.locationX = _p.X;
            this.vehicle.locationY = _p.Y;
        }

        public void SwitchRoad()
        {
            List<Tuple<int, int>> _laneSwapList = navigator.currentPath.LaneSwap.FindAll(x => x.Item1 == this.CurrentLaneIndex);
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
                this.LeavingCrossRoadSide();
            }


            if (this.navigator.nextRoad.roadtype == "Cross" && this.crossRoadOccupied)
            {
                this.vehicle.speed = 0;
                this.ForceCarLocation(this.goal.cord);

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

            this.signsOnRoadRead.Clear();
            this.SteerWheel(goal.degree);

            CheckSwitchLane();
        }

        public void SignalDone()
        {
            this.toDelete = true;
            this.vehicle.DeleteVehicle(false);
        }

        private void ReworkedCrossRoadCheck()
        {
            if (this.currentCrossRoad == null || this.vehicle.currentRoad.roadtype == "Cross") 
            {
                this.crossRoadOccupied = false;
                return; // NO CURRENTCROSSROAD
            }

            if (this.startedCrossing)
            {
               //Console.WriteLine("IS ON CROSSROAD");
                this.crossRoadOccupied = false;
                return; //YOU ARE ALREADY ON THE CROSSROAD
            }

            if(this.currentCrossRoadSide.driving == true && this.currentCrossRoadSide.aiOnSide.First() == this)
            {
                this.crossRoadOccupied = false;
                return; //YOU ARE ALREADY ON THE CROSSROAD
            }

            int Num = RoadMath.LanePointsInDistance(vehicle.brakeDistance, this.currentLanePointIndex, this.vehicle.currentLane.points);

            Num = Num <= 0 ? 1 : Num;

            if (vehicle.currentLane.points.Count() - this.currentLanePointIndex >= Num + 10 && this.vehicle.speed != 0)
            {
                this.crossRoadOccupied = false;
                return; //ENOUGH TIME TO BRAKE, NO NEED OT CHEKC NOW
            }

            if (this.currentCrossRoadSide == null)
            {
                Console.WriteLine("Warning you should never be here! ID 1");
                return;
            }

            string _side = this.currentCrossRoadSide.side;

            int _laneSwap = this.navigator.currentPath.LaneSwap.First().Item2;
            CrossRoadSide _tempside; //= this.currentCrossRoad.sides.First(x => x.hitbox.Contains(navigator.nextRoad.Drivinglanes[_laneSwap - 1].points.Last().cord));

            CrossRoad _nextRoad = (CrossRoad)this.nextRoad;

            _tempside = _nextRoad.sides.ToList().Find(x => x.hitbox.Contains(navigator.nextRoad.Drivinglanes[navigator.currentPath.LaneSwap.First().Item2 - 1].points.Last().cord));

            if (_tempside == null)
            {
                Console.WriteLine("Warning you should never be here! ID 2");
                return; 
            }

            string _goalSide = _tempside.side;
            this.Goal = _goalSide;

            bool _allowDrive = true;

            bool leftStatus = this.currentCrossRoad.sides.First(x => x.side == "Left").status;
            bool rightStatus = this.currentCrossRoad.sides.First(x => x.side == "Right").status;
            bool topStatus = this.currentCrossRoad.sides.First(x => x.side == "Top").status;
            bool bottomStatus = this.currentCrossRoad.sides.First(x => x.side == "Bottom").status;

            bool leftDriving = this.currentCrossRoad.sides.First(x => x.side == "Left").driving;
            bool rightDriving = this.currentCrossRoad.sides.First(x => x.side == "Right").driving;
            bool topDriving = this.currentCrossRoad.sides.First(x => x.side == "Top").driving;
            bool bottomDriving = this.currentCrossRoad.sides.First(x => x.side == "Bottom").driving;

            int leftPriority = this.currentCrossRoad.sides.First(x => x.side == "Left").priorityLevel;
            int rightPriority = this.currentCrossRoad.sides.First(x => x.side == "Right").priorityLevel;
            int topPriority = this.currentCrossRoad.sides.First(x => x.side == "Top").priorityLevel;
            int bottomPriority = this.currentCrossRoad.sides.First(x => x.side == "Bottom").priorityLevel;

            // You want to go your own right : Check if your left side has priority over u: otherwise go
            // You want to go your UP : Check right side & if left has priority
            // you want to go

            //Console.WriteLine("LEFT =  status: {0} - Driving : {1} - Priority {2}", leftStatus, leftDriving, leftPriority);
            //Console.WriteLine("TOP =  status: {0} - Driving : {1} - Priority {2}", topStatus, topDriving, topPriority);
            //Console.WriteLine("RIGHT =  status: {0} - Driving : {1} - Priority {2}", rightStatus, rightDriving, rightPriority);
            //Console.WriteLine("BOTTOM =  status: {0} - Driving : {1} - Priority {2}", bottomStatus, bottomDriving, bottomPriority);

            if (_side == "Top")
            {
                if(leftDriving || rightDriving || bottomDriving)
                {
                    _allowDrive = false;
                }

                if(_goalSide == "Right")
                {
                    if((leftStatus && leftPriority <= this.priority) || (bottomStatus && bottomPriority <= this.priority) || (rightStatus && rightPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else if(_goalSide == "Bottom")
                {
                    if((leftStatus && leftPriority <= this.priority) || (rightStatus && rightPriority > this.priority))// || (bottomStatus && bottomPriority > this.priority))
                    {
                        _allowDrive = false;
                    }

                    if (bottomDriving)
                    {
                        _allowDrive = false;
                    }
                }
                else if(_goalSide == "Left")
                {
                    if (rightStatus && rightPriority > this.priority)
                    {
                        _allowDrive = false;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR IN GOALSIDE, IN CROSSROAD RULES, YOU SHOULD NEVER BE HERE!!");
                }

                if (rightDriving)
                {
                    _allowDrive = false;
                }
            }
            else if(_side == "Left")
            {
                if (rightDriving || topDriving || bottomDriving)
                {
                    _allowDrive = false;
                }

                if (_goalSide == "Right")
                {
                    if((bottomStatus && bottomPriority <= this.priority) || (topStatus && topPriority > this.priority)) //|| (rightStatus && rightPriority > this.priority))
                    {
                        _allowDrive = false;
                    }

                    if (rightDriving)
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Bottom")
                {
                    if(topStatus && topPriority > this.priority)
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Top")
                {
                    //RIGHT BOTTOM *TOP
                    if((rightStatus && rightPriority <= this.priority) || (bottomStatus && bottomPriority <= this.priority) || (topStatus && topPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR IN GOALSIDE, IN CROSSROAD RULES, YOU SHOULD NEVER BE HERE!!");
                }

                if (topDriving)
                {
                    _allowDrive = false;
                }
            }
            else if(_side == "Bottom")
            {
                if (leftDriving || rightDriving || topDriving)
                {
                    _allowDrive = false;
                }

                if (_goalSide == "Right")
                {
                    if(leftStatus && leftPriority > this.priority)
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Top")
                {
                    if((rightStatus && rightPriority <= this.priority) || (topStatus && topPriority > this.priority)) //|| (leftStatus && leftPriority > this.priority))
                    {
                        _allowDrive = false;
                    }

                    if (topDriving)
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Left")
                {
                    if((rightStatus && rightPriority <= this.priority) || (topStatus && topPriority <= this.priority) || (leftStatus && leftPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR IN GOALSIDE, IN CROSSROAD RULES, YOU SHOULD NEVER BE HERE!!");
                }

                if (leftDriving)
                {
                    _allowDrive = false;
                }
            }
            else if(_side == "Right")
            {
                if (leftDriving || topDriving || bottomDriving)
                {
                    _allowDrive = false;
                }

                if (_goalSide == "Top")
                {
                    if(bottomStatus && bottomPriority < this.priority)
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Bottom")
                {
                    //left top *bottom
                    if((topStatus && topPriority <= this.priority ) || (leftStatus && leftPriority <= this.priority) || (bottomStatus && bottomPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Left")
                {
                    if ((topStatus && topPriority <= this.priority) || (leftStatus && leftPriority > this.priority)) //|| (bottomStatus && bottomPriority > this.priority))
                    {
                        _allowDrive = false;
                    }

                    if (leftDriving)
                    {
                        _allowDrive = false;
                    }
                }

                if (bottomDriving)
                {
                    _allowDrive = false;
                }
            }

            if (_side == "Top")
            {
                if (!topDriving && topStatus && !bottomDriving && bottomStatus && !rightDriving && rightStatus && !leftDriving && leftStatus)
                {
                    _allowDrive = true;
                }
            }
            if (_side == "Right")
            {
                if (!topDriving && !topStatus && !bottomDriving && bottomStatus && !rightDriving && rightStatus && !leftDriving && leftStatus)
                {
                    _allowDrive = true;
                }
            }
            if (_side == "Bottom")
            {
                if (!topDriving && !topStatus && !bottomDriving && bottomStatus && !rightDriving && !rightStatus && !leftDriving && leftStatus)
                {
                    _allowDrive = true;
                }
            }
            if (_side == "Left")
            {
                if (!topDriving && !topStatus && !bottomDriving && !bottomStatus && !rightDriving && !rightStatus && !leftDriving && leftStatus)
                {
                    _allowDrive = true;
                }
            }

            if (_allowDrive)
            {       //THERE IS A CAR IN THE WAY; 
                //this.crossRoadOccupied = true;
                //this.handBreakOn = true;
                if (this.currentCrossRoadSide.aiOnSide.First() == this && (this.vehicle.currentLane.points.Count - this.currentLanePointIndex < 1)); 
                {
                    this.startedCrossing = true;
                    this.currentCrossRoadSide.driving = true;
                    this.currentCrossRoadSide.aiDriving++;
                }
            }
            else
            {
                this.crossRoadOccupied = true;
                this.startedCrossing = false;
            }
        }
    }
}