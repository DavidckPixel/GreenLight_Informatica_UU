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
        public static MainScreenController Main;

        public General_Form()
        {
            Main = new MainScreenController(this);
            Main.Initialize();
        }
    }
}
