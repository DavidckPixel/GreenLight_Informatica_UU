using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public abstract class AbstractSignController
    {
        protected Form mainScreen;
        protected Form settingScreen;

        public abstract void onSignClick(AbstractSign _sign);
        public abstract AbstractSign newSign();
        public abstract void deleteSign();
        public abstract void openMenu();
        public abstract void initSettingScreen();
    }
}
