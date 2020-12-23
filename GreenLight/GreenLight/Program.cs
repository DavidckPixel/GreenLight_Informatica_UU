using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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
<<<<<<< HEAD
            Application.EnableVisualStyles(); 
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new Startup());
=======
            //Switch between parts of our program by selecting the desired form to run.

            Application.EnableVisualStyles();   //new Vehicle("Auto", 1353, 4.77, 100, 4223, 0, 0, 0.35, 2.65)
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new General_form()); //For testing userinterface
            //Application.Run(new RoadTestForm()); //For testing Road
            //Application.Run(new Startup()); //For testing Cars
            //Application.Run(new GridController()); //For testing Grid
>>>>>>> main
        }
    }
}