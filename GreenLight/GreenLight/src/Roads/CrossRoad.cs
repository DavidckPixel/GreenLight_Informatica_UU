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
        public List<ConnectionPoint> connectPoints = new List<ConnectionPoint>();
        public List<ConnectionLink> connectLinks = new List<ConnectionLink>();

        public ConnectionPoint selectedPoint;
        public double Scale;
        public int Extra;
        

        public CrossRoadSide[] sides = new CrossRoadSide[4];

        //public RectHitbox[] sideHitboxes = new RectHitbox[4];
        //public bool[] sideStatus = new bool[4] { false, false, false, false };

        public CrossRoad(Point _point1, Point _point2, int _lanes, string _roadtype, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo) : base(_point1, _point2, _lanes, _roadtype, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo)
        {

            Extra = Roads.Config.crossroadExtra;
            Point[] _points = hitBoxPoints(_point1, _point1, _lanes, Roads.Config.laneWidth);
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

        public override Point[] hitBoxPoints(Point one, Point two, int _lanes, int _laneWidth)
        {
            Point[] _points = new Point[4];

            Rectangle _rec = new Rectangle(one, new Size(1, 1));
            int _inflate = _lanes * this.laneWidth / 2 + Extra;

            _rec.Inflate(_inflate, _inflate);

            _points[0] = _rec.Location;
            _points[1] = new Point(_rec.Right, _rec.Top);
            _points[2] = new Point(_rec.Left, _rec.Bottom);
            _points[3] = new Point(_rec.Right, _rec.Bottom);

            return _points;
        }

        private void createConnectionPoints()
        {
            int Width = (int)((500 - ((this.lanes * this.laneWidth) + 2 * Extra) * this.Scale));
            Console.WriteLine(Width);
            Console.WriteLine(this.Scale);
            Console.WriteLine(this.laneWidth);

            createConnectionPointSide(new Point((int)(Width + (Extra + this.laneWidth) / 2 * this.Scale), (int)(Width)), 1, 0, "Top");
            createConnectionPointSide(new Point((int)(Width + (Extra + this.laneWidth) / 2 * this.Scale), (int)(Width+ ((lanes * this.laneWidth + Extra) * this.Scale))), 1, 0, "Bottom");

            createConnectionPointSide(new Point((int)(Width), (int)(Width + (Extra + this.laneWidth) / 2 * this.Scale)), 0, 1, "Left");
            createConnectionPointSide(new Point((int)(Width + ((lanes * this.laneWidth + Extra) * this.Scale)), (int)(Width + (Extra + this.laneWidth) / 2 * this.Scale)), 0, 1, "Right");
        }

        private void createConnectionPointSide(Point _loc, int _X, int _Y, string _side)
        {
            for(int x = 0; x < this.lanes; x++)
            {
                connectPoints.Add(new ConnectionPoint(new Point(_loc.X + (int)(this.laneWidth * this.Scale) * x * _X, _loc.Y + (int)(this.laneWidth * this.Scale) * x * _Y), _side, this.Scale));
            }
        }

        public override void Draw(Graphics g)
        {
            Brush _b = new SolidBrush(Color.FromArgb(21, 21, 21));
            
            double roadWidth = (double)this.lanes * this.laneWidth;

            g.FillRectangle(_b, new Rectangle(new Point(point1.X - (int)(roadWidth / 2), point1.Y - (int)(roadWidth / 2)), new Size(this.lanes * this.laneWidth, this.lanes * this.laneWidth)));
            DrawSides(g, "Top", new Point(point1.X - (int)(roadWidth / 2), point1.Y - (int)(roadWidth / 2 ) - Extra), new Size((int)roadWidth, Extra), _b );
            DrawSides(g, "Right", new Point(point1.X + (int)(roadWidth / 2), point1.Y - (int)(roadWidth / 2)), new Size(Extra, (int)(roadWidth)), _b);
            DrawSides(g, "Left", new Point(point1.X - (int)(roadWidth / 2) - Extra, point1.Y - (int)(roadWidth / 2)), new Size(Extra, (int)(roadWidth)), _b);
            DrawSides(g, "Bottom", new Point(point1.X - (int)(roadWidth / 2), point1.Y + (int)(roadWidth / 2)), new Size((int)roadWidth, Extra), _b);

            DrawLine(g);
            this.hitbox.Draw(g);

            for(int x = 0; x < 4; x++)
            {
                this.sides[x].hitbox.Draw(g);
            }
        }

        public void DrawSides(Graphics g, string _side, Point _topleft, Size _size, Brush _b)
        {
            if (!this.connectPoints.Any(x => x.Side == _side && x.Active == false))
            {
                g.FillRectangle(_b, new Rectangle(_topleft, _size));
            }
        }
    }
}
