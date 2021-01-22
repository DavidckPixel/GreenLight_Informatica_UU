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
    public class AIController : EntityController
    {
        public List<AI> driverlist = new List<AI>();
        public DriverStats selectedAI;

        public override void Initialize()
        {
            Log.Write("Initializing the AIController");
        }

        public AIController()
        {

        }

        public void AddDriver(int _x, int _y)
        {
            //Hier dingen die waardes van menu halen ofzo, of bereken voor welke stats etc.

            DriverStats _stats = getDriverStats();
            Vehicle _vehicle = General_Form.Main.SimulationScreen.Simulator.vehicleController.getVehicle(_x,_y);

            driverlist.Add(new AI(_vehicle, _stats));
        }

        public DriverStats getDriverStats(DriverStats _stats = null)
        {
            if (_stats == null)
            {
                Random ran = new Random();
                int _index = ran.Next(0, AITypeConfig.aiTypes.Count() - 1);
                _stats = AITypeConfig.aiTypes[_index];
            }

            return _stats;
        }

        static public void addDriverStats(string _name, int _reactionTime, float _followInterval, int _speedRelativeToLimit, float _ruleBreakingChance, int _occurance, bool _locked)
        {
            DriverStats _temp = new DriverStats(_name, _reactionTime, _followInterval, _speedRelativeToLimit, _ruleBreakingChance, _occurance, _locked);

            if (AITypeConfig.aiTypes.Find(x => x == _temp) == null)
            {
                AITypeConfig.aiTypes.Add(_temp);
            }

            General_Form.Main.UserInterface.SimSDM.Selection_box.Add_Element(_temp.Name);
        }


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

        static public List<string> getStringDriverStats()
        {
            List<string> _temp = new List<string>();
            AITypeConfig.aiTypes.ForEach(x => _temp.Add(x.Name));
            return _temp;
        }

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