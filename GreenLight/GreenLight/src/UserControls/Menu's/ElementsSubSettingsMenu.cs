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
    /* This is the Settings sub menu class. This class has a method AdjustSize to fit the size of the users window.
       This user control is shown when the user is in the building screen and has clicked on the settings button.
       In this control the user can toggle the gridpoints and visualization of the lanepoints.
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class ElementsSubSettingsMenu : UserControl
    {
        public ElementsSubSettingsMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(_menuwidth, _form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(_form.Width - _menuwidth, UserControls.Config.buildElementsMenu["elementsXbase"]);
            Initialize(_form, _menuwidth, _dosisfontfamily);
        }
        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height - UserControls.Config.buildElementsMenu["elementsXbase"] - UserControls.Config.buildElementsMenu["elementsXplus"]);
            this.Location = new Point(_form.Width - _submenuwidth, UserControls.Config.buildElementsMenu["elementsXbase"]);
            this.Controls.Clear();
            Initialize(_form, _submenuwidth, _dosisfontfamily);
        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> menu = UserControls.Config.buildElementsMenu;
            int _ButtonSize = menu["buttonSize"];
            int _ButtonXbase = menu["buttonXbase"];
            int _ButtonXdiff = menu["buttonXdiff"];
            int _ButtonYbase = menu["buttonYbase"];
            int _ButtonYdiff = menu["buttonYdiff"];
            int _ButtonCurve = menu["buttonCurve"];

            Label showLanePointsLabel = new Label();
            showLanePointsLabel.Text = "Visualize Driving Lanes: ";
            showLanePointsLabel.Font = new Font(_dosisfontfamily, 9, FontStyle.Bold);
            showLanePointsLabel.ForeColor = Color.FromArgb(142, 140, 144);
            showLanePointsLabel.Location = new Point(_ButtonXbase, _ButtonYbase);
            showLanePointsLabel.Size = new Size(130, 30);
            Controls.Add(showLanePointsLabel);

            CheckBox showLanePoints = new CheckBox();
            Console.WriteLine("BXbase  " + _ButtonXbase);
            showLanePoints.Location = new Point(160, _ButtonYbase);
            showLanePoints.Checked = true;
            showLanePoints.Size = new Size(20, 20);
            showLanePoints.CheckedChanged += (object o, EventArgs ea) =>
            {
                General_Form.Main.BuildScreen.builder.roadBuilder.visualizeLanePoints = showLanePoints.Checked;
                General_Form.Main.BuildScreen.Screen.Invalidate();
            };
            Controls.Add(showLanePoints);

            Label Toggle_grid = new Label();
            Toggle_grid.Text = "Toggle Grid:";
            Toggle_grid.Font = new Font(_dosisfontfamily, 9, FontStyle.Bold);
            Toggle_grid.ForeColor = Color.FromArgb(142, 140, 144);
            Toggle_grid.Location = new Point(_ButtonXbase, _ButtonYbase + 30);
            Toggle_grid.Size = new Size(100, 30);
            Controls.Add(Toggle_grid);

            CheckBox toggleGridBox = new CheckBox();
            toggleGridBox.Location = new Point(160, _ButtonYbase + 38);
            toggleGridBox.Checked = true;
            toggleGridBox.Size = new Size(20, 20);
            toggleGridBox.CheckedChanged += (object o, EventArgs ea) =>
            {
                if (General_Form.Main.BuildScreen.builder.gridController.Set_visible == true)
                {
                    General_Form.Main.BuildScreen.builder.gridController.Set_visible = false;
                    General_Form.Main.Invalidate();
                }
                else
                {
                    General_Form.Main.BuildScreen.builder.gridController.Set_visible = true;
                    General_Form.Main.Invalidate();
                }
            };
            Controls.Add(toggleGridBox);
        }
    }
}
