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
    public partial class Simulation_sub_vehicle_menu : UserControl
    {
        public Simulation_sub_vehicle_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
        {

            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Sub_menu_width, General_form.Height - 230 - 135);
            this.Location = new Point(General_form.Width - Sub_menu_width, 230);
            this.AutoScroll = true;
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(Sub_menu_width, General_form.Height - 230 - 135);
                this.Location = new Point(General_form.Width - Sub_menu_width, 230);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width, Dosis_font_family);
            };
        }

        private void Initialize(General_form General_form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Selection_box Selection_box = new Selection_box(General_form, Dosis_font_family);
            if (General_form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(13,35);
            else Selection_box.Location = new Point(3, 35);

            this.Controls.Add(Selection_box);

            Slider Max_speed = new Slider(new Point(25, 360), 30, 300);
            this.Controls.Add(Max_speed);
            SliderText Max_speed_label = new SliderText(Dosis_font_family, new Point(25, 340), "Max speed:");
            this.Controls.Add(Max_speed_label);
            SliderText Max_speed_Value = new SliderText(Dosis_font_family, new Point(125, 340), Max_speed.Value.ToString() + " km/h");
            this.Controls.Add(Max_speed_Value);
            Max_speed.ValueChanged += (object o, EventArgs EA) => { Max_speed_Value.Text = Max_speed.Value.ToString() + " km/h"; };

            Slider Length = new Slider(new Point(25, 320), 3, 12);
            this.Controls.Add(Length);
            SliderText Length_label = new SliderText(Dosis_font_family, new Point(25, 300), "Length:");
            this.Controls.Add(Length_label);
            SliderText Length_Value = new SliderText(Dosis_font_family, new Point(125, 300), Length.Value.ToString() + " m");
            this.Controls.Add(Length_Value);
            Length.ValueChanged += (object o, EventArgs EA) => { Length_Value.Text = Length.Value.ToString() + " m"; };

            Slider Acceleration = new Slider(new Point(25, 280), 2, 5);
            this.Controls.Add(Acceleration);
            SliderText Acceleration_label = new SliderText(Dosis_font_family, new Point(25, 260), "Acceleration:");
            this.Controls.Add(Acceleration_label);
            SliderText Acceleration_Value = new SliderText(Dosis_font_family, new Point(125, 260), Acceleration.Value.ToString() + " m/s^2");
            this.Controls.Add(Acceleration_Value);
            Acceleration.ValueChanged += (object o, EventArgs EA) => { Acceleration_Value.Text = Acceleration.Value.ToString() + " m/s^2"; };

            Slider Weight = new Slider(new Point(25, 240), 0, 40000);
            this.Controls.Add(Weight);
            SliderText Weight_label = new SliderText(Dosis_font_family, new Point(25, 220), "Weight:");
            this.Controls.Add(Weight_label);
            SliderText Weight_Value = new SliderText(Dosis_font_family, new Point(125, 220), Weight.Value.ToString() + " kg");
            this.Controls.Add(Weight_Value);
            Weight.ValueChanged += (object o, EventArgs EA) => { Weight_Value.Text = Weight.Value.ToString() + " kg"; };

            Slider Occurunce = new Slider(new Point(25, 200), 0, 100);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(25, 180), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(125, 180), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => { Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; };

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(150, 30),
               new Point(50, 5), "../../User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);

        }
    }
}
