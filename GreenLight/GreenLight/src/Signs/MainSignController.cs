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
        public StopSignController stopSign;
        public YieldSignController yieldSignC;
        public PrioritySignController prioritySignC;

        public bool yieldSign = false;
        public bool prioritySign = false;

        Form main;

        public MainSignController(Form _main)
        {
            this.main = _main;

            this.speedSign = new SpeedSignController(_main, this);

            this.speedSign.initSettingScreen();

            this.stopSign = new StopSignController(_main, this);

            this.stopSign.initSettingScreen();

            this.stopSign = new StopSignController(_main, this);

            this.stopSign.initSettingScreen();

            this.prioritySignC = new PrioritySignController(_main, this);
            this.yieldSignC = new YieldSignController(_main, this);

            this.prioritySignC.initSettingScreen();
            this.yieldSignC.initSettingScreen();
        }

        public override void Initialize()
        {

        }

        public void placePriorityLevelSign()
        {
            if (yieldSign == true && prioritySign == false)
            {
                yieldSignC.placeSign();
            }
            else if (yieldSign == false && prioritySign == true)
            {
                prioritySignC.placeSign();
            }
        }

        public void deletePriorityLevelSign()
        {
            if (yieldSign == true && prioritySign == false)
            {
                yieldSignC.deleteSign();
            }
            else if (yieldSign == false && prioritySign == true)
            {
                prioritySignC.deleteSign();
            }
        }
    }
}
