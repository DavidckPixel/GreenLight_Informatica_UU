using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight.src.Driver.GPS
{
    class GPSData
    {
        List<Node> nodes;
        List<AbstractRoad> roads;

        public List<Knot> _allKnots;
        public List<Node> spawnNodes;

        public List<NodePath> nodePaths = new List<NodePath>();

        public GPSData(List<AbstractRoad> _roads)
        {
            nodes = new List<Node>();
            this.roads = _roads;

            FindAllKnots();
            CreateConnections();
        }

        private void FindAllKnots()
        {
            _allKnots = new List<Knot>();

            foreach (AbstractRoad _road in roads)
            {
                if (!(_road.roadtype == "Cross"))
                {
                    Knot _TempKnot = new Knot(_road, _road.endConnectedTo, _road.point2);
                    if (!TestDuplicateKnot(_TempKnot))
                    {
                        _allKnots.Add(_TempKnot);
                    }
                    _TempKnot = new Knot(_road.beginConnectedTo, _road, _road.point1);
                    if (!TestDuplicateKnot(_TempKnot))
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

                    Console.WriteLine("LEFT VALUE: " + _left);

                    FindAddRoad(_left, _crossRoad);
                    FindAddRoad(_right, _crossRoad);
                    FindAddRoad(_top, _crossRoad);
                    FindAddRoad(_bottom, _crossRoad);
                }
            }

            Console.WriteLine("Knot count: " + _allKnots.Count());
        }

        private void FindAddRoad(Point _p, AbstractRoad _mainroad)
        {
            AbstractRoad _road = this.roads.Find(x => x.hitbox.Contains(_p) && x.roadtype != "Cross");
            if(_road == null)
            {
                return;
            }
            _allKnots.Add(new Knot(_mainroad, _road, _p));
        }

        private bool TestDuplicateKnot(Knot _knot)
        {
            foreach(Knot _testKnot in _allKnots)
            {
                if (_knot.Equals(_testKnot))
                {
                    return true;
                }
            }
            return false;
        }

        private void CreateConnections()
        {
           List<Link> _allLinks = new List<Link>();

            foreach(AbstractRoad _road in roads)
            {
                _allLinks.AddRange(CreateConnectionPerRoad(_road));
            }

           foreach(Link _link in _allLinks)
           {
                _link.ConsolePrint();
           }

           foreach(Knot _knot in _allKnots)
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
                    _tempNode.isBackLinked = true;
                    if (!_endKnots.Contains(_tempNode))
                    {
                        _endKnots.Add(_tempNode);
                    }
                }
                _node.GiveConnectioned(_endKnots);
            }

            nodes.ForEach(x => x.spawn());
            this.spawnNodes = nodes.FindAll(x => x.canSpawn == true);

            foreach(Node _node in this.spawnNodes)
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

        private List<Link> CreateConnectionPerRoad(AbstractRoad _road)
        {
            List<Knot> _knots = _allKnots.FindAll(x => x.Road1 == _road || x.Road2 == _road);
            List<Link> _links = new List<Link>();

            if(_knots.Count() == 2 && _road.roadtype != "Cross")
            {
                Knot _firstKnot = _knots.Find(x => RoadMath.Distance(x.Cord, _road.point1) < 20);
                Knot _secondKnot = _knots.Find(x => RoadMath.Distance(x.Cord, _road.point2) < 20);

                Console.WriteLine("DISTANCE: " + RoadMath.Distance(_knots[0].Cord, _road.point1));
                Console.WriteLine("DISTANCE: " + RoadMath.Distance(_knots[1].Cord, _road.point2));

                //Console.WriteLine("Knot Point: {0}, Road Point {1}", _knots[0].Cord, _road.point1);
                //Console.WriteLine("Knot Point: {0}, Road Point {1}", _knots[1].Cord, _road.point2);

                if (_firstKnot == null || _secondKnot == null)
                {
                    Console.WriteLine("You should never come here, if this happens Please chekc out GPSDATA CreateCOnnection");
                    _firstKnot = _knots.First();
                    _secondKnot = _knots[1];
                }
                
                foreach(Lane _lane in _road.Drivinglanes)
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
            else if(_knots.Count() <= 1)
            {
                return _links;
            }
            else if(_road.roadtype == "Cross")
            {
                CrossRoad _crossRoad = (CrossRoad)_road;
                List<ConnectionLink> _crosslinks = _crossRoad.connectLinks;
                List<string> _stringsides = new List<string>();
                foreach (Knot _knot in _knots)
                {
                    CrossRoadSide _Temp = _crossRoad.sides.ToList().Find(x => x.hitbox.Contains(_knot.Cord));
                    if(_Temp != null)
                    {
                        _stringsides.Add(_Temp.side);
                    }
                    else
                    {
                        _stringsides.Add("");
                    }
                }

                foreach(ConnectionLink _connectionLink in _crosslinks)
                {
                    int _stringIndex = _stringsides.IndexOf(_connectionLink.begin.Side);
                    int _stringIndex2 = _stringsides.IndexOf(_connectionLink.end.Side);
                    if (_stringIndex != -1 && _stringIndex2 != -1 )
                    {
                        Console.WriteLine("Index Values: {0} , {1}", _stringIndex, _stringIndex2);

                        Knot _knot1 = _knots[_stringIndex];
                        Knot _knot2 = _knots[_stringIndex2];
                        int _index = _crossRoad.Drivinglanes.Find(x => x.link.begin == _connectionLink.begin && x.link.end == _connectionLink.end).thisLane;

                        _links.Add(new Link(_knot1, _knot2, _index));
                    }
                }
            }

            return _links;
        }

        public List<Node> getNodes()
        {
            return this.nodes;
        }

        public void Draw(Graphics g, NodePath _path)
        {
            Console.WriteLine("pathlink count: " + _path.linkPath.Count);
            foreach(Path _linkPath in _path.linkPath)
            {
                Console.WriteLine("current index: " + _path.linkPath.IndexOf(_linkPath));
                _linkPath.laneIndex.ForEach(x => Console.WriteLine(x));

                if (_linkPath.NextLaneIndex == null)
                {
                    _linkPath.road.Drivinglanes[_linkPath.laneIndex.First() - 1].DrawLine(g, Pens.Blue);
                }
                else
                {
                    
                    Console.WriteLine("NEXTLANE INDEX: " + _linkPath.NextLaneIndex.First());
                    Console.WriteLine("DrivingLanes: " + _linkPath.road.Drivinglanes.Count);
                    foreach(int _possiblelane in _linkPath.NextLaneIndex.First())
                    {
                        _linkPath.road.Drivinglanes[_possiblelane].DrawLine(g, Pens.Green);
                    }
                    //_linkPath.road.Drivinglanes[_linkPath.NextLaneIndex.First()].DrawLine(g, Pens.Green);
                }
                Console.WriteLine(_linkPath.road.point1);
            }
        }
    }
}
