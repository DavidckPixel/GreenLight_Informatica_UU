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
    public partial class Elements_sub_lights_menu : UserControl
    {
        public Elements_sub_lights_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.Green;
            this.Size = new Size(Menu_width, General_form.Height - 400 - 135);
            this.Location = new Point(General_form.Width - Menu_width, 400);

            Initialize(General_form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height - 210 - 135);
            this.Location = new Point(General_form.Width - Sub_menu_width, 210);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
           
        }
    }
}
