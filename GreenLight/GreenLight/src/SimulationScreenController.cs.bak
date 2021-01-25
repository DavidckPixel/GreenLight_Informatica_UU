using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

//This is very similar to the build controller but instead is called when switching to the simulation screen.

namespace GreenLight
{
    public class SimulationScreenController : ScreenController
    {
        public string ActiveSubMenu;
        public SimulationController Simulator;

        public SimulationScreenController(Form _tempform)
        {
            this.Screen = new PictureBox();
            this.Screen.Width = _tempform.Width - 250;
            this.Screen.Height = _tempform.Height;
            this.Screen.Location = new Point(0, 0);
            this.Screen.BackColor = Color.FromArgb(196, 196, 198);
            this.Screen.Paint += DrawPictureBox;

            _tempform.Controls.Add(this.Screen);

            this.Simulator = new SimulationController(this.Screen);

            Log.Write("Created the Simulation Screen Controller");
        }

        public override void Initialize()
        {
            Log.Write("Initializing the SimulationScreenController");
        }

        public override void Activate()
        {
            General_Form.Main.UserInterface.Menu_to_simulation();
            SwitchSubMenus("Weather");
            Screen.Invalidate();

            Log.Write("Set Active Submenu to Weather");
        }

        public override void DeActivate()
        {
            if (this.Screen != null)
            {
                this.Screen.Hide();
                Log.Write("DeActivating SimulationScreenController");
            }
        }

        public void SwitchSubMenus(string _menu)
        {
            string _old = this.ActiveSubMenu;
            this.ActiveSubMenu = _menu;
            Log.Write("Switch Simulation Submenu from " + _old + " to " + _menu);
            switch (_menu)
            {
                case "Weather":
                    WeatherMenu();
                    break;
                case "Vehicle":
                    VehicleMenu();
                    break;
                case "Driver":
                    DriverMenu();
                    break;
                default:
                    this.ActiveSubMenu = _old;
                    Log.Write("Switch failed, returning back to " + _old);
                    break;
            }
        }

        private void WeatherMenu()
        {
            General_Form.Main.UserInterface.Menu_to_simulation_weather();
            Log.Write("Switch completed");
        }

        private void VehicleMenu()
        {
            General_Form.Main.UserInterface.Menu_to_simulation_vehicle();
            Log.Write("Switch completed");
        }

        private void DriverMenu()
        {
            General_Form.Main.UserInterface.Menu_to_simulation_driver();
            Log.Write("Switch completed");
        }

        public void DrawPictureBox(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            
            //Console.WriteLine("Drawing the Picturebox of simulator!");

            foreach (AbstractRoad _road in General_Form.Main.BuildScreen.builder.roadBuilder.roads)
            {
                _road.Draw(g);
            }
            Log.Write("Completed drawing the roads on the simulation screen");
        }
    }

}
