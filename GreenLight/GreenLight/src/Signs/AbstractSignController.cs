using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Abstract SignController is the base controller from which all the specific sign controllers inherit
//every sign controller needs to implement the features: onSignClick (what to do when a sign is clicked)
//newSign() a function that returns a new Sign
//openMenu() a function that opens the settingScreen where sign data can be changed
//initSettingScreen() is the function where all buttons are created
//deleteSign() is the function that deletes the sign

namespace GreenLight
{
    public abstract class AbstractSignController
    {
        protected Form mainScreen;
        protected Form settingScreen;

        public MainSignController signController;

        public abstract void onSignClick(AbstractSign _sign);
        public abstract AbstractSign newSign();
        public abstract void openMenu();
        public abstract void initSettingScreen();
        public abstract void deleteSign();

    }
}
