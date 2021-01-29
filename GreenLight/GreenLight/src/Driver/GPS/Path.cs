using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    //The Path class is the class that contains all the information for where to go on one road, Since sometimes you need to be on the correct lane
    //before the end of the road (for example when approaching a crossRoad). this is why we have the NextLaneIndex, this is a list can be null incase it does not matter
    //where the vehicle ends up, but if this list contains indexes, these are the lane indexes that the vehicle must end up being at before the end of the road.

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

            if ((_one.knot.Road1 == _two.knot.Road1 || _one.knot.Road1 == _two.knot.Road2 ) && _one.knot.Road1 != null )
            {
                    this.road = _one.knot.Road1;
                
            }
            else
            {
                    this.road = _one.knot.Road2;
            }
            if (this.road == null)
            {
                return;
            }
            if (this.road != null && this.road.roadtype == "Cross")
            {
                List<Link> _links = _one.links.FindAll(x => x.end == _two.knot);

                foreach (Link _nextlink in _links)
                {
                    Lane _crossLane = this.road.Drivinglanes[_nextlink.laneIndex - 1];
                    AbstractRoad _road = _two.knot.Road1;

                    if(this.road == _road)
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
                            Lane _forcedlane = null;
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
                    foreach (int _index in this.laneIndex) //KAN NETTER
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

            foreach(int _tuple in this.NextLaneIndex)
            {
            }
            
        }
    }
}
