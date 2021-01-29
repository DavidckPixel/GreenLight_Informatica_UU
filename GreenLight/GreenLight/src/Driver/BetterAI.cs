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

    //This is the AI class, one of the 2 classes that represents the vehicle driving on the road
    //Since at points in the simulation, there may be over 100 cars on the screen, and if the simulation where to run at max speed
    //It would be alot of calculating, so its very important that the betterAI class is as optimised as we could make it to keep
    //the simulation running smooth. The AI-Class is the class that controls the vehicle, and tells it preciesly what to do.

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

        //When a AI is created, it sets its parameters accordingly to the DriverStat instance that was given to it
        //The DriverStat instance contains its behavioural variables. The constructor also sets the Priority and target speed to
        //the base values

        public BetterAI(DriverStats _stats)
        {
            this.reactionSpeed = _stats.ReactionTime;
            this.followInterval = _stats.FollowInterval;
            this.speedRelativeToLimit = _stats.SpeedRelativeToLimit;
            this.ruleBreakingChance = _stats.RuleBreakingChance;

            ChangeTargetSpeed(targetspeed);
            ChangePriority(2);
        }

        //The InitGPS class is the method that initialized the GPS, the GPS is the tool that deals with pathfinding and the AI uses to navigator the roadSystem
        //It takes a start Node, it will then create a GPS which will find a random path from the startNode to any End nodes

        public void InitGPS(Node _startNode)
        {
            navigator = new BetterGPS(this, _startNode);
            this.SetPath();
        }

        //a Vehicle has an AI, but the AI also controls a vehicle, so then a vehicle is created with a specific ai, it will first tell the ai, so it knows which
        // vehicle its controlling. This is also the place where the DriverProfile is created, the driverProfile is the instance that stores most of the vehicles
        //Data which is then inturn used by the data collector.

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

        //The update function is the function that is called every Tick, to keep the code optimised it needs to calculate as little as possible every tick, only the neccessary things
        //When a vehicle is marked for deletion is will also instantly return here and execute no code, this is also the case when a vehicle is marked for HardStop (incase the simulation is paused)
        //After this is will execute its code in a specific order, it will first calculate its distance to all the other cars and determin whether it should brake, continue driving or switch lanes
        //if the AI is marked that it wants to switch lanes, it will then proceed to call the switchLane function.
        //It then checks if there is an intersection coming up and a method that deals with the intersection rule
        //It then calls a function that will set the Vehicle to braking incase neccessary
        //After this it must also check if it wants to accelerate or not
        //Finally it updates the profile(which is to say: collect certain data)

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

        //When the AI passes a speed sign, its target speed is changed to whatever the sign tells it
        //Since AI can have a variable that can tell it how much it needs to be over the speedlimit
        //it will add this to the target speed;

        public void ChangeTargetSpeed(double _speed)
        {
            this.targetspeed = Math.Abs((_speed + this.speedRelativeToLimit) / 10);
        }

        //When the AI passes a YIELD or PRIORITY sign, it must change its internal priority to whatever
        //the sign tells it. However since there is a change that the vehicle might brake rules and thus ignore the priority change, or increase its own
        //priority even higher then allowed. This all depends on its rulebraking chance

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

        //this function returns a bool whether or not the car should brake a rule, it also contains a multiplier incase you want the car to have a
        //Additional chance of braking a rule

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

        //Calculates some data for the profile

        private void UpdateProfile()
        {
            if (this.isBraking)
            {
                profile.AddBreakTick();
            }
            profile.CalculateFuel(this.vehicle.speed);
        }

        //In the DistanceToCars() method, the AI first finds all the other AI currently on the same road and drivniglane as himself.
        //for all these cars in then checks how far they are away. It also calculates how many lanepoints the car needs to brake and slow down, thus determining its follow distance
        //If the AI finds it is getting too close to a car it set its internal closeToCars value to true, it also sets the wantsToSwitch value to true, telling the AI
        //that it would prefer to switch lanes to pass the slower car.

        public void DistanceToCars()
        {
            closeToCars = false;

            List<BetterVehicle> allVehicles = General_Form.Main.SimulationScreen.Simulator.vehicleController.vehicleList;
            List<BetterVehicle> vehiclesOnLane = allVehicles.FindAll((x => x.currentLane == this.vehicle.currentLane && x.currentRoad == this.vehicle.currentRoad));

            int _distance = RoadMath.LanePointsInDistance(this.vehicle.brakeDistance + 20, this.currentLanePointIndex, this.vehicle.currentLane.points);

            _distance = _distance == 0 ? 1 : _distance;

            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLanePointIndex > this.currentLanePointIndex && x.vehicleAI.currentLanePointIndex < this.currentLanePointIndex  + _distance))
            {
                this.wantsToSwitch = true;
                this.closeToCars = true;

                if (this.nextRoad != null)
                {

                    if ((this.nextRoad.roadtype == "Cross" && this.vehicle.currentLane.points.Count - this.currentLanePointIndex < 10) || this.currentCrossRoadSide != null) 
                    {
                        this.wantsToSwitch = false;
                    }
                }
            }
        }

        //This is an old CrossRoadRule function that determined whether or not the car was allowed to drive
        //It is very complex and tries to use clever tricks to quickly forfill its purpose. this however makes
        //it very complicated and difficult to work with, its fun to read through and try to understand
        //but no longer used due to critical errors.

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

        //This method checks to see if there is a crossRoad coming up, if a crossroad is indeed closer then 60 lanepoints
        //it checks and sets from which CrossRoadSide the car is approaching.
        //if a side is found (which should always happen) its parameters and internal values are set that are used later on during
        //the crossing.

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
                    Log.Write("Error 2001 : A vehicle cannot approach from 2 sides at the same time");
                }

                CrossRoadSide _crossRoadSide = _crossRoadSideL.First();

                _crossRoadSide.status = true;
                _crossRoadSide.aiOnSide.Add(this);

                this.currentCrossRoad = _temproad;

                this.currentCrossRoadSide = _crossRoadSide;
                this.startedCrossing = false;
            }
        }

        //When a vehicle leaves the crossRoad, it must signal to the crossroad and the crossroad sides that it has left so other cars can pass
        //This method also resets all the AI's internal parameters regarding crossRoads

        private void LeavingCrossRoadSide()
        {
            if(currentCrossRoadSide == null)
            {
                Log.Write("Error 2002: The Vehicle is leaving the crossroad, but never found a crossRoadSide it was approaching from");
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
                    this.currentCrossRoadSide.priorityLevel = 2;
                }
            }

            this.currentCrossRoadSide.aiDriving--;

            if (this.currentCrossRoadSide.aiDriving <= 0)
            {
                this.currentCrossRoadSide.driving = false;
            }
            this.currentCrossRoadSide = null;
            this.currentCrossRoad = null;
            this.startedCrossing = false;
        }
        
        //This method is used to check if the vehicle is currently inrange of a sign. It checks whether the current vehicle location is inside the hitbox of a sign that is on its road
        //It then checks if the sign is in the same direction as he is drivnig, if this is the case, the sign is read
        //since we want signs to only be read once, we add this sign to the read sign list, so it will not be read again.


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

        //The NeedToBrake method is the method that sets the  isBraking variable
        //it checks a bunch of internal variables and behaves accordingly

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

        //This method is used to find a lane the car can switch too, when one is found, it then returns the lane.
        //When switching lanes a bunch of things need to be taking into account
        //It also needs to check whether or the lane Below (1) or above(-1) is actually available for switching and
        //will not make vehicles collide. This method will always prefer to switch up if possible.

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

        //This method is used to check per side if the lane is available. it first needs to check whether or not these lanes actually exist and are
        //Driving the same way as him. It also needs to check whether or not it is forced onto a lane by the navigator. And finally it checks whether
        //or not there is a car in the way. When a car is not able to switch to this lane, it returns false.

        private bool LaneSideAvailable(int _side) //can be 1 or -1
        {
            int _goalLaneIndex = this.vehicle.currentLane.thisLane + _side - 1;


            if (navigator.currentPath.NextLaneIndex.TrueForAll(x => x <= (this.CurrentLaneIndex)))
            {
                if (_side == 1)
                {
                    return false;
                }
            }

            if (navigator.currentPath.NextLaneIndex.TrueForAll(x => x >= (this.CurrentLaneIndex)))
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

            if (vehiclesOnLane.Any(x => x.vehicleAI.currentLanePointIndex - x.vehicleAI.lanePointsMovePerTick - 20 <= this.currentLanePointIndex && x.vehicleAI.currentLanePointIndex + x.vehicleAI.lanePointsMovePerTick + 20 > this.currentLanePointIndex))
            {
                return false; //THERE IS A CAR IN THE WAY
            }

            return true;
        }

        //This is the method that is called when the car wants to switch lane, and then sees if this is possible using the other 2 previously described methods
        //Incase a Lane is found that it can switch too, it sets all the parameters accordingly and switches.

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

            this.closeToCars = false;

            this.SteerWheel(RoadMath.CalculateAngle(new Point((int)vehicle.locationX, (int)vehicle.locationY), goal.cord)); //Lanes get switched But wrong way!!!
            DistanceToCars();
        }

        //This method checks whether or not the car can accelerate

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

        //To keep the car driving on a the road, it drives from lane point to lane point. The distance between these lane points can sometimes be smaller the one, so a car can pass multiple lanepoints
        //in one tick. When a lanePoint is passed it will find the nextLane point it needs to drive too, when its at the end of the lanePoint list, it will switch roads to the next road

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

        //This method is called when the AI wants the car to turn into a different direction

        public void SteerWheel(float _angel)
        {
            this.vehicle.currentAngle = _angel;
        }

        //Initial function that sets up the first path and road for the car to drive.

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

        //To keep the car on the lane, sometimes we need to force its position, we prefer to use this as little as possible

        private void ForceCarLocation(Point _p)
        {
            this.vehicle.locationX = _p.X;
            this.vehicle.locationY = _p.Y;
        }

        //When a road is switched, it will find the index of the next drivinglane its current driving lane connects to, since crossroads can have many more drivinglanes when normal roads
        //the translatin is not always easy and calls upon the GPS to help. if the currentroad it was driving on was a Crossroad, it will call the LeaveCrossRoadmethod
        //Incase the nextRoad is a crossroad and the crossroad was occupied, we do not want it to switchRoads, instead it should stop, so we set its speed to 0 and force its location.
        //if all is well, the road is switched and similar to the SetPath, the next road is selected as its current road and all the internal variables are set accordingly

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


            if (this.navigator.nextRoad != null && this.navigator.nextRoad.roadtype == "Cross" && this.crossRoadOccupied)
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

        //When a car is done it sets its internal values to be delete, it then tells the simulationController that it is save to delete it.

        public void SignalDone()
        {
            this.toDelete = true;
            this.vehicle.DeleteVehicle(false);
        }

        //This is the reworked method that checks for the crossRoad rules, the staps it takes are the following: 
        // 1) if im currently ontop of a crossroad, just keep driving, we do not want cars to stop in the middle of it
        // 2) If i already started crossing, just continue driving
        // 3) if Im first in the list of cars waiting for the intersection for my side, and my side it set to driving: start driving:
        // 4) If the amount of lanepoints needed for me to brake is bigger then my distance to the crossroad, no need to check the rules yet
        // 5) Check for possible errors that should never occur and deal with them accordingly.
        // 6) Gather all the information on the crossRoad and other crossroad sides
        // 7) See from which side to which side I want to drive
        // 8) Apply the general driving rules dependend on the information gathered in step 6 and 7
        // 9) Check if there is currently no car driving yet the crossroad is empty: there is a deadlock, the cars at the top may now drive
        // 10) If im allowed to drive, and im the first Car inLine to drive. Set my values to driving, also tell the crossroad that there is currently someone from my side driving
        // 11) Incase I was not allowed to drive, tell the vehicle it needs to brake.    

        private void ReworkedCrossRoadCheck()
        {
            if (this.currentCrossRoad == null || this.vehicle.currentRoad.roadtype == "Cross") 
            {
                this.crossRoadOccupied = false;
                return; // NO CURRENTCROSSROAD
            }

            if (this.startedCrossing)            {

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
                Log.Write("error 2003: the car is entering a crossroad, yet never found a CrossRoadSide");
                return;
            }

            string _side = this.currentCrossRoadSide.side;
            int _laneSwap = this.navigator.currentPath.LaneSwap.First().Item2;
            CrossRoadSide _tempside;
            CrossRoad _nextRoad = (CrossRoad)this.nextRoad;
            _tempside = _nextRoad.sides.ToList().Find(x => x.hitbox.Contains(navigator.nextRoad.Drivinglanes[navigator.currentPath.LaneSwap.First().Item2 - 1].points.Last().cord));

            if (_tempside == null)
            {
                Log.Write("Warning you should never be here! ID 2");
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

            if (_side == "Top")
            {
                if(leftDriving || rightDriving || bottomDriving)
                {
                    _allowDrive = false;
                }

                if(_goalSide == "Right")
                {
                    if((leftStatus && leftPriority >= this.priority) || (bottomStatus && bottomPriority >= this.priority) || (rightStatus && rightPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else if(_goalSide == "Bottom")
                {
                    if((leftStatus && leftPriority >= this.priority) || (rightStatus && rightPriority > this.priority))// || (bottomStatus && bottomPriority > this.priority))
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
                    Log.Write("ERROR IN GOALSIDE, IN CROSSROAD RULES, YOU SHOULD NEVER BE HERE!!");
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
                    if((bottomStatus && bottomPriority >= this.priority) || (topStatus && topPriority > this.priority)) //|| (rightStatus && rightPriority > this.priority))
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
                    if((rightStatus && rightPriority >= this.priority) || (bottomStatus && bottomPriority >= this.priority) || (topStatus && topPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else
                {
                    Log.Write("ERROR IN GOALSIDE, IN CROSSROAD RULES, YOU SHOULD NEVER BE HERE!!");
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
                    if((rightStatus && rightPriority >= this.priority) || (topStatus && topPriority > this.priority)) //|| (leftStatus && leftPriority > this.priority))
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
                    if((rightStatus && rightPriority >= this.priority) || (topStatus && topPriority >= this.priority) || (leftStatus && leftPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else
                {
                    Log.Write("ERROR IN GOALSIDE, IN CROSSROAD RULES, YOU SHOULD NEVER BE HERE!!");
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
                    if((topStatus && topPriority >= this.priority ) || (leftStatus && leftPriority >= this.priority) || (bottomStatus && bottomPriority > this.priority))
                    {
                        _allowDrive = false;
                    }
                }
                else if (_goalSide == "Left")
                {
                    if ((topStatus && topPriority >= this.priority) || (leftStatus && leftPriority > this.priority)) //|| (bottomStatus && bottomPriority > this.priority))
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