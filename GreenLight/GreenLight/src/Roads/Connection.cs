﻿using System.ComponentModel;
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
    class Connection
    {
        int _shift = 20;
        public Connection(Point _point1, Point _point2, int _lanes, string _dir, string _dir2, AbstractRoad _roadOne, AbstractRoad _roadTwo, int _count)
        {
            Console.WriteLine("-- Connection --");

            Point _temp1 = _roadOne.getPoint1();
            Point _temp2 = _roadOne.getPoint2();
            Point _temp3 = _roadTwo.getPoint1();
            Point _temp4 = _roadTwo.getPoint2();

            if (_lanes > 3)
            {
                _shift = 60;
            }
            else if(_lanes > 1)
            {
                _shift = 40;
            }

            if (_roadOne.Type == "Diagonal" && _roadTwo.Type == "Diagonal")
            {
                char _roadOneEnds;
                char _roadTwoEnds;
                if ((_roadOne.slp > -1 && _roadOne.slp < 1 && _roadOne.slp != 0) || (_roadOne.slp == 0 && _temp1.Y == _temp2.Y))
                    _roadOneEnds = 'v';
                else
                    _roadOneEnds = 'h';
                if ((_roadTwo.slp > -1 && _roadTwo.slp < 1 && _roadTwo.slp != 0) || (_roadTwo.slp == 0 && _temp3.Y == _temp4.Y))
                    _roadTwoEnds = 'v';
                else
                    _roadTwoEnds = 'h';

                Console.WriteLine(_roadOneEnds + "--------" + _roadTwoEnds);

                if (_roadOne.slp == _roadTwo.slp)
                {
                    StraightandStraight(_point1, _point2, _lanes, _temp1, _temp2, _temp3, _temp4, _roadOne, _roadTwo, _roadOneEnds, _roadTwoEnds);
                }
                else //(_roadOne.slp != _roadTwo.slp)
                {

                    if ((_roadOne.slp == 0 && _roadTwo.slp != 0) || (_roadOne.slp != 0 && _roadTwo.slp == 0))
                    {
                        StraightandDiagonal(_point1, _point2, _lanes, _temp1, _temp2, _temp3, _temp4, _roadOne, _roadTwo, _roadOneEnds, _roadTwoEnds);
                    }
                    else //(_roadOne.slp !=0 && _roadTwo.slp !=0)
                    {
                        DiagonalandDiagonal(_point1, _point2, _lanes, _temp1, _temp2, _temp3, _temp4, _roadOne, _roadTwo, _roadOneEnds, _roadTwoEnds);
                    }
                }

            }
            else
            {

            }
        }

        public void StraightandStraight(Point _point1, Point _point2, int _lanes, Point _temp1, Point _temp2, Point _temp3, Point _temp4, AbstractRoad _roadOne, AbstractRoad _roadTwo, char _roadOneEnds, char _roadTwoEnds)
        {
            RoadController.roads.Remove(_roadOne);
            RoadController.roads.Remove(_roadTwo);
            string _curvedType = "Curved";
            if (_roadOne.slp == _roadTwo.slp && _roadOne.slp == 0)
            {
                if (_roadOneEnds == 'v' && _roadTwoEnds == 'v')
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(Math.Min(Math.Min(_temp1.X, _temp2.X), Math.Min(_temp3.X, _temp4.X)), _temp1.Y), new Point(Math.Max(Math.Max(_temp1.X, _temp2.X), Math.Max(_temp3.X, _temp4.X)), _temp1.Y), _lanes, _roadTwo.beginconnection, _roadTwo.endconnection, _roadTwo.beginConnectedTo, _roadTwo.endConnectedTo);
                }
                else if (_roadOneEnds == 'h' && _roadTwoEnds == 'h')
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, Math.Min(Math.Min(_temp1.Y, _temp2.Y), Math.Min(_temp3.Y, _temp4.Y))), new Point(_temp1.X, Math.Max(Math.Max(_temp1.Y, _temp2.Y), Math.Max(_temp3.Y, _temp4.Y))), _lanes, _roadTwo.beginconnection, _roadTwo.endconnection, _roadTwo.beginConnectedTo, _roadTwo.endConnectedTo);
                }
                else if (_roadOneEnds == 'h' && _roadTwoEnds == 'v')
                {
                    Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);
                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp1.X, _temp1.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y + _shift - 2), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp1.X, _temp1.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y - (_shift - 2)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp2.X, _temp2.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y - (_shift - 2)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp2.X, _temp2.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y + _shift - 2), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }

                    if (Math.Abs(_temp3.X - _point2.X) < Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            _curvedend = new Point(_temp3.X + _shift, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + _shift - 2, _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            _curvedend = new Point(_temp3.X - _shift, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - (_shift - 2), _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp3.X - _point2.X) > Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp4.X < _temp3.X)
                        {
                            _curvedend = new Point(_temp4.X + _shift, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + _shift - 2, _temp4.Y), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, _roadOne);
                        }
                        else
                        {
                            _curvedend = new Point(_temp4.X - _shift, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - (_shift - 2), _temp4.Y), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, _roadOne);
                        }
                    }
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, _curvedType, true, true, RoadController.roads[RoadController.roads.Count - 2], RoadController.roads[RoadController.roads.Count - 1]);
                }

                else if (_roadOneEnds == 'v' && _roadTwoEnds == 'h')
                {
                    Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);
                    if (Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp1.X + _shift, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + _shift - 2, _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            _curvedend = new Point(_temp1.X - _shift, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - (_shift - 2), _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp1.X - _point1.X) > Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp2.X - _shift, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - _shift + 2, _temp2.Y), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedend = new Point(_temp2.X + _shift, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + (_shift - 2), _temp2.Y), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp3.X, _temp3.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y + _shift - 2), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp3.X, _temp3.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y - (_shift - 2)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp4.Y < _temp3.Y)
                        {
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp4.X, _temp4.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y + _shift - 2), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp4.X, _temp4.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y - (_shift - 2)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, _curvedType, true, true, RoadController.roads[RoadController.roads.Count - 1], RoadController.roads[RoadController.roads.Count - 2]);
                }
                Console.WriteLine(_curvedType);
            }

            else if (_roadOne.slp == _roadTwo.slp)
            {
                Console.WriteLine(RoadController.roads.Count);
                if (_roadOne.slp > 0)
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(Math.Min(Math.Min(_temp1.X, _temp2.X), Math.Min(_temp3.X, _temp4.X)), Math.Min(Math.Min(_temp1.Y, _temp2.Y), Math.Min(_temp3.Y, _temp4.Y))),
                        new Point(Math.Max(Math.Max(_temp1.X, _temp2.X), Math.Max(_temp3.X, _temp4.X)), Math.Max(Math.Max(_temp1.Y, _temp2.Y), Math.Max(_temp3.Y, _temp4.Y))), _lanes, _roadTwo.beginconnection, _roadTwo.endconnection, _roadTwo.beginConnectedTo, _roadTwo.endConnectedTo);
                }
                else
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(Math.Min(Math.Min(_temp1.X, _temp2.X), Math.Min(_temp3.X, _temp4.X)), Math.Max(Math.Max(_temp1.Y, _temp2.Y), Math.Max(_temp3.Y, _temp4.Y))),
                        new Point(Math.Max(Math.Max(_temp1.X, _temp2.X), Math.Max(_temp3.X, _temp4.X)), Math.Min(Math.Min(_temp1.Y, _temp2.Y), Math.Min(_temp3.Y, _temp4.Y))), _lanes, _roadTwo.beginconnection, _roadTwo.endconnection, _roadTwo.beginConnectedTo, _roadTwo.endConnectedTo);
                }
            }
        }

        public void StraightandDiagonal(Point _point1, Point _point2, int _lanes, Point _temp1, Point _temp2, Point _temp3, Point _temp4, AbstractRoad _roadOne, AbstractRoad _roadTwo, char _roadOneEnds, char _roadTwoEnds)
        {
            int _direction;
            bool _beginconnection, _endconnection;
            AbstractRoad _beginConnectedTo, _endConnectedTo;
            string _curvedType = "Curved";

            if (_roadOneEnds == 'v' && _roadTwoEnds == 'v')
            {
                int _distance = Math.Abs(_point1.X - _point2.X) + 1;
                Console.WriteLine(_distance);

                if (_roadOne.slp == 0 && Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) < Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)))
                {
                    if (_temp1.X < _temp2.X)
                    {
                        _direction = -1;
                        _beginconnection = true;
                        _endconnection = _roadOne.endconnection;
                    }

                    else
                    {
                        _direction = 1;
                        _beginconnection = true;
                        _endconnection = _roadOne.endconnection;
                    }

                    _beginConnectedTo = _roadTwo;
                    _endConnectedTo = _roadOne.endConnectedTo;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + _distance * _direction, _temp1.Y), _temp2, _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);

                }
                else if (_roadOne.slp == 0 && Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) > Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)))
                {
                    if (_temp2.X < _temp1.X)
                    {
                        _direction = -1;
                        _beginconnection = _roadOne.beginconnection; ;
                        _endconnection = true;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }

                    _beginConnectedTo = _roadOne.beginConnectedTo;
                    _endConnectedTo = _roadTwo;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + _distance * _direction, _temp2.Y), _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);

                }
                else if (_roadTwo.slp == 0 && Math.Sqrt(Math.Pow(_point2.X - _temp3.X, 2) + Math.Pow(_point2.Y - _temp3.Y, 2)) < Math.Sqrt(Math.Pow(_point2.X - _temp4.X, 2) + Math.Pow(_point2.Y - _temp4.Y, 2)))
                {
                    if (_temp3.X < _temp4.X)
                    {
                        _direction = -1;
                        _beginconnection = true;
                        _endconnection = _roadTwo.endconnection;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = true;
                        _endconnection = _roadTwo.endconnection;
                    }

                    _beginConnectedTo = _roadOne;
                    _endConnectedTo = _roadTwo.endConnectedTo;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + _distance * _direction, _temp3.Y), _temp4, _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
                else
                {
                    if (_temp4.X < _temp3.X)
                    {
                        _direction = -1;
                        _beginconnection = _roadTwo.beginconnection;
                        _endconnection = true;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = _roadTwo.beginconnection;
                        _endconnection = true;
                    }

                    _beginConnectedTo = _roadTwo.beginConnectedTo;
                    _endConnectedTo = _roadOne;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + _distance * _direction, _temp4.Y), _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
            }

            else if (_roadOneEnds == 'h' && _roadTwoEnds == 'h')
            {
                int _distance = Math.Abs(_point1.Y - _point2.Y) + 1;
                Console.WriteLine(_distance);

                if (_roadOne.slp == 0 && Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) < Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)))
                {
                    if (_temp1.Y < _temp2.Y)
                    {
                        _direction = -1;
                        _beginconnection = true;
                        _endconnection = _roadOne.endconnection;
                    }

                    else
                    {
                        _direction = 1;
                        _beginconnection = true;
                        _endconnection = _roadOne.endconnection;
                    }

                    _beginConnectedTo = _roadTwo;
                    _endConnectedTo = _roadOne.endConnectedTo;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y + _distance * _direction), _temp2, _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
                else if (_roadOne.slp == 0 && Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) > Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)))
                {
                    if (_temp2.Y < _temp1.Y)
                    {
                        _direction = -1;
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }

                    _beginConnectedTo = _roadOne.beginConnectedTo;
                    _endConnectedTo = _roadTwo;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y + _distance * _direction), _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
                else if (_roadTwo.slp == 0 && Math.Sqrt(Math.Pow(_point2.X - _temp3.X, 2) + Math.Pow(_point2.Y - _temp3.Y, 2)) < Math.Sqrt(Math.Pow(_point2.X - _temp4.X, 2) + Math.Pow(_point2.Y - _temp4.Y, 2)))
                {
                    if (_temp3.Y < _temp4.Y)
                    {
                        _direction = -1;
                        _beginconnection = true;
                        _endconnection = _roadTwo.endconnection;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = true;
                        _endconnection = _roadTwo.endconnection;
                    }

                    _beginConnectedTo = _roadOne;
                    _endConnectedTo = _roadTwo.endConnectedTo;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y + _distance * _direction), _temp4, _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
                else
                {
                    if (_temp4.Y < _temp3.Y)
                    {
                        _direction = -1;
                        _beginconnection = _roadTwo.beginconnection;
                        _endconnection = true;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = _roadTwo.beginconnection;
                        _endconnection = true;
                    }

                    _beginConnectedTo = _roadTwo.beginConnectedTo;
                    _endConnectedTo = _roadOne;
                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    RoadController.roads.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y + _distance * _direction), _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
            }

            else if (_roadOneEnds == 'h' && _roadTwoEnds == 'v')
            {
                RoadController.roads.Remove(_roadOne);
                RoadController.roads.Remove(_roadTwo);
                Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);

                if (_roadOne.slp == 0)
                {
                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            Console.WriteLine(1.1);
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp1.X, _temp1.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y + _shift - 2), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.beginConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(1.2);
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp1.X, _temp1.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y - (_shift - 2)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.beginConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            Console.WriteLine(1.3);
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp2.X, _temp2.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y + _shift - 2), _lanes, _roadOne.beginconnection, true, _roadOne.endConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(1.4);
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp2.X, _temp2.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y - (_shift - 2)), _lanes, _roadOne.beginconnection, true, _roadOne.endConnectedTo, null);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            Console.WriteLine(1.5);
                            _curvedend = new Point(_temp3.X + _shift, (int)(_temp3.Y + _shift * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + (_shift - 1), (int)(_temp3.Y + (_shift - 1) * _roadTwo.slp)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(1.6);
                            _curvedend = new Point(_temp3.X - _shift, (int)(_temp3.Y - _shift * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - (_shift - 1), (int)(_temp3.Y - (_shift - 1) * _roadTwo.slp)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            Console.WriteLine(1.7);
                            _curvedend = new Point(_temp4.X - _shift, (int)(_temp4.Y - _shift * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - (_shift - 1), (int)(_temp4.Y - (_shift - 1) * _roadTwo.slp)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(1.8);
                            _curvedend = new Point(_temp4.X + _shift, (int)(_temp4.Y + _shift * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + (_shift - 1), (int)(_temp4.Y + (_shift - 1) * _roadTwo.slp)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                }

                else // if(_roadTwo.slp == 0)
                {
                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            Console.WriteLine(2.1);
                            _curvedType = "Curved";
                            _curvedstart = new Point((int)(_temp1.X + _shift / _roadOne.slp), _temp1.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X + (_shift - 1) / _roadOne.slp), _temp1.Y + (_shift - 1)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(2.2);
                            _curvedType = "Curved2";
                            _curvedstart = new Point((int)(_temp1.X - _shift / _roadOne.slp), _temp1.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X - (_shift - 1) / _roadOne.slp), _temp1.Y - (_shift - 1)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            Console.WriteLine(2.3);
                            _curvedType = "Curved2";
                            _curvedstart = new Point((int)(_temp2.X - _shift / _roadOne.slp), _temp2.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X - (_shift - 1) / _roadOne.slp), _temp2.Y - (_shift - 1)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(2.4);
                            _curvedType = "Curved";
                            _curvedstart = new Point((int)(_temp2.X + _shift / _roadOne.slp), _temp2.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X + (_shift - 1) / _roadOne.slp), _temp2.Y + (_shift - 1)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }

                    if (Math.Abs(_temp3.X - _point2.X) < Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            Console.WriteLine(2.5);
                            _curvedend = new Point(_temp3.X + _shift, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + (_shift - 2), _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(2.6);
                            _curvedend = new Point(_temp3.X - _shift, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - (_shift - 2), _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp3.X - _point2.X) > Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            Console.WriteLine(2.7);
                            _curvedend = new Point(_temp4.X - _shift, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - (_shift - 2), _temp4.Y), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(2.8);
                            _curvedend = new Point(_temp4.X + _shift, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + (_shift - 2), _temp4.Y), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                }
                General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, _curvedType, true, true, RoadController.roads[RoadController.roads.Count - 2], RoadController.roads[RoadController.roads.Count - 1]);
            }

            else if (_roadOneEnds == 'v' && _roadTwoEnds == 'h')
            {
                RoadController.roads.Remove(_roadOne);
                RoadController.roads.Remove(_roadTwo);
                Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);

                if (_roadTwo.slp == 0)
                {
                    if (Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            Console.WriteLine(3.1);
                            _curvedend = new Point(_temp1.X + _shift, (int)(_temp1.Y + _shift * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + (_shift - 1), (int)(_temp1.Y + (_shift - 1) * _roadOne.slp)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(3.2);
                            _curvedend = new Point(_temp1.X - _shift, (int)(_temp1.Y - _shift * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - (_shift - 1), (int)(_temp1.Y - (_shift - 1) * _roadOne.slp)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp1.X - _point1.X) > Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            Console.WriteLine(3.3);
                            _curvedend = new Point(_temp2.X - _shift, (int)(_temp2.Y - _shift * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - (_shift - 1), (int)(_temp2.Y - (_shift - 1) * _roadOne.slp)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(3.4);
                            _curvedend = new Point(_temp2.X + _shift, (int)(_temp2.Y + _shift * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + (_shift - 1), (int)(_temp2.Y + (_shift - 1) * _roadOne.slp)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            Console.WriteLine(3.5);
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp3.X, _temp3.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y + (_shift - 2)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(3.6);
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp3.X, _temp3.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y - (_shift - 2)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            Console.WriteLine(3.7);
                            _curvedType = "Curved2";
                            _curvedstart = new Point(_temp4.X, _temp4.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y - (_shift - 2)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(3.8);
                            _curvedType = "Curved";
                            _curvedstart = new Point(_temp4.X, _temp4.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y + (_shift - 2)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                }

                else // if(_roadOne.slp == 0)
                {
                    if (Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            Console.WriteLine(4.1);
                            _curvedend = new Point(_temp1.X + _shift, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + (_shift - 2), _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(4.2);
                            _curvedend = new Point(_temp1.X - _shift, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - (_shift - 2), _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp1.X - _point1.X) > Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            Console.WriteLine(4.3);
                            _curvedend = new Point(_temp2.X - _shift, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - (_shift - 2), _temp2.Y), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(4.4);
                            _curvedend = new Point(_temp2.X + _shift, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + (_shift - 2), _temp2.Y), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            Console.WriteLine(4.5);
                            _curvedType = "Curved";
                            _curvedstart = new Point((int)(_temp3.X + _shift / _roadTwo.slp), _temp3.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X + (_shift - 1) / _roadTwo.slp), _temp3.Y + (_shift - 1)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            Console.WriteLine(4.6);
                            _curvedType = "Curved2";
                            _curvedstart = new Point((int)(_temp3.X - _shift / _roadTwo.slp), _temp3.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X - (_shift - 1) / _roadTwo.slp), _temp3.Y - (_shift - 1)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            Console.WriteLine(4.7);
                            _curvedType = "Curved2";
                            _curvedstart = new Point((int)(_temp4.X - _shift / _roadTwo.slp), _temp4.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X - (_shift - 1) / _roadTwo.slp), _temp4.Y - (_shift - 1)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            Console.WriteLine(4.8);
                            _curvedType = "Curved";
                            _curvedstart = new Point((int)(_temp4.X + _shift / _roadTwo.slp), _temp4.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X + (_shift - 1) / _roadTwo.slp), _temp4.Y + (_shift - 1)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                }
                General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, _curvedType, true, true, RoadController.roads[RoadController.roads.Count - 1], RoadController.roads[RoadController.roads.Count - 2]);
            }
        }

        public void DiagonalandDiagonal(Point _point1, Point _point2, int _lanes, Point _temp1, Point _temp2, Point _temp3, Point _temp4, AbstractRoad _roadOne, AbstractRoad _roadTwo, char _roadOneEnds, char _roadTwoEnds)
        {
            double _distance = Math.Sqrt(Math.Pow(_point1.X - _point2.X, 2) + Math.Pow(_point1.Y - _point2.Y, 2));
            int _direction = 1;
            bool _beginconnection, _endconnection;
            AbstractRoad _beginConnectedTo, _endConnectedTo;

            if (_roadOneEnds == 'v' && _roadTwoEnds == 'v')
            {
                if(Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                {
                    if (_temp1.X < _temp2.X)
                        _direction = -2;
                    else
                        _direction = 2;

                    _beginconnection = true;
                    _endconnection = _roadOne.endconnection;

                    _beginConnectedTo = _roadTwo;
                    _endConnectedTo = _roadOne.endConnectedTo;

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);

                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_point2.X + _direction, _point2.Y), _temp2, _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
                else
                {
                    if (_temp1.X < _temp2.X)
                        _direction = 2;
                    else
                        _direction = -2;

                    _beginconnection = _roadOne.beginconnection;
                    _endconnection = true;

                    _beginConnectedTo = _roadOne.beginConnectedTo;
                    _endConnectedTo = _roadTwo;

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);

                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_point2.X + _direction, _point2.Y), _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
            }

            else if (_roadOneEnds == 'h' && _roadTwoEnds == 'h')
            {
                if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                {
                    if (_temp1.Y < _temp2.Y)
                        _direction = -2;
                    else
                        _direction = 2;

                    _beginconnection = true;
                    _endconnection = _roadOne.endconnection;

                    _beginConnectedTo = _roadTwo;
                    _endConnectedTo = _roadOne.endConnectedTo;

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);

                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_point2.X, _point2.Y + _direction), _temp2, _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
                else
                {
                    if (_temp1.Y < _temp2.Y)
                        _direction = 2;
                    else
                        _direction = -2;

                    _beginconnection = _roadOne.beginconnection;
                    _endconnection = true;

                    _beginConnectedTo = _roadOne.beginConnectedTo;
                    _endConnectedTo = _roadTwo;

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);

                    RoadController.roads.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_point2.X, _point2.Y + _direction), _lanes, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
                }
            }

            else if (_roadOneEnds == 'v' && _roadTwoEnds == 'h')
            {
                RoadController.roads.Remove(_roadOne);
                RoadController.roads.Remove(_roadTwo);
                Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);
                string _curveType = "Curved";

                if (Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                {
                    if (_temp1.X < _temp2.X)
                    {
                        Console.WriteLine(1.1);
                        if (_point2.Y == Math.Min(_temp3.Y, _temp4.Y))
                        {
                            _curvedend = new Point(_temp1.X + _shift, (int)(_temp1.Y - _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + (_shift - 2), (int)(_temp1.Y - (_shift - 2) * Math.Abs(_roadOne.slp))), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            _curvedend = new Point(_temp1.X + _shift, (int)(_temp1.Y + _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + (_shift - 2), (int)(_temp1.Y + (_shift - 2) * Math.Abs(_roadOne.slp))), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else
                    {
                        Console.WriteLine(1.2);
                        if (_point2.Y == Math.Min(_temp3.Y, _temp4.Y))
                        {
                            _curvedend = new Point(_temp1.X - _shift, (int)(_temp1.Y - _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - (_shift - 2), (int)(_temp1.Y - (_shift - 2) * Math.Abs(_roadOne.slp))), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            _curvedend = new Point(_temp1.X - _shift, (int)(_temp1.Y + _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - (_shift - 2), (int)(_temp1.Y + (_shift - 2) * Math.Abs(_roadOne.slp))), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                }
                else if (Math.Abs(_temp1.X - _point1.X) > Math.Abs(_temp2.X - _point1.X))
                {
                    if (_temp1.X < _temp2.X)
                    {
                        Console.WriteLine(1.3);
                        if (_point2.Y == Math.Min(_temp3.Y, _temp4.Y))
                        {
                            _curvedend = new Point(_temp2.X - _shift, (int)(_temp2.Y - _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - (_shift - 2), (int)(_temp2.Y - (_shift - 2) * Math.Abs(_roadOne.slp))), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedend = new Point(_temp2.X - _shift, (int)(_temp2.Y + _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - (_shift - 2), (int)(_temp2.Y + (_shift - 2) * Math.Abs(_roadOne.slp))), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }
                    else
                    {
                        Console.WriteLine(1.4);
                        if(_point2.Y == Math.Min(_temp3.Y, _temp4.Y))
                        {
                            _curvedend = new Point(_temp2.X + _shift, (int)(_temp2.Y - _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + (_shift - 2), (int)(_temp2.Y - (_shift - 2) * Math.Abs(_roadOne.slp))), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedend = new Point(_temp2.X + _shift, (int)(_temp2.Y + _shift * Math.Abs(_roadOne.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + (_shift - 2), (int)(_temp2.Y + (_shift - 2) * Math.Abs(_roadOne.slp))), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }
                }
                ////////////////////////
                if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                {
                    if (_temp3.Y < _temp4.Y)
                    {
                        Console.WriteLine(1.5);
                        _curveType = "Curved";
                        if (_point1.X == Math.Min(_temp1.X, _temp2.X))
                        {
                            _curvedstart = new Point((int)(_temp3.X - _shift / Math.Abs(_roadTwo.slp)), _temp3.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X - (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp3.Y + (_shift - 1)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp3.X + _shift / Math.Abs(_roadTwo.slp)), _temp3.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X + (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp3.Y + (_shift - 1)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else
                    {
                        _curveType = "Curved2";
                        Console.WriteLine(1.6);
                        if (_point1.X == Math.Min(_temp1.X, _temp2.X))
                        {
                            _curvedstart = new Point((int)(_temp3.X - _shift / Math.Abs(_roadTwo.slp)), _temp3.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X - (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp3.Y - (_shift - 1)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp3.X + _shift / Math.Abs(_roadTwo.slp)), _temp3.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X + (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp3.Y - (_shift - 1)), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                }
                else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                {
                    _curveType = "Curved2";
                    if (_temp3.Y < _temp4.Y)
                    {
                        Console.WriteLine(1.7);
                        if (_point1.X == Math.Min(_temp1.X, _temp2.X))
                        {
                            _curvedstart = new Point((int)(_temp4.X - _shift / Math.Abs(_roadTwo.slp)), _temp4.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X - (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp4.Y - (_shift - 1)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp4.X + _shift / Math.Abs(_roadTwo.slp)), _temp4.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X + (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp4.Y - (_shift - 1)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                    else
                    {
                        _curveType = "Curved";
                        Console.WriteLine(1.8);
                        if (_point1.X == Math.Min(_temp1.X, _temp2.X))
                        {
                            _curvedstart = new Point((int)(_temp4.X - _shift / Math.Abs(_roadTwo.slp)), _temp4.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X - (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp4.Y + (_shift - 1)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp4.X + _shift / Math.Abs(_roadTwo.slp)), _temp4.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X + (_shift - 1) / Math.Abs(_roadTwo.slp)), _temp4.Y + (_shift - 1)), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                }
                General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, _curveType, true, true, RoadController.roads[RoadController.roads.Count - 1], RoadController.roads[RoadController.roads.Count - 2]);
            }

            else if (_roadOneEnds == 'h' && _roadTwoEnds == 'v')
            {
                RoadController.roads.Remove(_roadOne);
                RoadController.roads.Remove(_roadTwo);
                Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);
                string _curveType = "Curved";

                if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                {
                    if (_temp1.Y < _temp2.Y)
                    {
                        _curveType = "Curved";
                        Console.WriteLine(2.1);
                        if (_point2.X == Math.Min(_temp3.X, _temp4.X))
                        {
                            _curvedstart = new Point((int)(_temp1.X - _shift / Math.Abs(_roadOne.slp)), (_temp1.Y + _shift));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X - (_shift - 2) / Math.Abs(_roadOne.slp)), _temp1.Y + (_shift - 2)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp1.X + _shift / Math.Abs(_roadOne.slp)), (_temp1.Y + _shift));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X + (_shift - 2) / Math.Abs(_roadOne.slp)), _temp1.Y + (_shift - 2)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                    else
                    {
                        Console.WriteLine(2.2);

                        _curveType = "Curved2";
                        if (_point2.X == Math.Min(_temp3.X, _temp4.X))
                        {
                            _curvedstart = new Point((int)(_temp1.X - _shift / Math.Abs(_roadOne.slp)), (_temp1.Y - _shift));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X - (_shift - 2) / Math.Abs(_roadOne.slp)), _temp1.Y - (_shift - 2)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp1.X + _shift / Math.Abs(_roadOne.slp)), (_temp1.Y - _shift));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X + (_shift - 2) / Math.Abs(_roadOne.slp)), _temp1.Y - (_shift - 2)), _temp2, _lanes, true, _roadOne.endconnection, null, _roadOne.endConnectedTo);
                        }
                    }
                }
                else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                {
                    if (_temp1.Y < _temp2.Y)
                    {
                        Console.WriteLine(2.3);
                        _curveType = "Curved2";
                        if (_point2.X == Math.Min(_temp3.X, _temp4.X))
                        {
                            _curvedstart = new Point((int)(_temp2.X - _shift / Math.Abs(_roadOne.slp)), _temp2.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X - (_shift - 2) / Math.Abs(_roadOne.slp)), _temp2.Y - (_shift - 2)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp2.X + _shift / Math.Abs(_roadOne.slp)), _temp2.Y - _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X + (_shift - 2) / Math.Abs(_roadOne.slp)), _temp2.Y - (_shift - 2)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }
                    else
                    {
                        Console.WriteLine(2.4);
                        _curveType = "Curved";
                        if (_point2.X == Math.Min(_temp3.X, _temp4.X))
                        {
                            _curvedstart = new Point((int)(_temp2.X - _shift / Math.Abs(_roadOne.slp)), _temp2.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X - (_shift - 2) / Math.Abs(_roadOne.slp)), _temp2.Y + (_shift - 2)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp2.X + _shift / Math.Abs(_roadOne.slp)), _temp2.Y + _shift);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X + (_shift - 2) / Math.Abs(_roadOne.slp)), _temp2.Y + (_shift - 2)), _lanes, _roadOne.beginconnection, true, _roadOne.beginConnectedTo, null);
                        }
                    }
                }

                // _roadTwo 

                if (Math.Abs(_temp3.X - _point2.X) < Math.Abs(_temp4.X - _point2.X))
                {
                    if (_temp3.X < _temp4.X)
                    {
                        Console.WriteLine(2.5);
                        if (_point1.Y == Math.Min(_temp1.Y, _temp2.Y))
                        {
                            _curvedend = new Point(_temp3.X + _shift, (int)(_temp3.Y - _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + (_shift - 1), (int)(_temp3.Y - (_shift - 1) * Math.Abs(_roadTwo.slp))), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            _curvedend = new Point(_temp3.X + _shift, (int)(_temp3.Y + _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + (_shift -1), (int)(_temp3.Y + (_shift -1) * Math.Abs(_roadTwo.slp))), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                    else
                    {
                        Console.WriteLine(2.6);
                        if (_point1.Y == Math.Min(_temp1.Y, _temp2.Y))
                        {
                            _curvedend = new Point(_temp3.X - _shift, (int)(_temp3.Y - _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - (_shift - 1), (int)(_temp3.Y - (_shift - 1) * Math.Abs(_roadTwo.slp))), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                        else
                        {
                            _curvedend = new Point(_temp3.X - _shift, (int)(_temp3.Y + _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - (_shift - 1), (int)(_temp3.Y + (_shift - 1) * Math.Abs(_roadTwo.slp))), _temp4, _lanes, true, _roadTwo.endconnection, null, _roadTwo.endConnectedTo);
                        }
                    }
                }
                else if (Math.Abs(_temp3.X - _point2.X) > Math.Abs(_temp4.X - _point2.X))
                {
                    if (_temp3.X < _temp4.X)
                    {
                        Console.WriteLine(2.7);
                        if (_point1.Y == Math.Min(_temp1.Y, _temp2.Y))
                        {
                            _curvedend = new Point(_temp4.X - _shift, (int)(_temp4.Y - _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - (_shift - 1), (int)(_temp4.Y - (_shift - 1) * Math.Abs(_roadTwo.slp))), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedend = new Point(_temp4.X - _shift, (int)(_temp4.Y + _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - (_shift - 1), (int)(_temp4.Y + (_shift - 1) * Math.Abs(_roadTwo.slp))), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                    else
                    {
                        Console.WriteLine(2.8);
                        if (_point1.X == Math.Min(_temp1.Y, _temp2.Y))
                        {
                            _curvedend = new Point(_temp4.X + _shift, (int)(_temp4.Y - _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + (_shift - 1), (int)(_temp4.Y - (_shift - 1) * Math.Abs(_roadTwo.slp))), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                        else
                        {
                            _curvedend = new Point(_temp4.X + _shift, (int)(_temp4.Y + _shift * Math.Abs(_roadTwo.slp)));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + (_shift - 1), (int)(_temp4.Y + (_shift - 1) * Math.Abs(_roadTwo.slp))), _lanes, _roadTwo.beginconnection, true, _roadTwo.beginConnectedTo, null);
                        }
                    }
                }
                General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, _curveType, true, true, RoadController.roads[RoadController.roads.Count - 1], RoadController.roads[RoadController.roads.Count - 2]);


            }


        }
    }
}

