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
            int _lanes = 1;
            try { _lanes = int.Parse(General_Form.Main.UserInterface.ElemSRM.LaneAmount.Text); }
            catch { _lanes = 1; }

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

                case "X":
                    break;
            }

            Screen.Invalidate();
        }

    }
}