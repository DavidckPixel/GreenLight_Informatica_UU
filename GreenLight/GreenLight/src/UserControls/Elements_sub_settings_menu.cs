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
    public partial class Elements_sub_buildings_menu : UserControl
    {
        public List<CurvedButtons> ESBM_Buttons = new List<CurvedButtons>();
        public Elements_sub_buildings_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - User_Controls.Config.buildElementsMenu["elementsXbase"] - User_Controls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Menu_width, User_Controls.Config.buildElementsMenu["elementsXbase"]);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - User_Controls.Config.buildElementsMenu["elementsXbase"] - User_Controls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(Form.Width - Sub_menu_width, User_Controls.Config.buildElementsMenu["elementsXbase"]);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = User_Controls.Config.buildElementsMenu;
            int _ButtonSize = menu["buttonSize"];
            int _ButtonXbase = menu["buttonXbase"];
            int _ButtonXdiff = menu["buttonXdiff"];
            int _ButtonYbase = menu["buttonYbase"];
            int _ButtonYdiff = menu["buttonYdiff"];
            int _ButtonCurve = menu["buttonCurve"];

            //-------------------------------------------------------------

            CheckBox showLanePoints = new CheckBox();
            showLanePoints.Location = new Point(_ButtonXbase + (int)(1.5 * _ButtonXdiff), _ButtonYbase +3);
            showLanePoints.Checked = true;
            showLanePoints.Size = new Size(20, 20);
            showLanePoints.CheckedChanged += (object o, EventArgs ea) =>
            {
                General_Form.Main.BuildScreen.builder.roadBuilder.visualizeLanePoints = showLanePoints.Checked;
                General_Form.Main.BuildScreen.Screen.Invalidate();
            };
            Controls.Add(showLanePoints);

            Label showLanePointsLabel = new Label();
            showLanePointsLabel.Text = "Visualize Driving Lanes: ";
            showLanePointsLabel.Font= new Font(Dosis_font_family, 9, FontStyle.Bold);
            showLanePointsLabel.ForeColor = Color.FromArgb(142, 140, 144);
            showLanePointsLabel.Location = new Point(_ButtonXbase, _ButtonYbase);
            showLanePointsLabel.Size = new Size(150, 30);
            Controls.Add(showLanePointsLabel);

        }
        private void ResetButtons(CurvedButtons Selected, string Filepath)
        {
            foreach (CurvedButtons x in ESBM_Buttons)
            {
                x.Selected = false;
                x.Image = Image.FromFile(x.Image_path.Remove(x.Image_path.Length - 10) + "Button.png");
            }
            Selected.Selected = true;
            Selected.Image = Image.FromFile(Filepath.Remove(Filepath.Length - 10) + "Select.png");
        }
    }
}
