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
    public partial class Start_sub_recent_projects_menu : UserControl
    {

        string[] _recentProjects;

        public Start_sub_recent_projects_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - User_Controls.Config.standardSubMenu["deviderY"] - (User_Controls.Config.startSubMenu["divider2Y"] - 40));
            this.Location = new Point(Form.Width - Menu_width, User_Controls.Config.standardSubMenu["deviderY"] + 20);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - User_Controls.Config.standardSubMenu["deviderY"] - User_Controls.Config.startSubMenu["divider2Y"] - 40);
            this.Location = new Point(Form.Width - Sub_menu_width, User_Controls.Config.standardSubMenu["deviderY"] + 20);
            this.Controls.Clear();
            _recentProjects = File.ReadAllLines(General_Form.Main.recent_project);
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> startmenu = User_Controls.Config.startSubMenu;

            _recentProjects = File.ReadAllLines(General_Form.Main.recent_project);

            PictureBox Project_header = new PictureBox();
            Project_header.Size = new Size(startmenu["headerXsize"], startmenu["headerYsize"]);
            Project_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Project_header.Location = new Point(startmenu["headerX"], startmenu["headerY"]);
            Project_header.Image = Image.FromFile("../../User Interface Recources/Recent_Project_Header.png");
            this.Controls.Add(Project_header);


            //string[] _temp = File.ReadAllLines(General_Form.Main.recent_project);
            /* for (int t = 0; t < _temp.Count() - 1; t++)
             {
                 string[] _temp2 = _temp[t].Split(' ');
                 _temp[t] = _temp2[0];
             }*/

            int i = 0;
            if (_recentProjects != null)
            {
                
                for (int t = _recentProjects.Length - 1; t >= _recentProjects.Length - 5; t--)
                {
                    try
                    {
                        string[] _temp = _recentProjects[t].Split(' ');

                        CurvedButtons Project = new CurvedButtons(new Size(startmenu["projectXsize"], startmenu["projectYsize"]), new Point(Sub_menu_width / 2 - startmenu["projectX"], startmenu["projectYbase"] + i * startmenu["projectYdiff"]), startmenu["projectButtonCurve"], _temp[2], _temp[0], Dosis_font_family, Form, Color.White, 1);
                        Project.Location = new Point(Sub_menu_width / 2 - startmenu["projectX"], startmenu["projectYbase"] + i * startmenu["projectYdiff"]);
                        Project.Click += (object o, EventArgs ea) => { General_Form.Main.MenuController.SwitchToBuild(); General_Form.Main.Load(_temp[1]); };
                        this.Controls.Add(Project);
                        i++;
                    }

                    catch (Exception e) { }
                }
            }
        }
    }
}
