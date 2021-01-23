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

            if (_roadOne.Type == "Cross" && _roadTwo.Type == "Cross")
            {
                CrossandCross();
            }
            else if ((_roadOne.Type == "Cross" && _roadTwo.Type == "Diagonal") || (_roadOne.Type == "Diagonal" && _roadTwo.Type == "Cross"))
            {
                if((_roadTwo.Type == "Diagonal" && _roadTwo.slp == 0) || (_roadOne.Type == "Diagonal" && _roadOne.slp == 0))
                {
                    CrossandStraight();
                }
                else
                {
                    CrossandDiagonal();
                }
            }
            else if ((_roadOne.Type == "Cross" && (_roadTwo.Type == "Curved" || _roadTwo.Type == "Curved2")) || ((_roadOne.Type == "Curved" || _roadOne.Type == "Curved2") && _roadTwo.Type == "Cross"))
            {
                CrossandCurved();
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

            for (int t = 0; t < Math.Max(roadOne.lanes, roadTwo.lanes) - 1 && _buildroad; t++)
            {
                for (int x = 0; x <= 1; x++)
                {
                    if (x == 0)
                        _dir = -1;
                    else
                        _dir = 1;

                    _place = _cp.Place + t * _dir;
                    _place2 = _cp2.Place + t * _dir;

                    if (_place >= 1 && _place2 >= 1 && _place <= roadOne.lanes && _place2 <= roadTwo.lanes)
                    {
                        if (!(roadOne.connectPoints[_place - 1 + _side].Active && roadTwo.connectPoints[_place - 1 + _side2].Active))
                        {
                            _buildroad = false;
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
        }

        public void CrossandStraight()
        {
            char _diagonalEnds;

            if (roadOne.Type == "Diagonal" && temp1.Y == temp2.Y)
            {
                _diagonalEnds = 'h';
            }
            else if (roadOne.Type == "Diagonal" && temp1.X == temp2.X)
            {
                _diagonalEnds = 'v';
            }
            else if (roadTwo.Type == "Diagonal" && temp3.Y == temp4.Y)
            {
                _diagonalEnds = 'h';
            }
            else // if (roadTwo.Type == "Diagonal" && temp3.X == temp4.X)
            {
                _diagonalEnds = 'v';
            }


        }

        public void CrossandDiagonal()
        {
            char _diagonalEnds;
            if (roadOne.Type == "Diagonal" && roadOne.slp < 1 && roadOne.slp > -1)
            {
                _diagonalEnds = 'v';
            }
            else if (roadOne.Type == "Diagonal" && (roadOne.slp >= 1 || roadOne.slp <= -1))
            {
                _diagonalEnds = 'h';
            }
            else if (roadTwo.Type == "Diagonal" && roadTwo.slp < 1 && roadTwo.slp > -1)
            {
                _diagonalEnds = 'v';
            }
            else // if (roadTwo.Type == "Diagonal" && (roadTwo.slp >= 1 || roadTwo.slp <= -1))
            {
                _diagonalEnds = 'h';
            }

        }

        public void CrossandCurved()
        {
            char _curvedEnd;
            if (roadOne.Type == "Curved" || roadOne.Type == "Curved2")
            {
                if (Math.Abs(temp1.X - point1.X) < Math.Abs(temp2.X - point1.X))
                {
                    _curvedEnd = 'h';
                }
                else
                {
                    _curvedEnd = 'v';
                }
            }
            else
            {
                if (Math.Abs(temp3.X - point2.X) < Math.Abs(temp4.X - point2.X))
                {
                    _curvedEnd = 'h';
                }
                else
                {
                    _curvedEnd = 'v';
                }
            }
        }
    }
}
