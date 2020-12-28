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
    public partial class Simulation_sub_vehicle_menu : UserControl
    {
        public Simulation_sub_vehicle_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - User_Controls.Config.simElementsMenu["menuY"] - User_Controls.Config.simElementsMenu["menuSizeY"]); //menuSizeY
            this.Location = new Point(Form.Width - Menu_width, User_Controls.Config.simElementsMenu["menuY"]);  //menuY
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - User_Controls.Config.simElementsMenu["menuY"] - User_Controls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(Form.Width - Sub_menu_width, User_Controls.Config.simElementsMenu["menuY"]);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = User_Controls.Config.simElementsMenu;
            int _sliderX = menu["sliderX"];
            int _start = menu["sliderStart"];
            int _diff = menu["sliderDiffY"];

            Slider temp1 = new Slider(new Point(_sliderX, _start + _diff * 4), 0, 100, 10); //sliderDiffY //sliderStart:100 / sliderX:25 //headerSizeX //headerSizeY //headerX //headerY
            this.Controls.Add(temp1);

            Slider temp2 = new Slider(new Point(_sliderX, _start + _diff * 3), 0, 100, 10);
            this.Controls.Add(temp2);

            Slider temp3 = new Slider(new Point(_sliderX, _start + _diff * 2), 0, 100, 10);
            this.Controls.Add(temp3);

            Slider temp4 = new Slider(new Point(_sliderX, _start + _diff), 0, 100, 10);
            this.Controls.Add(temp4);

            Slider temp5 = new Slider(new Point(_sliderX, _start), 0, 100, 10);
            this.Controls.Add(temp5);

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);

        }
    }
}
