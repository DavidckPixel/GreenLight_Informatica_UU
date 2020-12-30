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
    class GridController : Form
    {
        //This controller temperaly draws and creates the GridPoints, this will be changed in later versions
        //The data for the GridPoints (distance width/ height) is stored within a json file, and read in in the GridConfig class
        //Every Gridpoint object contains a rectangle Hitbox, to see if the user has clicked on a point this class returns a bool
        //whether or not the point is in the Hitbox.

        public List<Gridpoint> Gridpoints = new List<Gridpoint>();
        public GridConfig config;
        bool firstClick;
        string Selected;
        
        public GridController()
        {
            GridConfig.Init(ref this.config);
            CreateGridPoints();
            this.Paint += DrawGridPoints;
            this.MouseClick += OnClick;

            this.Invalidate();
        }

        public void CreateGridPoints()
        {
            for(int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Gridpoints.Add(new Gridpoint(new Point(x * config.SpacingWidth, y * config.SpacingHeight), new Size(5,5)));
                }
            }
        }

        public void OnClick(Object o, MouseEventArgs mea)
        {
            Gridpoint _firstPoint = null;
            Gridpoint _secondPoint = null;


            if (firstClick)
            {
                _firstPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_firstPoint != null)
                {
                    Console.WriteLine("PointClick!");
                    firstClick = false;
                }

                
            }
            else
            {
                _secondPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_secondPoint != null && _secondPoint != _firstPoint)
                {
                    firstClick = true;
                    if (Selected == "Roads") { };
                    //CreateRoad(_firstPoint, _secondPoint);

                    Console.WriteLine("PointClick!");
                }
            }
        }

        

        

        public void DrawGridPoints(Object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            foreach(Gridpoint x in Gridpoints)
            {
                x.DrawGrid(g);
            }
        }

        //CLICK
    }
}
