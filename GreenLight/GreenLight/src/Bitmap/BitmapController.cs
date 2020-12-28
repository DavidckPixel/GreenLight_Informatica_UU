using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    class BitmapController
    {
        //This is the BitmapController class which will make sure all screenobjects will be drawn on the same bitmap.
        //For now this class is pretty much empty.

        public static BitmapController bitmapController;

        public Bitmap drawfield;

        public BitmapController()
        {
            drawfield = new Bitmap(200, 200);
        }
    }
}
