using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using GreenLight.src.Data_Collection;
using GreenLight.src.Driver.GPS;

//This is the controller that deals with everything simulation related, and holds the vehicle and AI controller

namespace GreenLight
{
    public class SimulationController : AbstractController
    {
        public bool SimulationRunning;
        public bool SimulationPaused;

        public SimulationScreenController screenController;
        public VehicleController vehicleController;
        public AIController aiController;
        public WorldController worldController;
        public DriverProfileController profileController;

        public delegate void UpdateTextCallback();
        public delegate void RemoveAI(BetterAI _ai, bool _dump);
        public delegate void RemoveVehicle(BetterVehicle _veh, bool _dump);

        public int SimulationInterval = 16;
        public int SimulationIntervalDivider = 1;

        public int spawnBetweenTick = 200;
        public bool canSpawn = true;

        Thread Simulation;

        public SimulationController(SimulationScreenController _screenController)
        {
            this.vehicleController = new VehicleController(this);
            this.aiController = new AIController();
            this.worldController = new WorldController();
            this.worldController.Initialize();

            this.screenController = _screenController;
            this.profileController = new DriverProfileController(this.screenController.Screen);
            this.profileController.Initialize();

            Simulation = new Thread(this.update);
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public void StartSimulation()
        {
            if (!SimulationRunning)
            {
                this.initSimulation();

                SimulationRunning = true;
                Simulation.Start();
            }
            else
            {
                this.profileController.UnPauseSimulation();
                this.SimulationPaused = false;
            }
        }

        public void PauseSimulation()
        {
            this.SimulationPaused = true;
            this.profileController.PauseSimulation(vehicleController.vehicleList);
            this.screenController.Screen.Invalidate();
        }

        public void initSimulation()
        {
            // this.dataController = new DataController(this.screenController.Screen);
            //this.dataController.Initialize();
            General_Form.Main.DataScreen.dataController.DataControllerReset();

            this.worldController.SimulationWorld = this.worldController.currentSelected;
            this.vehicleController.initvehList();
            this.aiController.initDriverList();

            this.screenController.Screen.Invalidate();
        }

        public void resetSimulation()
        {
            this.vehicleController.vehicleList.Clear();
            this.aiController.driverlist.Clear();
            General_Form.Main.UserInterface.SimDataM.stopWatch.Reset();

            initSimulation();
        }

        private void update()
        {
            int x = 0;
            while (true)
            {
                Thread.Sleep((int)(this.SimulationInterval / this.SimulationIntervalDivider));

                if (!this.SimulationPaused)
                {

                    foreach (BetterVehicle car in vehicleController.vehicleList)
                    {
                        car.vehicleAI.Update();
                        car.Update();
                    }

                    if (x % 30 == 0)
                    {
                        foreach (BetterVehicle car in vehicleController.toDelete)
                        {
                            vehicleController.vehicleList.Remove(car);
                            this.screenController.Screen.BeginInvoke(new RemoveAI(General_Form.Main.DataScreen.dataController.collector.RemoveAI), new object[] { car.vehicleAI, true });
                            this.screenController.Screen.BeginInvoke(new RemoveVehicle(General_Form.Main.DataScreen.dataController.collector.RemoveVehicle), new object[] { car, true });

                        }

                        this.screenController.Screen.BeginInvoke(new UpdateTextCallback(General_Form.Main.DataScreen.dataController.collector.CollectAllData));
                        vehicleController.toDelete.Clear();
                    }

                    if (x % this.spawnBetweenTick == 0 && this.canSpawn)
                    {
                        vehicleController.getVehicle(this.screenController.gpsData.getRandomStartNode(), true);

                    }

                    if (x % 600 == 0)
                    {
                        Console.WriteLine(x);
                    }

                    this.screenController.Screen.Invalidate();
                    x++;
                }
            }
        }

        public void ChangeSimIntervalTimer(int _div)
        {
            _div = _div <= 0 ? 1 : _div;
            this.SimulationIntervalDivider = _div;
        }

        public void ChangeCarSpawn(int _perc)
        {
            if(_perc <= 0)
            {
                this.canSpawn = false;
                return;
            }

            this.canSpawn = true;
            this.spawnBetweenTick = 16 * 100 / _perc;
        }
    }
}
