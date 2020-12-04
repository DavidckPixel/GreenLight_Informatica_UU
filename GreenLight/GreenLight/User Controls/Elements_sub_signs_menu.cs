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
    public partial class Elements_sub_signs_menu : UserControl
    {
        public Elements_sub_signs_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, General_form.Height - 210 - 135);
            this.Location = new Point(General_form.Width - Menu_width, 210);

            Initialize(General_form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height - 210 - 135);
            this.Location = new Point(General_form.Width - Sub_menu_width, 210);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(0, 10);
            this.Controls.Add(Divider1);

            Slider temp1 = new Slider(new Point(10, 220));
            this.Controls.Add(temp1);

            Slider temp2 = new Slider(new Point(10, 190));
            this.Controls.Add(temp2);

            Slider temp3 = new Slider(new Point(10, 160));
            this.Controls.Add(temp3);

            Slider temp4 = new Slider(new Point(10, 130));
            this.Controls.Add(temp4);

            Slider temp5 = new Slider(new Point(10, 100));
            this.Controls.Add(temp5);

            PictureBox Driver_header = new PictureBox();
            Driver_header.Size = new Size(150, 25);
            Driver_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Driver_header.Location = new Point(50, 30);
            Driver_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Driver_header);
        }
    }
}
