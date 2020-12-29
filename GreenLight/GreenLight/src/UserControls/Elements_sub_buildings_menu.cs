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

            //-------------------------------------------------------------

            CurvedButtons Hand = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 0, _ButtonYbase), _ButtonCurve, "../../User Interface Recources/Hand_Button.png", this.BackColor);
            Hand.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Hand);

            CurvedButtons Fence = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 1, _ButtonYbase), _ButtonCurve, "../../User Interface Recources/Fence_Button.png", this.BackColor);
            Fence.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Fence);

            CurvedButtons Building = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point(_ButtonXbase + _ButtonXdiff * 2, _ButtonYbase), _ButtonCurve, "../../User Interface Recources/Building_Button.png", this.BackColor);
            Building.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Building);
        }
    }
}
