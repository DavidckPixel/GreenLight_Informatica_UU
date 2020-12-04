using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();   //new Vehicle("Auto", 1353, 4.77, 100, 4223, 0, 0, 0.35, 2.65)
            Application.SetCompatibleTextRenderingDefault(false);
            //Vehicle v = new Vehicle("Auto", 1353, 4.77, 100, 4223, 0, 0, 0.35, 2.65);
            Application.Run(new Startup());
        }
    }
}
