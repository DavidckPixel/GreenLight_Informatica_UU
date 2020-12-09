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

        bool simulate;
        //Vehicle v = VehicleTypeConfig.types[0];
        public List<Vehicle> carlist = new List<Vehicle> { };
        public List<AI> driverList = new List<AI> { };

        AbstractRoad Test; //TEMP OM TE TESTEN

        public Startup()
        {
            Test = new DiagonalRoad(new Point(20, 20), new Point(220, 220), 2, "S"); //TEMP OM TE TESTEN

            simulate = true;
            this.DoubleBuffered = true;
            this.Paint += Drawing; // Hijacked

            Thread run = new Thread(simulation);
            run.Start();

            Thread drivers = new Thread(createDriver);
            drivers.Start();
            Console.WriteLine(VehicleTypeConfig.types[0]);
        }

        private void createDriver()
        {
            for (int aantal = 0; simulate && aantal <= 10; aantal++)
            {
                Thread.Sleep(3000);
                Vehicle v = new Vehicle("Auto", 1353, 4.77f, 100, 4223, 0, 0, 0.35f, 2.65f);
                //carlist.Add(v);
                //carlist[aantal] = VehicleTypeConfig.types[0];
                AI driver = new AI(v, 250, 2, 0, 0);
                driverList.Add(driver);
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
        
        public void teken(object o, PaintEventArgs pea)
        {
            for(int t = 0; t < driverList.Count; t++)
            {
                driverList[t].v.tekenAuto(pea.Graphics);
            }
        }

        public void Drawing(Object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            foreach (DrivingLane _temp in Test.Drivinglanes)
            {
                _temp.Draw(g);
            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Startup
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Startup";
            this.Load += new System.EventHandler(this.Startup_Load);
            this.ResumeLayout(false);

        }

        private void Startup_Load(object sender, EventArgs e)
        {

        }
    }
}
