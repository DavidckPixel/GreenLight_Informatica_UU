using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GreenLight
{
    public partial class Start_sub_menu : UserControl
    {

        
        public Start_sub_menu(int Menu_width, Form Form, FontFamily Dosis_font_family, string[] Recent_projects)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(User_Controls.Config.standardSubMenu["subMenuWidth"], Form.Height);
            this.Location = new Point(Form.Width - Menu_width, 0);

            Initialize(Form, Menu_width, Dosis_font_family, Recent_projects);
        }

        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family, string[] Recent_projects)
        {
            this.Size = new Size(Sub_menu_width, Form.Height);
            this.Location = new Point(Form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family, Recent_projects);
        }

        //Cleaner but General_form should be just form
        /* public Start_sub_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family, string[] Recent_projects)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(250, General_form.Height);
            this.Location = new Point(General_form.Width - Sub_menu_width, 0);
            Initialize(General_form, Sub_menu_width, Dosis_font_family, Recent_projects);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(Sub_menu_width, General_form.Height);
                this.Location = new Point(General_form.Width - Sub_menu_width, 0);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width, Dosis_font_family, Recent_projects);
            };
        }*/

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family, string[] Recent_projects)
        {
            Dictionary<string, int> menu = User_Controls.Config.standardSubMenu;
            Dictionary<string, int> startmenu = User_Controls.Config.startSubMenu;

            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(menu["logoX"], menu["logoY"]);
            this.Controls.Add(Logo);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(menu["deviderX"], menu["deviderY"]);
            this.Controls.Add(Divider1);

            Move_panel Drag_pad = new Move_panel(Form);
            this.Controls.Add(Drag_pad);

            PictureBox Project_header = new PictureBox();
            Project_header.Size = new Size(startmenu["headerXsize"], startmenu["headerYsize"]);
            Project_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Project_header.Location = new Point(startmenu["headerX"], startmenu["headerY"]);
            Project_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Project_header);
            
            int i = 0;

            foreach (string tekst in Recent_projects)
            {
                CurvedButtons Project = new CurvedButtons(new Size(startmenu["projectXsize"], startmenu["projectYsize"]), new Point(Sub_menu_width / 2 - startmenu["projectX"], startmenu["projectYbase"] + i * startmenu["projectYdiff"]), startmenu["projectButtonCurve"], "../../User Interface Recources/Custom_Button.png", tekst, Dosis_font_family, Form, Color.White);
                Project.Location = new Point(Sub_menu_width / 2 - startmenu["projectX"], startmenu["projectYbase"] + i * startmenu["projectYdiff"]);
                this.Controls.Add(Project);
                i++;
            }

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(startmenu["divider2X"], this.Height - startmenu["divider2Y"]);
            this.Controls.Add(Divider2);


            CurvedButtons About_button = new CurvedButtons(new Size(startmenu["projectXsize"], startmenu["projectYsize"]),
                new Point(Sub_menu_width / 2 - startmenu["aboutX"], Form.Height - startmenu["aboutY"]), startmenu["aboutCurve"], "../../User Interface Recources/Custom_Button.png",
                "About", Dosis_font_family, Form, this.BackColor);
            About_button.Click += (object o, EventArgs ea) => { System.Diagnostics.Process.Start("https://github.com/DavidckPixel/GreenLight_Informatica_UU"); };
            this.Controls.Add(About_button);
        }
    }
}
