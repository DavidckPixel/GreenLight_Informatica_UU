using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace GreenLight
{
    class Slider : TrackBar
    {
        public Slider(Point location, int min, int max, int tickfrequency)
        {
            this.Location = location;
            this.TickStyle = TickStyle.None;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Width = 200;
            this.Minimum = min;
            this.Maximum = max;
            this.Value = (max - min) / 2;
            this.TickFrequency = tickfrequency; 
        }


    }
}
