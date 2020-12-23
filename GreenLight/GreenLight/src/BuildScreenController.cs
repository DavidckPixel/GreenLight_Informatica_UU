using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public class BuildScreenController : ScreenController
    {
        public string ActiveSubMenu;
        public BuilderController builder;

        public BuildScreenController(Form _tempform)
        {
            this.form = _tempform;
            this.Screen = new PictureBox();
            this.Screen.Width = 700;
            this.Screen.Height = 300;
            this.Screen.Location = new Point(100, 100);
            
            this.Screen.Paint += DrawPictureBox;
            builder = new BuilderController(this.Screen);

            _tempform.Controls.Add(this.Screen);

            Console.WriteLine("BuildController made!");
        }

        public override void Initialize()
        {
            Log.Write("Initializing the BuildScreenController..");

            SwitchSubMenus("Roads");

            Log.Write("Completed the Initialization of BuildScreenController");
        }

        public override void Activate()
        {
            Log.Write("Activating BuildScreenController");

            General_Form.Main.UserInterface.Menu_to_build();
            SwitchSubMenus("Roads");
        }

        public override void DeActivate()
        {
            Log.Write("DeActivating BuildScreenController");

            if (this.Screen != null)
            {
                this.Screen.Hide();
            }
        }

        public void SwitchSubMenus(string _menu)
        {
            Log.Write("Switching Buildscreen SubMenu from {0} to {1}", this.ActiveSubMenu, _menu);

            string _old = this.ActiveSubMenu;
            this.ActiveSubMenu = _menu;

            switch (_menu)
            {
                case "Roads":
                    RoadsMenu();
                    break;
                case "Signs":
                    SignsMenu();
                    break;
                case "Lights":
                    LightsMenu();
                    break;
                case "Buildings":
                    BuildingsMenu();
                    break;
                default:
                    Log.Write("Switch Failed, Returning back to " + _old);
                    this.ActiveSubMenu = _old;
                    break;
            }
        }

        private void RoadsMenu()
        {
            General_Form.Main.UserInterface.Menu_to_roads();
        }
        private void SignsMenu()
        {
            General_Form.Main.UserInterface.Menu_to_signs();
        }
        private void LightsMenu()
        {
            General_Form.Main.UserInterface.Menu_to_lights();
        }
        private void BuildingsMenu()
        {
            General_Form.Main.UserInterface.Menu_to_buildings();
        }

        public void SwitchSimulationScreen()
        {
            General_Form.Main.SwitchControllers(General_Form.Main.SimulationScreen);
        }

        
        public void DrawPictureBox(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            builder.gridController.DrawGridPoints(g);

            foreach(AbstractRoad _road in builder.roadBuilder.roads)
            {
                _road.Draw(g);
            }
        }  
    }
}
