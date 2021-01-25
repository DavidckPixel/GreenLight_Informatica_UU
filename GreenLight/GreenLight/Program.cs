using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GreenLight.src.Data_Collection;

namespace GreenLight
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Switch between parts of our program by selecting the desired form to run.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new General_Form()); //For testing userinterface
            //Application.Run(new RoadTestForm()); //For testing Road
            //Application.Run(new Startup()); //For testing Cars
            //Application.Run(new GridController()); //For testing Grid
            //Application.Run(new HitBox_form()); //For testing Hitboxes

            //Application.Run(new BetterVehicleTest());
            //Application.Run(new DataTestForm());
        }
    }
}