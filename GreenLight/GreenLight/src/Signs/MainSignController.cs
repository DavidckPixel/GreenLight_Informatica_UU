using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public class MainSignController : EntityController
    {

        public List<AbstractSign> Signs = new List<AbstractSign>();
        public SpeedSignController speedSign;
        //public StopSignController stopSign;

        Form main;

        public MainSignController(Form _main)
        {
            this.main = _main;

            this.speedSign = new SpeedSignController(_main, this);

            this.speedSign.initSettingScreen();
        }

        public override void Initialize()
        {

        }
    }
}
