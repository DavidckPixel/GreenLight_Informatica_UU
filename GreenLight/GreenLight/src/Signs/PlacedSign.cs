using System;
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
        public bool flipped, hitboxflipped = false;
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
            setLocation();

            Console.WriteLine("Dir" + dir);

            Console.WriteLine("SLp draw " + Road.slp);

        }
        public override void Read(BetterAI _ai)
        {
            throw new NotImplementedException();
        }
        public void draw(Graphics g)
        {
            if (_points != null)
                this.Hitbox = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);

            // g.DrawImage(Sign_image, Location.X, Location.Y, 20, 20);
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
            
            this.Hitbox.Draw(g);
            //g.FillRectangle(Notsolid, this.Hitbox);
        }
        public void setFlipped()
        {
            hitboxflipped = !hitboxflipped;
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

            int Xsign;
            int Ysign;
            int _lane;

            for (int t = 0; t < 2; t++)
            {
                Console.WriteLine(t);

                if (t == 0)
                {
                    _lane = _outerLane;

                    Xsign = Mea.X - 10;
                    Ysign = Mea.Y - 10;
                }
                else
                {
                    _lane = this.Road.lanes / 2;

                    if (Road.Type == "Diagonal")
                    {
                        if (Road.slp == 0)
                        {
                            if (Road.getPoint1().X == Road.getPoint2().X)
                            {
                                Xsign = this.Location.X;
                                Ysign = this.Location.Y + 12;
                            }
                            else
                            {
                                Xsign = this.Location.X + 12;
                                Ysign = this.Location.Y;
                            }
                        }
                        else if (Road.slp <= -1 && Road.slp >= 1)
                        {
                            Xsign = this.Location.X + 20 * _direction;
                            Ysign = this.Location.Y + 12;
                        }
                        else
                        {
                            Xsign = this.Location.X + 12;
                            Ysign = this.Location.Y + 20 * _direction;
                        }

                    }
                    else
                    {
                        Xsign = this.Location.X;
                        Ysign = this.Location.Y;
                    }
                }

                Point l = new Point(-100, -100), _l = new Point(-100, -100);
                Point h = new Point(-100, -100), _h = new Point(-100, -100);
                int I = 0;

                try
                {
                    List<LanePoints> _lanepoints = this.Road.Drivinglanes[_lane].points;
                    float _shortDistance = 2000;
                    
                    for (int i = 0; i < _lanepoints.Count; i++)
                    {
                        float _distance = (float)Math.Sqrt((Xsign - _lanepoints[i].cord.X) * (Xsign - _lanepoints[i].cord.X) + (Ysign - _lanepoints[i].cord.Y) * (Ysign - _lanepoints[i].cord.Y));

                        if (_shortDistance > _distance)
                        {
                            _shortDistance = _distance;
                            
                            l = _lanepoints[i].cord;
                            h = _lanepoints[i].cord;
                            I = i;
                        }
                    }
                    if (t == 0)
                    {
                        this.Location = l;
                        this.Hitboxoffset = h;

                        int _dir = dir;

                        int offset = Math.Abs((_dir - 180) - 45) / 45 * 2;
                        int X1 = Location.X - ((_dir - 90) / 9) * 2;
                        int X2 = Location.X - (20 - (_dir - 180) / 9 * 2 * Math.Abs((_dir - 225) / 45 * 2));
                        int Y1 = Location.Y - 3;
                        int Y2 = Location.Y - (_dir - 180) / 9 * 2;
                        int Y3 = Location.Y - (20 - (_dir - 270) / 9 * 2);

                        if (Road.Type == "Diagonal")
                        {

                            if (_dir >= 0 && _dir <= 90)
                            {
                                this.Location = l;
                            }
                            else if (_dir > 90 && _dir < 180)
                            {
                                this.Location = new Point(X1, Y1);
                            }
                            else if (_dir >= 180 && _dir < 270)
                            {
                                this.Location = new Point(X2, Y2);
                            }
                            else
                            {
                                this.Location = new Point(Location.X, Y3);
                            }
                        }
                        if (Road.Type == "Curved2")
                        {
                            this.Location = new Point(Location.X -20, Location.Y - 20);
                        }
                    }
                    else
                    {
                        List<LanePoints> _hitboxLanepoints = this.Road.Drivinglanes[this.Road.lanes / 2].points;
                        bool _stop = true;

                        _l = l;
                        _h = h;

                        for (int j = 0; j < _hitboxLanepoints.Count && _stop; j++)
                        {
                            int _hitboxdistance = (int)Math.Sqrt(Math.Pow(_l.X - _h.X, 2) + Math.Pow(_l.Y - _h.Y, 2));

                            if (_hitboxdistance >= 80)
                            {
                                _stop = false;
                            }


                            _l = _hitboxLanepoints[I + j].cord;
                            _h = _hitboxLanepoints[I - j].cord;
                        }

                        _hitboxlocation = _l;
                        _hitboxoffset = _h;

                        this._points = RoadMath.hitBoxPointsDiagonal(_hitboxlocation, _hitboxoffset, this.Road.lanes + 2, 20, true, RoadMath.calculateSlope(_hitboxlocation, _hitboxoffset), false);
                        //_points = RoadMath.hitBoxPointsDiagonal(_hitboxoffset, _hitboxlocation, this.Road.lanes + 2, 20, true, RoadMath.calculateSlope(_hitboxoffset, _hitboxlocation));
                        this.Hitbox = new RectHitbox(this._points[1], _points[0], _points[3], _points[2], Color.Red);
                     }
                }
                catch (Exception e)
                {
                    _points = RoadMath.hitBoxPointsDiagonal(_hitboxoffset, _hitboxlocation, this.Road.lanes + 2, 20, true, RoadMath.calculateSlope(_hitboxoffset, _hitboxlocation), false);
                }

            }


           Console.WriteLine(flipped);

            Point _temp1 = Road.getPoint1();
            Point _temp2 = Road.getPoint2();
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