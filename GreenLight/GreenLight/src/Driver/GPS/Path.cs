using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenLight.src.Driver.GPS
{
    // A path is an object that holds a road, a list of Indexes of lanes that road has and that go in the right direction, a list of indexes the next road 
    //The constructor takes three nodes. The links between the knot of the first node and the knot of the second node are found, and the indexes added to the list of laneindexes
    //The road the first two nodes both have is determined and is set to this.road.
    //If this road is a crossroad, for each link between the two knots the crosslane is found in the lanes of the road. 
    //the other road that is part of the second node knot is determined and set as a temporary value, and each lane of that road is compared to the crosslane. 
    //If there is a distance of less then five between the last point of one of them and the first point of the other, a tuple is made of the indexes of both lanes, and added to the laneswap list of tuples.
    //If this road isn't a crossroad, but there is a third node, and the neither of the roads of the knot of the second node are null,
    //if one of these roads is a crossroad, for each of the links of that road the crosslane is found and compared to every lane of this.road.
    //If there is a distance of less then five between the last point of one of them and the first point of the other, a tuple is made of the indexes of both lanes, and added to the laneswap list of tuples.
    //If neither one of these roads were a crossroad, a tuple is made and added to laneswap for each index in the list of laneindexes. 
    //The same is done if one of these roads were null or there was no third knot.

    public class Path
    {
        public AbstractRoad road;
        public List<int> laneIndex;
        public List<int> NextLaneIndex;
        public List<Tuple<int, int>> LaneSwap;

        public Path(Node _one, Node _two, Node _next)
        {
            this.NextLaneIndex = new List<int>();
            this.laneIndex = new List<int>();
            this.LaneSwap = new List<Tuple<int, int>>();
            _one.links.FindAll(x => x.end == _two.knot).ForEach(x => this.laneIndex.Add(x.laneIndex));

            if ((_one.knot.Road1 == _two.knot.Road1 || _one.knot.Road1 == _two.knot.Road2) && _one.knot.Road1 != null)
            {
                this.road = _one.knot.Road1;
            }
            else
            {
                this.road = _one.knot.Road2;
            }

            if (this.road != null && this.road.roadtype == "Cross")
            {
                List<Link> _links = _one.links.FindAll(x => x.end == _two.knot);

                foreach (Link _nextlink in _links)
                {
                    Lane _crossLane = this.road.Drivinglanes[_nextlink.laneIndex - 1];
                    AbstractRoad _road = _two.knot.Road1;

                    if (this.road == _road)
                    {
                        _road = _two.knot.Road2;
                    }

                    foreach (Lane _thislane in _road.Drivinglanes)
                    {
                        if (RoadMath.Distance(_crossLane.points.Last().cord, _thislane.points.First().cord) < 5)
                        {
                            this.LaneSwap.Add(new Tuple<int, int>(_crossLane.thisLane, _thislane.thisLane));
                        }
                    }
                }
            }
            else if (_two.knot.Road1 != null && _two.knot.Road2 != null && _next != null)
            {
                if ((_two.knot.Road1.roadtype == "Cross" || _two.knot.Road2.roadtype == "Cross") && this.road.roadtype != "Cross")
                {
                    List<Link> _links = _two.links.FindAll(x => x.end == _next.knot);
                    List<int> _allpossiblelanes = new List<int>();
                    foreach (Link _nextlink in _links)
                    {
                        if (_two.knot.Road1.roadtype == "Cross")
                        {
                            Lane _crossLane = _two.knot.Road1.Drivinglanes[_nextlink.laneIndex - 1];
                            int _forcedindex = laneIndex.First();

                            foreach (Lane _thislane in this.road.Drivinglanes)
                            {
                                if (RoadMath.Distance(_crossLane.points.First().cord, _thislane.points.Last().cord) < 5)
                                {
                                    this.LaneSwap.Add(new Tuple<int, int>(_thislane.thisLane, _crossLane.thisLane));
                                    _allpossiblelanes.Add(_thislane.thisLane);

                                }
                            }
                        }

                        if (_two.knot.Road2.roadtype == "Cross")
                        {
                            Lane _crossLane = _two.knot.Road2.Drivinglanes[_nextlink.laneIndex - 1];
                            int _forcedindex = laneIndex.First();
                            foreach (Lane _thislane in this.road.Drivinglanes)
                            {
                                if (RoadMath.Distance(_crossLane.points.First().cord, _thislane.points.Last().cord) < 10)
                                {
                                    this.LaneSwap.Add(new Tuple<int, int>(_thislane.thisLane, _crossLane.thisLane));
                                    _allpossiblelanes.Add(_thislane.thisLane);
                                }
                            }
                        }

                        if (_allpossiblelanes.Any())
                        {
                            _allpossiblelanes.ForEach(x => this.NextLaneIndex.Add(x));
                        }
                        _allpossiblelanes.Clear();
                    }
                }
                else
                {
                    foreach (int _index in this.laneIndex)
                    {
                        this.LaneSwap.Add(new Tuple<int, int>(_index, _index));
                    }
                }
            }
            else
            {
                foreach (int _index in this.laneIndex)
                {
                    this.LaneSwap.Add(new Tuple<int, int>(_index, _index));
                }
            }
        }
    }
}
