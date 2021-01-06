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
        PictureBox Screen;
        public string roadType = "X";

        public BuilderController(PictureBox _screen)
        {
            this.Screen = _screen;
            this.gridController = new GridController(_screen, this);
            this.roadBuilder = new RoadController();
        }

        public override void Initialize()
        {        }

        public void BuildRoad(Point _point1, Point _point2)
        {

            int _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);

            switch (roadType)
            {
                case "Diagonal":
                    roadBuilder.BuildDiagonalRoad(_point1, _point2, _lanes, false, false);
                    break;
                case "Curved":
                    if(_point1.X != _point2.X && _point1.Y != _point2.Y)
                        roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, false, false);
                    break;
                case "X":
                    break;
            }

            Screen.Invalidate();
        }
    }
}
