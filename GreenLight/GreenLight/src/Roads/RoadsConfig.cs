using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    // Here some base values are saved for all roads

    class RoadsConfig
    {
        public int laneWidth;
        public int scaleOffset;
        public int crossroadExtra;

        public int opStandardWeight;

        public Dictionary<string, int> settingsScreen;
    }
}
