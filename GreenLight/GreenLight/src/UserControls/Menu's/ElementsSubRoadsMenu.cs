using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    /* This is the Elements sub menu class for roads. This class has a method AdjustSize to fit the size of the users window.
   This user control is shown when the user is in the building screen and has selected roads to build.
   Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class ElementsSubRoadsMenu : UserControl
    {
        public TextBox LaneAmount;
        public List<CurvedButtons> esrmButtons = new List<CurvedButtons>();
        public CurvedButtons Hand, diagonalRoad, curvedRoad, curvedRoad2, crossRoad;
        public PictureBox laneAmountDesign;

        public ElementsSubRoadsMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(_menuwidth, _form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(_form.Width - _menuwidth, UserControls.Config.buildElementsMenu["elementsXbase"]);
            Initialize(_form, _menuwidth, _dosisfontfamily);
        }
        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(_form.Width - _submenuwidth, UserControls.Config.buildElementsMenu["elementsXbase"]);
            this.Controls.Clear();
            Initialize(_form, _submenuwidth, _dosisfontfamily);
        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> menu = UserControls.Config.buildElementsMenu;
            int _ButtonSize = menu["buttonSize"];
            int _ButtonXbase = menu["buttonXbase"];
            int _ButtonXdiff = menu["buttonXdiff"];
            int _ButtonYbase = menu["buttonYbase"];
            int _ButtonYdiff = menu["buttonYdiff"];
            int _ButtonCurve = menu["buttonCurve"];

            laneAmountDesign = new PictureBox();
            laneAmountDesign.Image = Image.FromFile("../../src/User Interface Recources/Lane_Amount_Border.png");
            laneAmountDesign.Location = new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase + _ButtonYdiff * 1);
            laneAmountDesign.SizeMode = PictureBoxSizeMode.Zoom;
            laneAmountDesign.Size = new Size(_ButtonSize, _ButtonSize);
            this.Controls.Add(laneAmountDesign);

            this.LaneAmount = new TextBox();
            this.LaneAmount.BorderStyle = BorderStyle.None;
            this.LaneAmount.Text = "1";
            this.LaneAmount.MaxLength = 1;
            this.LaneAmount.Width = 20;
            this.LaneAmount.Height = 20;
            this.LaneAmount.TextAlign = HorizontalAlignment.Center;
            this.LaneAmount.Location = new Point((_ButtonXbase + _ButtonXdiff * 2) + (_ButtonSize / 2 - LaneAmount.Size.Width / 2), _ButtonYbase + _ButtonYdiff + _ButtonSize / 2 - LaneAmount.Size.Height / 2);
            this.Controls.Add(LaneAmount);
            this.LaneAmount.BringToFront();

            /*     Buttons & Dividers    */

            CurvedButtons Hand = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Hand_Button.png", this.BackColor);
            Hand.Click += (object o, EventArgs EA) => 
            { 
                ResetButtons(Hand, Hand.Image_path);
                General_Form.Main.BuildScreen.builder.roadBuilder.lastType = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;
                General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "X"; 
            };
            this.Controls.Add(Hand);
            ESRM.Add(Hand);

            CurvedButtons Diagonal_Road = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Road_Button.png", this.BackColor);
            Diagonal_Road.Click += (object o, EventArgs EA) => 
            { 
                ResetButtons(Diagonal_Road, Diagonal_Road.Image_path);
                General_Form.Main.BuildScreen.builder.roadBuilder.lastType = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;
                General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Diagonal";                 
            };
            this.Controls.Add(Diagonal_Road);
            ESRM.Add(Diagonal_Road);

            CurvedButtons Curved_Road = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Curved_Road_Button.png", this.BackColor);
            Curved_Road.Click += (object o, EventArgs EA) => 
            {
                ResetButtons(Curved_Road, Curved_Road.Image_path);
                General_Form.Main.BuildScreen.builder.roadBuilder.lastType = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;
                General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Curved"; 
            };
            this.Controls.Add(Curved_Road);
            ESRM.Add(Curved_Road);

            CurvedButtons Curved_Road2 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/CurveHollow_Button.png", this.BackColor);
            Curved_Road2.Click += (object o, EventArgs EA) => 
            { 
                ResetButtons(Curved_Road2, Curved_Road2.Image_path);
                General_Form.Main.BuildScreen.builder.roadBuilder.lastType = General_Form.Main.BuildScreen.builder.roadBuilder.roadType;
                General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Curved2"; 
            };
            this.Controls.Add(Curved_Road2);
            ESRM.Add(Curved_Road2);

            Hand = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Hand_Button.png", this.BackColor);
            Hand.Click += (object o, EventArgs EA) => { ResetButtons(Hand, Hand.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "X"; };
            this.Controls.Add(Hand);
            esrmButtons.Add(Hand);

            diagonalRoad = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Road_Button.png", this.BackColor);
            diagonalRoad.Click += (object o, EventArgs EA) => { ResetButtons(diagonalRoad, diagonalRoad.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Diagonal"; };
            this.Controls.Add(diagonalRoad);
            esrmButtons.Add(diagonalRoad);

            curvedRoad = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Curved_Road_Button.png", this.BackColor);
            curvedRoad.Click += (object o, EventArgs EA) => { ResetButtons(curvedRoad, curvedRoad.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Curved"; };
            this.Controls.Add(curvedRoad);
            esrmButtons.Add(curvedRoad);

            curvedRoad2 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/CurveHollow_Button.png", this.BackColor);
            curvedRoad2.Click += (object o, EventArgs EA) => { ResetButtons(curvedRoad2, curvedRoad2.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Curved2"; };
            this.Controls.Add(curvedRoad2);
            esrmButtons.Add(curvedRoad2);

            crossRoad = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/CrossRoad_Button.png", this.BackColor);
            crossRoad.Click += (object o, EventArgs EA) => { ResetButtons(crossRoad, crossRoad.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Cross"; };
            this.Controls.Add(crossRoad);
            esrmButtons.Add(crossRoad);
        }

        private void ResetButtons(CurvedButtons _selected, string _filepath)
        {
            foreach (CurvedButtons x in esrmButtons)
            {
                x.Selected = false;
                x.Image = Image.FromFile(x.Image_path.Remove(x.Image_path.Length - 10) + "Button.png");
            }
            _selected.Selected = true;
            _selected.Image = Image.FromFile(_filepath.Remove(_filepath.Length - 10) + "Select.png");
        }
    }
}