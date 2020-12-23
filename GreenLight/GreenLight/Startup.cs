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
    class Startup : Form
    {
        //Quick temporary form for testing purposes, on which a few driving cars are simulated

        bool simulate;
        //Vehicle v = VehicleTypeConfig.types[0];
        //public List<Vehicle> carlist = new List<Vehicle> { };
        public List<AI> driverList = new List<AI> { };

       
        public Startup()
        {

            simulate = true;
            this.DoubleBuffered = true;
            this.Paint += teken;

            Thread run = new Thread(simulation);
            run.Start();
            createDriver();
            /*Thread drivers = new Thread(createDriver);
            drivers.Start();*/
           
            /*KeyPress += testmethod;*/
            MouseClick += clickmethod;
        }

        /*private void testmethod(object sender, EventArgs ea)
        {
            

            for (int t = 0; t < driverList.Count; t++)
            {
                driverList[t].v.tryBrake(0);
            }
        }*/
        private void createDriver()
        {
            Vehicle v = new Vehicle("Auto", 1353, 4.77f, 100, 4223, 10, 10, 0.35f, 2.65f);            
            AI driver = new AI(v, 250, 2, 0, 0);
            driverList.Add(driver);

            /*for (int aantal = 0; simulate && aantal < 10; aantal++)
            {
                Thread.Sleep(3000);
                Vehicle v = new Vehicle("Auto", 1353, 4.77f, 100, 4223, 10, 10, 0.35f, 2.65f);
                //carlist.Add(v);
                //carlist[aantal] = VehicleTypeConfig.types[0];
                AI driver = new AI(v, 250, 2, 0, 0);
                driverList.Add(driver);
            }*/
        }
        private void simulation()
        {
            while (simulate)
            {
                Thread.Sleep(16);
                this.Invalidate();
            }
        }

        public void teken(object o, PaintEventArgs pea)
        {
            for (int t = 0; t < driverList.Count; t++)
            {
                driverList[t].v.tekenAuto(pea.Graphics);
            }
        }


        public void clickmethod(object sender, MouseEventArgs mea)
        {

            Point clickPos = this.PointToClient(Cursor.Position);


            for (int t = 0; t < driverList.Count; t++)
            {
                driverList[t].changeDestination(clickPos.X, clickPos.Y);
            }
        }

    }
}