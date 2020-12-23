using System;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public partial class Start_sub_menu : UserControl
    {

        public Start_sub_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family, string[] Recent_projects)
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
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family, string[] Recent_projects)
        {
            CurvedButtons Logo = new CurvedButtons(General_form, 1);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            Move_panel Drag_pad = new Move_panel(General_form);
            this.Controls.Add(Drag_pad);

            PictureBox Project_header = new PictureBox();
            Project_header.Size = new Size(150, 25);
            Project_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Project_header.Location = new Point(50, 120);
            Project_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Project_header);
            
            int i = 0;

            foreach (string tekst in Recent_projects)
            {
                CurvedButtons Project = new CurvedButtons(new Size(160, 38), new Point(Sub_menu_width / 2 - 80, 160 + i * 45), 25, "../../User Interface Recources/Custom_Button.png", tekst, Dosis_font_family, General_form, Color.White);
                Project.Location = new Point(Sub_menu_width / 2 - 80, 160 + i * 45);
                this.Controls.Add(Project);
                i++;
            }

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, this.Height - 75);
            this.Controls.Add(Divider2);


            CurvedButtons About_button = new CurvedButtons(new Size(160, 38),
                new Point(Sub_menu_width / 2 - 80, General_form.Height - 55), 25, "../../User Interface Recources/Custom_Button.png",
                "About", Dosis_font_family, General_form, this.BackColor);
            this.Controls.Add(About_button);
        }
    }
}
