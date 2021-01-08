using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public class SimulationController : AbstractController
    {
        public PictureBox Screen;
        public VehicleController vehicleController;
        public AIController aiController;

        public SimulationController(PictureBox _screen)
        {
            this.Screen = _screen;
            this.vehicleController = new VehicleController();
            this.aiController = new AIController();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
