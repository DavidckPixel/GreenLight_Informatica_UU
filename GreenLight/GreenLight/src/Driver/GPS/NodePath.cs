using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{

    //A node Path is the full Path from a begin node to the end node, consistent of a list of paths 

    public class NodePath
    {
        public Node begin;
        Node end;
        List<Node> path;

        public List<Path> linkPath;

        public NodePath(Node begin, Node end, List<Node> path)
        {
            this.begin = begin;
            this.end = end;
            this.path = path;
            if (this.path == null)
            {
                this.path = new List<Node>();
            }

            this.linkPath = new List<Path>();

            CreateLinkPath();
        }

        //During a transition from a Normal Road to a crossRoad or viseversa. The drivinglane index number nolonger properly correspond, 
        //so to compensate for that, we create a link between the two, so here we can say that if on road one you are on laneIndex 1, the next LaneIndex u will be on is laneIndex 12

        private void CreateLinkPath()
        {
            Node _prevNode = null;
            Node _next = null;

            int x = 1;

            foreach(Node _node in this.path)
            {

                if(!(x >= this.path.Count() - 1))
                {
                    _next = this.path[x];
                }
                else
                {
                    _next = null;
                }

                if (_prevNode != null)
                {
                    linkPath.Add(new Path(_prevNode, _node, _next));
                }

                _prevNode = _node;
                x++;
            }
        }

        public bool CheckMatch(Node _begin, Node _end)
        {
            if(_begin == this.begin && _end == this.end)
            {
                return true;
            }
            return false;
        }
    }
}
