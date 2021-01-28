using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;



namespace GreenLight
{
    // The BuilderController is the controller that deals with all the actual building of the road junctions. 
    // Its most important feature is the BuildRoad feature that can be called and will then proceed to build a road.
    // It is also the controller that holds three different very important controllers; 
    // the RoadController that does al the road building and drawing,
    // the gridcontroller that draws the backgrid and also deals with the clicking on the points,
    // the signcontroller that deals with all the sign data.

    public class BuilderController : AbstractController
    {
        
        public GridController gridController;
        public RoadController roadBuilder;
        public MainSignController signController;

        public PictureBox Screen;

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
                _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text);
            }
            catch 
            { 
                _lanes = 1; 
            }

            if(_lanes > 5)
            {
                _lanes = 5;
                General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text = "5";
            }

            switch (roadBuilder.roadType)
            {
                case "Diagonal":
                    if (_point1 != _point2)
                        roadBuilder.BuildDiagonalRoad(_point1, _point2, _lanes, false, false, null, null);
                    else
                        Console.WriteLine("Same point clicked");
                    break;

                case "Curved":
                    if (_point1 != _point2 && _point1.X != _point2.X && _point1.Y != _point2.Y)
                    {
                        if (Math.Abs(_point1.X - _point2.X) / Math.Abs(_point1.Y - _point2.Y) < 8 && Math.Abs(_point1.Y - _point2.Y) / Math.Abs(_point1.X - _point2.X) < 8)
                            roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, "Curved", false, false, null, null);
                        else
                            Console.WriteLine("This is to steep for a curved road");
                    }
                    else
                        Console.WriteLine("This is not a valid region");
                    break;

                case "Curved2":
                    if (_point1 != _point2 && _point1.X != _point2.X && _point1.Y != _point2.Y)
                    {
                        if (Math.Abs(_point1.X - _point2.X) / Math.Abs(_point1.Y - _point2.Y) < 8 && Math.Abs(_point1.Y - _point2.Y) / Math.Abs(_point1.X - _point2.X) < 8)
                            roadBuilder.BuildCurvedRoad(_point1, _point2, _lanes, "Curved2", false, false, null, null);
                        else
                            Console.WriteLine("This is to steep for a curved road");
                    }
                    else
                        Console.WriteLine("This is not a valid region");
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
