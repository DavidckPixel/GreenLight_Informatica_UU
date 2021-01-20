using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    class RoadsConfig
    {
        //Size of (cross)roads
        public int laneWidth;
        public int scaleOffset;
        public int crossroadExtra;

        //OriginPoints
        public int opStandardWeight;

        //settingsscreen roads
        public Dictionary<string, int> settingsScreen;
    }
}
