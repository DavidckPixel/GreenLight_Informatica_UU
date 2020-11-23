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
    public partial class Build_sub_menu : UserControl
    {
        public Build_sub_menu(int Form_width, int Form_height,int Menu_width, General_form General_form)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250,Form_height);
            this.Location = new Point(Form_width-Menu_width, Form_height);
        }
    }
}
