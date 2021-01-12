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
    class Connection
    {
        List<AbstractRoad> roadlist;
        public Connection(Point _point1, Point _point2, int _lanes, string _dir, string _dir2, AbstractRoad _roadOne, AbstractRoad _roadTwo, int _count)
        {
            Console.WriteLine("-- Connection --");

            Point _temp1 = _roadOne.getPoint1();
            Point _temp2 = _roadOne.getPoint2();
            Point _temp3 = _roadTwo.getPoint1();
            Point _temp4 = _roadTwo.getPoint2();

            roadlist = General_Form.Main.BuildScreen.builder.roadBuilder.roads;

            if (_roadOne.roadtype == "DiagonalRoad" && _roadTwo.roadtype == "DiagonalRoad")
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

                if(_roadOne.slp == _roadTwo.slp)
                {
                    StraightandStraight(_point1, _point2, _lanes, _temp1, _temp2, _temp3, _temp4, _roadOne, _roadTwo, _roadOneEnds, _roadTwoEnds);
                }
                else //(_roadOne.slp != _roadTwo.slp)
                {

                    if ((_roadOne.slp == 0 && _roadTwo.slp != 0) || (_roadOne.slp != 0 && _roadTwo.slp == 0))
                    {
                        StraightandDiagonal(_point1, _point2, _lanes, _temp1, _temp2, _temp3, _temp4, _roadOne, _roadTwo, _roadOneEnds, _roadTwoEnds);
                    }
                }

            }
            else
            {

            }
        }

        public void StraightandStraight(Point _point1, Point _point2, int _lanes, Point _temp1, Point _temp2, Point _temp3, Point _temp4, AbstractRoad _roadOne, AbstractRoad _roadTwo, char _roadOneEnds, char _roadTwoEnds)
        {
            
            roadlist.Remove(_roadOne);

            roadlist.Remove(_roadTwo);

            if (_roadOne.slp == _roadTwo.slp && _roadOne.slp == 0)
            {
                if (_roadOneEnds == 'v' && _roadTwoEnds == 'v')
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(Math.Min(Math.Min(_temp1.X, _temp2.X), Math.Min(_temp3.X, _temp4.X)), _temp1.Y), new Point(Math.Max(Math.Max(_temp1.X, _temp2.X), Math.Max(_temp3.X, _temp4.X)), _temp1.Y), _lanes, _roadTwo.beginconnection, _roadTwo.endconnection);
                }
                else if (_roadOneEnds == 'h' && _roadTwoEnds == 'h')
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, Math.Min(Math.Min(_temp1.Y, _temp2.Y), Math.Min(_temp3.Y, _temp4.Y))), new Point(_temp1.X, Math.Max(Math.Max(_temp1.Y, _temp2.Y), Math.Max(_temp3.Y, _temp4.Y))), _lanes, _roadTwo.beginconnection, _roadTwo.endconnection);
                }
                else if (_roadOneEnds == 'h' && _roadTwoEnds == 'v')
                {
                    Point _curvedstart = new Point(0,0), _curvedend = new Point(0, 0);
                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if(_temp1.Y < _temp2.Y)
                        {
                            _curvedstart = new Point(_temp1.X, _temp1.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y + 18), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp1.X, _temp1.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y - 18), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedstart = new Point(_temp2.X, _temp2.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y + 18), _lanes, _roadOne.beginconnection, true);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp2.X, _temp2.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y - 18), _lanes, _roadOne.beginconnection, true);
                        }
                    }

                    if(Math.Abs(_temp3.X - _point2.X) < Math.Abs(_temp4.X - _point2.X))
                    {
                        if(_temp3.X < _temp4.X)
                        {
                            _curvedend = new Point(_temp3.X + 20, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + 18, _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                        else
                        {
                            _curvedend = new Point(_temp3.X - 20, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - 18, _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp3.X - _point2.X) > Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp4.X < _temp3.X)
                        {
                            _curvedend = new Point(_temp4.X + 20, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + 18, _temp4.Y), _lanes, _roadTwo.beginconnection, true);
                        }
                        else
                        {
                            _curvedend = new Point(_temp4.X - 20, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - 18, _temp4.Y), _lanes, _roadTwo.beginconnection, true);
                        }
                    }
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, true, true);
                }

                else if (_roadOneEnds == 'v' && _roadTwoEnds == 'h')
                {
                    Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);

                    if (Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp1.X + 20, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + 18, _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                        else
                        {
                            _curvedend = new Point(_temp1.X - 20, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - 18, _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp1.X - _point1.X) > Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp2.X + 20, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + 18, _temp2.Y), _lanes, _roadOne.beginconnection, true);
                        }
                        else
                        {
                            _curvedend = new Point(_temp2.X - 20, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - 18, _temp2.Y), _lanes, _roadOne.beginconnection, true);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            _curvedstart = new Point(_temp3.X, _temp3.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y + 18), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp3.X, _temp3.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y - 18), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp4.Y < _temp3.Y)
                        {
                            _curvedstart = new Point(_temp4.X, _temp4.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y + 18), _lanes, _roadTwo.beginconnection, true);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp4.X, _temp4.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y - 18), _lanes, _roadTwo.beginconnection, true);
                        }
                    }
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, true, true);
                }
                    Console.WriteLine(roadlist.Count);
            }

            else if (_roadOne.slp == _roadTwo.slp)
            {
                Console.WriteLine(roadlist.Count);
                if (_roadOne.slp > 0)
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(Math.Min(Math.Min(_temp1.X, _temp2.X), Math.Min(_temp3.X, _temp4.X)), Math.Min(Math.Min(_temp1.Y, _temp2.Y), Math.Min(_temp3.Y, _temp4.Y))),
                        new Point(Math.Max(Math.Max(_temp1.X, _temp2.X), Math.Max(_temp3.X, _temp4.X)), Math.Max(Math.Max(_temp1.Y, _temp2.Y), Math.Max(_temp3.Y, _temp4.Y))), _lanes, false, false);
                }
                else
                {
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(Math.Min(Math.Min(_temp1.X, _temp2.X), Math.Min(_temp3.X, _temp4.X)), Math.Max(Math.Max(_temp1.Y, _temp2.Y), Math.Max(_temp3.Y, _temp4.Y))),
                        new Point(Math.Max(Math.Max(_temp1.X, _temp2.X), Math.Max(_temp3.X, _temp4.X)), Math.Min(Math.Min(_temp1.Y, _temp2.Y), Math.Min(_temp3.Y, _temp4.Y))), _lanes, false, false);
                }
            }
        }

        public void StraightandDiagonal(Point _point1, Point _point2, int _lanes, Point _temp1, Point _temp2, Point _temp3, Point _temp4, AbstractRoad _roadOne, AbstractRoad _roadTwo, char _roadOneEnds, char _roadTwoEnds)
        {
            int _direction;
            bool _beginconnection, _endconnection;
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
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + _distance * _direction, _temp1.Y), _temp2, _lanes, _beginconnection, _endconnection);

                }
                else if (_roadOne.slp == 0 && Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) > Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)))
                {
                    if (_temp2.X < _temp1.X)
                    {
                        _direction = -1;
                        _beginconnection = true;
                        _endconnection = _roadOne.endconnection;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + _distance * _direction, _temp2.Y), _lanes, _beginconnection, _endconnection);

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
                        _beginconnection = _roadTwo.beginconnection;
                        _endconnection = true;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + _distance * _direction, _temp3.Y), _temp4, _lanes, _beginconnection, _endconnection);
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
                        _beginconnection = true;
                        _endconnection = _roadTwo.endconnection;
                    }


                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + _distance * _direction, _temp4.Y), _lanes, _beginconnection, _endconnection);
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
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y + _distance * _direction), _temp2, _lanes, _beginconnection, _endconnection);
                }
                else if (_roadOne.slp == 0 && Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) > Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)))
                {
                    if (_temp2.Y < _temp1.Y)
                    {
                        _direction = -1;
                        _beginconnection = true;
                        _endconnection = _roadOne.endconnection;
                    }
                    else
                    {
                        _direction = 1;
                        _beginconnection = _roadOne.beginconnection;
                        _endconnection = true;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadOne);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y + _distance * _direction), _lanes, _beginconnection, _endconnection);
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
                        _beginconnection = _roadTwo.beginconnection;
                        _endconnection = true;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y + _distance * _direction), _temp4, _lanes, _beginconnection, _endconnection);
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
                        _beginconnection = true;
                        _endconnection = _roadTwo.endconnection;
                    }

                    Console.WriteLine(_beginconnection + "---------" + _endconnection);
                    roadlist.Remove(_roadTwo);
                    General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y + _distance * _direction), _lanes, _beginconnection, _endconnection);
                }
            }

            else if(_roadOneEnds == 'h' && _roadTwoEnds == 'v')
            {
                roadlist.Remove(_roadOne);
                roadlist.Remove(_roadTwo);
                Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);

                if (_roadOne.slp == 0)
                {
                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedstart = new Point(_temp1.X, _temp1.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y + 18), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp1.X, _temp1.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X, _temp1.Y - 18), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedstart = new Point(_temp2.X, _temp2.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y + 18), _lanes, _roadOne.beginconnection, true);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp2.X, _temp2.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X, _temp2.Y - 18), _lanes, _roadOne.beginconnection, true);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            _curvedend = new Point(_temp3.X + 20, (int)(_temp3.Y + 20 * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + 19, (int)(_temp3.Y + 19 * _roadTwo.slp)), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                        else
                        {
                            _curvedend = new Point(_temp3.X - 20, (int)(_temp3.Y - 20 * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - 19, (int)(_temp3.Y - 19 * _roadTwo.slp)), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            _curvedend = new Point(_temp4.X - 20, (int)(_temp4.Y - 20 * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - 19, (int)(_temp4.Y - 19 * _roadTwo.slp)), _lanes, _roadTwo.beginconnection, true);
                        }
                        else
                        {
                            _curvedend = new Point(_temp4.X + 20, (int)(_temp4.Y + 20 * _roadTwo.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X + 19, (int)(_temp4.Y + 19 * _roadTwo.slp)), _lanes, _roadTwo.beginconnection, true);
                        }
                    }
                }

                else // if(_roadTwo.slp == 0)
                {
                    if (Math.Abs(_temp3.X - _point2.X) < Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            _curvedend = new Point(_temp3.X + 20, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X + 18, _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                        else
                        {
                            _curvedend = new Point(_temp3.X - 20, _temp3.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X - 18, _temp3.Y), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp3.X - _point2.X) > Math.Abs(_temp4.X - _point2.X))
                    {
                        if (_temp3.X < _temp4.X)
                        {
                            _curvedend = new Point(_temp4.X - 20, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X - 18, _temp4.Y), _lanes, _roadTwo.beginconnection, true);
                        }
                        else
                        {
                            _curvedend = new Point(_temp4.X + 20, _temp4.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad( _temp3, new Point(_temp4.X + 18, _temp4.Y), _lanes, _roadTwo.beginconnection, true);
                        }
                    }

                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedstart = new Point((int)(_temp1.X + 20 / _roadOne.slp), _temp1.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X + 19 / _roadOne.slp), _temp1.Y + 19), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp1.X - 20 / _roadOne.slp), _temp1.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp1.X - 19 / _roadOne.slp), _temp1.Y - 19), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.Y < _temp2.Y)
                        {
                            _curvedstart = new Point((int)(_temp2.X - 20 / _roadOne.slp), _temp2.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X - 19 / _roadOne.slp), _temp2.Y - 19), _lanes, _roadOne.beginconnection, true);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp2.X + 20 / _roadOne.slp), _temp2.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point((int)(_temp2.X + 19 / _roadOne.slp), _temp2.Y + 19), _lanes, _roadOne.beginconnection, true);
                        }
                    }
                }
                General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, true, true);
            }

            else if (_roadOneEnds == 'v' && _roadTwoEnds == 'h')
            {
                roadlist.Remove(_roadOne);
                roadlist.Remove(_roadTwo);
                Point _curvedstart = new Point(0, 0), _curvedend = new Point(0, 0);

                if (_roadTwo.slp == 0)
                {
                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            _curvedstart = new Point(_temp3.X, _temp3.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y + 18), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp3.X, _temp3.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp3.X, _temp3.Y - 18), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            _curvedstart = new Point(_temp4.X, _temp4.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y - 18), _lanes, _roadTwo.beginconnection, true);
                        }
                        else
                        {
                            _curvedstart = new Point(_temp4.X, _temp4.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp4.X, _temp4.Y + 18), _lanes, _roadTwo.beginconnection, true);
                        }
                    }

                    if (Math.Abs(_temp1.Y - _point1.Y) < Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp1.X + 20, (int)(_temp1.Y + 20 * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + 19, (int)(_temp1.Y + 19 * _roadOne.slp)), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                        else
                        {
                            _curvedend = new Point(_temp1.X - 20, (int)(_temp1.Y - 20 * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - 19, (int)(_temp1.Y - 19 * _roadOne.slp)), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp1.Y - _point1.Y) > Math.Abs(_temp2.Y - _point1.Y))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp2.X - 20, (int)(_temp2.Y - 20 * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - 19, (int)(_temp2.Y - 19 * _roadOne.slp)), _lanes, _roadOne.beginconnection, true);
                        }
                        else
                        {
                            _curvedend = new Point(_temp2.X + 20, (int)(_temp2.Y + 20 * _roadOne.slp));
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point(_temp2.X + 19, (int)(_temp2.Y + 19 * _roadOne.slp)), _lanes, _roadOne.beginconnection, true);
                        }
                    }
                }

                else // if(_roadOne.slp == 0)
                {
                    if (Math.Abs(_temp1.X - _point1.X) < Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp1.X + 20, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X + 18, _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                        else
                        {
                            _curvedend = new Point(_temp1.X - 20, _temp1.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point(_temp1.X - 18, _temp1.Y), _temp2, _lanes, true, _roadOne.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp1.X - _point1.X) > Math.Abs(_temp2.X - _point1.X))
                    {
                        if (_temp1.X < _temp2.X)
                        {
                            _curvedend = new Point(_temp2.X - 20, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X - 18, _temp2.Y), _lanes, _roadOne.beginconnection, true);
                        }
                        else
                        {
                            _curvedend = new Point(_temp2.X + 20, _temp2.Y);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp1, new Point(_temp2.X + 18, _temp2.Y), _lanes, _roadOne.beginconnection, true);
                        }
                    }

                    if (Math.Abs(_temp3.Y - _point2.Y) < Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            _curvedstart = new Point((int)(_temp3.X + 20 / _roadTwo.slp), _temp3.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X + 19 / _roadTwo.slp), _temp3.Y + 19), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp3.X - 20 / _roadTwo.slp), _temp3.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(new Point((int)(_temp3.X - 19 / _roadTwo.slp), _temp3.Y - 19), _temp4, _lanes, true, _roadTwo.endconnection);
                        }
                    }
                    else if (Math.Abs(_temp3.Y - _point2.Y) > Math.Abs(_temp4.Y - _point2.Y))
                    {
                        if (_temp3.Y < _temp4.Y)
                        {
                            _curvedstart = new Point((int)(_temp4.X - 20 / _roadTwo.slp), _temp4.Y - 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X - 19 / _roadTwo.slp), _temp4.Y - 19), _lanes, _roadTwo.beginconnection, true);
                        }
                        else
                        {
                            _curvedstart = new Point((int)(_temp4.X + 20 / _roadTwo.slp), _temp4.Y + 20);
                            General_Form.Main.BuildScreen.builder.roadBuilder.BuildDiagonalRoad(_temp3, new Point((int)(_temp4.X + 19 / _roadTwo.slp), _temp4.Y + 19), _lanes, _roadTwo.beginconnection, true);
                        }
                    }
                }
                General_Form.Main.BuildScreen.builder.roadBuilder.BuildCurvedRoad(_curvedstart, _curvedend, _lanes, true, true);
            }
        }
    }
}
