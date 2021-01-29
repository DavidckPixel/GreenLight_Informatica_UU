using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GreenLight
{
    /*This is the Start sub menu class. This class has a method AdjustSize to fit the size of the users window.
      This user control is the first screen shown when the program starts.
      Switching from this user control to other user controls happens in the UserInterfaceController class. */
    public partial class StartSubMenu : UserControl
    {
        public CurvedButtons Logo, aboutButton, Divider1, Divider2;
        public StartSubMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(UserControls.Config.standardSubMenu["subMenuWidth"], _form.Height);
            this.Location = new Point(_form.Width - _menuwidth, 0);

            Initialize(_form, _menuwidth, _dosisfontfamily);
        }

        public void Size_adjust(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height);
            this.Location = new Point(_form.Width - _submenuwidth, 0);
            this.Controls.Clear();

            Initialize(_form, _submenuwidth, _dosisfontfamily);
        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> menu = UserControls.Config.standardSubMenu;
            Dictionary<string, int> startmenu = UserControls.Config.startSubMenu;

            Logo = new CurvedButtons(_form, 1);
            Logo.Location = new Point(menu["logoX"], menu["logoY"]);
            this.Controls.Add(Logo);

            /*     Buttons & Dividers    */

            Divider1 = new CurvedButtons();
            Divider1.Location = new Point(menu["deviderX"], menu["deviderY"]);
            this.Controls.Add(Divider1);

            Divider2 = new CurvedButtons();
            Divider2.Location = new Point(startmenu["divider2X"], this.Height - startmenu["divider2Y"]);
            this.Controls.Add(Divider2);

            CurvedButtons aboutButton = new CurvedButtons(new Size(startmenu["aboutXsize"], startmenu["aboutYsize"]), new Point(_submenuwidth / 2 - startmenu["aboutX"], _form.Height - startmenu["aboutY"]), startmenu["aboutCurve"], "../../src/User Interface Recources/Custom_Button.png", "About", _dosisfontfamily, _form, this.BackColor);
            aboutButton.Click += (object o, EventArgs ea) => { System.Diagnostics.Process.Start("https://www.marcdejong.online/projects/green-light-district/"); };
            this.Controls.Add(aboutButton);

            MovePanel dragPad = new MovePanel(_form);
            this.Controls.Add(dragPad);

        }
    }
}
