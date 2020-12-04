using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GreenLight
{
    class Startup : Form
    {
        //Vehicle v;
        AI driver;
        bool simulate;
        Vehicle v = VehicleTypeConfig.types[0];
        public Startup()
        {

            //v = new Vehicle("Auto", 1353, 4.77, 100, 4223, 0, 0, 0.35, 2.65);
            driver = new AI(v, 0.25, 2, 0, 0);
            simulate = true;
            this.DoubleBuffered = true;
            this.Paint += teken;

            Thread run = new Thread(simulation);
            run.Start();
        }

        private void simulation()
        {
            while (simulate)
            {
                Thread.Sleep(10);
                this.Invalidate();
            }

        }

        public void teken (object o, PaintEventArgs pea)
        {
            v.tekenAuto(pea.Graphics);
        }
    }
}
