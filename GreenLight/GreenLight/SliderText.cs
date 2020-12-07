using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public class SliderText : Label
    {
        public SliderText(FontFamily Dosis_Font_Familly, Point p, string text)
        {
            this.Font = new Font(Dosis_Font_Familly, 10, FontStyle.Bold);
            this.Location = p;
            this.Text = text;
            this.ForeColor = Color.FromArgb(142, 140, 144);
        }
    }
}
