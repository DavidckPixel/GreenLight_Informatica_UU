using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

//The build controller is the controller that deals with all the actual building on the road builder. Its most important feature is the BuildRoad feature that can be called and will
//then proceed to build the road
//Its also the controller that holds 3 different very important controllers, 
//the roadcontroller that does al the road building and drawing
//the gridcontroller that draws the backgrid and also deals with the clicking on the points
//the signcontroller that deals with all the sign data

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
            int _lanes = 1;
            try
            {
                int _lanestext = Int32.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
                
                if (_lanestext > 0 && _lanestext < 20)
                {
                    _lanes = _lanestext;
                }
                else
                {
                    General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text = _lanes.ToString();
                }
                
            }
            catch (Exception e)
            {                
                General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text = _lanes.ToString();
            }
            
             
            

            switch (roadBuilder.roadType)
            {
                case "Diagonal":
                    roadBuilder.BuildDiagonalRoad(_point1, _point2, _lanes, false, false, null, null);
                    break;
                case "Curved":
                    if(_point1.X != _point2.X && _point1.Y != _point2.Y)
                        roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, "Curved", false, false, null, null);
                    break;
                case "Curved2":
                    if (_point1.X != _point2.X && _point1.Y != _point2.Y)
                        roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, "Curved2", false, false, null, null);
					break;
                case "Cross":
                    roadBuilder.BuildCrossRoad(_point1, _lanes, false, false);
                    break;
                case "X":
                    break;
            }

            Screen.Invalidate();
        }

    }
}
