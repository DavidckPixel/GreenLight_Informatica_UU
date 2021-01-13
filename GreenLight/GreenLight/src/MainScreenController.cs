using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public class MainScreenController : ScreenController
    {
        public BuildScreenController BuildScreen;
        public SimulationScreenController SimulationScreen;
        public MenuController MenuController;

        public ScreenController Active;

        public InterfaceController UserInterface;

        public MainScreenController(Form _tempform) 
        {
            this.form = _tempform;

            this.form.Paint += DrawMain;

            form.Controls.Add(this.Screen);
            this.form.Invalidate();

            Log.Write("Created the Main Controller");
        }

        public override void Initialize()
        {
            Log.Write("Initializing the MainController..");

            UserInterface = new InterfaceController(this.form);
            UserInterface.Initialize();

            BuildScreen = new BuildScreenController(this.form);
            SimulationScreen = new SimulationScreenController(this.form);
            MenuController = new MenuController(this.Screen);

            BuildScreen.Initialize();
            SimulationScreen.Initialize();
            MenuController.Initialize();

            Log.Write("Setting Active Controller to MenuController");

            this.Active = this.MenuController;
            this.Active.Activate();

            Log.Write("Completed the Initialization of MainController");
        }

        public void SwitchControllers(ScreenController _controller)
        {
            Log.Write("Switched Active Controller from " + this.Active.GetType().ToString() + "to " + _controller.GetType().ToString());

            this.Active.DeActivate();
            this.Active = _controller;
            this.Active.Activate();
            this.form.Invalidate();

            Console.WriteLine("Switched and invalidated!");
        }

        public override void Activate()
        {
            
        }

        public override void DeActivate()
        {
            throw new NotImplementedException();
        }

        public void DrawMain(Object o, PaintEventArgs pea)
        {
            Console.WriteLine(this.Active);

            if (this.Active.Screen != null)
            {
                this.Active.Screen.Show();
                this.Active.Screen.BringToFront();
                this.Active.Screen.Invalidate();
            }
        }
    }
}
