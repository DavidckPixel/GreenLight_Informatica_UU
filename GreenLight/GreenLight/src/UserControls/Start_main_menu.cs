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
    public partial class Start_main_menu : UserControl
    {
        public Start_main_menu(int Width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Width, Form.Height);
            Initialize(Form, Width);
        }

        public void Size_adjust(Form Form, int Sub_menu_width)
        {
            this.Size = new Size(Form.Width - Sub_menu_width, Form.Height);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width);
        }

        private void Initialize(Form Form, int Sub_menu_width) 

        //Cleaner but General_form should be just form
        /*public Start_main_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(General_form.Width - Sub_menu_width, General_form.Height);
            Initialize(General_form, Sub_menu_width);

            General_form.SizeChanged += (object o, EventArgs EA) =>
            {
                this.Size = new Size(General_form.Width - Sub_menu_width, General_form.Height);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width);
            };
        }
        private void Initialize(General_form General_form, int Sub_menu_width) */
        {
            int _ButtonSize = User_Controls.Config.startMainMenu["buttonSize"];
            int _ButtonXdiff = User_Controls.Config.startMainMenu["buttonXdiff"];
            int _ButtonYdiff = User_Controls.Config.startMainMenu["buttonYdiff"];
            int _ButtonCurve = User_Controls.Config.startMainMenu["buttonCurve"];
            CurvedButtons New_project_button = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point((Form.Width - Sub_menu_width) / 2 - _ButtonXdiff * 2  - (int) (0.1*Form.Width), (Form.Height / 3) * 2 - _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/New_Project_Button.png", this.BackColor);
            CurvedButtons Choose_preset_button = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point((Form.Width-Sub_menu_width)/2 - _ButtonXdiff, (Form.Height/3)*2 - _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Choose_Preset_Button.png", this.BackColor);
            CurvedButtons Browse_directory_button = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point((Form.Width - Sub_menu_width) / 2 + (int)(0.1 * Form.Width), (Form.Height / 3) * 2 - _ButtonYdiff), _ButtonCurve, "../../User Interface Recources/Browse_Directory_Button.png", this.BackColor);

            PictureBox Logo = new PictureBox();
            Logo.Size = new Size(User_Controls.Config.startMainMenu["logoXsize"], User_Controls.Config.startMainMenu["logoYsize"]);
            Logo.SizeMode = PictureBoxSizeMode.Zoom;
            Logo.Location = new Point((Form.Width-Sub_menu_width) / 2 - User_Controls.Config.startMainMenu["logoXfromMiddle"], Form.Height / 2 - User_Controls.Config.startMainMenu["logoYfromMiddle"]);
            Logo.Image = Image.FromFile("../../User Interface Recources/Logo.png");

            this.Controls.Add(New_project_button);
            this.Controls.Add(Choose_preset_button);
            this.Controls.Add(Browse_directory_button);
            this.Controls.Add(Logo);

            New_project_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToBuild(); };
            Choose_preset_button.Click += (object o, EventArgs EA) => { };
            Browse_directory_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToBuild(); General_Form.Main.LoadDialog(); };
        }
    }
}
