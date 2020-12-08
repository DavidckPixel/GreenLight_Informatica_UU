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
        public Simulation_sub_vehicle_menu(int Menu_width, General_form General_form, FontFamily Dosis_font_family)
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
            Selection_box.Location = new Point(3, 35);
            this.Controls.Add(Selection_box);

            Slider Max_speed = new Slider(new Point(25, 260), 0, 100, 10);
            this.Controls.Add(Max_speed);
            SliderText Max_speed_label = new SliderText(Dosis_font_family, new Point(25, 240), "Max speed:");
            this.Controls.Add(Max_speed_label);
            SliderText Max_speed_Value = new SliderText(Dosis_font_family, new Point(125, 240), Max_speed.Value.ToString() + " km/h");
            this.Controls.Add(Max_speed_Value);
            Max_speed.ValueChanged += (object o, EventArgs EA) => { Max_speed_Value.Text = Max_speed.Value.ToString() + " km/h"; };

            Slider Length = new Slider(new Point(25, 220), 0, 100, 10);
            this.Controls.Add(Length);
            SliderText Length_label = new SliderText(Dosis_font_family, new Point(25, 200), "Length:");
            this.Controls.Add(Length_label);
            SliderText Length_Value = new SliderText(Dosis_font_family, new Point(125, 200), Length.Value.ToString() + " m");
            this.Controls.Add(Length_Value);
            Length.ValueChanged += (object o, EventArgs EA) => { Length_Value.Text = Length.Value.ToString() + " m"; };

            Slider Acceleration = new Slider(new Point(25, 180), 0, 100, 10);
            this.Controls.Add(Acceleration);
            SliderText Acceleration_label = new SliderText(Dosis_font_family, new Point(25, 160), "Acceleration:");
            this.Controls.Add(Acceleration_label);
            SliderText Acceleration_Value = new SliderText(Dosis_font_family, new Point(125, 160), Acceleration.Value.ToString() + " m/s^2");
            this.Controls.Add(Acceleration_Value);
            Acceleration.ValueChanged += (object o, EventArgs EA) => { Acceleration_Value.Text = Acceleration.Value.ToString() + " m/s^2"; };

            Slider Weight = new Slider(new Point(25, 140), 0, 100, 10);
            this.Controls.Add(Weight);
            SliderText Weight_label = new SliderText(Dosis_font_family, new Point(25, 120), "Weight:");
            this.Controls.Add(Weight_label);
            SliderText Weight_Value = new SliderText(Dosis_font_family, new Point(125, 160), Weight.Value.ToString() + " kg");
            this.Controls.Add(Weight_Value);
            Weight.ValueChanged += (object o, EventArgs EA) => { Weight_Value.Text = Weight.Value.ToString() + " kg"; };

            Slider Occurunce = new Slider(new Point(25, 100), 0, 100, 10);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(25, 80), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(125, 80), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => { Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; };

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(150, 30),
               new Point(50, 5), "../../User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);

        }
    }
}
