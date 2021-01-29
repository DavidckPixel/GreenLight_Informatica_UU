using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //A NodePath has two nodes, and two list of paths. It's contruction takes a beginNode and an Endnode and a list of Path's.
    //When constructed, it calls it's own method, CreateLinkPath().
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

        //foreach node in this.path, there will be determined if there is a next node in the list, if not, it will be null
        //If there is a previousnode for the node, a new path will be constructed with the previousnode, the node and the next node (which can be null).
        //This path would be added to the Linkpath list of Paths.
        private void CreateLinkPath()
        {
            Node _prevNode = null;
            Node _next;

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

        //This method is used to check if a NodePath is a match for this NodePath.
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
