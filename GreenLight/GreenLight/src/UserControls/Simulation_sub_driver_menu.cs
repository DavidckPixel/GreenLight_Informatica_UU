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
    public partial class Simulation_sub_driver_menu : UserControl
    {
        public Selection_box Selection_box;

        public Simulation_sub_driver_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - User_Controls.Config.simElementsMenu["menuY"] - User_Controls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(Form.Width - Menu_width, User_Controls.Config.simElementsMenu["menuY"]);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - User_Controls.Config.simElementsMenu["menuY"] - User_Controls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(Form.Width - Sub_menu_width, User_Controls.Config.simElementsMenu["menuY"]);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = User_Controls.Config.simElementsMenu;
            int _sliderX = menu["sliderX"];
            int _start = menu["sliderStartDriver"];
            int _diff = menu["sliderDiffY"];
            int _text = menu["textX"];
            int _textstart = menu["textStartDriver"];

            

            List<string> _temp = AIController.getStringDriverStats();
            Dictionary<string, int> DriverMenu = User_Controls.Config.simDriver;

            Selection_box = new Selection_box(Form, Dosis_font_family, _temp, null);
            if (Form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(User_Controls.Config.standardSubMenu["selectionBoxMaxX"], User_Controls.Config.standardSubMenu["selectionBoxMaxY"]);
            else Selection_box.Location = new Point(User_Controls.Config.standardSubMenu["selectionBoxX"], User_Controls.Config.standardSubMenu["selectionBoxY"]);
            this.Controls.Add(Selection_box);

            Slider Reaction_time = new Slider(new Point(_sliderX, _start + _diff * 4), DriverMenu["reactionTimeMin"], DriverMenu["reactionTimeMax"]);
            this.Controls.Add(Reaction_time);
            SliderText Reaction_time_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff * 4), "Reaction time:");
            this.Controls.Add(Reaction_time_label);
            SliderText Reaction_time_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff * 4), (Reaction_time.Value / 10).ToString() + " s");
            this.Controls.Add(Reaction_time_Value);
            Reaction_time.ValueChanged += (object o, EventArgs EA) => { Reaction_time_Value.Text = (((Reaction_time.Value)) / 10).ToString() + " s"; };

            Slider Follow_interval = new Slider(new Point(_sliderX, _start + _diff * 3), DriverMenu["followIntervalMin"], DriverMenu["followIntervalMax"]);
            this.Controls.Add(Follow_interval);
            SliderText Follow_interval_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff * 3), "Follow interval:");
            this.Controls.Add(Follow_interval_label);
            SliderText Follow_interval_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff * 3), (Follow_interval.Value / 10).ToString() + " s");
            this.Controls.Add(Follow_interval_Value);
            Follow_interval.ValueChanged += (object o, EventArgs EA) => { Follow_interval_Value.Text = (((double)(Follow_interval.Value)) / 10).ToString() + " s"; };

            Slider Speeding = new Slider(new Point(_sliderX, _start + _diff * 2), DriverMenu["speedingMin"], DriverMenu["speedingMax"]);
            this.Controls.Add(Speeding);
            SliderText Speeding_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff * 2), "Speeding:");
            this.Controls.Add(Speeding_label);
            SliderText Speeding_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff * 2), Speeding.Value.ToString() + " km/h");
            this.Controls.Add(Speeding_Value);
            Speeding.ValueChanged += (object o, EventArgs EA) => { Speeding_Value.Text = Speeding.Value.ToString() + " km/h"; };

            Slider Rulebreaking = new Slider(new Point(_sliderX, _start + _diff), DriverMenu["ruleBreakingMin"], DriverMenu["ruleBreakingMax"]);
            this.Controls.Add(Rulebreaking);
            SliderText Rulebreaking_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff), "Rulebreaking:");
            this.Controls.Add(Rulebreaking_label);
            SliderText Rulebreaking_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff), Rulebreaking.Value.ToString() + " %");
            this.Controls.Add(Rulebreaking_Value);
            Rulebreaking.ValueChanged += (object o, EventArgs EA) => { Rulebreaking_Value.Text = Rulebreaking.Value.ToString() + " %"; };

            Slider Occurunce = new Slider(new Point(_sliderX, _start), DriverMenu["occurenceMin"], DriverMenu["occurenceMax"]);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => { Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; };

            //----------------------------------------------

            CurvedButtons Edit_Driver_Header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../User Interface Recources/Edit_Driver_Header.png");
            this.Controls.Add(Edit_Driver_Header);
        }
    }
}
