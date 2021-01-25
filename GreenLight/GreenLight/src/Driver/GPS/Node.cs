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
        Knot knot;
        List<Link> links;
        List<Knot> connections;
        bool canSpawn;

        public Node(Knot _knot, List<Link> _links, List<Knot> _connections)
        {
            this.knot = _knot;
            this.links = _links;            
            this.connections = _connections;
            this.canSpawn = this.connections.Count() <= 1 ? true : false;
        }
    }
}
