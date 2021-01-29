using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    public class DijkstraPaths
    {
        public List<Node> visited;
        public bool closed;
        public Node currentNode;

        public DijkstraPaths(Node _current, List<Node> _visited)
        {
            this.visited = new List<Node>();
            this.visited.AddRange(_visited);
            this.visited.Add(_current);
        }

        public void AddNode(Node _node)
        {
            if (!visited.Contains(_node))
            {
                this.visited.Add(currentNode);
                this.currentNode = _node;
            }
        }
    }
}
