using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{
    //Een node is an object that consists of a knot, a list of links, a list of nodes, and two bools
    //A knot and a list of links that have that knot is needed to construct a node. The rest: the list of nodes it's connected to, and the bools are assigned by the GPSdata class.

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

        //A method to assign a list of nodes to connections.
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
