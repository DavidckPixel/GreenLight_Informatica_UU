using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //A class with a List of nodes a that are connected, and can be driven over. It's constructed using A node the vehicle is currently on, and a list of nodes the vehicle has been on.
    //The node the vehicle is currently on is promptly added to the list of already visited nodes.
    public class DijkstraPaths
    {
        public List<Node> visited;

        public DijkstraPaths(Node _current, List<Node> _visited)
        {
            this.visited = new List<Node>();
            this.visited.AddRange(_visited);
            this.visited.Add(_current);
        }
    }
}
