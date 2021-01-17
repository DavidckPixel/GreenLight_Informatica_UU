using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class VehicleSpawner : EntityController
    {
        int totalcars = 10;
        int spawnspeed = 2; //2 cars per second
        public OriginPointController OPC = new OriginPointController();
        public List<AI> driverList = new List<AI> ();
        public List<bool> listchoice = new List<bool> ();

        public VehicleSpawner()
        {
            Thread drivers = new Thread(createDriver);
            drivers.Start();
        }

        public override void Initialize()
        {
        }

        private void createDriver()
        {
            for (int n = 0; n < totalcars; n++)
            {
                Vehicle v = new Vehicle(VehicleController.getVehicleStat("VW Passat variant 2015"), OPC.GetSpawnPoint.X, OPC.GetSpawnPoint.Y);
                AI driver = new AI(v, AIController.getDriverStat("Normal driver"));
                driverList.Add(driver);
                listchoice.Add(true);
                Thread.Sleep(1000/spawnspeed);
                //Console.WriteLine("Created vehicle");
            }
        }

        public void refresh()
        {
            while (true)
            {
                Console.WriteLine("YES!");
                Thread.Sleep(16);
                General_Form.Main.SimulationScreen.Screen.Invalidate();
            }
        }

        public void drawList(Graphics g)
        {
            for (int t = 0; t < driverList.Count; t++)
            {
                if (listchoice[t] && driverList[t].v.frame <= 624)
                {
                    driverList[t].v.drawVehicle(g, driverList[t].location);
                    if (driverList[t].v.frame == 624)
                    {
                        listchoice[t] = false;
                        driverList[t].v.frame = 0;
                        Console.WriteLine("Switch naar lijst 2 van vehicle " + t + ".");
                    }
                }
                else if (!listchoice[t] && driverList[t].v.frame <= 624)
                {
                    driverList[t].v.drawVehicle(g, driverList[t].location2);
                    if (driverList[t].v.frame == 624)
                    {
                        listchoice[t] = true;
                        driverList[t].v.frame = 0;
                        Console.WriteLine("Switch naar lijst 1 van vehicle " + t + ".");
                    }
                }
            }
        }
    }
}