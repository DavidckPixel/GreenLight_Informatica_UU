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
    /* This is the Slider class. This class is used to create a Trackbar
       with on a location with a minimumvalue and maximumvalue.           */
    public class Slider : TrackBar
    {
        public Slider(Point _location, int _minvalue, int _maxvalue)
        {
            this.Width = 200;
            this.Location = _location;
            
            this.TickStyle = TickStyle.None;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Minimum = _minvalue;
            this.Maximum = _maxvalue;
            this.Value = (_maxvalue - _minvalue) / 2 + _minvalue;

            this.MouseWheel += (object o, MouseEventArgs MEA) => { ((HandledMouseEventArgs)MEA).Handled = true; };
        }
    }
}
