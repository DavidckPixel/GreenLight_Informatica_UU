using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

//This is the screen controller class, it has 2 important features that every class which inherits from this must implement:
//The Activate() & DeActivate() function which as the name implies are called when a ScreenController switches from being used to no longer being used
//An example of this is when the Road builder is enabled and the Menu screen is Deactivated.

namespace GreenLight
{
    public abstract class ScreenController : AbstractController
    {
        public PictureBox Screen;
        protected Form form;

        public ScreenController(PictureBox _screen = null)
        {
            this.Screen = _screen;
        }

        public abstract void Activate();

        public abstract void DeActivate();

    }
}
