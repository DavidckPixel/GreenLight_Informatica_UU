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
    public partial class Data_sub_menu : UserControl
    {
        public Data_sub_menu(int Menu_width, Form Form)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(User_Controls.Config.standardSubMenu["subMenuWidth"], Form.Height);
            this.Location = new Point(Form.Width - Menu_width, Form.Height);
            Initialize(Form, Menu_width, DrawData.Dosis_font_family);
        }

        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height);
            this.Location = new Point(Form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, DrawData.Dosis_font_family);

        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = User_Controls.Config.simSubMenu;

            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(User_Controls.Config.standardSubMenu["logoX"], User_Controls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            Move_panel Drag_pad = new Move_panel(Form);
            this.Controls.Add(Drag_pad);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(User_Controls.Config.standardSubMenu["deviderX"], User_Controls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Divider1);

            CurvedButtons Settings_header = new CurvedButtons(new Size(menu["settingsHeaderSizeX"], menu["settingsHeaderSizeY"]),  //settingsHeaderSizeX //settingsHeaderSizeY
               new Point(menu["settingsHeaderX"], menu["settingsHeaderY"]), "../../src/User Interface Recources/Settings_Header.png"); //settingsHeaderX //settingsHeaderY
            this.Controls.Add(Settings_header);

            CurvedButtons ReturnButton = new CurvedButtons(new Size(200, 50), new Point(10, 500), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Return to Simulation", DrawData.Dosis_font_family, null, this.BackColor);
            ReturnButton.Click += (object o, EventArgs ea) => { General_Form.Main.SwitchControllers(General_Form.Main.SimulationScreen); };
            this.Controls.Add(ReturnButton);

            CurvedButtons ExportButton = new CurvedButtons(new Size(200, 50), new Point(10, 300), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Export Data", DrawData.Dosis_font_family, null, this.BackColor);
            ExportButton.Click += (object o, EventArgs ea) =>
            {
                string _fileName = Interaction.InputBox("Enter Name: ", "File", "Export", 100, 100);
                General_Form.Main.DataScreen.dataController.ExportData(_fileName);
            };
            this.Controls.Add(ExportButton);
        }
    }
}
