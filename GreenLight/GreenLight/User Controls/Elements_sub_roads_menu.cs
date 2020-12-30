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
    public partial class Elements_sub_roads_menu : UserControl
    {
        public Elements_sub_roads_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, General_form.Height - 290 - 80);
            this.Location = new Point(General_form.Width - Menu_width, 290);
            this.AutoScroll = true;
            Initialize(General_form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height - 290 - 80);
            this.Location = new Point(General_form.Width - Sub_menu_width, 290);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            CurvedButtons temp1 = new CurvedButtons(new Size(60, 60), new Point(18, 18), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp1.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp1);

            CurvedButtons temp2 = new CurvedButtons(new Size(60, 60), new Point(95, 18), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp2.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp2);

            CurvedButtons temp3 = new CurvedButtons(new Size(60, 60), new Point(172, 18), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp3.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp3);

            CurvedButtons temp4 = new CurvedButtons(new Size(60, 60), new Point(18, 96), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp4.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp4);

            CurvedButtons temp5 = new CurvedButtons(new Size(60, 60), new Point(95, 96), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp5.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp5);

            CurvedButtons temp6 = new CurvedButtons(new Size(60, 60), new Point(172, 96), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp6.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp6);

            CurvedButtons temp7 = new CurvedButtons(new Size(60, 60), new Point(18, 174), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp7.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp7);

            CurvedButtons temp8 = new CurvedButtons(new Size(60, 60), new Point(95, 174), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp8.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp8);

            CurvedButtons temp9 = new CurvedButtons(new Size(60, 60), new Point(172, 174), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp9.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp9);

            CurvedButtons temp10 = new CurvedButtons(new Size(60, 60), new Point(18, 252), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp10.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp10);

            CurvedButtons temp11 = new CurvedButtons(new Size(60, 60), new Point(95, 252), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            temp11.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp11);

            CurvedButtons temp12 = new CurvedButtons(new Size(60, 60), new Point(172, 252), 25, "../../User Interface Recources/Yield_Sign_Button.png", this.BackColor);
            temp12.Click += (object o, EventArgs EA) => { General_form.Menu_to_lights(); };
            this.Controls.Add(temp12);
        }
    }
}