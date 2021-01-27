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
    public partial class ElementsSubRoadsMenu : UserControl
    {
        public TextBox LaneAmount;
        public List<CurvedButtons> ESRM = new List<CurvedButtons>();
        public ElementsSubRoadsMenu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Menu_width, UserControls.Config.buildElementsMenu["elementsXbase"]);
            //this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Sub_menu_width, UserControls.Config.buildElementsMenu["elementsXbase"]);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = UserControls.Config.buildElementsMenu;
            int _ButtonSize = menu["buttonSize"];
            int _ButtonXbase = menu["buttonXbase"];
            int _ButtonXdiff = menu["buttonXdiff"];
            int _ButtonYbase = menu["buttonYbase"];
            int _ButtonYdiff = menu["buttonYdiff"];
            int _ButtonCurve = menu["buttonCurve"];

            CurvedButtons Hand = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Hand_Button.png", this.BackColor);
            Hand.Click += (object o, EventArgs EA) => { ResetButtons(Hand, Hand.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "X"; };
            this.Controls.Add(Hand);
            ESRM.Add(Hand);

            CurvedButtons Diagonal_Road = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Road_Button.png", this.BackColor);
            Diagonal_Road.Click += (object o, EventArgs EA) => { ResetButtons(Diagonal_Road, Diagonal_Road.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Diagonal"; };
            this.Controls.Add(Diagonal_Road);
            ESRM.Add(Diagonal_Road);

            CurvedButtons Curved_Road = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Curved_Road_Button.png", this.BackColor);
            Curved_Road.Click += (object o, EventArgs EA) => { ResetButtons(Curved_Road, Curved_Road.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Curved"; };
            this.Controls.Add(Curved_Road);
            ESRM.Add(Curved_Road);

            CurvedButtons Curved_Road2 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/CurveHollow_Button.png", this.BackColor);
            Curved_Road2.Click += (object o, EventArgs EA) => { ResetButtons(Curved_Road2, Curved_Road2.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Curved2"; };
            this.Controls.Add(Curved_Road2);
            ESRM.Add(Curved_Road2);


            CurvedButtons Crossroad = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/CrossRoad_Button.png", this.BackColor);
            Crossroad.Click += (object o, EventArgs EA) => { ResetButtons(Crossroad, Crossroad.Image_path); General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "Cross"; };
            this.Controls.Add(Crossroad);
            ESRM.Add(Crossroad);

            PictureBox LaneAmount_background = new PictureBox();
            LaneAmount_background.Image = Image.FromFile("../../src/User Interface Recources/Lane_Amount_Border.png");
            LaneAmount_background.Location = new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase+_ButtonYdiff*1);
            LaneAmount_background.SizeMode = PictureBoxSizeMode.Zoom;
            LaneAmount_background.Size = new Size(_ButtonSize, _ButtonSize);
            this.Controls.Add(LaneAmount_background);

            this.LaneAmount = new TextBox();
            LaneAmount.BorderStyle = BorderStyle.None;
            LaneAmount.Text = "1";
            LaneAmount.MaxLength = 1;
            LaneAmount.Width = 20;
            LaneAmount.Height = 20;
            LaneAmount.TextAlign = HorizontalAlignment.Center;
            LaneAmount.Location = new Point((_ButtonXbase + _ButtonXdiff * 2) + (_ButtonSize / 2 - LaneAmount.Size.Width / 2), _ButtonYbase + _ButtonYdiff + _ButtonSize / 2 - LaneAmount.Size.Height / 2);
            this.Controls.Add(LaneAmount);
            LaneAmount.BringToFront();

        }
        private void ResetButtons(CurvedButtons Selected, string Filepath)
        {
            foreach (CurvedButtons x in ESRM)
            {
                x.Selected = false;
                x.Image = Image.FromFile(x.Image_path.Remove(x.Image_path.Length - 10) + "Button.png");
            }
            Selected.Selected = true;
            Selected.Image = Image.FromFile(Filepath.Remove(Filepath.Length - 10) + "Select.png");
        }
    }
}