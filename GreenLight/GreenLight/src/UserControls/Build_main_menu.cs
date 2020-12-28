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
    public partial class Build_main_menu : UserControl
    {
        public Build_main_menu(int Width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Form.Width - Width, Form.Height);
            // Place bitmap reference here
            Form.SizeChanged += (object o, EventArgs EA) =>
            {
                this.Size = new Size(Form.Width - Width, Form.Height);
                this.Controls.Clear();
                Initialize(Form, Width);
            };
        }

        public void Size_adjust(Form Form, int Sub_menu_width)
        {
            this.Size = new Size(Form.Width - Sub_menu_width, Form.Height);
            this.Controls.Clear();
            Initialize(Form,Sub_menu_width);
        }

        private void Initialize(Form Form, int Sub_menu_width) 
        {
            
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Form.Width-Sub_menu_width, Form.Height);
            int _ButtonSize = User_Controls.Config.standardMainMenu["infobuttonsize"];
            RoundButtons Info_button = new RoundButtons(new Size(_ButtonSize, _ButtonSize), new Point(User_Controls.Config.standardMainMenu["infoX"], Form.Height - User_Controls.Config.standardMainMenu["infoYfromtop"]), "../../User Interface Recources/Info_Button.png");

            this.Controls.Add(Info_button);
        }
    }
}
