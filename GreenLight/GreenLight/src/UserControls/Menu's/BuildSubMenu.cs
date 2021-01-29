using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace GreenLight
{
    /* This is the Build sub menu class. This class has a method AdjustSize to fit the size of the users window.
       This user control is shown when the user is in the building screen.
       In the initialize void the controls are added to the submenu.
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class BuildSubMenu : UserControl
    {
        public List<CurvedButtons> bsmButtons = new List<CurvedButtons>();
        public CurvedButtons homeButton, saveButton, roadButton, signButton, settingsButton, Logo, undoButton, toggleButton, startSimulationButton, Divider1, Divider2, Divider3, Divider4;
        public Label autoSaveLabel;
        public CheckBox autoSave;
        public PictureBox elementsHeader;

        public BuildSubMenu(int menu_width, Form _form, FontFamily _dosisfontfamily)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(UserControls.Config.standardSubMenu["subMenuWidth"], _form.Height);
            this.Location = new Point(_form.Width - menu_width, 0);

            Initialize(_form, menu_width, _dosisfontfamily);
        }
        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height);
            this.Location = new Point(_form.Width - _submenuwidth, 0);
            this.Controls.Clear();
            Initialize(_form,_submenuwidth, _dosisfontfamily);
        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> menu = UserControls.Config.buildSubMenu;

            Logo = new CurvedButtons(_form, 1);
            Logo.Location = new Point(UserControls.Config.standardSubMenu["logoX"], UserControls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            elementsHeader = new PictureBox();
            elementsHeader.Size = new Size(menu["elementHeaderSizeX"], menu["elementHeaderSizeY"]);
            elementsHeader.SizeMode = PictureBoxSizeMode.StretchImage;
            elementsHeader.Location = new Point(menu["elementHeaderX"], menu["elementHeaderY"]);
            elementsHeader.Image = Image.FromFile("../../src/User Interface Recources/Elements_Header.png");
            this.Controls.Add(elementsHeader);

            MovePanel DragPad = new MovePanel(_form);
            this.Controls.Add(DragPad);

            System.Timers.Timer t = new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += (object to, ElapsedEventArgs EEA) =>
            {
                General_Form.Main.Save();
                Console.WriteLine("AutoSaved");
            };

            autoSaveLabel = new Label();
            autoSaveLabel.Text = "Autosave:";
            autoSaveLabel.Font = new Font(_dosisfontfamily, 11, FontStyle.Bold);
            autoSaveLabel.ForeColor = Color.FromArgb(142, 140, 144);
            autoSaveLabel.Location = new Point(menu["autosavelabelX"], menu["autosaveY"]);
            this.Controls.Add(autoSaveLabel);

            autoSave = new CheckBox();
            autoSave.Checked = false;
            autoSave.Location = new Point(menu["autosaveboxX"], menu["autosaveY"] + menu["autosavediff"]);
            autoSave.Size = new Size(25, 25);
            autoSave.Click += (object o, EventArgs EA) =>
            {
                if (autoSave.Checked)
                {
                    General_Form.Main.Save();
                    t.Start();
                }
                else
                {
                    t.Stop();
                }
            };
            this.Controls.Add(autoSave);

            /*     Buttons & Dividers    */

            homeButton = new CurvedButtons(new Size(menu["buttonSizeL"], menu["buttonSizeS"]), new Point(_submenuwidth / 2 - menu["buttonHome"], menu["buttonL"]), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Home", _dosisfontfamily, _form, this.BackColor);
            homeButton.Click += (object o, EventArgs EA) => { General_Form.Main.SwitchControllers(General_Form.Main.MenuController); };
            this.Controls.Add(homeButton);

            saveButton = new CurvedButtons(new Size(menu["buttonSizeL"], menu["buttonSizeS"]), new Point(_submenuwidth / 2 + menu["buttonSave"], menu["buttonL"]), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Save", _dosisfontfamily, _form, this.BackColor);
            saveButton.Click += (object o, EventArgs EA) => { General_Form.Main.Save(); };
            this.Controls.Add(saveButton);

            roadButton = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(_submenuwidth / 2 - menu["buttonRoad"], menu["buttonS"]), 25, "../../src/User Interface Recources/Road_Button.png", this.BackColor);
            roadButton.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.ResetAllButtons(roadButton, roadButton.Image_path); General_Form.Main.BuildScreen.SwitchSubMenus("Roads"); };
            this.Controls.Add(roadButton);
            bsmButtons.Add(roadButton);

            signButton = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(_submenuwidth / 2 - menu["buttonSign"], menu["buttonS"]), 25, "../../src/User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            signButton.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.ResetAllButtons(signButton, signButton.Image_path); General_Form.Main.BuildScreen.SwitchSubMenus("Signs"); };
            this.Controls.Add(signButton);
            bsmButtons.Add(signButton);

            settingsButton = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(_submenuwidth / 2 + menu["buttonBuilding"], menu["buttonS"]), 25, "../../src/User Interface Recources/Setting_Button.png", this.BackColor);
            settingsButton.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.ResetAllButtons(settingsButton, settingsButton.Image_path); General_Form.Main.BuildScreen.SwitchSubMenus("Buildings"); };
            this.Controls.Add(settingsButton);
            bsmButtons.Add(settingsButton);

            undoButton = new CurvedButtons(new Size(30, 30), new Point(10, _form.Height - menu["simStartY"] + 3), 20, "../../src/User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            undoButton.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.builder.roadBuilder.UndoRoad(); };
            this.Controls.Add(undoButton);

            toggleButton = new CurvedButtons(new Size(30, 30), new Point(_submenuwidth - 40, _form.Height - menu["simStartY"] + 3), 20, "../../src/User Interface Recources/Toggle_Button.png", this.BackColor);
            toggleButton.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.Toggle = General_Form.Main.BuildScreen.ToggleHitbox(); General_Form.Main.BuildScreen.Screen.Invalidate(); };
            this.Controls.Add(toggleButton);
            bsmButtons.Add(toggleButton);

            startSimulationButton = new CurvedButtons(new Size(menu["simStartSizeX"], menu["simStartSizeY"]), new Point(_submenuwidth / 2 - menu["simStartX"], _form.Height - menu["simStartY"]), 25,
                "../../src/User Interface Recources/Custom_Button.png", "Start simulation", _dosisfontfamily, _form, this.BackColor);
            startSimulationButton.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToSimulation(); };
            this.Controls.Add(startSimulationButton);

            Divider1 = new CurvedButtons();
            Divider1.Location = new Point(UserControls.Config.standardSubMenu["deviderX"], UserControls.Config.standardSubMenu["deviderY"]);
            this.Controls.Add(Divider1);

            Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, menu["divider2"]);
            this.Controls.Add(Divider2);

            Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, menu["divider3"]);
            this.Controls.Add(Divider3);

            Divider4 = new CurvedButtons();
            Divider4.Location = new Point(0, _form.Height - menu["divider4"]);
            this.Controls.Add(Divider4);

        }
    }
}
