using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
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
