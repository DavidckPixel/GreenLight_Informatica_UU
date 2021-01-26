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
    public class GridController : EntityController
    {
        //This controller temperaly draws and creates the GridPoints, this will be changed in later versions
        //The data for the GridPoints (distance width/ height) is stored within a json file, and read in in the GridConfig class
        //Every Gridpoint object contains a rectangle Hitbox, to see if the user has clicked on a point this class returns a bool
        //whether or not the point is in the Hitbox.

        public List<Gridpoint> Gridpoints = new List<Gridpoint>();
        bool firstClick;
        PictureBox canvas;
        private bool _points_visible = true;

        public Gridpoint firstPoint = null;
        public Gridpoint secondPoint = null;

        private Point mousecords;
        private bool legal;
        BuilderController builder;

        public GridController(PictureBox _bitmap, BuilderController _builder)
        {
            this.canvas = _bitmap;
            CreateGridPoints();
            this.canvas.MouseClick += OnClick;
            this.canvas.MouseMove += moveMouse;
            this.firstClick = true;

            this.builder = _builder;
        }

        public bool Set_visible
        {
            set
            {
                _points_visible = value;
            }
        }

        public override void Initialize()
        {

        }

        public void canvas_resize(Size _size)
        {
            this.canvas.Size = _size;
        }

        public void CreateGridPoints()
        {
            int _amountX = canvas.Width / Grid.Config.SpacingWidth;
            int _amountY = canvas.Height / Grid.Config.SpacingHeight;

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

            Gridpoint _point = Gridpoints.Find(x => x.Collision(mea.Location));

            if (_type == "X" || _type == "D" /*|| _point == null*/)
            {

                return;
            }

            if (_type == "Cross")
            {
                if (legal)
                {
                    builder.BuildRoad(_point.Cords, _point.Cords);
                    Hitbox temp4 = calculateRect(firstPoint.Cords, secondPoint.Cords);
                    flipGridpoints(temp4);
                }
                return;
            }


            Console.WriteLine("MouseClick Button: " + mea.Button);

            if (mea.Button == MouseButtons.Right)
            {
                this.ResetPoints();
            }
            else if (firstClick)
            {
                legal = true;
                if (_point != null)
                {
                    Console.WriteLine("First PointClick!");
                    Console.WriteLine(_point.Cords);
                    this.firstClick = false;
                    this.firstPoint = _point;
                }
            }
            else
            {
                this.secondPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (this.secondPoint != null && this.secondPoint != this.firstPoint && legal)
                {
                    Console.WriteLine("Second PointClick!");
                    Console.WriteLine(_point.Cords);
                    this.secondPoint = _point;
                    builder.BuildRoad(this.firstPoint.Cords, this.secondPoint.Cords);
                    Hitbox temp3 = calculateRect(firstPoint.Cords, secondPoint.Cords);
                    flipGridpoints(temp3);
                    this.ResetPoints();
                }
                else if (!legal)
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
            this.canvas.Invalidate();
        }

        public void moveMouse(object o, MouseEventArgs mea)
        {
            if (firstClick == true && General_Form.Main.BuildScreen.builder.roadBuilder.roadType != "Cross")
            {
                return;
            }

            mousecords = mea.Location;



            //Gridpoints.Find(x => x.Cords);
            //Gridpoint X = Gridpoints.Aggregate((x, y) => Math.Abs(x.Cords.X - mea.Location.X) > Math.Abs(y.Cords.X - mea.Location.X) && Math.Abs(x.Cords.Y - mea.Location.Y) > Math.Abs(y.Cords.Y - mea.Location.Y) ? x : y);


            canvas.Invalidate();
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

                if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Cross")
                {
                    Rectangle _rec = new Rectangle(mousecords, new Size(1, 1));
                    int _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
                    int _inflate = _lanes * 20 / 2;
                    _rec.Inflate(_inflate, _inflate);

                    if (Gridpoints.Find(x => x.Collision(mousecords)) != null)
                    {
                        Hitbox temp2 = calculateRect(mousecords, new Point(0, 0));
                        if (!gridpointsLegal(temp2))
                        {
                            temp2.ShowOverlap(g);
                            Console.WriteLine("Overlap!");
                            legal = false;
                            g.FillRectangle(Notsolidred, _rec);
                        }
                        else
                        {
                            legal = true;
                            g.FillRectangle(Notsolidgreen, _rec);
                        }
                    }
                    else
                    {
                        g.FillRectangle(Notsolidorange, _rec);
                    }

                    return;
                }

                if (firstClick == true)
                {
                    return;
                }

                //if (Gridpoints.Find(x => x.Collision(mousecords)) != null)  // If the cursor hovers over a gridpoint on the secondclick
                //{ //met deze regel niet gebruikt zie je nog iets beter direct wanneer het niet kan, moet het wel extra berekeningen maken, maar het is nog niet traag, dus is wel oke denk ik.
                Hitbox temp = calculateRect(firstPoint.Cords, mousecords);
                legal = true;
                if (!gridpointsLegal(temp))
                {
                    legal = false;
                    temp.ShowOverlap(g);
                    Console.WriteLine("Overlap");
                }

                Rectangle rec = new Rectangle(Math.Min(firstPoint.Cords.X, mousecords.X), Math.Min(firstPoint.Cords.Y, mousecords.Y), Math.Abs(firstPoint.Cords.X - mousecords.X), Math.Abs(firstPoint.Cords.Y - mousecords.Y));

                if (!legal)
                {
                    g.FillRectangle(Notsolidred, rec);
                }
                else if (Gridpoints.Find(x => x.Collision(mousecords)) == null)
                {
                    g.FillRectangle(Notsolidorange, rec);
                }
                else
                {
                    g.FillRectangle(Notsolidgreen, rec);
                }


            }
        }

        Hitbox calculateRect(Point firstpoint, Point mousecords)
        {
            Point topleft, topright, bottomleft, bottomright;
            int lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
            if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Diagonal") //Type = Diagonal
            {
                Point[] _points = RoadMath.hitBoxPointsDiagonal(firstpoint, mousecords, lanes, 20, true, RoadMath.calculateSlope(firstpoint, mousecords));
                return new RectHitbox(_points[1], _points[0], _points[3], _points[2], Color.Red);
                /*if (Math.Max(firstpoint.X, mousecords.X) - Math.Min(firstpoint.X, mousecords.X) < 10) //Vertical 
                {
                    topleft = new Point(Math.Min(firstpoint.X, mousecords.X) - (20 * lanes) / 2, Math.Min(firstpoint.Y, mousecords.Y));
                    topright = new Point(Math.Max(firstpoint.X, mousecords.X) + (20 * lanes) / 2, Math.Min(firstpoint.Y, mousecords.Y));
                    bottomleft = new Point(Math.Min(firstpoint.X, mousecords.X) - (20 * lanes) / 2, Math.Max(firstpoint.Y, mousecords.Y));
                    bottomright = new Point(Math.Max(firstpoint.X, mousecords.X) + (20 * lanes) / 2, Math.Max(firstpoint.Y, mousecords.Y));                    
                    return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);
                    
                }

                else if (Math.Max(firstpoint.Y, mousecords.Y) - Math.Min(firstpoint.Y, mousecords.Y) < 10) //Horizontal
                {
                    topleft = new Point(Math.Min(firstpoint.X, mousecords.X), Math.Min(firstpoint.Y, mousecords.Y) - (20 * lanes) / 2);
                    topright = new Point(Math.Max(firstpoint.X, mousecords.X), Math.Min(firstpoint.Y, mousecords.Y) - (20 * lanes) / 2);
                    bottomleft = new Point(Math.Min(firstpoint.X, mousecords.X), Math.Max(firstpoint.Y, mousecords.Y) + (20 * lanes) / 2);
                    bottomright = new Point(Math.Max(firstpoint.X, mousecords.X), Math.Max(firstpoint.Y, mousecords.Y) + (20 * lanes) / 2);                    
                    return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);
                }

                else //Diagonal
                {
                    
                    float xDif = mousecords.X - firstpoint.X;
                    float yDif = mousecords.Y - firstpoint.Y;

                    double angle = (Math.Atan2(yDif, xDif) * 180.0 / Math.PI);

                    if (0 < angle && angle < 90) //SE
                    {
                        topleft = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        topright = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        bottomleft = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        bottomright = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));                        
                        return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);
                    }

                    if (90 < angle && angle < 180) //SW 
                    {
                        topleft = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        topright = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        bottomleft = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        bottomright = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);
                    }

                    if (-180 < angle && angle < -90) //NW
                    {
                        topleft = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        topright = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        bottomleft = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        bottomright = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);
                    }

                    if (-90 < angle && angle < 0) //NE
                    {
                        topleft = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        topright = new Point((int)(mousecords.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(mousecords.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        bottomleft = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle - 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle - 90)));
                        bottomright = new Point((int)(firstpoint.X + ((20 * lanes) / 2) * Math.Cos(angle + 90)), (int)(firstpoint.Y + ((20 * lanes) / 2) * Math.Sin(angle + 90)));
                        return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);
                    }*/

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
                /*topleft = new Point(Math.Min(firstpoint.X, mousecords.X) - (20 * lanes) / 2, Math.Min(firstpoint.Y, mousecords.Y));
                topright = new Point(Math.Max(firstpoint.X, mousecords.X) + (20 * lanes) / 2, Math.Min(firstpoint.Y, mousecords.Y));
                bottomleft = new Point(Math.Min(firstpoint.X, mousecords.X) - (20 * lanes) / 2, Math.Max(firstpoint.Y, mousecords.Y));
                bottomright = new Point(Math.Max(firstpoint.X, mousecords.X) + (20 * lanes) / 2, Math.Max(firstpoint.Y, mousecords.Y));
                return new RectHitbox(topleft, topright, bottomleft, bottomright, Color.Red);*/

            }

            else if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Cross") //Type = Cross
            {
                Point[] _points = RoadMath.hitBoxPointsCross(firstpoint, mousecords, lanes, 20, true);
                return new RectHitbox(_points[0], _points[1], _points[2], _points[3], Color.Red);
            }
            return null;
        }

        public bool gridpointsLegal(Hitbox h)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (h.Contains(p.Cords))
                {
                    if (p.used)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void flipGridpoints(Hitbox h)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (h.Contains(p.Cords))
                {
                    p.used = true;
                }
            }
        }

        public void undoGridpoints(AbstractRoad r)
        {
            foreach (Gridpoint p in Gridpoints)
            {
                if (r.hitbox.Contains(p.Cords))
                {
                    p.used = false;
                }
            }
        }

        public void resetGridpoints()
        {
            foreach (Gridpoint p in Gridpoints)
            {
                p.used = false;
            }
        }
        //CLICK
    }
}
