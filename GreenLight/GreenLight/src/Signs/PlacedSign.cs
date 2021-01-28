﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

//this is the object that is actually placed on the Road that has a hitbox and says which specific sign is clicked

namespace GreenLight
{
    public class PlacedSign : AbstractSign
    {
        public RectHitbox Hitbox;
        public Point Location, Hitboxoffset, Mea;
        public string Direction;
        public AbstractSign Sign;
        public Image Sign_image;
        public AbstractRoad Road;
        public string signType;
        public Speedsign speedSign;
        public bool flipped;
        public Point[] _points;
        int dir;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign, Image _Sign_image, AbstractRoad _road, string _signType, Point _hitboxoffset, Point _mea, bool _flipped)
        {
            this.speedSign = new Speedsign(new Size(20, 20), _location);
            this.Location = _location;
            this.Direction = _direction;
            this.Sign = _sign;
            this.Sign_image = _Sign_image;
            this.Road = _road;
            this.signType = _signType;
            int _dir = (int)Road.Drivinglanes[0].AngleDir;
            this.Hitboxoffset = _hitboxoffset;
            this.flipped = _flipped;
            this.Mea = _mea;

            dir = (int)Road.Drivinglanes[0].AngleDir;

            Console.WriteLine("Dir" + dir);

            Console.WriteLine("SLp draw " + Road.slp);

            if  (this.Location == new Point(-100,  -100))
                setLocation();
        }
        public override void Read(BetterAI _ai)
        {
            throw new NotImplementedException();
        }
        public void draw(Graphics g)
        {

            this.Hitbox = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);

            int _dir = dir;

            int offset = Math.Abs((_dir - 180) - 45) / 45 * 2;
            int X1 = Location.X - ((_dir - 90) / 9) * 2;
            int X2 = Location.X - (20 - (_dir - 180) / 9 * 2 * Math.Abs((_dir - 225) / 45 * 2));
            int Y1 = Location.Y - 3;
            int Y2 = Location.Y - (_dir - 180) / 9 * 2;
            int Y3 = Location.Y - (20 - (_dir - 270) / 9 * 2);

            Console.WriteLine("Dir draw " + _dir);

            // g.DrawImage(Sign_image, Location.X, Location.Y, 20, 20);
            if (Road.Type == "Diagonal")
            {

                if (_dir >= 0 && _dir <= 20)
                {
                    // this.Hitbox = new RectHitbox(new Point(Location.X - 15, Location.Y + 5), new Point(Location.X + 15, Location.Y + 5), new Point(Location.X - 15, Location.Y + 35), new Point(Location.X + 5, Location.Y + 35), Color.Red);
                }
                if (_dir > 20 && _dir <= 45 || _dir >= 270)
                {
                    //  this.Hitbox = new RectHitbox(new Point(Hitboxoffset.X - 15, Hitboxoffset.Y - 15), new Point(Hitboxoffset.X + 15, Hitboxoffset.Y - 15), new Point(Hitboxoffset.X - 15, Hitboxoffset.Y + 15), new Point(Hitboxoffset.X + 15, Hitboxoffset.Y + 15), Color.Red);
                }
                if (_dir >= 0 && _dir <= 90)
                {
                    if (signType == "trafficLight")
                    {
                        g.DrawImage(Sign_image, Location.X, Location.Y, 25, 25);
                    }
                    else
                    {
                        if (signType == "speedSign")
                        {
                            this.speedSign.Location = new Point(Location.X, Location.Y);
                            this.speedSign.speed = Sign.speed;
                            this.Sign_image = speedSign.speedImage();
                        }
                        g.DrawImage(Sign_image, Location.X, Location.Y, 20, 20);
                    }

                }
                else if (_dir > 90 && _dir < 180)
                {
                    if (signType == "trafficLight")
                    {
                        g.DrawImage(Sign_image, X1, Y1, 25, 25);
                    }
                    else
                    {
                        if (signType == "speedSign")
                        {
                            this.speedSign.Location = new Point(X1, Y1);
                            this.speedSign.speed = Sign.speed;
                            this.Sign_image = speedSign.speedImage();
                        }
                        g.DrawImage(Sign_image, X1, Y1, 20, 20);

                    }
                }
                else if (_dir >= 180 && _dir < 270)
                {
                    if (signType == "trafficLight")
                    {
                        g.DrawImage(Sign_image, X2, Y2, 25, 25);
                    }
                    else
                    {
                        if (signType == "speedSign")
                        {
                            this.speedSign.Location = new Point(X2, Y2);
                            this.speedSign.speed = Sign.speed;
                            this.Sign_image = speedSign.speedImage();
                        }
                        g.DrawImage(Sign_image, X2, Y2, 20, 20);
                    }


                }
                else
                {
                    if (signType == "trafficLight")
                    {
                        g.DrawImage(Sign_image, Location.X, Y3, 25, 25);
                    }
                    else
                    {
                        if (signType == "speedSign")
                        {
                            this.speedSign.Location = new Point(Location.X, Y3);
                            this.speedSign.speed = Sign.speed;
                            this.Sign_image = speedSign.speedImage();
                        }
                        g.DrawImage(Sign_image, Location.X, Y3, 20, 20);
                    }

                }
            }
            else if (Road.Type == "Curved")
            {
                if (signType == "trafficLight")
                {
                    g.DrawImage(Sign_image, Location.X, Location.Y, 25, 25);
                }
                else
                {
                    if (signType == "speedSign")
                    {
                        this.speedSign.Location = new Point(Location.X, Y3);
                        this.speedSign.speed = Sign.speed;
                        this.Sign_image = speedSign.speedImage();
                    }
                    g.DrawImage(Sign_image, Location.X, Location.Y, 20, 20);
                }

            }
            else if (Road.Type == "Curved2")
            {
                if (signType == "trafficLight")
                {
                    g.DrawImage(Sign_image, Location.X - 20, Location.Y, 25, 25);
                }
                else
                {
                    if (signType == "speedSign")
                    {
                        this.speedSign.Location = new Point(Location.X, Y3);
                        this.speedSign.speed = Sign.speed;
                        this.Sign_image = speedSign.speedImage();
                    }
                    g.DrawImage(Sign_image, Location.X - 20, Location.Y, 20, 20);
                }
            }

