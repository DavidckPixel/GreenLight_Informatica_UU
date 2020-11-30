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
    public partial class Build_main_menu : UserControl
    {
        public Build_main_menu(int Width, General_form General_form, FontFamily Dosis_font_family)
        {
            // Place bitmap reference here
        }

        public void Size_adjust(General_form General_form, int Sub_menu_width)
        {
            this.Size = new Size(General_form.Width - Sub_menu_width, General_form.Height);
            this.Controls.Clear();
            Initialize(General_form,Sub_menu_width);
        }

        private void Initialize(General_form General_form, int Sub_menu_width) 
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(General_form.Width-Sub_menu_width, General_form.Height);

            RoundButtons Info_button = new RoundButtons(new Size(40, 40), new Point(15, General_form.Height - 55), "../../User Interface Recources/Info_Button.png");

            this.Controls.Add(Info_button);
        }
    }
}
