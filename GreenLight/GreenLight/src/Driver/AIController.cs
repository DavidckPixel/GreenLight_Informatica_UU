using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GreenLight
{

    //The AIController deals with anything AI based, it also holds the list of the currently selected DriverStats
    //This is the controller that is responsible for creating the AI with the correct stats

    public class AIController : EntityController
    {
        public DriverStats selectedAI;
        public List<DriverStats> availableDriverStats = new List<DriverStats>();

        public override void Initialize()
        {
            Log.Write("Initializing the AIController");
        }

        public AIController()
        {

        }

        //this Method creates and returns a AU

        public BetterAI GetDriver()
        {
            DriverStats _stats = getDriverStats();

            return new BetterAI(_stats);
        }

        //This method gets from the userInterface which AIstats are all currently selected, it then puts all of this in a list, incase none
        //where selected, all are selected

        public void initDriverList()
        {
            this.availableDriverStats.Clear();

            List<string> availableVehicleStatsString = General_Form.Main.UserInterface.SimSDM.Selection_box.elementsSelected;
            availableVehicleStatsString.ForEach(x => this.availableDriverStats.Add(getDriverStat(x)));
            this.availableDriverStats.RemoveAll(x => x == null);

            Log.Write("AMOUNT OF STATS AI LODED: " + this.availableDriverStats.Count());

            if (!this.availableDriverStats.Any())
            {
                this.availableDriverStats = AITypeConfig.aiTypes;
            }
        }

        //This method will return a random driver stat from the available driver stats list
        //when returning a random one it will also take into account the occurunce of each driverStat

        public DriverStats getDriverStats(DriverStats _stats = null)
        {
            if (_stats == null)
            {
                Random ran = new Random();

                int _totalOccurance = this.availableDriverStats.Sum(x => x.Occurance);
                int _ranNumber = ran.Next(0, _totalOccurance);

                foreach(DriverStats _stat in this.availableDriverStats)
                {
                    _totalOccurance = -_stat.Occurance;

                    if(_totalOccurance <= 0)
                    {
                        return _stat;
                    }
                }

                if (this.availableDriverStats.Any())
                {
                    return this.availableDriverStats.First();
                }

                Log.Write("No Driverstats Available");
                AITypeConfig.ReadJson();
                this.availableDriverStats = AITypeConfig.aiTypes;
                return AITypeConfig.aiTypes.First();
            }

            return _stats;
        }

        //This function creates and adds a new driverStat

        static public void addDriverStats(string _name, int _reactionTime, float _followInterval, int _speedRelativeToLimit, float _ruleBreakingChance, int _occurance, bool _locked)
        {
            DriverStats _temp = new DriverStats(_name, _reactionTime, _followInterval, _speedRelativeToLimit, _ruleBreakingChance, _occurance, _locked);

            if (AITypeConfig.aiTypes.Find(x => x == _temp) == null)
            {
                AITypeConfig.aiTypes.Add(_temp);
            }

            General_Form.Main.UserInterface.SimSDM.Selection_box.AddElement(_temp.Name);
        }

        //this function returns a driverStat based on its name as String

        static public DriverStats getDriverStat(string _name)
        {
            DriverStats _temp = AITypeConfig.aiTypes.Find(x => x.Name == _name);

            if (_temp == null)
            {
                try
                {
                    _temp = AITypeConfig.aiTypes[0];
                }
                catch (Exception)
                {
                    _temp = new DriverStats("", 1, 1, 1, 1,1,false);
                }
            }

            return _temp;
        }

        //This function converts and returns all the driverStats as strings

        static public List<string> getStringDriverStats()
        {
            List<string> _temp = new List<string>();
            AITypeConfig.aiTypes.ForEach(x => _temp.Add(x.Name));
            return _temp;
        }

        //The following functions are all used during the creation/ editing of DriverStats in the slider menu, all follow the same prinicple:
        // 1) see if the value can be edited or if it is locked
        // 2) see if the value is allowed
        // 3) Update the value


        public void DeleteAI(DriverStats _stats)
        {
            AITypeConfig.aiTypes.Remove(_stats);
        }

        public void SelectAI(DriverStats _stats)
        {
            this.selectedAI = _stats;
        }

        private bool AllowEdit()
        {
            if (this.selectedAI.Locked)
            {
                //ERROR MESSAGE HERE!

                return (true);
            }
            return (false);
        }

        public void ChangeReactionTime(Slider o)
        {
            if (this.selectedAI == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            if (AllowEdit())
            {
                o.Value = (int)(this.selectedAI.ReactionTime * 10);
                return;
            }
            this.selectedAI.ReactionTime = (float)o.Value / 10;
        }

        public void ChangeFollowInterval(Slider o)
        {
            if (this.selectedAI == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            if (AllowEdit())
            {
                o.Value = (int)(this.selectedAI.ReactionTime * 10);
                return;
            }
            this.selectedAI.ReactionTime = (float)o.Value / 10;
        }

        public void ChangeSpeedRelativeToLimit(Slider o)
        {
            if (this.selectedAI == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            if (AllowEdit())
            {
                o.Value = this.selectedAI.SpeedRelativeToLimit;
                return;
            }
            this.selectedAI.SpeedRelativeToLimit = o.Value;
        }

        public void ChangeRuleBreakingChance(Slider o)
        {
            if (this.selectedAI == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            if (AllowEdit())
            {
                o.Value = (int)this.selectedAI.RuleBreakingChance;
                return;
            }
            this.selectedAI.RuleBreakingChance = o.Value;
        }

        public void ChangeOccurance(Slider o)
        {
            if (this.selectedAI == null)
            {
                //NO VEHICLE SELECTED;
                return;
            }

            this.selectedAI.Occurance = o.Value;
        }
    }
}