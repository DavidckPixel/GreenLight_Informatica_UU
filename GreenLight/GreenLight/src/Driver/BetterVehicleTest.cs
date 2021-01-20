using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GreenLight
{
    public partial class BetterVehicleTest : Form
    {
        public static List<BetterVehicle> vehiclelist = new List<BetterVehicle>();
        List<AbstractRoad> roads = new List<AbstractRoad>();
        List<AbstractRoad> roads2 = new List<AbstractRoad>();
        bool simulate = true;

        AbstractRoad testRoad;
        AbstractRoad testRoad2;
        AbstractRoad testRoad3;
        BetterVehicle testVehicle;
        BetterAI testAI;

        BetterVehicle testVehicle2;
        BetterAI testAI2;
        PictureBox pictureboxTemp;

        CrossRoadController controller;

        public BetterVehicleTest()
        {

            pictureboxTemp = new PictureBox();
            pictureboxTemp.Size = new Size(1000, 1000);
            pictureboxTemp.Location = new Point(0, 0);

            this.Controls.Add(pictureboxTemp);

            this.controller = new CrossRoadController(pictureboxTemp);

            this.Size = new Size(1000, 1000);

            Point start = new Point(50, 50);
            Point end = new Point(700, 700);

            /*
            testRoad = controller.newCrossRoad(start, 2, "David");
            testRoad2 = new DiagonalRoad(new Point(300, 450), new Point(300, 350), 2, "N", "Diagonal", false, false, null, null);
            testRoad3 = new DiagonalRoad(new Point(300, 550), new Point(300, 450), 2, "N", "Diagonal", false, false, null, null);

            roads.Add(testRoad3);
            roads.Add(testRoad2);
            roads.Add(testRoad);

            CrossRoad testRoadc = (CrossRoad)testRoad;
            */
            

            testRoad = new CurvedRoad(start, new Point(400, 400), 1, "NE", "Curved", false, false, null, null);
            testRoad2 = new DiagonalRoad(new Point(400,400), end, 1, "D", "Diagonal", false, false, null, null);

            roads.Add(testRoad);
            roads.Add(testRoad2);

            roads.Add(new CurvedRoad(end, new Point(900, 500), 1, "NW", "Curved", false, false, null, null));
            roads.Add(new DiagonalRoad(new Point(900, 500), new Point(700, 200), 1, "D", "Diagonal", false, false, null, null));
            roads.Add(new CurvedRoad(new Point(700, 200), new Point(400, 100), 1, "SW", "Curved", false, false, null, null));
            roads.Add(new CurvedRoad(new Point(400, 100), new Point(250, 50), 1, "NE", "Curved", false, false, null, null));
            roads.Add(new CurvedRoad(new Point(250, 50), new Point(150, 0), 1, "SW", "Curved", false, false, null, null));
            roads.Add(new CurvedRoad(new Point(150, 0), start, 1, "SE", "Curved", false, false, null, null));

            testRoad3 = new DiagonalRoad(new Point(100, 800), new Point(800, 900), 2, "D", "Diagonal", false, false, null, null);
            roads2.Add(testRoad3);

             

            VehicleStats vehicleStats = new VehicleStats("test", 1352, (float)4.77, 61, 4223, (float)2.65, (float)0.35);
            DriverStats driverStats = new DriverStats("David", 2.0f, 2.0f, 2, 2.0f);

            testVehicle = new BetterVehicle(vehicleStats, new Point(300,450));
            testAI = new BetterAI(driverStats, testVehicle);

            testVehicle2 = new BetterVehicle(vehicleStats, new Point(300, 550));
            testAI2 = new BetterAI(driverStats, testVehicle2);

            testVehicle.vehicleAI.SetPath(roads, 1);
            testVehicle2.vehicleAI.SetPath(roads, 0);

            testVehicle.vehicleAI.targetspeed = 3 ;
            testVehicle2.vehicleAI.targetspeed = 5;
            //testVehicle2.SetPath(roads, 0);
            //testVehicle2.vehicleAI.targetspeed = 6;

            //BetterVehicle testVehicle3 = new BetterVehicle(vehicleStats, testRoad3.Drivinglanes.Last().points.First().cord);
            //BetterAI testAI3 = new BetterAI(driverStats, testVehicle3);

            //testVehicle3.SetPath(roads2, 0);
            //testVehicle3.vehicleAI.wantsToSwitch = true;

            vehiclelist.Add(testVehicle);
            vehiclelist.Add(testVehicle2);
            //vehiclelist.Add(testVehicle3);
            //vehiclelist.Add(testVehicle2);
            //testAI.locationGoal = start;

            //-----------------------------------

            pictureboxTemp.Paint += Draw;
            pictureboxTemp.MouseClick += click;

            Thread run = new Thread(simulation);
            Thread _update = new Thread(update);

            run.Start();
            _update.Start();
        }

        private void click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("set the targetspeed!!");

        }

        private void simulation()
        {
            while (simulate)
            {
                Thread.Sleep(16);
                pictureboxTemp.Invalidate();
            }
        }

        private void update()
        {
            int x = 0;
            while (simulate)
            {
                Thread.Sleep(16);

                foreach(BetterVehicle car in vehiclelist)
                {
                    car.vehicleAI.Update();
                    car.Update();
                }

                if (x % 60 == 0)
                {
                    //Console.WriteLine("SWITCH!");
                    //testVehicle2.vehicleAI.wantsToSwitch = true;
                }

                x++;
            }
        }

        private void Draw(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            foreach(AbstractRoad z in roads)
            {
                z.Draw(g);
            }

            foreach (BetterVehicle car in vehiclelist)
            {
                car.Draw(g);
            }

        }

        private void BetterVehicleTest_Load(object sender, EventArgs e)
        {

        }
    }
}
