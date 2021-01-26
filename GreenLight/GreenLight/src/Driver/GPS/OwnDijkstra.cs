using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    static class OwnDijkstra
    {
        public static List<Node> GetShortestPath(Node _start, Node _goal)
        {
            DijkstraPaths temp = calculatePath(_start, _goal);

            if (temp == null)
            {
                return null;
            }
            else{
                return temp.visited;
            }
        }
        

        public static DijkstraPaths calculatePath(Node _start, Node _goal)
        {
            List<DijkstraPaths> dijkstraPaths = new List<DijkstraPaths>();

            if(_start == _goal)
            {
                return null;
            }

            dijkstraPaths.Add(new DijkstraPaths(_start, new List<Node>()));

            List<DijkstraPaths> nextSetList = new List<DijkstraPaths>();

            for(int x = 0; x < 100; x++)
            {
                foreach(DijkstraPaths _dijkPath in dijkstraPaths)
                {
                        foreach (Node _node in _dijkPath.visited.Last().connections)
                        {
                            if (_node == _goal)
                            {
                                return new DijkstraPaths(_node, _dijkPath.visited);
                            }

                            if (!_dijkPath.visited.Contains(_node))
                            {
                                nextSetList.Add(new DijkstraPaths(_node, _dijkPath.visited));
                            }
                        }
                }
                dijkstraPaths.Clear();
                dijkstraPaths.AddRange(nextSetList);
            }

            return null;
        }
    }

    class DijkstraPaths
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
