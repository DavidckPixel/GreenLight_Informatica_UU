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
        public int SignCount;
        int dir;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign, Image _Sign_image, AbstractRoad _road, string _signType, Point _hitboxoffset, Point _mea, bool _flipped, int _signCount)
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
            this.SignCount = _signCount;
            dir = (int)Road.Drivinglanes[0].AngleDir;
            
            if(this.Location.X == -100)
                setLocation();
        }
        public override void Read(BetterAI _ai)
        {
            throw new NotImplementedException();
        }
        public void draw(Graphics g)
        {
            if (_points != null)
                this.Hitbox = new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);

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

            flipped = !flipped;
            dir = Math.Abs(360 - dir);

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

            int Xsign = Mea.X -10;
            int Ysign = Mea.Y -10;

            List<LanePoints> _lanepoints = this.Road.Drivinglanes[_outerLane].points;
            float _shortDistance = 2000;

            Point l = new Point(-100, -100), _l = new Point(-100, -100), _h = new Point(-100, -100);
            LanePoints h = _lanepoints.First();

            for (int i = 0; i < _lanepoints.Count; i++)
            {
                float _distance = (float)Math.Sqrt((Xsign - _lanepoints[i].cord.X) * (Xsign - _lanepoints[i].cord.X) + (Ysign - _lanepoints[i].cord.Y) * (Ysign - _lanepoints[i].cord.Y));

                if (_shortDistance > _distance)
                {
                    _shortDistance = _distance;

                    l = _lanepoints[i].cord;
                    h = _lanepoints[i];
                }
            }

            this.Location = l;
            this.Hitboxoffset = l;

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
            if (Road.Type == "Curved")
            {
                if (!flipped)
                {
                    if (Road.Dir == "SW")
                        this.Location = new Point(Location.X - 20, Location.Y - 20);
                    else
                        this.Location = new Point(Location.X, Location.Y);

                }
                else
                {
                    if (Road.Dir == "SW")
                        this.Location = new Point(Location.X, Location.Y);
                    else
                        this.Location = new Point(Location.X - 20, Location.Y - 20);
                }
            }
            else
            {
                if (flipped)
                {
                    if (Road.Dir == "NE")
                        this.Location = new Point(Location.X - 5, Location.Y + 5);
                    else
                        this.Location = new Point(Location.X - 20, Location.Y - 20);
                }
                else
                {
                    if (Road.Dir == "NE")
                        this.Location = new Point(Location.X - 10, Location.Y - 15);
                    else
                        this.Location = new Point(Location.X - 5, Location.Y + 5);
                }
            }

            List<LanePoints> _hitboxLanepoints = this.Road.Drivinglanes[Road.lanes / 2].points;
            bool _stop = true;
            int I = 0;

            if (Road.Type == "Diagonal")
            {


                if (Road.slp == 0)
                {
                    if (Road.getPoint1().X == Road.getPoint2().X)
                    {
                        int _y = this.Location.Y + 10;
                        LanePoints _temp;
                        _temp = _hitboxLanepoints.Aggregate((x, y) => Math.Abs(x.cord.Y - _y) < Math.Abs(y.cord.Y - _y) ? x : y);
                        _l.X = _temp.cord.X;
                        _l.Y = _temp.cord.Y;
                        I = _hitboxLanepoints.FindIndex(p => p == _temp);
                    }
                    else
                    {
                        int _x = this.Location.X + 10; 
                        LanePoints _temp;
                        _temp = _hitboxLanepoints.Aggregate((x, y) => Math.Abs(x.cord.X - _x) < Math.Abs(y.cord.X - _x) ? x : y);
                        _l.X = _temp.cord.X;
                        _l.Y = _temp.cord.Y;
                        I = _hitboxLanepoints.FindIndex(p => p == _temp);
                    }
                }
                else if (Road.slp <= -1 || Road.slp >= 1)
                {
                    int _y = this.Location.Y + 10;
                    LanePoints _temp;
                    _temp = _hitboxLanepoints.Aggregate((x, y) => Math.Abs(x.cord.Y - _y) < Math.Abs(y.cord.Y - _y) ? x : y);
                    _l.X = _temp.cord.X;
                    _l.Y = _temp.cord.Y;
                    I = _hitboxLanepoints.FindIndex(p => p == _temp);

                }
                else
                {

                    int _x = this.Location.X + 10;
                    LanePoints _temp;
                    _temp = _hitboxLanepoints.Aggregate((x, y) => Math.Abs(x.cord.X - _x) < Math.Abs(y.cord.X - _x) ? x : y);
                    _l.X = _temp.cord.X;
                    _l.Y = _temp.cord.Y;
                    I = _hitboxLanepoints.FindIndex(p => p == _temp);
                }


                _h = _l;
            }
            else
            {
                float Degree = h.degree;


                int Distance = (int)(Road.lanes / 2 * Road.laneWidth);
                int X = this.Location.X + 10, Y = this.Location.Y + 10;

                Point p = new Point(-100, -100);

                int _counter = 0, _count = 0;
                float _ShortDistance = 2000;
                for (int i = 0; i < _hitboxLanepoints.Count; i++)
                {
                    float _Distance = (float)Math.Sqrt((X - _hitboxLanepoints[i].cord.X) * (X - _hitboxLanepoints[i].cord.X) + (Y - _hitboxLanepoints[i].cord.Y) * (Y - _hitboxLanepoints[i].cord.Y));

                    if (_ShortDistance > _Distance)
                    {
                        _ShortDistance = _Distance;

                        p = _hitboxLanepoints[i].cord;
                        _count = _counter;
                    }
                    _counter++;
                }
                _l = p;
                _h = _l;
                I = _count;
            }


            for (int j = 0; j < _hitboxLanepoints.Count && _stop; j++)
            {
                int _hitboxdistance = (int)Math.Sqrt(Math.Pow(_l.X - _h.X, 2) + Math.Pow(_l.Y - _h.Y, 2));

                if (_hitboxdistance >= 60)
                {
                    _stop = false;
                }

                if (_hitboxLanepoints.Count <= I + j)
                {
                    _l = _hitboxLanepoints[_hitboxLanepoints.Count -1].cord;

                    if (0 > I - j)
                    {
                        _h = _hitboxLanepoints[0].cord;
                    }
                    else
                    {
                        _h = _hitboxLanepoints[I - j].cord;
                    }
                }

                else if (0 > I - j)
                {
                    _l = _hitboxLanepoints[I + j].cord;
                    _h = _hitboxLanepoints[0].cord;
                }

                else
                {
                    _l = _hitboxLanepoints[I + j].cord;
                    _h = _hitboxLanepoints[I - j].cord;
                }
            }

            _hitboxlocation = _l;
            _hitboxoffset = _h;

            if (Road.Type == "Diagonal")
            {
                this._points = RoadMath.hitBoxPointsDiagonal(_hitboxlocation, _hitboxoffset, this.Road.lanes + 2, 20, true, RoadMath.calculateSlope(_hitboxlocation, _hitboxoffset), false);
            }
                //this._points = RoadMath.hitBoxPointsCurved(_hitboxoffset, this.Road.lanes + 2, 20, true, Road.Dir);
            else
            {
                this._points = RoadMath.hitBoxPointsDiagonal(new Point(_hitboxlocation.X - 10, _hitboxlocation.Y), new Point(_hitboxoffset.X - 10, _hitboxoffset.Y), this.Road.lanes + 2, 25, true, RoadMath.calculateSlope(new Point(_hitboxlocation.X - 10, _hitboxlocation.Y), new Point(_hitboxoffset.X - 10, _hitboxoffset.Y)), false);
            }
            this.Hitbox = new RectHitbox(this._points[1], _points[0], _points[3], _points[2], Color.Red);
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