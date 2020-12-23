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
    public partial class Simulation_sub_weather_menu : UserControl
    {
        public Simulation_sub_weather_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - 230 - 135);
            this.Location = new Point(Form.Width - Menu_width, 230);
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }

        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - 230 - 135);
            this.Location = new Point(Form.Width - Sub_menu_width, 230);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);

        }

        //Cleaner but General_form should be just form
        /*public Simulation_sub_weather_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
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
            };*/
        

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
           

            Slider Slippery = new Slider(new Point(25, 180), 0, 100);
            this.Controls.Add(Slippery);

            SliderText Slippery_label = new SliderText(Dosis_font_family, new Point(25, 160), "Slipperiness:");
            this.Controls.Add(Slippery_label);

            SliderText Slippery_Value = new SliderText(Dosis_font_family, new Point(125, 160), Slippery.Value.ToString() + " %");
            this.Controls.Add(Slippery_Value);
            Slippery.ValueChanged += (object o, EventArgs EA) => { Slippery_Value.Text = Slippery.Value.ToString() + " %"; };

            Slider Sight = new Slider(new Point(25, 140), 0, 100);
            this.Controls.Add(Sight);

            SliderText Sight_label = new SliderText(Dosis_font_family, new Point(25, 120), "Sight:");
            this.Controls.Add(Sight_label);

            SliderText Sight_Value = new SliderText(Dosis_font_family, new Point(125, 120), Sight.Value.ToString() + " m");
            this.Controls.Add(Sight_Value);
            Sight.ValueChanged += (object o, EventArgs EA) => { Sight_Value.Text = Sight.Value.ToString() + " m"; };

            Slider Snow = new Slider(new Point(25, 100), 0, 5);
            this.Controls.Add(Snow);

            SliderText Snow_label = new SliderText(Dosis_font_family, new Point(25, 80), "Snow:");
            this.Controls.Add(Snow_label);

            SliderText Snow_Value = new SliderText(Dosis_font_family, new Point(125, 80), Snow.Value.ToString() + " mm");
            this.Controls.Add(Snow_Value);
            Snow.ValueChanged += (object o, EventArgs EA) => { Snow_Value.Text = Snow.Value.ToString() + " mm"; };

            Slider Rainfall = new Slider(new Point(25, 60), 0, 10);
            this.Controls.Add(Rainfall);

            SliderText Rainfall_label = new SliderText(Dosis_font_family, new Point(25, 40), "Rainfall:");
            this.Controls.Add(Rainfall_label);

            SliderText Rainfall_Value = new SliderText(Dosis_font_family, new Point(125, 40), Rainfall.Value.ToString() + " mm/h");
            this.Controls.Add(Rainfall_Value);
            Rainfall.ValueChanged += (object o, EventArgs EA) => { Rainfall_Value.Text = Rainfall.Value.ToString() + " mm/h"; };

            CurvedButtons Weather_header = new CurvedButtons(new Size(150, 30),
               new Point(50, 5), "../../User Interface Recources/Weather_Header.png");
            this.Controls.Add(Weather_header);
        }
    }
}
