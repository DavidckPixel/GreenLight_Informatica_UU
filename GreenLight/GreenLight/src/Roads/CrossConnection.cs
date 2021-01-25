using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace GreenLight
{
    class CrossConnection
    {
        RoadController controller = General_Form.Main.BuildScreen.builder.roadBuilder;
        Point point1;
        Point point2;
        string dir1;
        string dir2;
        AbstractRoad roadOne;
        AbstractRoad roadTwo;
        Point temp1, temp2, temp3, temp4;

        public CrossConnection(Point _point1, Point _point2, string _dir1, string _dir2, AbstractRoad _roadOne, AbstractRoad _roadTwo)
        {
            this.point1 = _point1;
            this.point2 = _point2;
            this.dir1 = _dir1;
            this.dir2 = _dir2;
            this.roadOne = _roadOne;
            this.roadTwo = _roadTwo;

            temp1 = _roadOne.getPoint1();
            temp2 = _roadOne.getPoint2();
            temp3 = _roadTwo.getPoint1();
            temp4 = _roadTwo.getPoint2();

            Console.WriteLine(" ---- CrossConnection ---- ");

            if (_roadOne.Type == "Cross" && _roadTwo.Type == "Cross")
            {
                CrossandCross();
            }
            else if ((_roadOne.Type == "Cross" && (_roadTwo.Type == "Diagonal" || _roadTwo.Type == "Curved" || _roadTwo.Type == "Curved2")) || ((_roadOne.Type == "Diagonal" || _roadOne.Type == "Curved" || _roadOne.Type == "Curved2") && _roadTwo.Type == "Cross"))
            {
                CrossandDiagonal();
            }
            else if ((_roadOne.Type == "Cross" && (_roadTwo.Type == "Curved" || _roadTwo.Type == "Curved2")) || ((_roadOne.Type == "Curved" || _roadOne.Type == "Curved2") && _roadTwo.Type == "Cross"))
            {
               // CrossandCurved();
            }
        }

        public void CrossandCross()
        {
            ConnectionPoint _cp = null;
            ConnectionPoint _cp2 = null;
            ConnectionPoint _cpLink = null;
            ConnectionPoint _cpLink2 = null;
            Point _diagonalbegin = new Point(0,0);
            Point _diagonalend = new Point(0, 0);
            bool _buildroad = true;
            List<ConnectionPoint> _connectedLanes = new List<ConnectionPoint>();
            int _isEven = 0;

            foreach (ConnectionPoint x in roadOne.translatedconnectPoints)
            {
                if(point1 == x.Location)
                {
                    _cp = x;
                    foreach (ConnectionPoint y in roadOne.connectPoints)
                    {
                        if (_cp.Side == y.Side && _cp.Place == y.Place)
                        {
                            _cpLink = y;
                        }
                    }
                }
            }
            foreach (ConnectionPoint x in roadTwo.translatedconnectPoints)
            {
                if (point2 == x.Location)
                {
                    _cp2 = x;
                    foreach (ConnectionPoint y in roadTwo.connectPoints)
                    {
                        if (_cp2.Side == y.Side && _cp2.Place == y.Place)
                        {
                            _cpLink2 = y;
                        }
                    }
                }
            }

            int _place, _place2, _dir, _side, _side2;
            if (_cp.Side == "Top")
            {
                _side = 0;
                _side2 = roadTwo.lanes;
            }
            else if (_cp.Side == "Bottom")
            {
                _side = roadOne.lanes;
                _side2 = 0;
            }
            else if (_cp.Side == "Left")
            {
                _side = roadOne.lanes * 2;
                _side2 = roadTwo.lanes * 3;
            }
            else
            {
                _side = roadOne.lanes * 3;
                _side2 = roadTwo.lanes * 2;
            }

            Console.WriteLine(_side + " " + _side2);
            for (int t = 0; t < Math.Max(roadOne.lanes, roadTwo.lanes) && _buildroad; t++)
            {
                for (int x = 0; x <= 1; x++)
                {
                    if (t == 0 && x == 1)
                        break;

                    if (x == 0)
                        _dir = -1;
                    else
                        _dir = 1;
                    

                    _place = _cp.Place + t * _dir;
                    _place2 = _cp2.Place + t * _dir;
                    Console.WriteLine(_place + " " + _place2);
                    if (_place >= 1 && _place2 >= 1 && _place <= roadOne.lanes && _place2 <= roadTwo.lanes)
                    {
                        if (!(roadOne.connectPoints[_place - 1 + _side].Active && roadTwo.connectPoints[_place - 1 + _side2].Active))
                        {
                            _buildroad = false;
                        }
                        else
                        {
                            _connectedLanes.Add(roadOne.connectPoints[_place - 1 + _side]);
                            _connectedLanes.Add(roadTwo.connectPoints[_place - 1 + _side2]);
                        }
                    }
                    else if (_place >= 1 && _place <= roadOne.lanes)
                    {
                        if(roadOne.connectPoints[_place - 1 + _side].Active)
                        {
                            _buildroad = false;
                        }
                    }
                    else if (_place2 >= 1 && _place2 <= roadTwo.lanes)
                    {
                        if (roadTwo.connectPoints[_place - 1 + _side].Active)
                        {
                            _buildroad = false;
                        }
                    }
                }
            }

            if(_buildroad)
            {
                Console.WriteLine("Build Crossconnection " + _cp.Side);
                if((_connectedLanes.Count / 2) % 2 == 0)
                {
                    _isEven = -10;
                }
                else
                {
                    _isEven = 0;
                }

                if(_cp.Side == "Top")
                {
                    int _middleX = 0;
                    for(int t = 0; t < _connectedLanes.Count; t++)
                    {
                        foreach(ConnectionPoint c in roadOne.translatedconnectPoints)
                        {
                            if (c.Side == _connectedLanes[t].Side && c.Place == _connectedLanes[t].Place && c.Side == "Top")
                            {
                                _middleX += c.Location.X;
                            }
                        }
                    }
                    _middleX = _middleX / (_connectedLanes.Count / 2) + _isEven;
                    Console.WriteLine(_middleX + " " + _connectedLanes.Count);

                    controller.BuildDiagonalRoad(new Point(_middleX, _cp.Location.Y), new Point(_middleX, _cp2.Location.Y), _connectedLanes.Count / 2, true, true, roadOne, roadTwo);
                }
                else if (_cp.Side == "Bottom")
                {
                    int _middleX = 0;
                    for (int t = 0; t < _connectedLanes.Count; t++)
                    {
                        foreach (ConnectionPoint c in roadOne.translatedconnectPoints)
                        {
                            if (c.Side == _connectedLanes[t].Side && c.Place == _connectedLanes[t].Place && c.Side == "Bottom")
                            {
                                _middleX += c.Location.X;
                            }
                        }
                    }
                    _middleX = _middleX / (_connectedLanes.Count / 2) + _isEven;
                    Console.WriteLine(_middleX + " " + _connectedLanes.Count);

                    controller.BuildDiagonalRoad(new Point(_middleX, _cp.Location.Y), new Point(_middleX, _cp2.Location.Y), _connectedLanes.Count / 2, true, true, roadOne, roadTwo);
                }
                else if (_cp.Side == "Left")
                {
                    int _middleY = 0;
                    for(int t = 0; t < _connectedLanes.Count; t++)
                    {
                        foreach (ConnectionPoint c in roadOne.translatedconnectPoints)
                        {
                            if (c.Side == _connectedLanes[t].Side && c.Place == _connectedLanes[t].Place && c.Side == "Left")
                            {
                                _middleY += c.Location.Y;
                            }
                        }
                        _middleY += _connectedLanes[t].Location.Y;
                    }
                    _middleY = _middleY / (_connectedLanes.Count / 2) + _isEven;

                    Console.WriteLine(_middleY + " " + _connectedLanes.Count);
                    Console.WriteLine(_cp.Location.X + " " + _cp2.Location.X);
                    controller.BuildDiagonalRoad(new Point(_cp.Location.X, _middleY), new Point(_cp2.Location.X, _middleY), _connectedLanes.Count / 2, true, true, roadOne, roadTwo);
                }
                else
                {
                    int _middleY = 0;
                    for (int t = 0; t < _connectedLanes.Count; t++)
                    {
                        foreach (ConnectionPoint c in roadOne.translatedconnectPoints)
                        {
                            if (c.Side == _connectedLanes[t].Side && c.Place == _connectedLanes[t].Place && c.Side == "Right")
                            {
                                _middleY += c.Location.Y;
                            }
                        }
                        _middleY += _connectedLanes[t].Location.Y;
                    }
                    _middleY = _middleY / (_connectedLanes.Count / 2) + _isEven;

                    Console.WriteLine(_middleY + " " + _connectedLanes.Count);
                    Console.WriteLine(_cp.Location.X + " " + _cp2.Location.X);
                    controller.BuildDiagonalRoad(new Point(_cp.Location.X, _middleY), new Point(_cp2.Location.X, _middleY), _connectedLanes.Count / 2, true, true, roadOne, roadTwo);
                }

                int _OuterInner = 0;
                for (int t = controller.roads[controller.roads.Count - 1].lanes; t > 0 && _connectedLanes.Count > 0; t--)
                {
                    int _tracker = 0;
                    if ((_OuterInner % 2 == 0 && controller.roads[controller.roads.Count - 1].lanes % 2 == 0) || (_OuterInner % 2 == 1 && controller.roads[controller.roads.Count - 1].lanes % 2 == 1))
                    {
                        ConnectionPoint _highest = null;
                        int x = -1;
                        foreach(ConnectionPoint c in _connectedLanes)
                        {
                            x++;
                            if (_highest == null || c.Place > _highest.Place)
                            {
                                _highest = c;
                                _tracker = x;
                            }
                        }
                    }
                    else
                    {
                        ConnectionPoint _lowest = null;
                        int x = -1;
                        foreach (ConnectionPoint c in _connectedLanes)
                        {
                            x++;
                            if (_lowest == null || c.Place < _lowest.Place)
                            {
                                _lowest = c;
                                _tracker = x;
                            }
                        }
                    }

                    if (_tracker % 2 == 0)
                    {
                        foreach (CrossLane c in roadOne.Drivinglanes)
                        {
                            if (c.link.begin == _connectedLanes[_tracker])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].endConnectedTo.Add(c);
                                c.beginConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                            else if (c.link.end == _connectedLanes[_tracker])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].beginConnectedTo.Add(c);
                                c.endConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                        }
                        foreach (CrossLane c in roadTwo.Drivinglanes)
                        {
                            if (c.link.begin == _connectedLanes[_tracker + 1])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].endConnectedTo.Add(c);
                                c.beginConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                            else if (c.link.end == _connectedLanes[_tracker + 1])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].beginConnectedTo.Add(c);
                                c.endConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                        }
                        _connectedLanes.RemoveAt(_tracker + 1);
                        _connectedLanes.RemoveAt(_tracker);
                    }
                    else
                    {
                        foreach (CrossLane c in roadOne.Drivinglanes)
                        {
                            if (c.link.begin == _connectedLanes[_tracker - 1])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].endConnectedTo.Add(c);
                                c.beginConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                            else if (c.link.end == _connectedLanes[_tracker - 1])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].beginConnectedTo.Add(c);
                                c.endConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                        }
                        foreach (CrossLane c in roadTwo.Drivinglanes)
                        {
                            if (c.link.begin == _connectedLanes[_tracker])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].endConnectedTo.Add(c);
                                c.beginConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                            else if (c.link.end == _connectedLanes[_tracker])
                            {
                                controller.roads[controller.roads.Count - 1].Drivinglanes[t].beginConnectedTo.Add(c);
                                c.endConnectedTo.Add(controller.roads[controller.roads.Count - 1].Drivinglanes[t]);
                            }
                        }
                        _connectedLanes.RemoveAt(_tracker);
                        _connectedLanes.RemoveAt(_tracker - 1);
                    }
                } 
            }
        }

        public void CrossandDiagonal()
        {
            Console.WriteLine("Cross and Diagonal");
            char _roadEnds;
            ConnectionPoint _cp = null;
            ConnectionPoint _cpLink = null;
            bool _buildroad = true;
            List<ConnectionPoint> _connectedLanes = new List<ConnectionPoint>();
            int _isEven = 0;
            AbstractRoad _Crossroad, _road;
            Point _Crosspoint, _roadPoint;


            if (roadOne.Type == "Diagonal" && temp1.Y == temp2.Y && roadOne.slp == 0)
            {
                _roadEnds = 'v';
            }
            else if (roadOne.Type == "Diagonal" && temp1.X == temp2.X && roadOne.slp == 0)
            {
                _roadEnds = 'h';
            }
            else if (roadTwo.Type == "Diagonal" && temp3.Y == temp4.Y && roadTwo.slp == 0)
            {
                _roadEnds = 'v';
            }
            else if (roadTwo.Type == "Diagonal" && temp3.X == temp4.X && roadTwo.slp == 0)
            {
                _roadEnds = 'h';
            }
            else if (roadOne.Type == "Diagonal" && roadOne.slp < 1 && roadOne.slp > -1)
            {
                _roadEnds = 'v';
            }
            else if (roadOne.Type == "Diagonal" && (roadOne.slp >= 1 || roadOne.slp <= -1))
            {
                _roadEnds = 'h';
            }
            else if (roadTwo.Type == "Diagonal" && roadTwo.slp < 1 && roadTwo.slp > -1)
            {
                _roadEnds = 'v';
            }
            else if (roadTwo.Type == "Diagonal" && (roadTwo.slp >= 1 || roadTwo.slp <= -1))
            {
                _roadEnds = 'h';
            }
            else if (roadOne.Type == "Curved" || roadOne.Type == "Curved2")
            {
                if (Math.Abs(temp1.X - point1.X) < Math.Abs(temp2.X - point1.X))
                {
                    _roadEnds = 'h';
                }
                else
                {
                    _roadEnds = 'v';
                }
            }
            else
            {
                if (Math.Abs(temp3.X - point2.X) < Math.Abs(temp4.X - point2.X))
                {
                    _roadEnds = 'h';
                }
                else
                {
                    _roadEnds = 'v';
                }
            }

            Console.WriteLine(_roadEnds);

            if (roadOne.Type == "Cross")
            {
                _Crossroad = roadOne;
                _Crosspoint = point1;
                _road = roadTwo;
                _roadPoint = point2;
            }
            else
            {
                _Crossroad = roadTwo;
                _Crosspoint = point2;
                _road = roadOne;
                _roadPoint = point1;
            }

            foreach (ConnectionPoint x in _Crossroad.translatedconnectPoints)
            {
                if (_Crosspoint == x.Location)
                {
                    _cp = x;
                    foreach (ConnectionPoint y in _Crossroad.connectPoints)
                    {
                        if (_cp.Side == y.Side && _cp.Place == y.Place)
                        {
                            _cpLink = y;
                        }
                    }
                }
            }
            
            if (((_cp.Side == "Top" || _cp.Side == "Bottom") && _roadEnds == 'v') || ((_cp.Side == "Left" || _cp.Side == "Right") && _roadEnds == 'h'))
                return;

            Console.WriteLine("CrossConnection with the same ending");

            int _place, _dir, _side;

            if (_cp.Side == "Top")
            {
                _side = 0;
            }
            else if (_cp.Side == "Bottom")
            {
                _side = _Crossroad.lanes;
            }
            else if (_cp.Side == "Left")
            {
                _side = _Crossroad.lanes * 2;
            }
            else
            {
                _side = _Crossroad.lanes * 3;
            }

            for (int t = 0; t < _Crossroad.lanes && _buildroad; t++)
            {
                for (int x = 0; x <= 1; x++)
                {
                    if (t == 0 && x == 1)
                        break;

                    if (x == 0)
                        _dir = -1;
                    else
                        _dir = 1;


                    _place = _cp.Place + t * _dir;

                    if (_place >= 1 && _place <= _Crossroad.lanes)
                    {
                        if (_Crossroad.connectPoints[_place - 1 + _side].Active)
                        {
                            foreach(ConnectionPoint c in _Crossroad.translatedconnectPoints)
                            {
                                bool _found = false;
                                if (c.Side == _Crossroad.connectPoints[_place - 1 + _side].Side && c.Place == _Crossroad.connectPoints[_place - 1 + _side].Place)
                                {
                                    foreach(DrivingLane d in _road.Drivinglanes)
                                    {
                                        if (_cp.Side == "Top" || _cp.Side == "Bottom")
                                        {
                                            if (Math.Abs(d.points.First().cord.Y - _roadPoint.Y) < Math.Abs(d.points.Last().cord.Y - _roadPoint.Y))
                                            {
                                                if (c.Location.X == d.points.First().cord.X)
                                                {
                                                    _connectedLanes.Add(_Crossroad.connectPoints[_place - 1 + _side]);
                                                    foreach(CrossLane crosslane in _Crossroad.Drivinglanes)
                                                    {
                                                        if(crosslane.link.begin == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.beginConnectedTo.Add(d);
                                                            d.beginConnectedTo.Add(crosslane);
                                                        }
                                                        else if(crosslane.link.end == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.endConnectedTo.Add(d);
                                                            d.beginConnectedTo.Add(crosslane);
                                                        }
                                                    }
                                                    _found = true;
                                                    Console.WriteLine("Found for " + c.Place);
                                                }
                                            }
                                            else 
                                            {
                                                if (c.Location.X == d.points.Last().cord.X)
                                                {
                                                    _connectedLanes.Add(_Crossroad.connectPoints[_place - 1 + _side]);
                                                    foreach (CrossLane crosslane in _Crossroad.Drivinglanes)
                                                    {
                                                        if (crosslane.link.begin == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.beginConnectedTo.Add(d);
                                                            d.endConnectedTo.Add(crosslane);
                                                        }
                                                        else if (crosslane.link.end == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.endConnectedTo.Add(d);
                                                            d.endConnectedTo.Add(crosslane);
                                                        }
                                                    }
                                                    _found = true;
                                                    Console.WriteLine("Found for " + c.Place);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Math.Abs(d.points.First().cord.X - _roadPoint.X) < Math.Abs(d.points.Last().cord.X - _roadPoint.X))
                                            {
                                                if (c.Location.Y == d.points.First().cord.Y)
                                                {
                                                    _connectedLanes.Add(_Crossroad.connectPoints[_place - 1 + _side]);
                                                    foreach (CrossLane crosslane in _Crossroad.Drivinglanes)
                                                    {
                                                        if (crosslane.link.begin == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.beginConnectedTo.Add(d);
                                                            d.beginConnectedTo.Add(crosslane);
                                                        }
                                                        else if (crosslane.link.end == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.endConnectedTo.Add(d);
                                                            d.beginConnectedTo.Add(crosslane);
                                                        }
                                                    }
                                                    _found = true;
                                                    Console.WriteLine("Found for " + c.Place);
                                                }
                                            }
                                            else
                                            {
                                                if (c.Location.Y == d.points.Last().cord.Y)
                                                {
                                                    _connectedLanes.Add(_Crossroad.connectPoints[_place - 1 + _side]);
                                                    foreach (CrossLane crosslane in _Crossroad.Drivinglanes)
                                                    {
                                                        if (crosslane.link.begin == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.beginConnectedTo.Add(d);
                                                            d.endConnectedTo.Add(crosslane);
                                                        }
                                                        else if (crosslane.link.end == _Crossroad.connectPoints[_place - 1 + _side])
                                                        {
                                                            crosslane.endConnectedTo.Add(d);
                                                            d.endConnectedTo.Add(crosslane);
                                                        }
                                                    }
                                                    _found = true;
                                                    Console.WriteLine("Found for " + c.Place);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!_found)
                                {
                                    Console.WriteLine("Not Found");
                                    //_buildroad = false;
                                }
                                else
                                {
                                   // _buildroad = true;
                                }
                            }
                        }
                    }

                    // hier gaat meerdere lanes fout
                    
                    foreach(DrivingLane d in _road.Drivinglanes)
                    {
                        if (_cp.Side == "Top" || _cp.Side == "Bottom")
                        {
                            if (Math.Abs(d.points.First().cord.Y - _roadPoint.Y) < Math.Abs(d.points.Last().cord.Y - _roadPoint.Y))
                            {
                                if (d.beginConnectedTo.Count == 0)
                                    _buildroad = false;
                            }
                            else
                            {
                                if (d.endConnectedTo.Count == 0)
                                    _buildroad = false;
                            }
                        }
                        else
                        {
                            if (Math.Abs(d.points.First().cord.X - _roadPoint.X) < Math.Abs(d.points.Last().cord.X - _roadPoint.X))
                            {
                                if (d.beginConnectedTo.Count == 0)
                                    _buildroad = false;
                            }
                            else
                            {
                                if (d.endConnectedTo.Count == 0)
                                    _buildroad = false;
                            }
                        }
                    } 

                    if(_buildroad)
                    {
                        Console.WriteLine("Build Connecting Road");
                        if (_connectedLanes.Count % 2 == 0)
                        {
                            _isEven = -10;
                        }
                        else
                        {
                            _isEven = 0;
                        }


                        //controller.DeleteRoad(_road);

                        controller.roads.Remove(_road);

                        if (_cp.Side == "Top" || _cp.Side == "Bottom")
                        {
                            int _middleX = 0;
                            foreach(ConnectionPoint c in _connectedLanes)
                            {
                                foreach (ConnectionPoint transcp in _Crossroad.translatedconnectPoints)
                                {
                                    if(transcp.Side == c.Side && transcp.Place == c.Place)
                                        _middleX += transcp.Location.X;
                                }
                            }
                            _middleX = (_middleX / _connectedLanes.Count) + _isEven;
                            Console.WriteLine(_middleX + " " + _connectedLanes.Count);

                            if (_roadPoint == _road.point1)
                            {
                                if (_road.Type == "Diagonal")
                                    controller.BuildDiagonalRoad(new Point(_middleX, _Crosspoint.Y), _road.point2, _connectedLanes.Count, true, _road.endconnection, _Crossroad, _road.endConnectedTo);
                                else
                                    controller.BuildCurvedRoad(new Point(_middleX, _Crosspoint.Y), _road.point2, _connectedLanes.Count, _road.Type, true, _road.endconnection, _Crossroad, _road.endConnectedTo);
                            }
                            else
                            {
                                if (_road.Type == "Diagonal")
                                    controller.BuildDiagonalRoad(_road.point1, new Point(_middleX, _Crosspoint.Y), _connectedLanes.Count, _road.beginconnection, true, _road.beginConnectedTo, _Crossroad);
                                else 
                                    controller.BuildCurvedRoad(_road.point1, new Point(_middleX, _Crosspoint.Y), _connectedLanes.Count, _road.Type, _road.beginconnection, true, _road.beginConnectedTo, _Crossroad);
                            }
                        }
                        else
                        {
                            int _middleY = 0;
                            foreach (ConnectionPoint c in _connectedLanes)
                            {
                                foreach (ConnectionPoint transcp in _Crossroad.translatedconnectPoints)
                                {
                                    if (transcp.Side == c.Side && transcp.Place == c.Place)
                                        _middleY += transcp.Location.Y;
                                }
                            }
                            _middleY = _middleY / _connectedLanes.Count + _isEven;

                            if (_roadPoint == _road.point1)
                            {
                                if(_road.Type == "Diagonal")
                                    controller.BuildDiagonalRoad(new Point(_Crosspoint.X, _middleY), _road.point2, _connectedLanes.Count, true, _road.endconnection, _Crossroad, _road.endConnectedTo);
                                else
                                    controller.BuildCurvedRoad(new Point(_Crosspoint.X, _middleY), _road.point2, _connectedLanes.Count, _road.Type, true, _road.endconnection, _Crossroad, _road.endConnectedTo);
                            }
                            else
                            {
                                if(_road.Type == "Diagonal")
                                    controller.BuildDiagonalRoad(_road.point1, new Point(_Crosspoint.X, _middleY), _connectedLanes.Count, _road.beginconnection, true, _road.beginConnectedTo, _Crossroad);
                                else
                                    controller.BuildCurvedRoad(_road.point1, new Point(_Crosspoint.X, _middleY), _connectedLanes.Count, _road.Type , _road.beginconnection, true, _road.beginConnectedTo, _Crossroad);
                            }
                        }
                    }

                    else 
                    {
                        if (_roadPoint == _road.point1)
                        {
                            foreach (DrivingLane d in _road.Drivinglanes)
                            {
                                d.beginConnectedTo.Clear();
                            }
                            foreach(CrossLane crosslane in _Crossroad.Drivinglanes)
                            {
                                foreach(ConnectionPoint c in _connectedLanes)
                                {
                                    if (crosslane.link.begin.Side == c.Side && crosslane.link.begin.Place == c.Place)
                                        crosslane.beginConnectedTo.Clear();
                                    else if (crosslane.link.end.Side == c.Side && crosslane.link.end.Place == c.Place)
                                        crosslane.endConnectedTo.Clear();
                                }
                            }
                        }
                        else
                        {
                            foreach (DrivingLane d in _road.Drivinglanes)
                            {
                                d.endConnectedTo.Clear();
                            }
                            foreach (CrossLane crosslane in _Crossroad.Drivinglanes)
                            {
                                foreach (ConnectionPoint c in _connectedLanes)
                                {
                                    if (crosslane.link.begin.Side == c.Side && crosslane.link.begin.Place == c.Place)
                                        crosslane.beginConnectedTo.Clear();
                                    else if (crosslane.link.end.Side == c.Side && crosslane.link.end.Place == c.Place)
                                        crosslane.endConnectedTo.Clear();
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
