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
        public Simulation_sub_driver_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, General_form.Height - 230 - 135);
            this.Location = new Point(General_form.Width - Menu_width, 230);
            this.AutoScroll = true;
            Initialize(General_form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, General_form.Height - 230 - 135);
            this.Location = new Point(General_form.Width - Sub_menu_width, 230);
            this.Controls.Clear();
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Selection_box Selection_box = new Selection_box(General_form, Dosis_font_family);
            if (General_form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(13, 35);
            else Selection_box.Location = new Point(3, 35);
            this.Controls.Add(Selection_box);

            Slider Reaction_time = new Slider(new Point(25, 360), 0, 100, 10);
            this.Controls.Add(Reaction_time);
            SliderText Reaction_time_label = new SliderText(Dosis_font_family, new Point(25, 340), "Reaction time:");
            this.Controls.Add(Reaction_time_label);
            SliderText Reaction_time_Value = new SliderText(Dosis_font_family, new Point(125, 340), Reaction_time.Value.ToString() + " s");
            this.Controls.Add(Reaction_time_Value);
            Reaction_time.ValueChanged += (object o, EventArgs EA) => { Reaction_time_Value.Text = Reaction_time.Value.ToString() + " s"; };

            Slider Follow_interval = new Slider(new Point(25, 320), 0, 100, 10);
            this.Controls.Add(Follow_interval);
            SliderText Follow_interval_label = new SliderText(Dosis_font_family, new Point(25, 300), "Follow interval:");
            this.Controls.Add(Follow_interval_label);
            SliderText Follow_interval_Value = new SliderText(Dosis_font_family, new Point(125, 300), Follow_interval.Value.ToString() + " s");
            this.Controls.Add(Follow_interval_Value);
            Follow_interval.ValueChanged += (object o, EventArgs EA) => { Follow_interval_Value.Text = Follow_interval.Value.ToString() + " s"; };

            Slider Speed_relative_to_limit = new Slider(new Point(25, 280), 0, 100, 10);
            this.Controls.Add(Speed_relative_to_limit);
            SliderText SRTL_label = new SliderText(Dosis_font_family, new Point(25, 260), "Speed relative to limit:");
            this.Controls.Add(SRTL_label);
            SliderText SRTL_Value = new SliderText(Dosis_font_family, new Point(125, 260), Speed_relative_to_limit.Value.ToString() + " km/h");
            this.Controls.Add(SRTL_Value);
            Speed_relative_to_limit.ValueChanged += (object o, EventArgs EA) => { SRTL_Value.Text = Speed_relative_to_limit.Value.ToString() + " km/h"; };

            Slider Rulebreaking = new Slider(new Point(25, 240), 0, 100, 10);
            this.Controls.Add(Rulebreaking);
            SliderText Rulebreaking_label = new SliderText(Dosis_font_family, new Point(25, 220), "Rulebreaking:");
            this.Controls.Add(Rulebreaking_label);
            SliderText Rulebreaking_Value = new SliderText(Dosis_font_family, new Point(125, 220), Rulebreaking.Value.ToString() + " %");
            this.Controls.Add(Rulebreaking_Value);
            Rulebreaking.ValueChanged += (object o, EventArgs EA) => { Rulebreaking_Value.Text = Rulebreaking.Value.ToString() + " %"; };

            Slider Occurunce = new Slider(new Point(25, 200), 0, 100, 10);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(25, 180), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(125, 180), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => { Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; };

            CurvedButtons Edit_Driver_Header = new CurvedButtons(new Size(150, 30),
               new Point(50, 5), "../../User Interface Recources/Edit_Driver_Header.png");
            this.Controls.Add(Edit_Driver_Header);
        }
    }
}
