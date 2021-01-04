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
            this.Screen.Width = _tempform.Width - 250;
            this.Screen.Height = _tempform.Height;
            this.Screen.BackColor = Color.FromArgb(196, 196, 198);
            this.Screen.Location = new Point(0, 0);
            _tempform.Resize += (object o, EventArgs ea) => { this.resize(_tempform); };
           
            
            this.Screen.Paint += DrawPictureBox;
            builder = new BuilderController(this.Screen, _tempform);

            _tempform.Controls.Add(this.Screen);

            Console.WriteLine("BuildController made!");
        }

        int i = 0;
        private void resize(Form _tempform) 
        {
            i++;
            this.Screen.Width = _tempform.Width - 250;
            this.Screen.Height = _tempform.Height;
            builder.gridController.canvas_resize(new Size(_tempform.Width - 250, _tempform.Height));
            builder.gridController.CreateGridPoints();
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
