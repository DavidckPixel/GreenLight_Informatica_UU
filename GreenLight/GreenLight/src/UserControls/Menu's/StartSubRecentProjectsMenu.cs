using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GreenLight
{
    /*This is the StartSubRecentProjectMenu class. This class has a method AdjustSize to fit the size of the users window.
      This user control is shown when the program starts and shows your recent projects.
      Switching from this user control to other user controls happens in the UserInterfaceController class. */
    public partial class StartSubRecentProjectsMenu : UserControl
    {
        PictureBox projectHeader;
        string[] _recentProjects;

        public StartSubRecentProjectsMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(_menuwidth, _form.Height - UserControls.Config.standardSubMenu["deviderY"] - (UserControls.Config.startSubMenu["divider2Y"] - 40));
            this.Location = new Point(_form.Width - _menuwidth, UserControls.Config.standardSubMenu["deviderY"] + 20);
            this.AutoScroll = true;
            Initialize(_form, _menuwidth, _dosisfontfamily);
        }
        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height - UserControls.Config.standardSubMenu["deviderY"] - UserControls.Config.startSubMenu["divider2Y"] - 40);
            this.Location = new Point(_form.Width - _submenuwidth, UserControls.Config.standardSubMenu["deviderY"] + 20);
            this.Controls.Clear();
            _recentProjects = File.ReadAllLines(General_Form.Main.recent_project);
            Initialize(_form, _submenuwidth, _dosisfontfamily);
        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> startmenu = UserControls.Config.startSubMenu;

            _recentProjects = File.ReadAllLines(General_Form.Main.recent_project);

            projectHeader = new PictureBox();
            projectHeader.Size = new Size(startmenu["headerXsize"], startmenu["headerYsize"]);
            projectHeader.SizeMode = PictureBoxSizeMode.StretchImage;
            projectHeader.Location = new Point(startmenu["headerX"], startmenu["headerY"]);
            projectHeader.Image = Image.FromFile("../../src/User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(projectHeader);

            int _counter = 0;
            if (_recentProjects != null)
            {
                
                for (int t = _recentProjects.Length - 1; t >= _recentProjects.Length - 5; t--)
                {
                    try
                    {
                        string[] _temp = _recentProjects[t].Split(' ');

                        CurvedButtons Project = new CurvedButtons(new Size(startmenu["projectXsize"], startmenu["projectYsize"]), new Point(_submenuwidth / 2 - startmenu["projectX"], startmenu["projectYbase"] + _counter * startmenu["projectYdiff"]), startmenu["projectButtonCurve"], _temp[2], _temp[0], _dosisfontfamily, _form, Color.White, 1);
                        Project.Location = new Point(_submenuwidth / 2 - startmenu["projectX"], startmenu["projectYbase"] + _counter * startmenu["projectYdiff"]);
                        Project.Click += (object o, EventArgs ea) => { General_Form.Main.MenuController.SwitchToBuild(); General_Form.Main.Load(_temp[1]); };
                        this.Controls.Add(Project);
                        _counter++;
                    }

                    catch (Exception e) { }
                }
            }
        }
    }
}
