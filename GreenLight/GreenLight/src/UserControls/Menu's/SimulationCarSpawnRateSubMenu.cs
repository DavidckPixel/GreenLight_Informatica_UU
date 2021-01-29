using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GreenLight
{
    /* This is the Simulation car spawn rate class. This class has a method AdjustSize to fit the size of the users window.
       In the initialize void the controls are added to the submenu.
       This user control is shown when the user is in the simulation screen and has selected the settings menu.
       The user can change the spawnrate of the vehicles using a slider.
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class SimulationCarSpawnRateSubMenu : UserControl
    {
        public SimulationCarSpawnRateSubMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(_menuwidth, _form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(_form.Width - _menuwidth, UserControls.Config.simElementsMenu["menuY"]);
            this.AutoScroll = true;
            Initialize(_form, _menuwidth, _dosisfontfamily);
        }

        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(_form.Width - _submenuwidth, UserControls.Config.simElementsMenu["menuY"]);
            this.Controls.Clear();
            Initialize(_form, _submenuwidth, _dosisfontfamily);

        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = UserControls.Config.simElementsMenu;
            int _sliderX = menu["sliderX"];
            int _start = menu["sliderStart"];
            int _diff = menu["sliderDiffY"];
            int _textX = menu["textX"];
            int _startText = menu["textStart"];

            ListBox worlds = new ListBox();
            WorldConfig.physics.ForEach(x => worlds.Items.Add(x));
            worlds.Location = new Point(20, 10);
            worlds.Size = new Size(120, 80);
            worlds.GotFocus += (object o, EventArgs ea) =>
            {
                worlds.Items.Clear();
                WorldConfig.physics.ForEach(x => worlds.Items.Add(x));
            };

            worlds.SelectedIndexChanged += (object o, EventArgs ea) =>
            {
                General_Form.Main.SimulationScreen.Simulator.worldController.currentSelected = (World)worlds.SelectedItem;
            };
            this.Controls.Add(worlds);

            CurvedButtons Editbutton = new CurvedButtons(new Size(70, 35), new Point(160, 5), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Edit", DrawData.Dosis_font_family, null, this.BackColor);
            CurvedButtons Newbutton = new CurvedButtons(new Size(70, 35), new Point(160, 45), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "New", DrawData.Dosis_font_family, null, this.BackColor);

            Editbutton.Click += (object o, EventArgs ea) => { General_Form.Main.SimulationScreen.Simulator.worldController.EditWorld((World)worlds.SelectedItem); };
            Newbutton.Click += (object o, EventArgs ea) => { General_Form.Main.SimulationScreen.Simulator.worldController.CreateNewWorld(); };

            this.Controls.Add(Editbutton);
            this.Controls.Add(Newbutton);

            Slider carSpawn = new Slider(new Point(25, 120), 0, 100);
            this.Controls.Add(carSpawn);

            SliderText carSpawn_label = new SliderText(DrawData.Dosis_font_family, new Point(30, 90), "Car Spawn:");
            this.Controls.Add(carSpawn_label);

            SliderText carSpawn_value = new SliderText(DrawData.Dosis_font_family, new Point(130, 90), carSpawn.Value.ToString() + " %");
            this.Controls.Add(carSpawn_value);

            carSpawn.ValueChanged += (object o, EventArgs EA) => { carSpawn_value.Text = carSpawn.Value.ToString() + " %"; General_Form.Main.SimulationScreen.Simulator.ChangeCarSpawn(carSpawn.Value); };

            CurvedButtons graphWindow = new CurvedButtons(new Size(80, 35), new Point(85, 190), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Graph", DrawData.Dosis_font_family, null, this.BackColor);
            graphWindow.Click += (object o, EventArgs ea) =>
            {
                General_Form.Main.SwitchControllers(General_Form.Main.DataScreen);
            };
            this.Controls.Add(graphWindow);
        }
    }
}
