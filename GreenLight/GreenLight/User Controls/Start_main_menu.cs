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
        public Start_main_menu(int Width, int Form_height, General_form General_form)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Width, Form_height);

            CurvedButtons New_project_button = new CurvedButtons(new Size(200,200), new Point(125,250), "../../User Interface Recources/New_Project_Button.png", "../../User Interface Recources/New_Project_Button_On_Hover.png");
            Buttons Choose_preset_button = new Buttons(new Size(200, 200), new Point(350, 250), "../../User Interface Recources/New_Project_Button.png", "../../User Interface Recources/New_Project_Button_On_Hover.png");
            Buttons Browse_directory_button = new Buttons(new Size(200, 200), new Point(575, 250), "../../User Interface Recources/New_Project_Button.png", "../../User Interface Recources/New_Project_Button_On_Hover.png");

            PictureBox Logo = new PictureBox();
            Logo.Size = new Size(400,160);
            Logo.SizeMode = PictureBoxSizeMode.Zoom;
            Logo.Location = new Point(Width / 2 - 200, Height / 2 - 250);
            Logo.Image = Image.FromFile("../../User Interface Recources/Logo.png");

            this.Controls.Add(New_project_button);
            this.Controls.Add(Choose_preset_button);
            this.Controls.Add(Browse_directory_button);
            this.Controls.Add(Logo);

            New_project_button.Click += (object o, EventArgs EA) => { General_form.Menu_to_build(); };
            Choose_preset_button.Click += (object o, EventArgs EA) => { };
            Browse_directory_button.Click += (object o, EventArgs EA) => { };
        }
    }
}
