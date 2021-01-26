using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    
    //A roadtype for Crossroads.
    //The corners of the crossroad are calculated and used to contruct a Recthitbox for the crossroad
    //A scale is calculated, and used to calculate connectionpoints for this road at the right place for when a settingsscreen is opened for the crossroad.


    public class CrossRoad : AbstractRoad
    {
        public ConnectionPoint selectedPoint;
        public double Scale;
        public int Extra;
        

        public CrossRoadSide[] sides = new CrossRoadSide[4];

        //public RectHitbox[] sideHitboxes = new RectHitbox[4];
        //public bool[] sideStatus = new bool[4] { false, false, false, false };

        public CrossRoad(Point _point1, Point _point2, int _lanes, string _roadtype, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo) : base(_point1, _point2, _lanes, _roadtype, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo)
        {
            Extra = Roads.Config.crossroadExtra;
            Point[] _points = RoadMath.hitBoxPointsCross(_point1, _point1, _lanes, Roads.Config.laneWidth, false);
            hitbox = CreateHitbox(_points);

            int _width = (_lanes) * this.laneWidth + Roads.Config.scaleOffset * 2 + 2 * Extra;
            Scale = 500 / (double)_width;
            this.Type = _roadtype;

            createConnectionPoints();
            SwitchSelectedPoint(connectPoints.First());

            double lanewidth = (double)this.lanes * 20;
            this.sides[0] = new CrossRoadSide(new RectHitbox(_points[0], new Point(_points[0].X + 20, _points[0].Y), _points[2], new Point(_points[2].X + 20, _points[2].Y), Color.Green));
            this.sides[1] = new CrossRoadSide(new RectHitbox(new Point(_points[2].X, _points[2].Y - 20), new Point(_points[3].X, _points[3].Y - 20), _points[2], _points[3],Color.Green));
            this.sides[2] = new CrossRoadSide(new RectHitbox(new Point(point1.X + (int)(lanewidth / 2), point1.Y - (int)(lanewidth / 2)), new Point(point1.X + (int)(lanewidth / 2) + 20, point1.Y - (int)(lanewidth / 2)), new Point(point1.X + (int)(lanewidth / 2), point1.Y - (int)(lanewidth / 2) + (int)(lanewidth)), new Point(point1.X + (int)(lanewidth / 2) + 20, point1.Y - (int)(lanewidth / 2) + (int)lanewidth), Color.Green));
            this.sides[3] = new CrossRoadSide(new RectHitbox(_points[0], _points[1], new Point(_points[0].X, _points[0].Y + 20), new Point(_points[1].X, _points[1].Y + 20), Color.Green));
        }

        public void CreateArrowImages()
        {
            foreach (ConnectionPoint _point in this.connectPoints) //MOVE TO A MORE SENSICAL PLACE
            {
                Bitmap _combined = new Bitmap(20, 20);

                bool[] roads = new bool[4] { false, false, false, false };
                string[] dir = new string[4] { "Top", "Right", "Bottom", "Left" };
                List<ConnectionLink> _connectedlinks = this.connectLinks.FindAll(x => x.begin == _point);

                int _curIndex = dir.ToList().IndexOf(_point.Side);

                foreach (ConnectionLink _link in _connectedlinks)
                {
                    int _linkIndex = dir.ToList().IndexOf(_link.end.Side);
                    int _diffIndex = _linkIndex - _curIndex;

                    if (Math.Abs(_diffIndex) == 2)
                    {
                        roads[2] = true;
                    }
                    else if (_diffIndex == -1 || _diffIndex == 3)
                    {
                        roads[1] = true;
                    }
                    else if (_diffIndex == 1 || _diffIndex == -3)
                    {
                        roads[3] = true;
                    }
                }

                using (Graphics g = Graphics.FromImage(_combined))
                {
                    if (roads[3])
                    {
                        g.DrawImage(new Bitmap("../../Images/Left.png"), Point.Empty);
                    }
                    if (roads[2])
                    {
                        g.DrawImage(new Bitmap("../../Images/Up.png"), Point.Empty);
                    }
                    if (roads[1])
                    {
                        g.DrawImage(new Bitmap("../../Images/Right.png"), Point.Empty);
                    }
                    if (_point.end)
                    {
                        _point.arrowImg = new Bitmap("../../Images/non.png");
                    }
                }

                switch (_point.Side)
                {
                    case "Top":
                        _combined = DrawData.RotateImage(_combined, 180);
                        break;
                    case "Left":
                        _combined = DrawData.RotateImage(_combined, 90);
                        break;
                    case "Right":
                        _combined = DrawData.RotateImage(_combined, 270);
                        break;
                }

                _point.arrowImg = _combined;

            }
        }

        public override Hitbox CreateHitbox(Point[] _array)
        {
            return new RectHitbox(_array[0], _array[1], _array[2], _array[3], Color.Yellow);
        }

        public void SwitchSelectedPoint(ConnectionPoint _point)
        {
            if (selectedPoint != null)
            {
                selectedPoint.Hitbox.color = Color.Green;
            }

            if(selectedPoint == _point && selectedPoint != null)
            {
                selectedPoint = null;
                return;
            }
            else if(_point != null)
            {
                selectedPoint = _point;
                selectedPoint.Hitbox.color = Color.Blue;

            }
            else
            {
                selectedPoint = null;
            }

        }
        

        private void createConnectionPoints()
        {
            int Width = (int)((500 - ((this.lanes * this.laneWidth) + 2 * Extra) * this.Scale));
            //Console.WriteLine(Width);
            //Console.WriteLine(this.Scale);
            //Console.WriteLine(this.laneWidth);

            createConnectionPointSide(new Point((int)(Width + (Extra + this.laneWidth) / 2 * this.Scale), (int)(Width)), 1, 0, "Top");
            createConnectionPointSide(new Point((int)(Width + (Extra + this.laneWidth) / 2 * this.Scale), (int)(Width+ ((lanes * this.laneWidth + Extra) * this.Scale))), 1, 0, "Bottom");

            createConnectionPointSide(new Point((int)(Width), (int)(Width + (Extra + this.laneWidth) / 2 * this.Scale)), 0, 1, "Left");
            createConnectionPointSide(new Point((int)(Width + ((lanes * this.laneWidth + Extra) * this.Scale)), (int)(Width + (Extra + this.laneWidth) / 2 * this.Scale)), 0, 1, "Right");
        }

        private void createConnectionPointSide(Point _loc, int _X, int _Y, string _side)
        {
            for(int x = 0; x < this.lanes; x++)
            {
                connectPoints.Add(new ConnectionPoint(new Point(_loc.X + (int)(20 * this.Scale) * x * _X, _loc.Y + (int)(20 * this.Scale) * x * _Y), _side, this.Scale, x + 1));
            }
        }

        public override void Draw(Graphics g)
        {
            Brush _b = new SolidBrush(Color.FromArgb(100, 100, 100));
            
            double roadWidth = (double)this.lanes * this.laneWidth;

            g.FillRectangle(_b, new Rectangle(new Point(point1.X - (int)(roadWidth / 2), point1.Y - (int)(roadWidth / 2)), new Size(this.lanes * this.laneWidth, this.lanes * this.laneWidth)));
            DrawSides(g, "Top", new Point(point1.X - (int)(roadWidth / 2), point1.Y - (int)(roadWidth / 2 ) - Extra/2), new Size((int)roadWidth, Extra/2), _b );
            DrawSides(g, "Right", new Point(point1.X + (int)(roadWidth / 2), point1.Y - (int)(roadWidth / 2)), new Size(Extra/2, (int)(roadWidth)), _b);
            DrawSides(g, "Left", new Point(point1.X - (int)(roadWidth / 2) - Extra/2, point1.Y - (int)(roadWidth / 2)), new Size(Extra/2, (int)(roadWidth)), _b);
            DrawSides(g, "Bottom", new Point(point1.X - (int)(roadWidth / 2), point1.Y + (int)(roadWidth / 2)), new Size((int)roadWidth, Extra/2), _b);
            DrawLine(g);
            this.hitbox.Draw(g);

            foreach(ConnectionPoint _point in this.connectPoints)
            {
                g.DrawImage(_point.arrowImg, new Point(_point.transLocation.X - 10, _point.transLocation.Y - 10));
            }

            /*
            for(int x = 0; x < 4; x++)
            {
                this.sides[x].hitbox.Draw(g);
            }
            */
            foreach (ConnectionPoint x in connectPoints)
            {
                if (x.Active)
                {
                   // g.FillRectangle(Brushes.Red, x.Location.X, x.Location.Y, 10, 10);
                }
            }
        }

        public void DrawSides(Graphics g, string _side, Point _topleft, Size _size, Brush _b)
        {
            List<ConnectionPoint> _pointOnSide = this.connectPoints.FindAll(x => x.Side == _side);
            foreach (ConnectionPoint p in _pointOnSide)
            {
                if(p.Active)
                {
                    if (p.Side == "Top" || p.Side == "Bottom")
                    {
                        g.FillRectangle(_b, new Rectangle(new Point(_topleft.X + laneWidth * (p.Place - 1), _topleft.Y), new Size(_size.Width / _pointOnSide.Count, _size.Height)));
                        /*if(p.Place == 1 || !_pointOnSide[p.Place - 2].Active)
                        {

                        }*/
                    }
                    else
                    {
                        g.FillRectangle(_b, new Rectangle(new Point(_topleft.X, _topleft.Y + laneWidth * (p.Place - 1)), new Size(_size.Width, _size.Height / _pointOnSide.Count)));
                    }


                }
            }


        }
    }
}