            this.Hitbox.Draw(g);
            //g.FillRectangle(Notsolid, this.Hitbox);
        }
        public void setFlipped()
        {
            Console.WriteLine("Flipped!" + dir);
            flipped = !flipped;
            dir = Math.Abs(360 - dir);
            Console.WriteLine("After Flipped!" + dir);
            setLocation();
        }

        public void setLocation()
        {
            int _outerLane = 0;
            int _direction = 1;
            int _lanes = this.Road.getLanes();
            Point _hitboxlocation = new Point(-100, -100), _hitboxoffset = new Point(-100, -100);

            if (this.flipped)
            {
                _outerLane = 0;
                _direction = 1;
            }
            else
            {
                _outerLane = _lanes - 1;
                _direction = -1;
            }

            if (_lanes == 1)
                _outerLane = 0;

            for (int t = 0; t < 2; t++)
            {
                int _lane;

                if (t == 0)
                {
                    _lane = _outerLane;
                }
                else
                {
                    _lane = this.Road.lanes / 2;
                }

                try
                {
                    List<LanePoints> _lanepoints = this.Road.Drivinglanes[_lane].points;
                    float _shortDistance = 2000;
                    Point l = new Point(-100, -100);
                    Point h = new Point(-100, -100);
                    for (int i = 0; i < _lanepoints.Count; i++)
                    {
                        int Xsign = Mea.X - 10;
                        int Ysign = Mea.Y - 10;
                        float _distance = (float)Math.Sqrt((Xsign - _lanepoints[i].cord.X) * (Xsign - _lanepoints[i].cord.X) + (Ysign - _lanepoints[i].cord.Y) * (Ysign - _lanepoints[i].cord.Y));

                        if (_shortDistance > _distance)
                        {
                            _shortDistance = _distance;
                            l = _lanepoints[i].cord;
                            if (i - 10 >= 0)
                                h = _lanepoints[i - 10].cord;
                            if (i - 20 >= 0)
                                h = _lanepoints[i - 20].cord;
                            else
                                h = _lanepoints[i].cord;
                        }
                    }

                    if (t == 0)
                    {

                        this.Location = l;
                        this.Hitboxoffset = h;
                    }
                    else
                    {

                        _hitboxlocation = l;
                        _hitboxoffset = h;
                    }
                }
                catch (Exception e)
                {

                }
                Console.WriteLine(flipped);

                Point _temp1 = Road.getPoint1();
                Point _temp2 = Road.getPoint2();

                /*
                if (Road.Type == "Diagonal")
                {

                    if (Road.slp == 0)
                    {
                        if (_temp1.Y == _temp2.Y)
                            this.Hitbox = new RectHitbox(new Point(Location.X - (Math.Abs(-1 + _direction) * 15), Location.Y - _direction * 20), new Point(Location.X - (Math.Abs(-1 + _direction) * 15), Location.Y + _direction * (20 * _lanes - 5)), new Point(Location.X + 15 + (Math.Abs(1 + _direction) * 15), Location.Y - _direction * 20), new Point(Location.X + 15 + (Math.Abs(1 + _direction) * 15), Location.Y + _direction * (20 * _lanes - 5)), Color.Red);
                        else //if(_temp1.X == _temp2.X)
                            this.Hitbox = new RectHitbox(new Point(Location.X - _direction * 20, Location.Y + (15 + Math.Abs(-1 + _direction) * 15)), new Point(Location.X + _direction * (20 * _lanes - 5), Location.Y + (15 + Math.Abs(-1 + _direction) * 15)), new Point(Location.X - _direction * 20, l.Y - (Math.Abs(1 + _direction) * 15)), new Point(Location.X + _direction * (20 * _lanes - 5), l.Y - (Math.Abs(1 + _direction) * 15)), Color.Red);

                    }
                    else if (Road.slp <= -1 || Road.slp >= 1)
                    {
                        this.Hitbox = new RectHitbox(new Point(Location.X - (Math.Abs(-1 + _direction) * 15), Location.Y - _direction * 20), new Point(Location.X - (Math.Abs(-1 + _direction) * 15), Location.Y + _direction * (20 * _lanes - 5)), new Point(Location.X + 15 + (Math.Abs(1 + _direction) * 15), Location.Y - _direction * 20), new Point(Location.X + 15 + (Math.Abs(1 + _direction) * 15), Location.Y + _direction * (20 * _lanes - 5)), Color.Red);
                    }
                    else
                    {
                        this.Hitbox = new RectHitbox(new Point(Location.X - _direction * 20, Location.Y + (15 + Math.Abs(-1 + _direction) * 15)), new Point(Location.X + _direction * (20 * _lanes - 5), Location.Y + (15 + Math.Abs(-1 + _direction) * 15)), new Point(Location.X - _direction * 20, l.Y - (Math.Abs(1 + _direction) * 15)), new Point(Location.X + _direction * (20 * _lanes - 5), l.Y - (Math.Abs(1 + _direction) * 15)), Color.Red);
                    }
                }
                else
                {
                    this.Hitbox = new RectHitbox(new Point(Location.X - 15, Location.Y - 15), new Point(Location.X + 15, Location.Y - 15), new Point(Location.X - 15, Location.Y + 15), new Point(Location.X + 15, Location.Y + 15), Color.Red);
                } */



                if (!flipped)
                {
                    _points = RoadMath.hitBoxPointsDiagonal(_hitboxlocation, _hitboxoffset, this.Road.lanes + 2, 20, true, RoadMath.calculateSlope(_hitboxlocation, _hitboxoffset));
                }
                else
                {
                    _points = RoadMath.hitBoxPointsDiagonal(_hitboxoffset, _hitboxlocation, this.Road.lanes + 2, 20, true, RoadMath.calculateSlope(_hitboxoffset, _hitboxlocation));
                }

                this.Hitbox = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);
            }
        }

        public override string ToString()
        {
            string signString = "Sign" + " " + this.Location.X.ToString() + " " + this.Location.Y.ToString() + " " + this.signType
                + " " + Hitboxoffset.X.ToString() + " " + Hitboxoffset.Y.ToString() + " " + this.Mea.X.ToString() + " " + this.Mea.Y.ToString() + " " + this.flipped.ToString() 
                + " " + this._points[0].X.ToString() + " " + this._points[0].Y.ToString() + " " + this._points[1].X.ToString() + " " + this._points[1].Y.ToString() + " " + this._points[2].X.ToString() + " " + this._points[2].Y.ToString() + " " + this._points[3].X.ToString() + " " + this._points[3].Y.ToString();
            if (this.signType == "speedSign")
            {
                signString += " " + Sign.speed.ToString();
            }
            return signString;
        }
    }
}