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

            Initialize(General_form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height);
            this.Location = new Point(General_form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(General_form,Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family) 
        {
            CurvedButtons Logo = new CurvedButtons(General_form, 1);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            Move_panel Drag_pad = new Move_panel(General_form);
            this.Controls.Add(Drag_pad);

            PictureBox Elements_header = new PictureBox();
            Elements_header.Size = new Size(114, 30);
            Elements_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Elements_header.Location = new Point(68, 180);
            Elements_header.Image = Image.FromFile("../../User Interface Recources/Elements_Header.png");
            this.Controls.Add(Elements_header);

            CurvedButtons Home_button = new CurvedButtons(new Size(80, 40), new Point(Sub_menu_width / 2 - 90, 115), 25, "../../User Interface Recources/Custom_Button_Small.png", "Home", Dosis_font_family, General_form, this.BackColor);
            Home_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_start(); };
            this.Controls.Add(Home_button);

            CurvedButtons Save_button = new CurvedButtons(new Size(80, 40), new Point(Sub_menu_width / 2 + 10, 115), 25, "../../User Interface Recources/Custom_Button_Small.png", "Save", Dosis_font_family, General_form, this.BackColor);
            Save_button.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Save_button);

            CurvedButtons Road_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 - 107, 220), 25, "../../User Interface Recources/Custom_Button_Small.png", this.BackColor);
            Road_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_roads(); };
            this.Controls.Add(Road_button);

            CurvedButtons Light_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 - 49, 220), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            Light_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(Light_button);

            CurvedButtons Sign_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 + 9, 220), 25, "../../User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            Sign_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_signs(); };
            this.Controls.Add(Sign_button);

            CurvedButtons Building_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 + 67, 220), 25, "../../User Interface Recources/Custom_Button_Small.png", this.BackColor);
            Building_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_buildings(); };
            this.Controls.Add(Building_button);

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, 165);
            this.Controls.Add(Divider2);

            CurvedButtons Divider4 = new CurvedButtons();
            Divider4.Location = new Point(0, General_form.Height - 75);
            this.Controls.Add(Divider4);

            CurvedButtons Start_sim_button = new CurvedButtons(new Size(160, 38), new Point(Sub_menu_width / 2 - 80, General_form.Height - 55), 25,
                "../../User Interface Recources/Custom_Button.png", "Start simulation", Dosis_font_family, General_form, this.BackColor);
            Start_sim_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_simulation(); };
            this.Controls.Add(Start_sim_button);
        }
    }
}
