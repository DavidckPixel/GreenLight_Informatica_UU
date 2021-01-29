using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    /*
     This is the GridController class, it created and draws the gridpoint.
     The data for the GridPoints (distance width/ height) is stored within a json file, and read in in the GridConfig class.
     Every Gridpoint object contains a rectangle Hitbox, to see if the user has clicked on a point this class returns a bool.
     whether or not the point is in the Hitbox. This controller also keeps track of which gridpoints are used to prevent roads from overlapping.
     This is done by creating a hitbox for a road that the user wants to build. If this hitbox does not overlap with any used gridpoints, legal will
     turn true. When legal is true, roads can be build. This controller holds the buildercontroller to build roads when clicked on the gridpoints.   
                                                                                                                                                     */

    public class GridController : EntityController
    {
        public List<Gridpoint> Gridpoints = new List<Gridpoint>();
        bool firstClick;
        PictureBox Canvas;

        public Gridpoint firstPoint = null;
        public Gridpoint secondPoint = null;

        private Point mouseCords;
        private bool Legal;
        private bool _points_visible = true;

        public BuilderController Builder;

        public GridController(PictureBox _bitmap, BuilderController _builder)
        {
            this.Canvas = _bitmap;
            CreateGridPoints();
            this.Canvas.MouseClick += OnClick;
            this.Canvas.MouseMove += MoveMouse;
            this.firstClick = true;

            this.Builder = _builder;
        }

        public bool SetVisible
        {
            get
            {
                return _points_visible;
            }
            set
            {
                _points_visible = value;
            }
        }

        public override void Initialize()
        {
        }

        public void CanvasResize(Size _size)
        {
            this.Canvas.Size = _size;
        }

        public void CreateGridPoints()
        {
            int _amountX = Canvas.Width / Grid.Config.SpacingWidth;
            int _amountY = Canvas.Height / Grid.Config.SpacingHeight;

            for (int y = 0; y < _amountY; y++)
            {
                for (int x = 0; x < _amountX; x++)
                {

                    Gridpoints.Add(new Gridpoint(new Point(x * Grid.Config.SpacingWidth, y * Grid.Config.SpacingHeight), Grid.Config.BoxSize, Grid.Config.HitSize));
                }
            }
        }

        public void OnClick(Object o, MouseEventArgs mea)
        {
            string _type = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;
            General_Form.Main.BuildScreen.builder.roadBuilder.lastType = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;

            Gridpoint _point = Gridpoints.Find(x => x.Collision(mea.Location));

            if (_type == "X" || _type == "D")
            {
                return;
            }

            if (_type == "Cross")
            {
                if (Legal && _point != null)
                {
                    Builder.BuildRoad(_point.Cords, _point.Cords);
                }
                return;
            }

            if (mea.Button == MouseButtons.Right)
            {
                this.ResetPoints();
            }
            else if (firstClick)
            {
                Legal = true;
                if (_point != null)
                {
                    this.firstClick = false;
                    this.firstPoint = _point;
                }
            }
            else
            {
                this.secondPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (this.secondPoint != null && this.secondPoint != this.firstPoint && Legal)
                {
                    this.secondPoint = _point;
                    Builder.BuildRoad(this.firstPoint.Cords, this.secondPoint.Cords);
                    this.ResetPoints();
                }
                else if (!Legal)
                {
                    this.ResetPoints();
                }
            }

        }
        private void ResetPoints()
        {
            this.firstPoint = null;
            this.secondPoint = null;
            this.firstClick = true;
            this.Canvas.Invalidate();
        }

        public void MoveMouse(object o, MouseEventArgs mea)
        {
            if (firstClick == true && (General_Form.Main.BuildScreen.builder.roadBuilder.roadType != "Cross") && (General_Form.Main.BuildScreen.builder.roadBuilder.lastType != "Cross"))
            {
                return;
            }

            mouseCords = mea.Location;
            Canvas.Invalidate();
        }

        public void DrawGridPoints(Graphics g)
        {
            if (_points_visible)
            {
                foreach (Gridpoint x in Gridpoints)
                {
                    x.DrawGrid(g);
                }

                Brush Notsolidred = new SolidBrush(Color.FromArgb(100, Color.DarkRed));
                Brush Notsolidgreen = new SolidBrush(Color.FromArgb(100, Color.Green));
                Brush Notsolidorange = new SolidBrush(Color.FromArgb(100, Color.Orange));

                if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "X")
                {
                    return;
                }

                if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Cross")
                {
                    Rectangle _rec = new Rectangle(mouseCords, new Size(1, 1));
                    int _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
                    int _inflate = _lanes * 20 / 2;
                    _rec.Inflate(_inflate, _inflate);

                    if (Gridpoints.Find(x => x.Collision(mouseCords)) != null)
                    {
                        Hitbox temp2 = CalculateHitbox(mouseCords, new Point(0, 0));
                        if (!GridpointLegal(temp2))
                        {
                            temp2.ShowOverlap(g);
                            Console.WriteLine("Overlap!");
                            Legal = false;
                            g.FillRectangle(Notsolidred, _rec);
                        }
                        else
                        {
                            Legal = true;
                            g.FillRectangle(Notsolidgreen, _rec);
                        }
                    }
                    else
                    {
                        g.FillRectangle(Notsolidorange, _rec);
                    }

                    return;
                }

                if (firstClick == true || General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "X")
                {
                    return;
                }

                Hitbox temp = CalculateHitbox(firstPoint.Cords, mouseCords);
                Legal = true;
                if (!GridpointLegal(temp))
                {
                    Legal = false;
                    temp.ShowOverlap(g);
                    Console.WriteLine("Overlap");
                }

                Rectangle rec = new Rectangle(Math.Min(firstPoint.Cords.X, mouseCords.X), Math.Min(firstPoint.Cords.Y, mouseCords.Y), Math.Abs(firstPoint.Cords.X - mouseCords.X), Math.Abs(firstPoint.Cords.Y - mouseCords.Y));

                if (!Legal)
                {
                    g.FillRectangle(Notsolidred, rec);
                }
                else if (Gridpoints.Find(x => x.Collision(mouseCords)) == null)
                {
                    g.FillRectangle(Notsolidorange, rec);
                }
                else
                {
                    g.FillRectangle(Notsolidgreen, rec);
                }


            }
        }

        Hitbox CalculateHitbox(Point firstpoint, Point mousecords)
        {
            int lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
            if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Diagonal") //Type = Diagonal
            {
                Point[] _points = RoadMath.hitBoxPointsDiagonal(firstpoint, mousecords, lanes, 20, true, RoadMath.calculateSlope(firstpoint, mousecords), true);
                return new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);
            }

            else if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Curved" || General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Curved2") //Type = Curved
            {
                string _dir = RoadMath.Direction(firstpoint, mousecords, General_Form.Main.BuildScreen.builder.roadBuilder.roadType);
                Point _temp1 = firstpoint;
                Point _temp2 = mousecords;

                if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Curved")
                {
                    if (_dir == "NW")
                    {
                        _dir = "SE";
                        firstpoint = _temp2;
                        mousecords = _temp1;
                    }
                    else if (_dir == "NE")
                    {
                        _dir = "SW";
                        firstpoint = _temp2;
                        mousecords = _temp1;
                    }
                }
                else if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Curved2")
                {
                    if (_dir == "SE")
                    {
                        _dir = "NW";
                        firstpoint = _temp2;
                        mousecords = _temp1;
                    }
                    else if (_dir == "SW")
                    {
                        _dir = "NE";
                        firstpoint = _temp2;
                        mousecords = _temp1;
                    }
                }

                Point[] _points = RoadMath.hitBoxPointsCurved(firstpoint, mousecords, lanes, 20, true, _dir);
                return new CurvedHitbox(_points[0], _points[1], _points[2], _points[3], _dir, Color.Red);
            }

            else if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Cross") //Type = Cross
            {
                Point[] _points = RoadMath.hitBoxPointsCross(firstpoint, mousecords, lanes, 20, true);
                return new RectHitbox(_points[0], _points[1], _points[2], _points[3], Color.Red);
            }
            return null;
        }

        public bool GridpointLegal(Hitbox h)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (h.Contains(p.Cords))
                {
                    if (p.Used)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void FlipGridpointsToTrue(Hitbox h)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (h.Contains(p.Cords))
                {
                    p.Used = true;
                }
            }
        }

        public void FlipGridpointsToFalse(Hitbox h)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (h.Contains(p.Cords))
                {
                    p.Used = false;
                }
            }
        }

        public void UndoGridpoints(AbstractRoad r)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (r.hitbox.Contains(p.Cords))
                {
                    p.Used = false;
                }
            }
        }

        public void ResetAllGridpoints()
        {
            foreach (Gridpoint p in Gridpoints)
            {
                p.Used = false;
            }
        }

        public void ConnectionGridpointsToFalse()
        {
            Point _topleft, _topright, _bottomleft, _bottomright;
            foreach (AbstractRoad _road in Builder.roadBuilder.roads)
            {
                int _lanes = _road.lanes;
                if (!_road.beginconnection)
                {
                    if (_lanes == 1)
                    {
                        _topleft = new Point(_road.point1.X - 10, _road.point1.Y - 10);
                        _topright = new Point(_road.point1.X + 10, _road.point1.Y - 10);
                        _bottomleft = new Point(_road.point1.X - 10, _road.point1.Y + 10);
                        _bottomright = new Point(_road.point1.X + 10, _road.point1.Y + 10);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }

                    if (_lanes % 2 != 0 && _lanes != 1)
                    {
                        Console.WriteLine("Begin, oneven lanes, horuiteinde");
                        _topleft = new Point(_road.point1.X + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingWidth, _road.point1.Y + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingHeight);
                        _topright = new Point(_road.point1.X + ((_lanes - 1) / 2) * Grid.Config.SpacingWidth, _road.point1.Y + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingHeight);
                        _bottomleft = new Point(_road.point1.X + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingWidth, _road.point1.Y + ((_lanes - 1) / 2) * Grid.Config.SpacingHeight);
                        _bottomright = new Point(_road.point1.X + ((_lanes - 1) / 2) * Grid.Config.SpacingWidth, _road.point1.Y + ((_lanes - 1) / 2) * Grid.Config.SpacingHeight);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }
                    else
                    {
                        Console.WriteLine("Begin, even lanes, horuiteinde");
                        _topleft = new Point(_road.point1.X + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingWidth, _road.point1.Y + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingHeight);
                        _topright = new Point(_road.point1.X + (_lanes / 2) * Grid.Config.SpacingWidth, _road.point1.Y + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingHeight);
                        _bottomleft = new Point(_road.point1.X + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingWidth, _road.point1.Y + (_lanes / 2) * Grid.Config.SpacingHeight);
                        _bottomright = new Point(_road.point1.X + (_lanes / 2) * Grid.Config.SpacingWidth, _road.point1.Y + (_lanes / 2) * Grid.Config.SpacingHeight);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }
                }
                if (!_road.endconnection)
                {
                    if (_lanes == 1)
                    {
                        _topleft = new Point(_road.point2.X - 10, _road.point2.Y - 10);
                        _topright = new Point(_road.point2.X + 10, _road.point2.Y - 10);
                        _bottomleft = new Point(_road.point2.X - 10, _road.point2.Y + 10);
                        _bottomright = new Point(_road.point2.X + 10, _road.point2.Y + 10);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }
                    if (_lanes % 2 != 0)
                    {
                        Console.WriteLine("Eind, oneven lanes, horuiteinde");
                        _topleft = new Point(_road.point2.X + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingHeight);
                        _topright = new Point(_road.point2.X + ((_lanes - 1) / 2) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingHeight);
                        _bottomleft = new Point(_road.point2.X + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingWidth, _road.point2.Y + ((_lanes - 1) / 2) * Grid.Config.SpacingHeight);
                        _bottomright = new Point(_road.point2.X + ((_lanes - 1) / 2) * Grid.Config.SpacingWidth, _road.point2.Y + ((_lanes - 1) / 2) * Grid.Config.SpacingHeight);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }
                    else
                    {
                        Console.WriteLine("Eind, even lanes, horuiteinde");
                        _topleft = new Point(_road.point2.X + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingHeight);
                        _topright = new Point(_road.point2.X + (_lanes / 2) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingHeight);
                        _bottomleft = new Point(_road.point2.X + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingWidth, _road.point2.Y + (_lanes / 2) * Grid.Config.SpacingHeight);
                        _bottomright = new Point(_road.point2.X + (_lanes / 2) * Grid.Config.SpacingWidth, _road.point2.Y + (_lanes / 2) * Grid.Config.SpacingHeight);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }


                    if (_lanes % 2 != 0)
                    {
                        Console.WriteLine("Eind, oneven lanes, vertuiteinde");
                        _topleft = new Point(_road.point2.X + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingHeight);
                        _topright = new Point(_road.point2.X + ((_lanes - 1) / 2) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingHeight);
                        _bottomleft = new Point(_road.point2.X + (-1 * ((_lanes - 1) / 2)) * Grid.Config.SpacingWidth, _road.point2.Y + ((_lanes - 1) / 2) * Grid.Config.SpacingHeight);
                        _bottomright = new Point(_road.point2.X + ((_lanes - 1) / 2) * Grid.Config.SpacingWidth, _road.point2.Y + ((_lanes - 1) / 2) * Grid.Config.SpacingHeight);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);

                    }
                    else
                    {
                        Console.WriteLine("Eind, even lanes, vertuiteinde");
                        _topleft = new Point(_road.point2.X + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingHeight);
                        _topright = new Point(_road.point2.X + (_lanes / 2) * Grid.Config.SpacingWidth, _road.point2.Y + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingHeight);
                        _bottomleft = new Point(_road.point2.X + (-1 * ((_lanes / 2) - 1)) * Grid.Config.SpacingWidth, _road.point2.Y + (_lanes / 2) * Grid.Config.SpacingHeight);
                        _bottomright = new Point(_road.point2.X + (_lanes / 2) * Grid.Config.SpacingWidth, _road.point2.Y + (_lanes / 2) * Grid.Config.SpacingHeight);
                        RectHitbox _temp = new RectHitbox(_topleft, _topright, _bottomleft, _bottomright, Color.Red);
                        FlipGridpointsToFalse(_temp);
                    }

                }
            }
        }
    }
}
