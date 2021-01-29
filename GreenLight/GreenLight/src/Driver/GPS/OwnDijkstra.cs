using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //OwnDijkstra is a static class that can be used to find the shortest path over a map of connected roads. It workst with Nodes from the Nodes class. 
    //To use it, you need a startNode and a goal node.  

    public static class OwnDijkstra
    {
        
        //The method called to actually get the shortest path. It calls to the calculatePath method to get a DijkstraPath.
        //When the DijkstraPath isn't null, it takes the list of nodes in the dijkstraPath and returns it.
        public static List<Node> GetShortestPath(Node _start, Node _goal)
        {
            DijkstraPaths temp = CalculatePath(_start, _goal);

            if (temp == null)
            {
                return null;
            }
            else
            {
                return temp.visited;
            }
        }
        
        //The method called to actually calculate the shortest path. It uses a start and a goal node, and returns a Dijkstrapath.
        //It returns null when the startnode is the same node as the end node.
        //if not, it constructs a list of DijkstraPaths, and gives it just one to start with --> The start node and an empty list of nodes.
        //For each dijkstrapath in the first list of dijkstrapath, the method looks at the connecting nodes of last Node in the list of nodes, if one of those is the goalNode, a dijkstraPath is returned
        //If not, the method looks at how many connections a node has. If the node has more than two connections, it's on the edge of a crossroad, and checks if there is actually a lane between the connectionpoints (You can't always go everywhere on a crossroad).
        //Then it checks if it's not trying to re-enter a road it has already been to. If not, it adds the road it moving over to the list of visited roads, and adds a Dijkstrapath to a temporary list.
        //When this is done for all connections of the last node, it clears the list of DijkstraPaths, and adds the temporary list to it. Then, the loop starts again.
        //It loops untill there is a path from start to finish to return, or twice the amount of roads there are. If no path is found, the method returns null.

        public static DijkstraPaths CalculatePath(Node _start, Node _goal)
        {
            List<DijkstraPaths> dijkstraPaths = new List<DijkstraPaths>();

            if(_start == _goal)
            {
                return null;
            }

            dijkstraPaths.Add(new DijkstraPaths(_start, new List<Node>()));
            List<DijkstraPaths> nextSetList = new List<DijkstraPaths>();

            for(int x = 0; x < General_Form.Main.BuildScreen.builder.roadBuilder.roads.Count * 2; x++)
            {
                List<AbstractRoad> _visitedRoads = new List<AbstractRoad>();                

                foreach (DijkstraPaths _dijkPath in dijkstraPaths)
                {
                    List<AbstractRoad> _tempvisited = new List<AbstractRoad>();
                    foreach (Node _node in _dijkPath.visited.Last().connections)
                    {                        
                        if (_node == _goal)
                        {
                            return new DijkstraPaths(_node, _dijkPath.visited);
                        }

                        if (_dijkPath.visited.Last().connections.Count > 2)
                        {
                            foreach (Link _connectionlink in _node.links)
                            {
                                if (_dijkPath.visited.Last().knot == _connectionlink.end && !_dijkPath.visited.Contains(_node) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road2) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road1))
                                {
                                    _tempvisited.Add(_dijkPath.visited.Last().knot.Road2);
                                    _tempvisited.Add(_dijkPath.visited.Last().knot.Road1);
                                    
                                    nextSetList.Add(new DijkstraPaths(_node, _dijkPath.visited));                                    
                                }
                            }
                        }
                        else if (!_dijkPath.visited.Contains(_node) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road2) && !_visitedRoads.Contains(_dijkPath.visited.Last().knot.Road1))
                        {
                            _tempvisited.Add(_dijkPath.visited.Last().knot.Road2);
                            _tempvisited.Add(_dijkPath.visited.Last().knot.Road1);
                            
                            nextSetList.Add(new DijkstraPaths(_node, _dijkPath.visited));
                        }
                    }
                    _visitedRoads.AddRange(_tempvisited);                    
                }
                dijkstraPaths.Clear();
                dijkstraPaths.AddRange(nextSetList);
            }            
            return null;
        }
    }    
}
