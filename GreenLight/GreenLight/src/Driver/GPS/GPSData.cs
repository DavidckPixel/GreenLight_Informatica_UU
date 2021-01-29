using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight.src.Driver.GPS
{
    //this is the general controller class for the GPS, this is were all the paths nodes and links are calculated.
    //the sequence it operates in:
    //1) Find all the knots, a knot it the place where 2 roads meet, or incase of an endroad, only one road begins/ends, to note here is that the beginning/end of a road is ambiqious and unimportant here
    //2) Create all the possible links inbetween the knots, it uses the drivinglanes for this
    //3) It then creates the nodes which is a knot with a paths it can travel
    //4) Set which nodes are start/ end nodes
    //5) remove all the startnodes that have 0 paths

     public class GPSData
    {
        List<Node> nodes;
        List<AbstractRoad> roads;

        public List<Knot> _allKnots;
        public List<Node> spawnNodes;

        public List<NodePath> nodePaths = new List<NodePath>();

        //The constructor method only takes all _roads. It then creates an empty list of nodes, and calls FindAllKnots() to set the _allknots list.
        public GPSData(List<AbstractRoad> _roads)
        {
            nodes = new List<Node>();
            this.roads = _roads;

            FindAllKnots();
            CreateConnections();
        }

        //This method loops through all roads and determines for all of them two knots, one of which has this road and the one it's begin is connected to, and one which has this road and the one it's end is connected to
        //if the road isn't a crossroad and if both the beginconnected road and the endconnected road of the road are null, the knot is added to the allknots list.
        //if the beginconnected road or the endconnected road isn't null the TestDuplicateKnot method is called, and if it returns false (there is no duplicate in the allknots list), the knot is added to the allknotslist.
        //If the road is a crossroad, the mainroad and a point on a side are given to the FindAddRoad method, for each side.

        private void FindAllKnots()
        {
            _allKnots = new List<Knot>();

            foreach (AbstractRoad _road in roads)
            {
                if (!(_road.roadtype == "Cross"))
                {
                    Knot _TempKnot = new Knot(_road, _road.endConnectedTo, _road.point2);

                    if (_road.endConnectedTo == null && _road.beginConnectedTo == null)
                    {
                        _allKnots.Add(_TempKnot);
                    }
                    else if (!TestDuplicateKnot(_TempKnot))
                    {
                        _allKnots.Add(_TempKnot);
                    }
                    
                    _TempKnot = new Knot(_road.beginConnectedTo, _road, _road.point1);

                    if (_road.beginConnectedTo == null && _road.beginConnectedTo == null)
                    {
                        _allKnots.Add(_TempKnot);
                    }
                    else if (!TestDuplicateKnot(_TempKnot))
                    {
                        _allKnots.Add(_TempKnot);
                    }                   
                }
                else
                {
                    CrossRoad _crossRoad = (CrossRoad)_road;
                    int _width = _crossRoad.laneWidth * (_crossRoad.lanes / 2) + 15;

                    Point _left = new Point(_crossRoad.point1.X - _width, _crossRoad.point1.Y);
                    Point _right = new Point(_crossRoad.point1.X + _width, _crossRoad.point1.Y);
                    Point _top = new Point(_crossRoad.point1.X, _crossRoad.point1.Y - _width);
                    Point _bottom = new Point(_crossRoad.point1.X, _crossRoad.point1.Y + _width);

                    FindAddRoad(_left, _crossRoad);
                    FindAddRoad(_right, _crossRoad);
                    FindAddRoad(_top, _crossRoad);
                    FindAddRoad(_bottom, _crossRoad);
                }
            }
        }

        //This method finds the road that has the given point _p in it's hitbox, and isn't a crossroad, and adds a knot with the given mainroad and this found road to the allKnots list.
        private void FindAddRoad(Point _p, AbstractRoad _mainroad)
        {
            AbstractRoad _road = this.roads.Find(x => x.hitbox.Contains(_p) && x.roadtype != "Cross");
            if (_road == null)
            {
                return;
            }
            _allKnots.Add(new Knot(_mainroad, _road, _p));
        }

        //This method uses the Equals method of a knot, to test if a knot is already in Allknots
        private bool TestDuplicateKnot(Knot _knot)
        {
            foreach (Knot _testKnot in _allKnots)
            {
                if (_knot.Equals(_testKnot))
                {
                    return true;
                }
            }
            return false;
        }

        //This method is used to find all nodes and add them too the nodes list
        //It's then used to set their backlinked bool, and to determine the list of nodes they are connected with, which are then given too the node by calling the node's GiveConnectioned() method.
        //It then calls the spawnmethod for each node, and uses the canSpawn bool of each node to make a list of Spawnnodes.
        //It then uses these spawnodes to fill the Nodepaths list with constructed Nodepaths using OwnDijkstra.
        //All the Nodepaths that have an empty linkpath are then removed again from that list

        private void CreateConnections()
        {
            List<Link> _allLinks = new List<Link>();

            foreach (AbstractRoad _road in roads)
            {
                _allLinks.AddRange(CreateConnectionPerRoad(_road));
            }

            foreach (Knot _knot in _allKnots)
            {                
                List<Link> _knotlinks = _allLinks.FindAll(x => x.begin == _knot);
                nodes.Add(new Node(_knot, _knotlinks));
            }

            foreach (Node _node in nodes)
            {
                List<Node> _endKnots = new List<Node>();

                foreach (Link _link in _node.links)
                {
                    Node _tempNode = nodes.Find(x => x.knot == _link.end);
                    if(!_tempNode.links.Any(x => x.end == _node.knot))
                    {
                        _tempNode.isBackLinked = true;
                    }
                    if (!_endKnots.Contains(_tempNode))
                    {
                        _endKnots.Add(_tempNode);
                    }
                }
                _node.GiveConnectioned(_endKnots);
            }

            nodes.ForEach(x => x.spawn());
            this.spawnNodes = nodes.FindAll(x => x.canSpawn == true);

            foreach (Node _node in this.spawnNodes)
            {
                if (_node != null)
                {
                    foreach (Node _endNode in this.spawnNodes)
                    {
                        if (_endNode != null)
                        {
                            this.nodePaths.Add(new NodePath(_node, _endNode, OwnDijkstra.GetShortestPath(_node, _endNode)));
                        }
                    }
                }
            }
            this.nodePaths.RemoveAll(x => x.linkPath.Count == 0);
        }

        //This method makes a list of links for the road it has been given, taking crossroads into account.

        private List<Link> CreateConnectionPerRoad(AbstractRoad _road)
        {
            List<Knot> _knots = _allKnots.FindAll(x => x.Road1 == _road || x.Road2 == _road);
            List<Link> _links = new List<Link>();

            if (_knots.Count() == 2 && _road.roadtype != "Cross")
            {
                Knot _firstKnot = _knots.Find(x => RoadMath.Distance(x.Cord, _road.point1) < 20);
                Knot _secondKnot = _knots.Find(x => RoadMath.Distance(x.Cord, _road.point2) < 20);

                if (_firstKnot == null || _secondKnot == null)                
                {
                    MessageBox.Show("There is a problem, simulation may nog be accurate!");
                    _firstKnot = _knots.First();
                    _secondKnot = _knots[1];
                }

                foreach (Lane _lane in _road.Drivinglanes)
                {
                    if (!_lane.flipped)
                    {
                        _links.Add(new Link(_secondKnot, _firstKnot, _lane.thisLane));
                    }
                    else
                    {
                        _links.Add(new Link(_firstKnot, _secondKnot, _lane.thisLane));
                    }
                }
            }
            else if (_knots.Count() <= 1)
            {
                return _links;
            }
            else if (_road.roadtype == "Cross")
            {
                CrossRoad _crossRoad = (CrossRoad)_road;
                List<ConnectionLink> _crosslinks = _crossRoad.connectLinks;
                List<string> _stringsides = new List<string>();
                foreach (Knot _knot in _knots)
                {
                    CrossRoadSide _Temp = _crossRoad.sides.ToList().Find(x => x.hitbox.Contains(_knot.Cord));
                    if (_Temp != null)
                    {
                        _stringsides.Add(_Temp.side);
                    }
                    else
                    {
                        _stringsides.Add("");
                    }
                }

                foreach (ConnectionLink _connectionLink in _crosslinks)
                {
                    int _stringIndex = _stringsides.IndexOf(_connectionLink.begin.Side);
                    int _stringIndex2 = _stringsides.IndexOf(_connectionLink.end.Side);
                    if (_stringIndex != -1 && _stringIndex2 != -1)
                    {
                        Knot _knot1 = _knots[_stringIndex];
                        Knot _knot2 = _knots[_stringIndex2];
                        int _index = _crossRoad.Drivinglanes.Find(x => x.link.begin == _connectionLink.begin && x.link.end == _connectionLink.end).thisLane;

                        _links.Add(new Link(_knot1, _knot2, _index));
                    }
                }
            }

            return _links;
        }        

        //This method is uses by the betterGPS to look through the nodePaths and return the list of path's which endnode and beginnode match.
        public List<Path> GetPathListFromNode(Node _begin, Node _end)
        {
            if (!(_begin.canSpawn || _end.canSpawn))
            {
                return null;
            }            
            return this.nodePaths.Find(x => x.CheckMatch(_begin, _end)).linkPath;
        }
        
        //This method is used by the betterGPS to get the data
        public static GPSData GetGPSData()
        {
            return General_Form.Main.SimulationScreen.gpsData;
        }

        //This method is used by the betterGPS to look through the nodePaths and select and return a random list of Path's from the list of nodePaths.
        public List<Path> GetPathListFromBeginnin(Node _begin)
        {
            List<NodePath> _AllPossiblePaths = this.nodePaths.FindAll(x => x.begin == _begin);

            if (!_AllPossiblePaths.Any())
            {
                return null;
            }

            Random ran = new Random();

            return _AllPossiblePaths[ran.Next(0, _AllPossiblePaths.Count())].linkPath;
        }

        //This method is used by the betterGPS to get a random node that can spawn as a startnode for the NodePath it will take.
        public Node GetRandomStartNode()
        {
            Console.WriteLine(this.spawnNodes.Count());

            if (this.nodePaths.Any())
            {
                Random ran = new Random();
                return this.nodePaths[ran.Next(0, this.nodePaths.Count())].begin;
            }

            MessageBox.Show("Something has gone wrong, no possible paths were found, return to the builder!");
            General_Form.Main.SimulationScreen.Simulator.ResetSimulation();
            General_Form.Main.SwitchControllers(General_Form.Main.BuildScreen);
            General_Form.Main.UserInterface.SimDataM.ResetTimer();
            return null;
        }
    }
}
