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
            string _roadType = "Curved";

            switch (_roadType)
            {
                case "Straight":
                    roadBuilder.BuildStraightRoad(_point1, _point2);
                    break;
                case "Diagnol":
                    roadBuilder.BuildDiagnolRoad(_point1, _point2);
                    break;
                case "Curved":
                    roadBuilder.BuildCurvedRoad(_point1, _point2);
                    break;
            }

            Screen.Invalidate();
        }
    }
}
