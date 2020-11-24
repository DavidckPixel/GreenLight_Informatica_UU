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
        public Build_main_menu(int Width, int Form_height, General_form General_form)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Width, Form_height);

            Buttons Info_button = new Buttons(new Size(40,40), new Point(15,General_form.Height-55),"../../User Interface Recources/Info_Button.png", "../../User Interface Recources/Info_Button_On_Hover.png");
            this.Controls.Add(Info_button);

            // Place bitmap reference here
        }
    }
}
