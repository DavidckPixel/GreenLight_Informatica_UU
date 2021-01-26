using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.src.Driver.GPS
{
    class Path
    {
        public AbstractRoad road;
        public List<int> laneIndex;
        public List<int> NextLaneIndex = new List<int>();

        public Path(Node _one, Node _two, Node _next)
        {
            laneIndex = new List<int>();
            _one.links.FindAll(x => x.end == _two.knot).ForEach(x => this.laneIndex.Add(x.laneIndex));
            
            if(_one.knot.Road1 == _two.knot.Road1 || _one.knot.Road1 == _two.knot.Road2)
            {
                this.road = _one.knot.Road1;
            }
            else
            {
                this.road = _one.knot.Road2;
            }

            if (_two.knot.Road1 != null && _two.knot.Road2 != null)
            {
                if ((_two.knot.Road1.roadtype == "Cross" || _two.knot.Road2.roadtype == "Cross") && this.road.roadtype != "Cross")
                {
                    List<Link> _links = _two.links.FindAll(x => x.end == _next.knot);

                    if (_two.knot.Road1.roadtype == "Cross")
                    {

                        foreach (Link _nextlink in _links)
                        {
                            Console.WriteLine("road 1 driving lanes: " + _two.knot.Road1.Drivinglanes.Count);
                            Console.WriteLine("road 1 nextlink laneindex: " + (_nextlink.laneIndex - 1));
                            Lane _crossLane = _two.knot.Road1.Drivinglanes[_nextlink.laneIndex - 1];
                            Lane _forcedlane = null;
                            int _forcedindex = laneIndex.First();
                            foreach (Lane _thislane in this.road.Drivinglanes)
                            {
                                if (RoadMath.Distance(_crossLane.points.First().cord, _thislane.points.Last().cord) < 5)
                                {
                                    _forcedlane = _thislane;
                                    break;
                                }
                            }
                            if (_forcedlane != null)
                            {
                                Console.WriteLine("Added forcedlaneindex to NextLaneIndex");
                                _forcedindex = this.road.Drivinglanes.IndexOf(_forcedlane);
                                this.NextLaneIndex.Add(_forcedindex);
                            }

                        }
                    }

                    if (_two.knot.Road2.roadtype == "Cross")
                    {

                        foreach (Link _nextlink in _links)
                        {
                            Lane _crossLane = _two.knot.Road2.Drivinglanes[_nextlink.laneIndex];
                            Lane _forcedlane = null;
                            int _forcedindex = laneIndex.First();
                            foreach (Lane _thislane in this.road.Drivinglanes)
                            {
                                if (RoadMath.Distance(_crossLane.points.First().cord, _thislane.points.Last().cord) < 10)
                                {
                                    _forcedlane = _thislane;
                                    //break;
                                }
                            }
                            if (_forcedlane != null)
                            {
                                _forcedindex = this.road.Drivinglanes.IndexOf(_forcedlane);
                                
                                this.NextLaneIndex.Add(_forcedindex);
                            }

                        }
                    }
                    this.NextLaneIndex.ForEach(x => Console.WriteLine("Forced index: " + x));
                    
                    /*_links.ForEach(x => this.NextLaneIndex.Add(x.laneIndex));
                    this.NextLaneIndex.ForEach(x => Console.WriteLine("Forced index: " + x));*/
                }
                else
                {
                    this.NextLaneIndex = null;
                }

            }
            else
            {
                this.NextLaneIndex = null;
            }
            
        }
    }
}
