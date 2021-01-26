using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    class Speedsign : PictureBox
    {
        int speed;
   protected override void OnPaint(PaintEventArgs pe)
        {
           
        }
        public Speedsign(int speed_)
        {
            speed = speed_;
        }
    }
}
