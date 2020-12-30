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
        public Simulation_sub_vehicle_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, General_form.Height - 230 - 135);
            this.Location = new Point(General_form.Width - Menu_width, 230);
            this.AutoScroll = true;
            Initialize(General_form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height - 230 - 135);
            this.Location = new Point(General_form.Width - Sub_menu_width, 230);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {

            Slider temp1 = new Slider(new Point(25, 260), 0, 100, 10);
            this.Controls.Add(temp1);

            Slider temp2 = new Slider(new Point(25, 220), 0, 100, 10);
            this.Controls.Add(temp2);

            Slider temp3 = new Slider(new Point(25, 180), 0, 100, 10);
            this.Controls.Add(temp3);

            Slider temp4 = new Slider(new Point(25, 140), 0, 100, 10);
            this.Controls.Add(temp4);

            Slider temp5 = new Slider(new Point(25, 100), 0, 100, 10);
            this.Controls.Add(temp5);

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(150, 30),
               new Point(50, 5), "../../User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);

        }
    }
}
