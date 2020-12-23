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
        public Simulation_sub_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250,Form.Height);
            this.Location = new Point(Form.Width-Menu_width, Form.Height);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }

        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height);
            this.Location = new Point(Form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);

        }

        //Cleaner but General_form should be just form
        /*public bool Simulation_state_playing = false;
        public Simulation_sub_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250,General_form.Height);
            this.Location = new Point(General_form.Width-Sub_menu_width, General_form.Height);
            this.AutoScroll = true;
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(Sub_menu_width, General_form.Height);
                this.Location = new Point(General_form.Width - Sub_menu_width, 0);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width, Dosis_font_family);
            };
        }*/

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            Move_panel Drag_pad = new Move_panel(Form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            CurvedButtons Settings_header = new CurvedButtons(new Size(150, 35),
               new Point(50, 110), "../../User Interface Recources/Settings_Header.png");
            this.Controls.Add(Settings_header);

            CurvedButtons Weather = new CurvedButtons(new Size(60, 60),
                new Point(20, 150), 30,
                "../../User Interface Recources/Weather_Setting_Button.png", this.BackColor);
            this.Controls.Add(Weather);
            Weather.Click += (object obj, EventArgs args) => { General_Form.Main.SimulationScreen.SwitchSubMenus("Weather"); ; };

            CurvedButtons Vehicle = new CurvedButtons(new Size(60, 60),
                new Point(95, 150), 30,
                "../../User Interface Recources/Vehicle_Setting_Button.png", this.BackColor);
            this.Controls.Add(Vehicle);
            Vehicle.Click += (object obj, EventArgs args) => { General_Form.Main.SimulationScreen.SwitchSubMenus("Vehicle"); ; };

            CurvedButtons Driver = new CurvedButtons(new Size(60, 60),
                new Point(170, 150), 30,
                "../../User Interface Recources/Driver_Setting_Button.png", this.BackColor);
            this.Controls.Add(Driver);
            Driver.Click += (object obj, EventArgs args) => { General_Form.Main.SimulationScreen.SwitchSubMenus("Driver"); ; };

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, 220);
            this.Controls.Add(Divider2);

            CurvedButtons Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, this.Height - 135);
            this.Controls.Add(Divider3);

            CurvedButtons SimulationSpeed_header = new CurvedButtons(new Size(150, 30),
                new Point(15, this.Height - 130), "../../User Interface Recources/Simulation_Speed_Header.png");
            this.Controls.Add(SimulationSpeed_header);

            Slider SimulationSpeed = new Slider(new Point(25, this.Height - 105), 1, 10);
            SimulationSpeed.Value = 1;
            this.Controls.Add(SimulationSpeed);

            SliderText SimulationSpeed_Text = new SliderText(Dosis_font_family, new Point(175, this.Height - 127), SimulationSpeed.Value.ToString() + "x");
            SimulationSpeed_Text.Size = new Size(50, 50);
            this.Controls.Add(SimulationSpeed_Text);
           
            SimulationSpeed.ValueChanged += (object o, EventArgs EA) => 
                { SimulationSpeed_Text.Text = SimulationSpeed.Value.ToString() + "x"; General_Form.Main.UserInterface.SimDataM.Value_changed(SimulationSpeed.Value); };

            CurvedButtons Start = new CurvedButtons(new Size(60, 60),
                new Point(20, Form.Height - 80), 35,
                "../../User Interface Recources/Play_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Start);
            Start.BringToFront();

            CurvedButtons Pause = new CurvedButtons(new Size(60, 60),
               new Point(20, Form.Height - 80), 35,
               "../../User Interface Recources/Pause_Button.png", this.BackColor);
            Pause.Hide();
            this.Controls.Add(Pause);
            Pause.BringToFront();
            Pause.Click += (object o, EventArgs EA) => { Pause.Hide(); Start.Show(); General_Form.Main.UserInterface.SimDataM.Stop_timer(); };
            Start.Click += (object o, EventArgs EA) => { Start.Hide(); Pause.Show(); General_Form.Main.UserInterface.SimDataM.Start_timer(); };

            CurvedButtons Reset = new CurvedButtons(new Size(60, 60),
                new Point(95, Form.Height - 80), 35,
                "../../User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Reset);
            Reset.BringToFront();
            Reset.Click += (object o, EventArgs EA) => 
            {
                if (Pause.Visible)
                {
                    General_Form.Main.UserInterface.SimDataM.Reset_timer();
                    Pause.Hide(); 
                    Start.Show();
                    SimulationSpeed.Value = 1;
                }
                else if (Start.Visible && General_Form.Main.UserInterface.SimDataM.Stopwatch.Elapsed.ToString() != "00:00:00")
                {
                    General_Form.Main.UserInterface.SimDataM.Reset_timer();
                    SimulationSpeed.Value = 1;
                }
            };

            CurvedButtons Stop = new CurvedButtons(new Size(60, 60), new Point(170, Form.Height - 80), 35,
            "../../User Interface Recources/Stop_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Stop);
            Stop.BringToFront();
            Stop.Click += (object obj, EventArgs args) => 
            { 
                General_Form.Main.UserInterface.Menu_to_build();
                General_Form.Main.UserInterface.SimDataM.Reset_timer(); 
                Pause.Hide();
                Start.Show();
            };

        }
    }
}
