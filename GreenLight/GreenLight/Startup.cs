using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    class Startup : Form
    {
        public Startup()
        {
            new Vehicle("Auto", 1353, 4.77, 100, 4223, 0, 0, 0.35, 2.65);
        }
    }
}
