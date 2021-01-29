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
    //A class that has a SpacingWidth, SpacingHeight, Boxsize & Hitsize, all non-dependent values that are used for the grid. 
    //It's used to apply a Json to by the static Grid.cs, so that the Json values can be used all through the programm.
    class GridConfig
    {
        
        public int SpacingWidth;
        public int SpacingHeight;
        public int BoxSize;
        public int HitSize;

    }
}
