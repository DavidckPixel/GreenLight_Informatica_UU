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
    public partial class Simulation_sub_weather_menu : UserControl
    {
        public Simulation_sub_weather_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
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

            Slider temp1 = new Slider(new Point(25, 180), 0, 100);
            this.Controls.Add(temp1);

            Slider temp2 = new Slider(new Point(25, 140), 0, 100);
            this.Controls.Add(temp2);

            Slider temp3 = new Slider(new Point(25, 100), 0, 100);
            this.Controls.Add(temp3);

            Slider temp4 = new Slider(new Point(25, 60), 0, 100);
            this.Controls.Add(temp4);

            PictureBox Weather_header = new PictureBox();
            Weather_header.Size = new Size(150, 25);
            Weather_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Weather_header.Location = new Point(50, 10);
            Weather_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Weather_header);
        }
    }
}
