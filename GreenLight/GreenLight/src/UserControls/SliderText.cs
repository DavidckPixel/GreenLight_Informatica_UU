using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    /* This is the SliderText class.
       The SliderText is used to create labels for the sliders */
    public class SliderText : Label
    {
        public SliderText(FontFamily _dosisfontfamily, Point _location, string _text)
        {
            this.Font = new Font(_dosisfontfamily, 10, FontStyle.Bold);
            this.Location = _location;
            this.Text = _text;
            this.ForeColor = Color.FromArgb(142, 140, 144);
        }
    }
}
