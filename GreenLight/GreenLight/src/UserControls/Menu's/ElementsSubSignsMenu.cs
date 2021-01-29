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
    /* This is the Elements sub menu class for the signs. This class has a method AdjustSize to fit the size of the users window.
       This user control is shown when the user is in the building screen and has selected signs to build. 
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class ElementsSubSignsMenu : UserControl
    {
        public List<CurvedButtons> essmButtons = new List<CurvedButtons>();
        public CurvedButtons Hand, Traffic_light, stopSign, yieldSign, prioritySign, speedSign;

        public ElementsSubSignsMenu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Menu_width, UserControls.Config.buildElementsMenu["elementsXbase"]);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void AdjustSize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
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

            /*     Buttons & Dividers    */

            Hand = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Hand_Button.png", this.BackColor);
            Hand.Click += (object o, EventArgs EA) => { ResetButtons(Hand, Hand.Image_path); General_Form.Main.BuildScreen.builder.signController.signType = "X"; };
            this.Controls.Add(Hand);
            essmButtons.Add(Hand);

            Traffic_light = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            Traffic_light.Click += (object o, EventArgs EA) => { ResetButtons(Traffic_light, Traffic_light.Image_path); General_Form.Main.BuildScreen.builder.signController.signType = "trafficLight"; };
            this.Controls.Add(Traffic_light);
            essmButtons.Add(Traffic_light);

            speedSign = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            speedSign.Click += (object o, EventArgs EA) => { ResetButtons(speedSign, speedSign.Image_path); General_Form.Main.BuildScreen.builder.signController.signType = "speedSign"; };
            this.Controls.Add(speedSign);
            essmButtons.Add(speedSign);

            yieldSign = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase), menu["buttonCurve"], "../../src/User Interface Recources/Yield_Sign_Button.png", this.BackColor);
            yieldSign.Click += (object o, EventArgs EA) => { ResetButtons(yieldSign, yieldSign.Image_path); General_Form.Main.BuildScreen.builder.signController.signType = "yieldSign"; };
            this.Controls.Add(yieldSign);
            essmButtons.Add(yieldSign);

            prioritySign = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/Priority_Road_Sign_Button.png", this.BackColor);
            prioritySign.Click += (object o, EventArgs EA) => { ResetButtons(prioritySign, prioritySign.Image_path); General_Form.Main.BuildScreen.builder.signController.signType = "prioritySign"; };
            this.Controls.Add(prioritySign);
            essmButtons.Add(prioritySign);

            stopSign = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase + _ButtonYdiff), menu["buttonCurve"], "../../src/User Interface Recources/Stop_Sign_Button.png", this.BackColor);
            stopSign.Click += (object o, EventArgs EA) => { ResetButtons(stopSign, stopSign.Image_path); General_Form.Main.BuildScreen.builder.signController.signType = "stopSign"; };
            this.Controls.Add(stopSign);
            essmButtons.Add(stopSign);
        }
        private void ResetButtons(CurvedButtons _selected, string _filepath)
        {
            foreach (CurvedButtons x in essmButtons)
            {
                x.Selected = false;
                x.Image = Image.FromFile(x.Image_path.Remove(x.Image_path.Length - 10) + "Button.png");
            }
            _selected.Selected = true;
            _selected.Image = Image.FromFile(_filepath.Remove(_filepath.Length - 10) + "Select.png");
        }
    }
}