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

        public Selection_box Selection_box;

        public Simulation_sub_vehicle_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - User_Controls.Config.simElementsMenu["menuY"] - User_Controls.Config.simElementsMenu["menuSizeY"]); //menuSizeY
            this.Location = new Point(Form.Width - Menu_width, User_Controls.Config.simElementsMenu["menuY"]);  //menuY
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
            int _startY = menu["sliderStartVehicleY"];
            int _diffY = menu["sliderDiffY"];
            int _textX = menu["sliderTextX"];
            int _textY = menu["startTextY"];

            List<string> _temp = VehicleController.getStringVehicleStats();

            Selection_box = new Selection_box(Form, Dosis_font_family, _temp);
            if (Form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(13, 35);
            else Selection_box.Location = new Point(3, 35);

            this.Controls.Add(Selection_box);


            Slider Max_speed = new Slider(new Point(_sliderX, _startY), 30, 300);
            this.Controls.Add(Max_speed);
            SliderText Max_speed_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY), "Max speed:");
            this.Controls.Add(Max_speed_label);
            SliderText Max_speed_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY), Max_speed.Value.ToString() + " km/h");
            this.Controls.Add(Max_speed_Value);
            Max_speed.ValueChanged += (object o, EventArgs EA) => { Max_speed_Value.Text = Max_speed.Value.ToString() + " km/h"; };

            Slider Length = new Slider(new Point(_sliderX, _startY - _diffY), 3, 12);
            this.Controls.Add(Length);
            SliderText Length_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 1 * _diffY), "Length:");
            this.Controls.Add(Length_label);
            SliderText Length_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 1 * _diffY), Length.Value.ToString() + " m");
            this.Controls.Add(Length_Value);
            Length.ValueChanged += (object o, EventArgs EA) => { Length_Value.Text = Length.Value.ToString() + " m"; };

            Slider Acceleration = new Slider(new Point(_sliderX, _startY - 2* _diffY), 2, 5);
            this.Controls.Add(Acceleration);
            SliderText Acceleration_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 2 * _diffY), "Acceleration:");
            this.Controls.Add(Acceleration_label);
            SliderText Acceleration_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 2 * _diffY), Acceleration.Value.ToString() + " m/s^2");
            this.Controls.Add(Acceleration_Value);
            Acceleration.ValueChanged += (object o, EventArgs EA) => { Acceleration_Value.Text = Acceleration.Value.ToString() + " m/s^2"; };

            Slider Weight = new Slider(new Point(_sliderX, _startY - 3 * _diffY), 0, 40000);
            this.Controls.Add(Weight);
            SliderText Weight_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 3 * _diffY), "Weight:");
            this.Controls.Add(Weight_label);
            SliderText Weight_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 3 * _diffY), Weight.Value.ToString() + " kg");
            this.Controls.Add(Weight_Value);
            Weight.ValueChanged += (object o, EventArgs EA) => { Weight_Value.Text = Weight.Value.ToString() + " kg"; };

            Slider Occurunce = new Slider(new Point(_sliderX, _startY - 4 * _diffY), 0, 100);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 4 * _diffY), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 4 * _diffY), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => { Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; };

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);



            /* Slider temp1 = new Slider(new Point(_sliderX, _start + _diff * 4), 0, 100); //sliderDiffY //sliderStart:100 / sliderX:25 //headerSizeX //headerSizeY //headerX //headerY
            this.Controls.Add(temp1);

            Slider temp2 = new Slider(new Point(_sliderX, _start + _diff * 3), 0, 100);
            this.Controls.Add(temp2);

            Slider temp3 = new Slider(new Point(_sliderX, _start + _diff * 2), 0, 100);
            this.Controls.Add(temp3);

            Slider temp4 = new Slider(new Point(_sliderX, _start + _diff), 0, 100);
            this.Controls.Add(temp4);

            Slider temp5 = new Slider(new Point(_sliderX, _start), 0, 100);
            this.Controls.Add(temp5);

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);*/

        }
    }
}
