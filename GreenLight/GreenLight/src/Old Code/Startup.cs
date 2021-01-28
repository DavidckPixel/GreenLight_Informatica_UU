using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace GreenLight
{
    // This was a quick temporary form for testing purposes, on which a few driving cars are simulated
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project and cannot be used for tests anymore. 

    class Startup : Form
    {
        bool simulate;
        public List<AI> driverList = new List<AI> { };
        public List<bool> listchoice = new List<bool> { };
        public Startup()
        {
            simulate = true;
            this.DoubleBuffered = true;
            this.Paint += Draw;

            Thread run = new Thread(simulation);
            run.Start();
            Thread drivers = new Thread(createDriver);
            drivers.Start();
        }

        private void createDriver()
        {
            for (int n = 0; simulate && n < 1; n++)
            {
                Vehicle v = new Vehicle(new VehicleStats("Car", 1353, 4.77f, 100, 4223, 0.3f, 2.65f, false, 1), 10, 10);
                AI driver = new AI(v, new DriverStats("new driver", 250, 2, 0, 0, 50, false));
                driverList.Add(driver);
                listchoice.Add(true);
                Thread.Sleep(1000);
            }
        }
        private void simulation()
        {
            while (simulate)
            {
                Thread.Sleep(16);
                this.Invalidate();
            }
        }

        public void Draw(object o, PaintEventArgs pea)
        {
            for (int t = 0; t < driverList.Count; t++)
            {
                if (listchoice[t] && driverList[t].v.frame <= 624)
                {
                    driverList[t].v.drawVehicle(pea.Graphics, driverList[t].location);
                    if (driverList[t].v.frame == 624)
                    {
                        listchoice[t] = false;
                        driverList[t].v.frame = 0;
                    }
                }
                else if (!listchoice[t] && driverList[t].v.frame <= 624)
                {
                    driverList[t].v.drawVehicle(pea.Graphics, driverList[t].location2);
                    if (driverList[t].v.frame == 624)
                    {
                        listchoice[t] = true;
                        driverList[t].v.frame = 0;
                    }
                }
            }
        }
    }
}