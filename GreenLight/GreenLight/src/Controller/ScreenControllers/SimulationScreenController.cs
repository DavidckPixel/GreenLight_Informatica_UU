using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using GreenLight.src.Driver.GPS;


namespace GreenLight
{
    // This is the SimulationScreenController that deals with everything that happens in the Simulation screen, where the user can simulate traffic.
    // It also holds a function that enables switching between submenus.

    public class SimulationScreenController : ScreenController
    {
        public string ActiveSubMenu;
        public SimulationController Simulator;
        public Form mainForm;
        public GPSData gpsData;

        public Bitmap background;

        public SimulationScreenController(Form _tempform)
        {
            this.Screen = new PictureBox();
            this.Screen.Width = _tempform.Width - 250;
            this.Screen.Height = _tempform.Height;
            this.Screen.Location = new Point(0, 0);
            this.Screen.BackColor = Color.FromArgb(196, 196, 198);
            this.Screen.Paint += DrawPictureBox;
            this.Screen.MouseClick += ClickPictureBox;

            this.mainForm = _tempform;
            this.mainForm.SizeChanged += ChangeSize;

            this.Screen.Image = new System.Drawing.Bitmap(Screen.Width, Screen.Height);
            this.background = new Bitmap(this.Screen.Width, this.Screen.Height);

            this.Simulator = new SimulationController(this);
            Log.Write("Created the Simulation Screen Controller");

            this.mainForm.Controls.Add(this.Screen);
        }

        public override void Initialize()
        {
            Log.Write("Initializing the SimulationScreenController");
        }

        public override void Activate()
        {
            General_Form.Main.UserInterface.Menu_to_simulation();
            SwitchSubMenus("Weather");
            this.gpsData = new GPSData(General_Form.Main.BuildScreen.builder.roadBuilder.roads);
            this.Screen.Invalidate();
            this.CreateBackgroundImage();
            //this.Simulator.initSimulation();
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

            g.DrawImage(this.background, new Point(0, 0));
    
            Log.Write("Completed drawing the roads on the simulation screen");

            for(int x = 0; x < this.Simulator.vehicleController.vehicleList.Count(); x++)
            {
                this.Simulator.vehicleController.vehicleList[x].Draw(g);
                if (this.Simulator.SimulationPaused)
                {
                    if (this.Simulator.vehicleController.vehicleList[x].hitbox != null)
                    {
                        this.Simulator.vehicleController.vehicleList[x].hitbox.Draw(g);
                    }
                }
            }
        }

        public void CreateBackgroundImage()
        {
            this.background = new Bitmap(this.Screen.Width, this.Screen.Height);

            Graphics g = Graphics.FromImage(this.background);
            g.DrawImage(DriverProfileData.faces.First().backImg, 0, 0, this.Screen.Width, this.Screen.Height);

            foreach (AbstractRoad _road in General_Form.Main.BuildScreen.builder.roadBuilder.roads)
            {
                _road.Draw(g);
            }
        }

        public void ClickPictureBox(object o, MouseEventArgs mea)
        {

            if (this.Simulator.SimulationPaused)
            {
                this.Simulator.profileController.OnClick(mea.Location, this.Simulator.vehicleController.vehicleList);
            }
        }

        private void ChangeSize(object o, EventArgs ea)
        {
            this.Screen.Width = mainForm.Width - 250;
            this.Screen.Height = mainForm.Height;
            this.Screen.Invalidate();
        }
    }

}
