using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

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
