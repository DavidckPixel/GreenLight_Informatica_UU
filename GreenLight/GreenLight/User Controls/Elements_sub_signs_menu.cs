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
        public Elements_sub_signs_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Sub_menu_width, General_form.Height - 290 - 80);
            this.Location = new Point(General_form.Width - Sub_menu_width, 290);
            this.AutoScroll = true;
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(Sub_menu_width, General_form.Height - 290 - 80);
                this.Location = new Point(General_form.Width - Sub_menu_width, 290);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width, Dosis_font_family);
            };
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            CurvedButtons Hand = new CurvedButtons(new Size(60, 60), new Point(18, 18), 25, "../../User Interface Recources/Hand_Button.png", this.BackColor);
            Hand.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Hand);

            CurvedButtons Speed_sign = new CurvedButtons(new Size(60, 60), new Point(95, 18), 25, "../../User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            Speed_sign.Click += (object o, EventArgs EA) => {  };
            this.Controls.Add(Speed_sign);

            CurvedButtons Yield_sign = new CurvedButtons(new Size(60, 60), new Point(172, 18), 25, "../../User Interface Recources/Yield_Sign_Button.png", this.BackColor);
            Yield_sign.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Yield_sign);

            CurvedButtons Priority_road_sign = new CurvedButtons(new Size(60, 60), new Point(18, 96), 25, "../../User Interface Recources/Priority_Road_Sign_Button.png", this.BackColor);
            Priority_road_sign.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Priority_road_sign);

            CurvedButtons Stop_sign = new CurvedButtons(new Size(60, 60), new Point(95, 96), 25, "../../User Interface Recources/Stop_Sign_Button.png", this.BackColor);
            Stop_sign.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Stop_sign);
        }
    }
}