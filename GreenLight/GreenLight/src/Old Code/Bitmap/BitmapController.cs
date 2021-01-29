using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    // This is the BitmapController class. We created it in a very early stage of the project
    // and planned to use it to control everything that has to be drawn, but we ultimately decided
    // to draw everything using pictureboxes that can be accessed in all controller classes.
    // This class is not used anywhere in our project.

    class BitmapController
    {
        public static BitmapController bitmapController;

        public Bitmap drawfield;

        public BitmapController()
        {
            drawfield = new Bitmap(200, 200);
        }
    }
}
