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
            switch (roadType)
            {
                case "Straight":
                    roadBuilder.BuildStraightRoad(_point1, _point2);
                    break;
                case "Diagonal":
                    roadBuilder.BuildDiagnolRoad(_point1, _point2);
                    break;
                case "Curved":
                    if(_point1.X != _point2.X && _point1.Y != _point2.Y)
                        roadBuilder.BuildCurvedRoad(_point1, _point2);
                    break;
                case "X":
                    break;
            }

            Screen.Invalidate();
        }
    }
}
