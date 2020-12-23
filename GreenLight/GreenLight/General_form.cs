using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace GreenLight
{
    public partial class General_Form : Form
    {
        //This is the form that will be opened in the end product and will make sure all parts are implemented in the right way
        //For now it only opens the User Interface, without being able to draw roads or simulate
        
        public static MainScreenController Main;
        

        //[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        public General_Form()
        {
            Main = new MainScreenController(this);
            Main.Initialize();
        }
    }
}
