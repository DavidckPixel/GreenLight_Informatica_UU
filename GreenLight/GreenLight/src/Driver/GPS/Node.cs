using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{

    //A Node is similar to a knot but then combined with its Links and connetions, 
    //A node also holds the variables where or not it can be a begin/end point

    public class Node
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

        //A node is a spawn node if it is connected to only one other Node, and is not backlinked, backlinked means that it is linked by something, but does not link back to it.

        public void spawn()
        { 
            Log.Write("The Node at Location: " + this.knot.Cord + " has " + this.links.Count() + " links");

            if ((this.connections.Count<= 1 && !isBackLinked) || this.connections.Count < 1)
            {
                canSpawn = true;
            }
        }
    }
}
