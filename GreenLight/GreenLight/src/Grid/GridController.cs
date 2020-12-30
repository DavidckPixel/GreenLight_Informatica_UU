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
        string Selected;
        PictureBox canvas;
        private bool _points_visible = true;

        public Gridpoint firstPoint = null;
        public Gridpoint secondPoint = null;

        BuilderController builder;

        public GridController(PictureBox _bitmap, BuilderController _builder)
        {
            this.canvas = _bitmap;
            GridConfig.Init(ref this.config);
            CreateGridPoints();
            this.canvas.MouseClick += OnClick;
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
            Gridpoint _firstPoint = null;
            Gridpoint _secondPoint = null;

            Console.WriteLine("MouseClick Cords: " + mea.Location);

            if (firstClick)
            {
                _firstPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_firstPoint != null)
                {
                    Console.WriteLine("First PointClick!");
                    Console.WriteLine(_firstPoint.Cords);
                    this.firstClick = false;
                    this.firstPoint = _firstPoint;
                }
            }
            else
            {
                _secondPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_secondPoint != null && _secondPoint != _firstPoint)
                {
                    Console.WriteLine("Second PointClick!");
                    Console.WriteLine(_secondPoint.Cords);
                    this.secondPoint = _secondPoint;

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
        }

        public void DrawGridPoints(Graphics g)
        {
            if(_points_visible)
            foreach(Gridpoint x in Gridpoints)
            {
                x.DrawGrid(g);
            }
        }

        //CLICK
    }
}
