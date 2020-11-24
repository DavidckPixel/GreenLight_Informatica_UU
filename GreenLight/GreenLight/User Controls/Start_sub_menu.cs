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
    public partial class Start_sub_menu : UserControl
    {
        // Form_width && Form_height kunnen weg i.v.m. General_form
        public Start_sub_menu(int Form_width, int Form_height,int Menu_width, General_form General_form) 
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250,Form_height);
            this.Location = new Point(Form_width-Menu_width, 0);

            Buttons Logo = new Buttons(General_form);
            Logo.Location = new Point(40, 20);
            this.Controls.Add(Logo);

            Buttons Divider1 = new Buttons("Divider");
            Divider1.Location = new Point(0, 100);
            this.Controls.Add(Divider1);

            PictureBox Project_header = new PictureBox();
            Project_header.Size = new Size(150, 25);
            Project_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Project_header.Location = new Point(50, 120);
            Project_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Project_header);

            Buttons Divider2 = new Buttons("Divider");
            Divider2.Location = new Point(0, this.Height - 75);
            this.Controls.Add(Divider2);

            Buttons About_button = new Buttons(new Size(160, 38), new Point(Menu_width/2-80, General_form.Height-55), "../../User Interface Recources/Custom_Button.png", "../../User Interface Recources/Custom_Button_On_Hover.png", 1);
            About_button.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(About_button);
        }

    }
}
