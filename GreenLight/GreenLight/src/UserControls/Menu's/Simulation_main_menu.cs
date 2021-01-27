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
    public partial class Simulation_main_menu : UserControl
    {
        public Simulation_main_menu(int Width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(50, 50);
            this.Location = new Point(0, Form.Height - 50);
            Initialize(Form, 50);
        }

        public void Size_adjust(Form Form, int Sub_menu_width)
        {
            this.Size = new Size(50, 50);
            this.Location = new Point(0, Form.Height - 50);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width);
        }
        
        //Cleaner but General_form should be just form
        /*public Simulation_main_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
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
        }*/

        private void Initialize(Form Form, int Sub_menu_width)
        {
            int _ButtonSize = User_Controls.Config.standardMainMenu["infobuttonsize"];
            RoundButtons Info_button = new RoundButtons(new Size(_ButtonSize, _ButtonSize), new Point(10, 0), "../../src/User Interface Recources/Info_Button.png");
            Info_button.Click += (object o, EventArgs ea) => { System.Diagnostics.Process.Start("https://www.marcdejong.online/projects/green-light-district/"); };
            this.Controls.Add(Info_button);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // dont code anything here. Just leave blank
        }

        protected void InvalidateEx()
        {
            if (Parent == null)
                return;
            Rectangle rc = new Rectangle(this.Location, this.Size);
            Parent.Invalidate(rc, true);
        }
    }
}
