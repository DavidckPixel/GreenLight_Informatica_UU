﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class CrossLane : Lane
    {
        public CrossLane(List<LanePoints> _points, ConnectionLink _link, int _index)
        {
            this.points = _points;
            this.link = _link;
            this.thisLane = _index;
        }

        public override void Draw(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void DrawoffsetHitbox(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void FlipPoints()
        {
            throw new NotImplementedException();
        }
    }
}