﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    class World
    {
        public int Brakepwr;
        public double Density;
        public double Gravity;

        public World(int _Brakepwr, double _Density, double _Gravity)
        {
            this.Brakepwr = _Brakepwr;
            this.Density = _Density;
            this.Gravity = _Gravity;
        }
    }
}