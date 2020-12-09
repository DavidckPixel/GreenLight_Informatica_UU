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
    class GridController
    {
        public List<Gridpoint> Gridpoints = new List<Gridpoint>();
        protected List<DrivingLane> drivinglanes;
        public GridConfig config;
        bool firstClick;
        int lanes = 1;
        string Selected;
        string RoadType;
        AbstractRoad roadType;
        

        public GridController()
        {
            GridConfig.Init(ref this.config);
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
                    firstClick = false;
            }
            else
            {
                _secondPoint = Gridpoints.Find(x => x.Collision(mea.Location));
                if (_secondPoint != null && _secondPoint != _firstPoint)
                {
                    firstClick = true;
                    if (Selected == "Roads") ;
                        //CreateRoad(_firstPoint, _secondPoint);
                }
            }

            //Console.WriteLine(_selectedPoint);
        }

        

        

        public void DrawGridPoints(Object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            foreach(Gridpoint x in Gridpoints)
            {
                x.Draw(g);
            }
        }

        //CLICK
    }
}
