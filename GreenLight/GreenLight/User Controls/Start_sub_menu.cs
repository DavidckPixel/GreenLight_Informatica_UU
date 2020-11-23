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
        public Start_sub_menu(int Form_width, int Form_height,int Menu_width, General_form General_form)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250,Form_height);
            this.Location = new Point(Form_width-Menu_width, 0);

            PictureBox Logo = new PictureBox();
            Logo.Size = new Size(175, 70);
            Logo.SizeMode = PictureBoxSizeMode.Zoom;
            Logo.Location = new Point(40, 20);
            Logo.Image = Image.FromFile("../../User Interface Recources/Logo.png");
            Logo.MouseClick += new MouseEventHandler(Logo_click);
            this.Controls.Add(Logo);

            PictureBox Divider = new PictureBox();
            Divider.Size = new Size(250, 5);
            Divider.SizeMode = PictureBoxSizeMode.StretchImage;
            Divider.Location = new Point(0,100);
            Divider.Image = Image.FromFile("../../User Interface Recources/Sub_Menu_Divider.png");
            Divider.MouseClick += new MouseEventHandler(Logo_click);
            this.Controls.Add(Divider);

            PictureBox Project_header = new PictureBox();
            Project_header.Size = new Size(150, 25);
            Project_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Project_header.Location = new Point(50, 120);
            Project_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            Project_header.MouseClick += new MouseEventHandler(Logo_click);
            this.Controls.Add(Project_header);
        }

        private void Logo_click(object o, MouseEventArgs MEA) 
        { 
            // Title bar button functies
        }
    }
}
