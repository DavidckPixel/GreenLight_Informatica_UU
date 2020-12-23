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
        public Build_sub_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250, Form.Height);
            this.Location = new Point(Form.Width - Menu_width, 0);

            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height);
            this.Location = new Point(Form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(Form,Sub_menu_width, Dosis_font_family);
        }

        //Cleaner maar General_form moet form zijn
        /*
        public Build_sub_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250, General_form.Height);
            this.Location = new Point(General_form.Width - Sub_menu_width, 0);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(Sub_menu_width, General_form.Height);
                this.Location = new Point(General_form.Width - Sub_menu_width, 0);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width, Dosis_font_family);
            };
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }*/

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            Move_panel Drag_pad = new Move_panel(Form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Elements_header = new CurvedButtons(new Size(114, 30),
               new Point(68, 180), "../../User Interface Recources/Elements_Header.png");
            this.Controls.Add(Elements_header);

            CurvedButtons Home_button = new CurvedButtons(new Size(80, 40), new Point(Sub_menu_width / 2 - 90, 115), 25, "../../User Interface Recources/Custom_Button_Small.png", "Home", Dosis_font_family, Form, this.BackColor);
            Home_button.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Menu_to_start(); };
            this.Controls.Add(Home_button);

            CurvedButtons Save_button = new CurvedButtons(new Size(80, 40), new Point(Sub_menu_width / 2 + 10, 115), 25, "../../User Interface Recources/Custom_Button_Small.png", "Save", Dosis_font_family, Form, this.BackColor);
            Save_button.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Save_button);

            CurvedButtons Road_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 - 107, 220), 25, "../../User Interface Recources/Road_Button.png", this.BackColor);
            Road_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.SwitchSubMenus("Roads");  };
            this.Controls.Add(Road_button);

            CurvedButtons Light_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 - 49, 220), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            Light_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.SwitchSubMenus("Lights"); };
            this.Controls.Add(Light_button);

            CurvedButtons Sign_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 + 9, 220), 25, "../../User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            Sign_button.Click += (object o, EventArgs EA) =>  { General_Form.Main.BuildScreen.SwitchSubMenus("Signs"); };
            this.Controls.Add(Sign_button);

            CurvedButtons Building_button = new CurvedButtons(new Size(40, 40), new Point(Sub_menu_width / 2 + 67, 220), 25, "../../User Interface Recources/Building_Button.png", this.BackColor);
            Building_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.SwitchSubMenus("Buildings"); };
            this.Controls.Add(Building_button);

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, 165);
            this.Controls.Add(Divider2);

            CurvedButtons Divider4 = new CurvedButtons();
            Divider4.Location = new Point(0, Form.Height - 75);
            this.Controls.Add(Divider4);

            CurvedButtons Start_sim_button = new CurvedButtons(new Size(160, 38), new Point(Sub_menu_width / 2 - 80, Form.Height - 55), 25,
                "../../User Interface Recources/Custom_Button.png", "Start simulation", Dosis_font_family, Form, this.BackColor);
            Start_sim_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToSimulation(); ; };
            this.Controls.Add(Start_sim_button);

            CurvedButtons Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, 280);
            this.Controls.Add(Divider3);
        }
    }
}
