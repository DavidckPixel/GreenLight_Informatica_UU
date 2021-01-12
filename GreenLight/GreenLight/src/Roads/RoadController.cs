﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class RoadController : EntityController
    {

        //Very early version of the actual code that WILL connect the road system to the rest of our project
        //For now it just holds a calculate direction function
        //Nothing really of interest here yet, Come back later :)

        public static List<AbstractRoad> roads = new List<AbstractRoad>();
        public PictureBox Screen;
        public string roadType = "D";

        public RoadController(PictureBox _screen)
        {
            this.Screen = _screen;
            this.Screen.MouseClick += RoadClick;
        }

        /*public void BuildStraightRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "StraightRoad");
            AbstractRoad _road = new StraightRoad(_point1, _point2, 1, _dir);

            roads.Add(_road);
        }*/

        public void BuildDiagonalRoad(Point _point1, Point _point2, int _lanes, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo)
        {
            string _dir = Direction(_point1, _point2, "DiagonalRoad");
            Console.WriteLine("build" + _beginconnection + "-----" + _endconnection);
            AbstractRoad _road = new DiagonalRoad(_point1, _point2, _lanes, _dir, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
            roads.Add(_road);
            Connection(_point1, _point2, _lanes, _dir, _road, _beginconnection, _endconnection);
        }

        public void BuildCurvedRoad(Point _point1, Point _point2, int _lanes, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo)
        {
            string _dir = Direction(_point1, _point2, "CurvedRoad");
            AbstractRoad _road = new CurvedRoad(_point1, _point2, _lanes, _dir, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
            roads.Add(_road);
            Connection(_point1, _point2, _lanes, _dir, _road, _beginconnection, _endconnection);
        }

        public void Connection(Point _point1, Point _point2, int _lanes, string _dir, AbstractRoad _road, bool _beginconnection, bool _endconnection)
        {
            Point _temp1, _temp2;
            int _count = 0;
            try
            {
                foreach (AbstractRoad x in roads)
                {
                    if (x != _road)
                    {
                        _temp1 = x.getPoint1();
                        _temp2 = x.getPoint2();


                        if (x.getLanes() == _lanes)
                        {
                            if (_point1 == _temp1 || Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) <= 21)
                            {
                                if (_beginconnection == false)
                                {
                                    Connection _connection = new Connection(_point1, _temp1, _lanes, _dir, x.Drivinglanes[0].dir, _road, x, _count);
                                }
                                else
                                {
                                    x.beginconnection = true;
                                    x.beginConnectedTo = _road;
                                    _road.beginConnectedTo = x;
                                }
                            }
                            else if (_point1 == _temp2 || Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)) <= 21)
                            {
                                if (_beginconnection == false)
                                {
                                    Connection _connection = new Connection(_point1, _temp2, _lanes, _dir, x.Drivinglanes[0].dir, _road, x, _count);
                                }
                                else
                                {
                                    x.endconnection = true;
                                    x.endConnectedTo = _road;
                                    _road.beginConnectedTo = x;
                                }
                            }
                            else if (_point2 == _temp1 || Math.Sqrt(Math.Pow(_point2.X - _temp1.X, 2) + Math.Pow(_point2.Y - _temp1.Y, 2)) <= 21)
                            {
                                if (_endconnection == false)
                                {
                                    Connection connection = new Connection(_point2, _temp1, _lanes, _dir, x.Drivinglanes[0].dir, _road, x, _count);
                                }
                                else
                                {
                                    x.beginconnection = true;
                                    x.beginConnectedTo = _road;
                                    _road.endConnectedTo = x;

                                }
                            }
                            else if (_point2 == _temp2 || Math.Sqrt(Math.Pow(_point2.X - _temp2.X, 2) + Math.Pow(_point2.Y - _temp2.Y, 2)) <= 21)
                            {
                                if (_endconnection == false)
                                {
                                    Connection _connection = new Connection(_point2, _temp2, _lanes, _dir, x.Drivinglanes[0].dir, _road, x, _count);
                                }
                                else
                                {
                                    x.endconnection = true;
                                    x.endConnectedTo = _road;
                                    _road.endConnectedTo = x;
                                }
                            }
                        }
                    }
                    _count++;
                }
            }
            catch (Exception e) { };
        }

        public static string Direction(Point _firstPoint, Point _secondPoint, string _Roadtype)
        {
            string RoadDirection = "";
            string RoadType = _Roadtype;
            switch (RoadType)
            {
                case "CurvedRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NE";
                            else
                                RoadDirection = "SE";
                        }
                        else
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NW";
                            else
                                RoadDirection = "SW";
                        }
                    }
                    break;
                case "DiagonalRoad":
                    {
                        RoadDirection = "D";
                    }
                    break;
                /*case "StraightRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                            RoadDirection = "E";
                        else if (_secondPoint.X < _firstPoint.X)
                            RoadDirection = "W";
                        else if (_firstPoint.Y < _secondPoint.Y)
                            RoadDirection = "S";
                        else if (_firstPoint.Y > _secondPoint.Y)
                            RoadDirection = "N";
                    }
                    break;*/

            }
            return RoadDirection;
        }

        public void UndoRoad()
        {
            if (roads.Count != 0)
            {
                roads.RemoveAt(roads.Count - 1);
                General_Form.Main.BuildScreen.Screen.Invalidate();
            }
        }

        public override void Initialize()
        {

        }

        public void RoadClick(object o, MouseEventArgs mea)
        {
            if (this.roadType == "D") //Menu is Disabled
            {
                return;
            }

            AbstractRoad _selectedRoad = roads.Find(x => x.Hitbox2.Contains(mea.Location));
            if (_selectedRoad == null)
            {
                Console.Write("HitBOX test");
                return;
            }
            string message = "Do you want to delete this road?";
            string title = "Delete road";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                roads.Remove(_selectedRoad);
                Screen.Invalidate();
            }
            else
            {
                Application.ExitThread();
            }    
            Console.WriteLine(_selectedRoad.Cords.ToString());
        }
    }
}
