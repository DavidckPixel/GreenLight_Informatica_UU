using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{

    //This class is our own rendition of the Dijkstra Pathfinding algorithm, it will find paths between 2 end nodes through its links

    public static class OwnDijkstra
    {
        public static List<Node> GetShortestPath(Node _start, Node _goal)
        {
            DijkstraPaths temp = calculatePath(_start, _goal);

            if (temp == null)
            {
                return null;
            }
            else
            {
                return temp.visited;
            }
        }
        
        //Here the algorithm gets 2 nodes, the start and goal, it will then begin at the start, it will look at all the possible directions
        //the start can go to, and do this. It will then does the same for these new Nodes it found, until it reaches the goal

        public static DijkstraPaths calculatePath(Node _start, Node _goal)
        {
            List<DijkstraPaths> dijkstraPaths = new List<DijkstraPaths>();

            if(_start == _goal)
            {
                return null;
            }

            dijkstraPaths.Add(new DijkstraPaths(_start, new List<Node>()));

            List<DijkstraPaths> nextSetList = new List<DijkstraPaths>();
            int count = 0;
            Console.WriteLine("Road Count: " + General_Form.Main.BuildScreen.builder.roadBuilder.roads.Count);
            for(int x = 0; x < General_Form.Main.BuildScreen.builder.roadBuilder.roads.Count; x++)
            {
                List<Node> _notReachable = new List<Node>();
                List<AbstractRoad> _visitedRoads = new List<AbstractRoad>();
                foreach (DijkstraPaths _dijkPath in dijkstraPaths)
                {


                    
                    foreach (Node _node in _dijkPath.visited.Last().connections)
                    {

                        if (_node == _goal)
                        {
                            Console.WriteLine("COUNT UNREACHABLE : " + _notReachable.Count);
                            return new DijkstraPaths(_node, _dijkPath.visited);
                        }

                        if (_dijkPath.visited.Last().connections.Count >= 2)
                        {
                            foreach (Link _connectionlink in _node.links)
                            {
                                //Console.WriteLine("NODE 1 HAS A KNOT WITH CORD {0}, ROAD 1 {1} AND ROAD 2 {2}", _dijkPath.visited.Last().knot.Cord, _dijkPath.visited.Last().knot.Road1, _dijkPath.visited.Last().knot.Road2);
                                //Console.WriteLine("NODE 2 HAS A KNOT WITH CORD {0}, ROAD 1 {1} AND ROAD 2 {2}", _connectionlink.begin.Cord, _connectionlink.begin.Road1, _connectionlink.begin.Road2);
                                if (_dijkPath.visited.Last().knot == _connectionlink.end && !_dijkPath.visited.Contains(_node) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road2) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road1) && !_notReachable.Contains(_node))
                                {
                                    Console.WriteLine("YESSSS!!");

                                    _visitedRoads.Add(_dijkPath.visited.Last().knot.Road2);
                                    _visitedRoads.Add(_dijkPath.visited.Last().knot.Road1);

                                    _notReachable.AddRange(_dijkPath.visited.Last().connections);
                                    foreach (Node _othernode in _dijkPath.visited.Last().connections)
                                    {

                                            _notReachable.Add(_othernode);

                                    }
                                    nextSetList.Add(new DijkstraPaths(_node, _dijkPath.visited));
                                    count++;
                                }

                            }

                        }


                        else if (!_dijkPath.visited.Contains(_node) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road2) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road1) && !_notReachable.Contains(_node))
                        {

                            Console.WriteLine("ADDED DIJKSTRAPATH");
                            _visitedRoads.Add(_dijkPath.visited.Last().knot.Road2);
                            _visitedRoads.Add(_dijkPath.visited.Last().knot.Road1);
                            _notReachable.AddRange(_dijkPath.visited.Last().connections);
                            foreach (Node _othernode in _dijkPath.visited.Last().connections)
                            {

                                    _notReachable.Add(_othernode);

                            }
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

    public class DijkstraPaths
    {
        //Data Class used during the DijkstraPathFinding algorithm

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
