using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public partial class Simulation_main_menu : UserControl
    {
        public Simulation_main_menu(int Width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            RoundButtons Info_button = new RoundButtons(new Size(40, 40), new Point(15, General_form.Height - 55), "../../User Interface Recources/Info_Button.png", "../../User Interface Recources/Info_Button_On_Hover.png");
            this.Controls.Add(Info_button);
            this.Size = new Size(Width, General_form.Height);
        }

        public virtual void Size_adjust(Size Form_size) 
        {
            this.Size = Form_size;
        }
    }
}
