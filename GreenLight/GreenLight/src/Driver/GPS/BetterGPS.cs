using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //Object constructed with a betterAI and a Node to start with. It's possible to construct it with an Node to end with, but if not, it will find an ending node itself
    //It had a list of Path's named roadlist, a Path where it's currently on, currentPath. An AbstractRoad nextRoad, A PathIndex, A beginNode, An EndNode, a bool Done, A bool LastPath and a betterAI.
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

        //The constructormethod Used the GetGPSData method of the GPSData to get a GPSData. It then uses one of the the GetPathList methods of the GPSdata to set the roadlist.
        //If there is no roadlist set, or the roadlist is empty, the bool Done is set to true, and the SignalDOne method of the betterAi is called;
        //Else, the PathIndex, the currentPath and the nextRoad are set.
        public BetterGPS(BetterAI _ai, Node _begin, Node _end = null)
        {
            this.ai = _ai;
            this.begin = _begin;
            this.goal = _end;
            GPSData _data = GPSData.GetGPSData();

            if (_end == null)
            {
                this.roadlist = _data.GetPathListFromBeginnin(this.begin);
            }
            else
            {
                this.roadlist = _data.GetPathListFromNode(this.begin, this.goal);
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

        //This method is used by the betterAI to let the GPS know they switch roads. If there is no next Path, the bool Done is set to true and the SignalDOne method of the AI is called.
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
