using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    //A class that has all non-dependent values that are used for the userinterface. 
    //It's used to apply a Json to by the static UserControls.cs, so that the Json values can be used all through the programm.
    class UserControlsConfig
    {
        public string projectName;
        public string projectVersion;

        public int subMenuWidth;

        public Dictionary<string, int> standardSubMenu;

        public Dictionary<string, int> standardMainMenu;

        public Dictionary<string, int> startMainMenu;

        public Dictionary<string, int> startSubMenu; 

        public Dictionary<string, int> buildSubMenu; 

        public Dictionary<string, int> simSubMenu; 

        public Dictionary<string, int> buildElementsMenu; 

        public Dictionary<string, int> simElementsMenu; 

        public Dictionary<string, int> simDriver; 

        public Dictionary<string, int> simVehicle; 

        public Dictionary<string, int> simDataMenu;
    }
}
