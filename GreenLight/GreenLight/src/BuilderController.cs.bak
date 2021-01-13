using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class BuilderController : AbstractController
    {
        public RoadController roadBuilder;
        public GridController gridController;
        public MainSignController signController;

        PictureBox Screen;
        //public string roadType = "X";
        //public string signType = "X";

        public BuilderController(PictureBox _screen, Form _main)
        {
            this.Screen = _screen;
            this.gridController = new GridController(_screen, this);
            this.roadBuilder = new RoadController(_screen);
            this.signController = new MainSignController(_main, _screen);
        }

        public override void Initialize()
        {        }

        public void BuildRoad(Point _point1, Point _point2)
        {
            int _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);

            switch (roadBuilder.roadType)
            {
                case "Diagonal":
                    roadBuilder.BuildDiagonalRoad(_point1, _point2, _lanes, false, false);
                    break;
                case "Curved":
                    if(_point1.X != _point2.X && _point1.Y != _point2.Y)
                        roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, roadBuilder.roadType, false, false);
                    break;
                case "Curved2":
                    if (_point1.X != _point2.X && _point1.Y != _point2.Y)
                        roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, roadBuilder.roadType, false, false);
                    break;
                case "X":
                    break;
            }

            Screen.Invalidate();
        }

    }
}
