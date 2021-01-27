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
    /* This is the Simulation sub menu class. This class has a method AdjustSize to fit the size of the users window.
   This user control is shown when the user is in the simulation screen.
   Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class SimulationSubMenu : UserControl
    {
        public List<CurvedButtons> ssmButtons = new List<CurvedButtons>();
        public CurvedButtons Weather, Vehicle, Driver, Start, Pause, Stop, Reset, Divider1, Divider2, Divider3;
        public SimulationSubMenu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(UserControls.Config.standardSubMenu["subMenuWidth"], Form.Height);
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

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = UserControls.Config.simSubMenu;

            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(UserControls.Config.standardSubMenu["logoX"], UserControls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            MovePanel Drag_pad = new MovePanel(Form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Settings_header = new CurvedButtons(new Size(menu["settingsHeaderSizeX"], menu["settingsHeaderSizeY"]), 
               new Point(menu["settingsHeaderX"], menu["settingsHeaderY"]), "../../src/User Interface Recources/Settings_Header.png"); 
            this.Controls.Add(Settings_header);

            int _buttonSize = menu["ButtonSize"];

            /*     Buttons & Dividers    */

            Weather = new CurvedButtons(new Size(_buttonSize, _buttonSize), new Point(menu["buttonStart"] , menu["ButtonY"]), 30, "../../src/User Interface Recources/Weather_Setting_Button.png", this.BackColor);
            this.Controls.Add(Weather);
            ssmButtons.Add(Weather);
            Weather.Click += (object obj, EventArgs args) => { ResetButtons(Weather, Weather.Image_path); General_Form.Main.SimulationScreen.SwitchSubMenus("Weather"); ; };

            Vehicle = new CurvedButtons(new Size(_buttonSize, _buttonSize), new Point(menu["buttonStart"] + menu["ButtonX"], menu["ButtonY"]), 30, "../../src/User Interface Recources/Vehicle_Setting_Button.png", this.BackColor);
            this.Controls.Add(Vehicle);
            ssmButtons.Add(Vehicle);
            Vehicle.Click += (object obj, EventArgs args) => { ResetButtons(Vehicle, Vehicle.Image_path); General_Form.Main.SimulationScreen.SwitchSubMenus("Vehicle"); ; };

            Driver = new CurvedButtons(new Size(_buttonSize, _buttonSize), new Point(menu["buttonStart"] + menu["ButtonX"] * 2, menu["ButtonY"]), 30, "../../src/User Interface Recources/Driver_Setting_Button.png", this.BackColor);
            this.Controls.Add(Driver);
            ssmButtons.Add(Driver);
            Driver.Click += (object obj, EventArgs args) => { ResetButtons(Driver, Driver.Image_path); General_Form.Main.SimulationScreen.SwitchSubMenus("Driver"); ; };

            Divider1 = new CurvedButtons();
            Divider1.Location = new Point(UserControls.Config.standardSubMenu["deviderX"], UserControls.Config.standardSubMenu["deviderY"]);
            this.Controls.Add(Divider1);

            Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, menu["devider2"]);                             
            this.Controls.Add(Divider2);

            Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, this.Height - menu["devider3"]);          
            this.Controls.Add(Divider3);

            /*     Simulationspeed slider    */

            CurvedButtons SimulationSpeed_header = new CurvedButtons(new Size(menu["speedHeaderSizeX"], menu["speedHeaderSizeY"]),
                new Point(menu["speedHeaderX"], this.Height - menu["speedHeaderY"]), "../../src/User Interface Recources/Simulation_Speed_Header.png");
            this.Controls.Add(SimulationSpeed_header);

            Slider SimulationSpeed = new Slider(new Point(menu["speedX"], this.Height - menu["speedY"]), 1, 10);
            SimulationSpeed.Value = 1;
            this.Controls.Add(SimulationSpeed);

            SliderText SimulationSpeed_Text = new SliderText(Dosis_font_family, new Point(menu["speedHeaderX"] + menu["speedHeaderSizeX"], this.Height - menu["speedHeaderY"]), SimulationSpeed.Value.ToString() + "x");
            SimulationSpeed_Text.Font = new Font(Dosis_font_family, 13, FontStyle.Bold);
            this.Controls.Add(SimulationSpeed_Text);
            SimulationSpeed_Text.BringToFront();

            SimulationSpeed.ValueChanged += (object o, EventArgs EA) =>
            { SimulationSpeed_Text.Text = SimulationSpeed.Value.ToString() + "x"; General_Form.Main.UserInterface.SimDataM.ValueChanged(SimulationSpeed.Value); };

            /*     Simulation buttons   */

            Start = new CurvedButtons(new Size(_buttonSize, _buttonSize), new Point(menu["buttonStart"], Form.Height - menu["controlsY"]), 35, "../../src/User Interface Recources/Play_Simulation_Button.png", this.BackColor);
              Start.Click += (object o, EventArgs ea) => 
            {
                General_Form.Main.SimulationScreen.Simulator.initSimulation();
                General_Form.Main.SimulationScreen.Simulator.StartSimulation(); 
            };
            this.Controls.Add(Start);
            Start.BringToFront();

            Pause = new CurvedButtons(new Size(60, 60),new Point(20, Form.Height - 80), 35, "../../src/User Interface Recources/Pause_Button.png", this.BackColor);
            Pause.Hide();
            Pause.Click += (object o, EventArgs EA) => { General_Form.Main.SimulationScreen.Simulator.PauseSimulation(); };
            Pause.Click += (object o, EventArgs EA) => { Pause.Hide(); Start.Show(); General_Form.Main.UserInterface.SimDataM.Stop_timer(); };
            Start.Click += (object o, EventArgs EA) => { Start.Hide(); Pause.Show(); General_Form.Main.UserInterface.SimDataM.StartTimer(); };
            this.Controls.Add(Pause);
            Pause.BringToFront();

            Reset = new CurvedButtons(new Size(_buttonSize, _buttonSize), new Point(menu["buttonStart"] + menu["ButtonX"], Form.Height - menu["controlsY"]), 35, "../../src/User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            Reset.Click += (object o, EventArgs ea) =>
            {
                General_Form.Main.SimulationScreen.Simulator.resetSimulation();
            };
            this.Controls.Add(Reset);
            Reset.BringToFront();
            {
                if (Pause.Visible)
                {
                    General_Form.Main.UserInterface.SimDataM.ResetTimer();
                    Pause.Hide();
                    Start.Show();
                    SimulationSpeed.Value = 1;
                }
                else if (Start.Visible && General_Form.Main.UserInterface.SimDataM.stopWatch.Elapsed.ToString() != "00:00:00")
                {
                    General_Form.Main.UserInterface.SimDataM.ResetTimer();
                    SimulationSpeed.Value = 1;
                }
            };

            Stop = new CurvedButtons(new Size(_buttonSize, _buttonSize), new Point(menu["buttonStart"] + menu["ButtonX"] * 2, Form.Height - menu["controlsY"]), 35, "../../src/User Interface Recources/Stop_Simulation_Button.png", this.BackColor);
            Stop.Click += (object obj, EventArgs args) => 
            {
                General_Form.Main.SimulationScreen.Simulator.resetSimulation();
                General_Form.Main.SwitchControllers(General_Form.Main.BuildScreen);
                General_Form.Main.UserInterface.SimDataM.ResetTimer();
                Pause.Hide();
                Start.Show();
            };
            this.Controls.Add(Stop);
            Stop.BringToFront();
        }
        private void ResetButtons(CurvedButtons _selected, string _filepath)
        {
            foreach (CurvedButtons x in ssmButtons)
            {
                x.Selected = false;
                x.Image = Image.FromFile(x.Image_path.Remove(x.Image_path.Length - 10) + "Button.png");
            }
            _selected.Selected = true;
            _selected.Image = Image.FromFile(_filepath.Remove(_filepath.Length - 10) + "Select.png");
        }
    }
}
