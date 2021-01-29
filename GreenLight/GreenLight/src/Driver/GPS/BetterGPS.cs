using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //this is the GPS class used by the AI, ever AI has a gps class that contains the path it is requires to follow. In additional to more variables
    //that help it navigate the system

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

        //This method is called by the AI when it switches a road, telling the GPS it needs to set its index to the next path in the list;

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
        }
    }
}
