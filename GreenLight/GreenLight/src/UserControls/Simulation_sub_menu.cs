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
        public List<CurvedButtons> SSM = new List<CurvedButtons>();
        public CurvedButtons Weather;
        public Simulation_sub_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(User_Controls.Config.standardSubMenu["subMenuWidth"], Form.Height);
            this.Location = new Point(Form.Width-Menu_width, Form.Height);
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
            Dictionary<string, int> menu = User_Controls.Config.simSubMenu;

            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(User_Controls.Config.standardSubMenu["logoX"], User_Controls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            Move_panel Drag_pad = new Move_panel(Form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(User_Controls.Config.standardSubMenu["deviderX"], User_Controls.Config.standardSubMenu["deviderY"]);
            this.Controls.Add(Divider1);

            CurvedButtons Settings_header = new CurvedButtons(new Size(menu["settingsHeaderSizeX"], menu["settingsHeaderSizeY"]),  //settingsHeaderSizeX //settingsHeaderSizeY
               new Point(menu["settingsHeaderX"], menu["settingsHeaderY"]), "../../User Interface Recources/Settings_Header.png"); //settingsHeaderX //settingsHeaderY
            this.Controls.Add(Settings_header);

            //ButtonSize //ButtonY
            int _buttonSize = menu["ButtonSize"];

            Weather = new CurvedButtons(new Size(_buttonSize, _buttonSize),
                new Point(menu["buttonStart"] , menu["ButtonY"]), 30,                                         //weatherX 
                "../../User Interface Recources/Weather_Setting_Button.png", this.BackColor);
            this.Controls.Add(Weather);
            SSM.Add(Weather);
            Weather.Click += (object obj, EventArgs args) => { ResetButtons(Weather, Weather.Image_path); General_Form.Main.SimulationScreen.SwitchSubMenus("Weather"); ; };

            CurvedButtons Vehicle = new CurvedButtons(new Size(_buttonSize, _buttonSize),
                new Point(menu["buttonStart"] + menu["ButtonX"], menu["ButtonY"]), 30,                                         //vehicleX
                "../../User Interface Recources/Vehicle_Setting_Button.png", this.BackColor);
            this.Controls.Add(Vehicle);
            SSM.Add(Vehicle);
            Vehicle.Click += (object obj, EventArgs args) => { ResetButtons(Vehicle, Vehicle.Image_path); General_Form.Main.SimulationScreen.SwitchSubMenus("Vehicle"); ; };

            CurvedButtons Driver = new CurvedButtons(new Size(_buttonSize, _buttonSize),
                new Point(menu["buttonStart"] + menu["ButtonX"] * 2, menu["ButtonY"]), 30,                                        //driverX
                "../../User Interface Recources/Driver_Setting_Button.png", this.BackColor);
            this.Controls.Add(Driver);
            SSM.Add(Driver);
            Driver.Click += (object obj, EventArgs args) => { ResetButtons(Driver, Driver.Image_path); General_Form.Main.SimulationScreen.SwitchSubMenus("Driver"); ; };

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, menu["devider2"]);                              //devider2
            this.Controls.Add(Divider2);

            CurvedButtons Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, this.Height - menu["devider3"]);                //devider3
            this.Controls.Add(Divider3);

            CurvedButtons SimulationSpeed_header = new CurvedButtons(new Size(menu["speedHeaderSizeX"], menu["speedHeaderSizeY"]), //speedHeaderSizeX //speedHeaderSizeY
                new Point(menu["speedHeaderX"], this.Height - menu["speedHeaderY"]), "../../User Interface Recources/Simulation_Speed_Header.png"); //speedHeaderX //speedHeaderY
            this.Controls.Add(SimulationSpeed_header);

            Slider SimulationSpeed = new Slider(new Point(menu["speedX"], this.Height - menu["speedY"]), 1, 10); //speedX //speedY
            SimulationSpeed.Value = 1;
            this.Controls.Add(SimulationSpeed);

            SliderText SimulationSpeed_Text = new SliderText(Dosis_font_family, new Point(menu["speedHeaderX"] + menu["speedHeaderSizeX"], this.Height - menu["speedHeaderY"]), SimulationSpeed.Value.ToString() + "x");
            SimulationSpeed_Text.Font = new Font(Dosis_font_family, 13, FontStyle.Bold);
            this.Controls.Add(SimulationSpeed_Text);
            SimulationSpeed_Text.BringToFront();

            SimulationSpeed.ValueChanged += (object o, EventArgs EA) =>
            { SimulationSpeed_Text.Text = SimulationSpeed.Value.ToString() + "x"; General_Form.Main.UserInterface.SimDataM.Value_changed(SimulationSpeed.Value); };

            CurvedButtons Start = new CurvedButtons(new Size(_buttonSize, _buttonSize),           //controlsX, controlsY
                new Point(menu["buttonStart"], Form.Height - menu["controlsY"]), 35,
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

            CurvedButtons Reset = new CurvedButtons(new Size(_buttonSize, _buttonSize),
                new Point(menu["buttonStart"] + menu["ButtonX"], Form.Height - menu["controlsY"]), 35,
                "../../User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            
            this.Controls.Add(Reset);
            Reset.BringToFront();

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

            CurvedButtons Stop = new CurvedButtons(new Size(_buttonSize, _buttonSize),
               new Point(menu["buttonStart"] + menu["ButtonX"] * 2, Form.Height - menu["controlsY"]), 35,
               "../../User Interface Recources/Stop_Simulation_Button.png", this.BackColor);
            this.Controls.Add(Stop);
            Stop.BringToFront();
            Stop.Click += (object obj, EventArgs args) => {
                General_Form.Main.SwitchControllers(General_Form.Main.BuildScreen);
                General_Form.Main.UserInterface.SimDataM.Reset_timer();
                Pause.Hide();
                Start.Show();
            };
        }
        private void ResetButtons(CurvedButtons Selected, string Filepath)
        {
            foreach (CurvedButtons x in SSM)
            {
                x.Selected = false;
                x.Image = Image.FromFile(x.Image_path.Remove(x.Image_path.Length - 10) + "Button.png");
            }
            Selected.Selected = true;
            Selected.Image = Image.FromFile(Filepath.Remove(Filepath.Length - 10) + "Select.png");
        }
    }
}
