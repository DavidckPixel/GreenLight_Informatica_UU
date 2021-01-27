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
        public DataController dataController;

        public delegate void UpdateTextCallback();
        public delegate void RemoveAI(BetterAI _ai, bool _dump);
        public delegate void RemoveVehicle(BetterVehicle _veh, bool _dump);

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

            this.dataController = new DataController(this.screenController.Screen);
            this.dataController.Initialize();

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
            this.dataController = new DataController(this.screenController.Screen);
            this.dataController.Initialize();

            this.worldController.SimulationWorld = this.worldController.currentSelected;
            this.vehicleController.initvehList();
            this.aiController.initDriverList();

            this.screenController.Screen.Invalidate();
        }

        private void update()
        {
            int x = 0;
            while (true)
            {
                Thread.Sleep(32);

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
                            this.screenController.Screen.BeginInvoke(new RemoveAI(dataController.collector.RemoveAI), new object[] { car.vehicleAI, true });
                            this.screenController.Screen.BeginInvoke(new RemoveVehicle(dataController.collector.RemoveVehicle), new object[] { car, true });

                        }

                        this.screenController.Screen.BeginInvoke(new UpdateTextCallback(dataController.collector.CollectAllData));
                        vehicleController.toDelete.Clear();
                        //this.BeginInvoke(new UpdateTextCallback(dataController.UpdateBrakeChart));
                        //this.BeginInvoke(new UpdateTextCallback(dataController.UpdateBrakePerTickChart));
                    }

                    if (x % 30 == 0 && x < 900)
                    {
                        vehicleController.getVehicle(this.screenController.gpsData.getRandomStartNode(), true);
                        
                        /*
                        foreach (AbstractRoad _road in General_Form.Main.BuildScreen.builder.roadBuilder.roads)
                        {
                            if (_road.roadtype == "Cross")
                            {
                                CrossRoad _temproad = (CrossRoad)_road;
                                _temproad.ConsoleDump();
                            }

                        } */
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
    }
}
