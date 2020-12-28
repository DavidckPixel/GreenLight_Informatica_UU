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
    public partial class Elements_sub_buildings_menu : UserControl
    {
        public Elements_sub_buildings_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - User_Controls.Config.buildElementsMenu["elementsXbase"] - User_Controls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Menu_width, User_Controls.Config.buildElementsMenu["elementsXbase"]);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - User_Controls.Config.buildElementsMenu["elementsXbase"] - User_Controls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Sub_menu_width, User_Controls.Config.buildElementsMenu["elementsXbase"]);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = User_Controls.Config.buildElementsMenu;
            int _ButtonSize = menu["buttonSize"];
            int _ButtonXbase = menu["buttonXbase"];
            int _ButtonXdiff = menu["buttonXdiff"];
            int _ButtonYbase = menu["buttonYbase"];
            int _ButtonYdiff = menu["buttonYdiff"];
            int _ButtonCurve = menu["buttonCurve"];


            CurvedButtons temp1 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp1.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp1);

            CurvedButtons temp2 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp2.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp2);

            CurvedButtons temp3 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + 2 * _ButtonXdiff, _ButtonYbase), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp3.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp3);

            CurvedButtons temp4 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp4.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp4);

            CurvedButtons temp5 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase + _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp5.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp5);

            CurvedButtons temp6 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + 2 * _ButtonXdiff, _ButtonYbase + _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp6.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp6);

            CurvedButtons temp7 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + 2 * _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp7.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp7);

            CurvedButtons temp8 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase + 2 * _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp8.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp8);

            CurvedButtons temp9 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + 2 * _ButtonXdiff, _ButtonYbase + 2 * _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp9.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp9);

            CurvedButtons temp10 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase, _ButtonYbase + 3 * _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp10.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp10);

            CurvedButtons temp11 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff, _ButtonYbase + 3 * _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp11.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp11);

            CurvedButtons temp12 = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + 2 * _ButtonXdiff, _ButtonYbase + 3 * _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Yield_Sign_Button.png", this.BackColor);
            temp12.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_lights(); };
            this.Controls.Add(temp12);
        }
    }
}
