using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GreenLight
{
    /* This is the Data Sub menu class. This class has a method AdjustSize to fit the size of the users window.
      In the initialize void the controls are added to the submenu.
      This user control is shown when the user is in the simulation screen and entered the data stats of the simulation. 
      Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class DataSubMenu : UserControl
    {
        public DataSubMenu(int _menuwidth, Form _form)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(UserControls.Config.standardSubMenu["subMenuWidth"], _form.Height);
            this.Location = new Point(_form.Width - _menuwidth, _form.Height);
            Initialize(_form, _menuwidth, DrawData.Dosis_font_family);
        }

        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height);
            this.Location = new Point(_form.Width - _submenuwidth, 0);
            this.Controls.Clear();
            Initialize(_form, _submenuwidth, DrawData.Dosis_font_family);

        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> menu = UserControls.Config.simSubMenu;

            CurvedButtons Logo = new CurvedButtons(_form, 1);
            Logo.Location = new Point(UserControls.Config.standardSubMenu["logoX"], UserControls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            MovePanel Drag_pad = new MovePanel(_form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(UserControls.Config.standardSubMenu["deviderX"], UserControls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Divider1);


            CurvedButtons ReturnButton = new CurvedButtons(new Size(200, 50), new Point(25, 500), 25, "../../src/User Interface Recources/Custom_Button.png", "Simulation", DrawData.Dosis_font_family, null, this.BackColor);
            ReturnButton.Click += (object o, EventArgs ea) => { General_Form.Main.SwitchControllers(General_Form.Main.SimulationScreen); };
            this.Controls.Add(ReturnButton);

            CurvedButtons ExportButton = new CurvedButtons(new Size(200, 50), new Point(25, 425), 25, "../../src/User Interface Recources/Custom_Button.png", "Export Data", DrawData.Dosis_font_family, null, this.BackColor);
            ExportButton.Click += (object o, EventArgs ea) =>
            {
                string _fileName = Interaction.InputBox("Enter Name: ", "File", "Export", 100, 100);
                General_Form.Main.DataScreen.dataController.ExportData(_fileName);
            };
            this.Controls.Add(ExportButton);
        }
    }
}
