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
        bool SimulationRunning;

        public SimulationScreenController screenController;
        public VehicleController vehicleController;
        public AIController aiController;
        public WorldController worldController;

        Thread Simulation;

        public SimulationController(SimulationScreenController _screenController)
        {
            this.vehicleController = new VehicleController(this);
            this.aiController = new AIController();
            this.worldController = new WorldController();
            this.worldController.Initialize();

            this.screenController = _screenController;

            Simulation = new Thread(this.update);
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public void StartSimulation()
        {
            SimulationRunning = true;
            Simulation.Start();
        }

        public void PauseSimulation()
        {
            Simulation.Suspend();
        }

        private void update()
        {
            int x = 0;
            while (true)
            {
                Thread.Sleep(32);

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
                    }

                    vehicleController.toDelete.Clear();
                    //this.BeginInvoke(new UpdateTextCallback(dataController.UpdateBrakeChart));
                    //this.BeginInvoke(new UpdateTextCallback(dataController.UpdateBrakePerTickChart));
                }

                if (x % 60 == 0)
                {
                    vehicleController.getVehicle(this.screenController.gpsData.getRandomStartNode());
                    foreach(AbstractRoad _road in General_Form.Main.BuildScreen.builder.roadBuilder.roads)
                    {
                        if(_road.roadtype == "Cross")
                        {
                            CrossRoad _temproad = (CrossRoad)_road;
                            _temproad.ConsoleDump();
                        }

                    }
                }

                this.screenController.Screen.Invalidate();
                x++;
            }
        }
    }
}
