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
        public Build_sub_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250, General_form.Height);
            this.Location = new Point(General_form.Width - Menu_width, 0);

            Buttons Logo = new Buttons(General_form);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            Buttons Divider1 = new Buttons("Divider");
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            Move_panel Drag_pad = new Move_panel(General_form);
            this.Controls.Add(Drag_pad);

            Buttons Home_button = new Buttons(new Size(80, 40), new Point(Menu_width / 2 - 90, 115), "../../User Interface Recources/Custom_Button_Small.png", "Home", Dosis_font_family, General_form);
            Home_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_start(); };
            this.Controls.Add(Home_button);

            Buttons Save_button = new Buttons(new Size(80, 40), new Point(Menu_width / 2 +10, 115), "../../User Interface Recources/Custom_Button_Small.png","Save", Dosis_font_family,General_form);
            Save_button.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Save_button);

            Buttons Divider2 = new Buttons("Divider");
            Divider2.Location = new Point(0, 165);
            this.Controls.Add(Divider2);

            Buttons Divider3 = new Buttons("Divider");
            Divider3.Location = new Point(0, General_form.Height - 75);
            this.Controls.Add(Divider3);

            Buttons Start_sim_button = new Buttons(new Size(160, 38), new Point(Menu_width / 2 - 80, General_form.Height - 55), 
                "../../User Interface Recources/Custom_Button.png","Start simulation", Dosis_font_family, General_form);
            Start_sim_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_simulation(); };
            this.Controls.Add(Start_sim_button);
        }
    }
}
