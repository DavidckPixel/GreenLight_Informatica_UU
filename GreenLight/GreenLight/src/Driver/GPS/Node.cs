using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{
    class Node
    {
        public Knot knot;
        public List<Link> links;
        public List<Node> connections;
        public bool canSpawn;
        public bool isBackLinked;

        public Node(Knot _knot, List<Link> _links)
        {
            this.knot = _knot;
            this.links = _links;
        }

        public void GiveConnectioned(List<Node> _nodes)
        {
            this.connections = _nodes;
        }

        public void spawn()
        {
            Console.WriteLine("BACK LINKED: " + isBackLinked);

            if ((this.connections.Count<= 1 && !isBackLinked) || this.connections.Count < 1)
            {
                canSpawn = true;
            }
        }
    }
}
