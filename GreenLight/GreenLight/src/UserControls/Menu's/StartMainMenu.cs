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
    /*This is the Start main menu class. This class has a method AdjustSize to fit the size of the users window.
      This user control is the first screen shown when the program starts.
      In the initialize void the controls are added to the main menu.
      Switching from this user control to other user controls happens in the UserInterfaceController class. */
    public partial class StartMainMenu : UserControl
    {
        public StartMainMenu(int Width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(Width, Form.Height);
            Initialize(Form, Width);
        }

        public void Size_adjust(Form Form, int Sub_menu_width)
        {
            this.Size = new Size(Form.Width - Sub_menu_width, Form.Height);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width);
        }

        private void Initialize(Form Form, int Sub_menu_width) 
        {
            int _ButtonSize = UserControls.Config.startMainMenu["buttonSize"];
            int _ButtonXdiff = UserControls.Config.startMainMenu["buttonXdiff"];
            int _ButtonYdiff = UserControls.Config.startMainMenu["buttonYdiff"];
            int _ButtonCurve = UserControls.Config.startMainMenu["buttonCurve"];
            CurvedButtons New_project_button = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point((Form.Width - Sub_menu_width) / 2 - _ButtonXdiff * 2  - (int) (0.1*Form.Width), (Form.Height / 3) * 2 - _ButtonYdiff), _ButtonCurve, "../../src/User Interface Recources/New_Project_Button.png", this.BackColor);
            CurvedButtons Choose_preset_button = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point((Form.Width-Sub_menu_width)/2 - _ButtonXdiff, (Form.Height/3)*2 - _ButtonYdiff), _ButtonCurve, "../../src/User Interface Recources/Choose_Preset_Button.png", this.BackColor);
            CurvedButtons Browse_directory_button = new CurvedButtons(new Size(_ButtonSize, _ButtonSize), new Point((Form.Width - Sub_menu_width) / 2 + (int)(0.1 * Form.Width), (Form.Height / 3) * 2 - _ButtonYdiff), _ButtonCurve, "../../src/User Interface Recources/Browse_Directory_Button.png", this.BackColor);

            PictureBox Logo = new PictureBox();
            Logo.Size = new Size(UserControls.Config.startMainMenu["logoXsize"], UserControls.Config.startMainMenu["logoYsize"]);
            Logo.SizeMode = PictureBoxSizeMode.Zoom;
            Logo.Location = new Point((Form.Width-Sub_menu_width) / 2 - UserControls.Config.startMainMenu["logoXfromMiddle"], Form.Height / 2 - UserControls.Config.startMainMenu["logoYfromMiddle"]);
            Logo.Image = Image.FromFile("../../src/User Interface Recources/Logo.png");

            this.Controls.Add(New_project_button);
            this.Controls.Add(Choose_preset_button);
            this.Controls.Add(Browse_directory_button);
            this.Controls.Add(Logo);

            New_project_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToBuild(); };
            Choose_preset_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToBuild(); General_Form.Main.LoadPresets(); };
            Browse_directory_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToBuild(); General_Form.Main.LoadDialog(); };
        }
    }
}
