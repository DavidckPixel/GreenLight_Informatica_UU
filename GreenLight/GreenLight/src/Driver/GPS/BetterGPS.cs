using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    class BetterGPS
    {
        List<Path> roadlist;
        public Path currentPath;
        public AbstractRoad nextRoad;
        int PathIndex;
        Node begin;
        Node goal;
        public bool Done;
        public bool Lastpath;

        BetterAI ai;

        public BetterGPS(BetterAI _ai, Node _begin, Node _end = null)
        {
            this.ai = _ai;
            this.begin = _begin;
            this.goal = _end;
            GPSData _data = GPSData.getGPSData();

            if (_end == null)
            {
                this.roadlist = _data.getPathListFromBeginnin(this.begin);
            }
            else
            {
                this.roadlist = _data.getPathListFromNode(this.begin, this.goal);
            }

            if(this.roadlist == null)
            {
                this.ai.SignalDone();
                this.Done = true;
            }
            else if (!this.roadlist.Any())
            {
                this.ai.SignalDone();
                this.Done = true;
            }
            else
            {
                this.PathIndex = 0;
                this.currentPath = this.roadlist[this.PathIndex];

                if (this.roadlist.Count() > 1)
                {
                    this.nextRoad = this.roadlist[1].road;
                }
            }
        }

        public void NextPath()
        {
            this.PathIndex++;
            
            if(this.PathIndex < this.roadlist.Count())
            {
                if(this.PathIndex == this.roadlist.Count() - 1)
                {
                    this.Lastpath = true;
                }
                else
                {
                    this.nextRoad = this.roadlist[this.PathIndex + 1].road;
                }

                this.currentPath = this.roadlist[this.PathIndex];

            }
            else
            {
                this.Done = true;
                this.ai.SignalDone();
            }

            ai.SwitchedPaths();
        }
    }
}
