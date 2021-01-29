using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace GreenLight
{
    // The BuildScreenController deals with everything that relates to the RoadBuilder screen, where the user can build their own road junctions.
    // This is also the place where the picturebox containing all roads, signs and the grid is created.
    // It also holds a main draw function and a function that enables switching between submenus.

    public class BuildScreenController : ScreenController
    {
        public string ActiveSubMenu;
        public BuilderController builder;
        int i = 0;
        int j = 0;
        public bool Toggle;


        public BuildScreenController(Form _tempform)
        {
            this.form = _tempform;
            this.Screen = new PictureBox();
            this.Screen.Width = _tempform.Width - 250;
            this.Screen.Height = _tempform.Height;
            this.Screen.BackColor = Color.FromArgb(196, 196, 198);
            this.Screen.Location = new Point(0, 0);

            this.Screen.Image = new System.Drawing.Bitmap(Screen.Width, Screen.Height);

            _tempform.Resize += (object o, EventArgs ea) => { this.resize(_tempform); };


            this.Screen.Paint += DrawPictureBox;
            builder = new BuilderController(this.Screen, _tempform);

            _tempform.Controls.Add(this.Screen);
        }

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
            General_Form.Main.BuildScreen.builder.signController.signType = "D";
            General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "D";

            switch (_menu)
            {
                case "Roads":
                    RoadsMenu();
                    break;
                case "Signs":
                    SignsMenu();
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
            _DrawPictureBox(g);
        }

        public void _DrawPictureBox(Graphics g)
        {
            foreach (AbstractRoad _road in builder.roadBuilder.roads)
            {
                _road.Draw(g);
            }
            foreach (List<CrossArrow> _list in builder.roadBuilder.AllCrossArrows)
            {
                foreach(CrossArrow ca in _list)
                {
                    g.DrawImage(ca.bitmap, ca.Location);
                }
            }
        }  
    
        public bool ToggleHitbox()
        {
            if (j % 2 == 0)
            {
                j++;
                return true;
            }
            j++;
            return false;
        }
    }
}
