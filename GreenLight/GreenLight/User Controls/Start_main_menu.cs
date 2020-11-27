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
        public Start_main_menu(int Width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Width, General_form.Height);
            Initialize(General_form, Width);
        }

        public void Size_adjust(General_form General_form, int Sub_menu_width)
        {
            this.Size = new Size(General_form.Width - Sub_menu_width, General_form.Height);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width);
        }

        private void Initialize(General_form General_form, int Sub_menu_width) 
        {
            CurvedButtons New_project_button = new CurvedButtons(new Size(200, 200), new Point((General_form.Width - Sub_menu_width) / 2 - 200 - (int) (0.1*General_form.Width), (General_form.Height / 3) * 2 - 100), 100, "../../User Interface Recources/New_Project_Button.png", this.BackColor);
            CurvedButtons Choose_preset_button = new CurvedButtons(new Size(200, 200), new Point((General_form.Width-Sub_menu_width)/2 -100, (General_form.Height/3)*2-100), 100, "../../User Interface Recources/Choose_Preset_Button.png", this.BackColor);
            CurvedButtons Browse_directory_button = new CurvedButtons(new Size(200, 200), new Point((General_form.Width - Sub_menu_width) / 2 + (int)(0.1 * General_form.Width), (General_form.Height / 3) * 2 - 100), 100, "../../User Interface Recources/Browse_Directory_Button.png", this.BackColor);

            PictureBox Logo = new PictureBox();
            Logo.Size = new Size(400, 160);
            Logo.SizeMode = PictureBoxSizeMode.Zoom;
            Logo.Location = new Point((General_form.Width-Sub_menu_width) / 2 - 200, General_form.Height / 2 - 250);
            Logo.Image = Image.FromFile("../../User Interface Recources/Logo.png");

            this.Controls.Add(New_project_button);
            this.Controls.Add(Choose_preset_button);
            this.Controls.Add(Browse_directory_button);
            this.Controls.Add(Logo);

            New_project_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_build(); };
            Choose_preset_button.Click += (object o, EventArgs EA) => { };
            Browse_directory_button.Click += (object o, EventArgs EA) => {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "*.txt|*.*";
                open.Title = "Openen";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    General_form.Open(open.FileName);
                }
            };
        }
    }
}
