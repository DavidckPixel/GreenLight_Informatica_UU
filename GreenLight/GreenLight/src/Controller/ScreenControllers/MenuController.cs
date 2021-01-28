using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;



namespace GreenLight
{
    // This is the MenuController that controls the Main/Staring menu

    public class MenuController : ScreenController
    {
        public MenuController(PictureBox _bitmap) : base(_bitmap)
        {

        }

        public override void Initialize()
        {

        }

        public override void Activate()
        {
            Log.Write("Activating MenuScreenController");

            General_Form.Main.UserInterface.Menu_to_start();
        }

        public override void DeActivate()
        {
            Log.Write("DeActivating MenuScreenController");

            if (this.Screen != null)
            {
                this.Screen.Hide();
            }
        }

        public void SwitchToBuild()
        {
            General_Form.Main.SwitchControllers(General_Form.Main.BuildScreen);
        }

        public void SwitchToSimulation()
        {
            
            General_Form.Main.SwitchControllers(General_Form.Main.SimulationScreen);
        }
    }
}
