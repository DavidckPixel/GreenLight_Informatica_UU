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
    public partial class Simulation_sub_menu : UserControl
    {
        public Simulation_sub_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250,General_form.Height);
            this.Location = new Point(General_form.Width-Menu_width, General_form.Height);
            this.AutoScroll = true;
            Initialize(General_form, Menu_width, Dosis_font_family);
        }

        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height);
            this.Location = new Point(General_form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            CurvedButtons Logo = new CurvedButtons(General_form, 1);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            Move_panel Drag_pad = new Move_panel(General_form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            PictureBox Settings_header = new PictureBox();
            Settings_header.Size = new Size(150, 25);
            Settings_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Settings_header.Location = new Point(50, 120);
            Settings_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Settings_header);

            CurvedButtons Weather = new CurvedButtons(new Size(60, 60),
                new Point(20, 150), 30,
                "../../User Interface Recources/Weather_Setting_Button.png", this.BackColor);
            this.Controls.Add(Weather);
            Weather.Click += (object obj, EventArgs args) => { General_form.Menu_to_simulation_weather(); };

            CurvedButtons Vehicle = new CurvedButtons(new Size(60, 60),
                new Point(95, 150), 30,
                "../../User Interface Recources/Vehicle_Setting_Button.png", this.BackColor);
            this.Controls.Add(Vehicle);
            Vehicle.Click += (object obj, EventArgs args) => { General_form.Menu_to_simulation_vehicle(); };

            CurvedButtons Driver = new CurvedButtons(new Size(60, 60),
                new Point(170, 150), 30,
                "../../User Interface Recources/Driver_Setting_Button.png", this.BackColor);
            this.Controls.Add(Driver);
            Driver.Click += (object obj, EventArgs args) => { General_form.Menu_to_simulation_driver(); };

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, 220);
            this.Controls.Add(Divider2);

            CurvedButtons Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, this.Height - 135);
            this.Controls.Add(Divider3);

            CurvedButtons Start = new CurvedButtons(new Size(60, 60),
                new Point(20, General_form.Height - 80), 35,
                "../../User Interface Recources/Play_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Start);

            CurvedButtons Reset = new CurvedButtons(new Size(60, 60),
                new Point(95, General_form.Height - 80), 35,
                "../../User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Reset);

            CurvedButtons Stop = new CurvedButtons(new Size(60, 60),
                new Point(170, General_form.Height - 80), 35,
                "../../User Interface Recources/Stop_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Stop);

            PictureBox SimulationSpeed_header = new PictureBox();
            SimulationSpeed_header.Size = new Size(150, 25);
            SimulationSpeed_header.SizeMode = PictureBoxSizeMode.StretchImage;
            SimulationSpeed_header.Location = new Point(50, this.Height - 125);
            SimulationSpeed_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(SimulationSpeed_header);

            Slider SimulationSpeed = new Slider(new Point(25, this.Height - 105), 0, 100);
            this.Controls.Add(SimulationSpeed);

        }
    }
}
