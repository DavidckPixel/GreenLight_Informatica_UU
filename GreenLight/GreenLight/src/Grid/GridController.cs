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
        public GridConfig config;
        bool firstClick;
        PictureBox canvas;
        private bool _points_visible = true;

        public Gridpoint firstPoint = null;
        public Gridpoint secondPoint = null;

        private Point mousecords;

        BuilderController builder;

        public GridController(PictureBox _bitmap, BuilderController _builder)
        {
            this.canvas = _bitmap;
            GridConfig.Init(ref this.config);
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
            int _amountX = canvas.Width / config.SpacingWidth;
            int _amountY = canvas.Height / config.SpacingHeight;

            for (int y = 0; y < _amountY; y++)
            { 
                for (int x = 0; x < _amountX; x++)
                {
                    
                    Gridpoints.Add(new Gridpoint(new Point(x * config.SpacingWidth, y * config.SpacingHeight), 5));
                }
            }
        }

        public void OnClick(Object o, MouseEventArgs mea)
        {
            string _type = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;

            Gridpoint _point = Gridpoints.Find(x => x.Collision(mea.Location));

            if (_type == "X" || _type == "D" || _point == null)
            {

                return;
            }

            if (_type == "Cross")
            {
                builder.BuildRoad(_point.Cords, _point.Cords);
                return;
            }


            Console.WriteLine("MouseClick Button: " + mea.Button);

            if (mea.Button == MouseButtons.Right)
            {
                this.ResetPoints();
            }

            if (firstClick)
            {
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
                if (this.secondPoint != null && this.secondPoint != this.firstPoint)
                {
                    Console.WriteLine("Second PointClick!");
                    Console.WriteLine(_point.Cords);
                    this.secondPoint = _point;
                   
                    builder.BuildRoad(this.firstPoint.Cords, this.secondPoint.Cords);
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
            if(_points_visible)
            {
            foreach(Gridpoint x in Gridpoints)
            {
                x.DrawGrid(g);
                }

                Brush Notsolid = new SolidBrush(Color.FromArgb(100, Color.DarkRed));

                if (General_Form.Main.BuildScreen.builder.roadBuilder.roadType == "Cross")
                {
                    Rectangle _rec = new Rectangle(mousecords, new Size(1, 1));
                    int _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
                    int _inflate = _lanes * 20 / 2;
                    _rec.Inflate(_inflate,_inflate);
                    g.FillRectangle(Notsolid, _rec);

                    return;
                }

                if (firstClick == true)
                {
                    return;
                }

                /*if (Gridpoints.Find(x => x.Collision(mousecords)) != null)  // If the cursor hovers over a gridpoint on the secondclick
                {*/ //met deze regel niet gebruikt zie je nog iets beter direct wanneer het niet kan, moet het wel extra berekeningen maken, maar het is nog niet traag, dus is wel oke denk ik.
                    RectHitbox temp = new RectHitbox(new Point(Math.Min(firstPoint.Cords.X, mousecords.X), Math.Min(firstPoint.Cords.Y, mousecords.Y)),
                                                    new Point(Math.Max(firstPoint.Cords.X, mousecords.X), Math.Min(firstPoint.Cords.Y, mousecords.Y)),
                                                    new Point(Math.Min(firstPoint.Cords.X, mousecords.X), Math.Max(firstPoint.Cords.Y, mousecords.Y)),
                                                    new Point(Math.Max(firstPoint.Cords.X, mousecords.X), Math.Max(firstPoint.Cords.Y, mousecords.Y)), Color.Red); //Recthitbox
                    Rectangle rectemp = new Rectangle(Math.Min(firstPoint.Cords.X, mousecords.X), Math.Min(firstPoint.Cords.Y, mousecords.Y), Math.Abs(firstPoint.Cords.X - mousecords.X), Math.Abs(firstPoint.Cords.Y - mousecords.Y));

                    Console.WriteLine(temp.topleft + " " + temp.topright + " " + temp.bottomleft + " " + temp.bottomright); //this is just to check the cords of the rectangle that is created.
                    foreach (AbstractRoad road in builder.roadBuilder.roads) // loops through all roads
                    {
                        Console.WriteLine("Collides: " + temp.Collide(road.hitbox));
                        if (temp.Collide(road.hitbox)) // this always returns false for some reason...
                        {
                            g.FillRectangle(Brushes.Red, rectemp);
                            Console.WriteLine("Overlap!");
                            // ... code to not let the user place the road has to be written here...
                        }
                    }
                //}

                Rectangle rec = new Rectangle(Math.Min(firstPoint.Cords.X, mousecords.X), Math.Min(firstPoint.Cords.Y, mousecords.Y), Math.Abs(firstPoint.Cords.X - mousecords.X), Math.Abs(firstPoint.Cords.Y - mousecords.Y));
                g.FillRectangle(Notsolid, rec);
            }
        }

        //CLICK
    }
}
