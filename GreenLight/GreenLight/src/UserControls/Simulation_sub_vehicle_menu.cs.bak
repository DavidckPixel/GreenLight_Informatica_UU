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
            if (Form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(User_Controls.Config.standardSubMenu["selectionBoxMaxX"], User_Controls.Config.standardSubMenu["selectionBoxMaxY"]);
            else Selection_box.Location = new Point(User_Controls.Config.standardSubMenu["selectionBoxX"], User_Controls.Config.standardSubMenu["selectionBoxY"]);

            this.Controls.Add(Selection_box);

            Dictionary<string, int> vehiclemenu = User_Controls.Config.simVehicle;


            Slider Cw = new Slider(new Point(_sliderX, _startY + 2 * _diffY), vehiclemenu["surfaceMin"], vehiclemenu["surfaceMax"]);
            this.Controls.Add(Cw);
            SliderText Cw_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY + 2 * _diffY), "Drag Co:");
            this.Controls.Add(Cw_label);
            SliderText Cw_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY + 2 * _diffY), (Cw.Value / 10 ).ToString() + " ");
            this.Controls.Add(Cw_Value);
            Cw.ValueChanged += (object o, EventArgs EA) => { Cw_Value.Text = (((double)(Cw.Value)) / 10).ToString() + " "; };

            Slider Surface = new Slider(new Point(_sliderX, _startY + _diffY), vehiclemenu["surfaceMin"], vehiclemenu["surfaceMax"]);
            this.Controls.Add(Surface);
            SliderText Surface_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY + 1 * _diffY), "Frontal Surface:");
            this.Controls.Add(Surface_label);
            SliderText Surface_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY + 1* _diffY), (Surface.Value / 10).ToString() + " m^2");
            this.Controls.Add(Surface_Value);
            Surface.ValueChanged += (object o, EventArgs EA) => { Surface_Value.Text = (((double)(Surface.Value)) / 10).ToString() + " m^2"; };

            Slider Max_speed = new Slider(new Point(_sliderX, _startY), vehiclemenu["topSpeedMin"], vehiclemenu["topSpeedMax"]);
            this.Controls.Add(Max_speed);
            SliderText Max_speed_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY), "Topspeed:");
            this.Controls.Add(Max_speed_label);
            SliderText Max_speed_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY), Max_speed.Value.ToString() + " km/h");
            this.Controls.Add(Max_speed_Value);
            Max_speed.ValueChanged += (object o, EventArgs EA) => { Max_speed_Value.Text = Max_speed.Value.ToString() + " km/h"; };

            Slider Length = new Slider(new Point(_sliderX, _startY - _diffY), vehiclemenu["lengthMin"], vehiclemenu["lengthMax"]);
            this.Controls.Add(Length);
            SliderText Length_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 1 * _diffY), "Length:");
            this.Controls.Add(Length_label);
            SliderText Length_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 1 * _diffY), (Length.Value / 10).ToString() + " m");
            this.Controls.Add(Length_Value);
            Length.ValueChanged += (object o, EventArgs EA) => { Length_Value.Text = (((double)(Length.Value)) / 10).ToString() + " m"; };

            Slider HorsePower = new Slider(new Point(_sliderX, _startY - 2* _diffY), vehiclemenu["horsepwrMin"], vehiclemenu["horsepwrMax"]);
            this.Controls.Add(HorsePower);
            SliderText HorsePower_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 2 * _diffY), "Horsepower:");
            this.Controls.Add(HorsePower_label);
            SliderText HorsePower_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 2 * _diffY), HorsePower.Value.ToString() + " hp");
            this.Controls.Add(HorsePower_Value);
            HorsePower.ValueChanged += (object o, EventArgs EA) => { HorsePower_Value.Text = HorsePower.Value.ToString() + " hp"; };

            Slider Weight = new Slider(new Point(_sliderX, _startY - 3 * _diffY), vehiclemenu["weightMin"], vehiclemenu["weightMax"]);
            this.Controls.Add(Weight);
            SliderText Weight_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 3 * _diffY), "Weight:");
            this.Controls.Add(Weight_label);
            SliderText Weight_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 3 * _diffY), Weight.Value.ToString() + " kg");
            this.Controls.Add(Weight_Value);
            Weight.ValueChanged += (object o, EventArgs EA) => { Weight_Value.Text = Weight.Value.ToString() + " kg"; };

            Slider Occurunce = new Slider(new Point(_sliderX, _startY - 4 * _diffY), vehiclemenu["occurenceMin"], vehiclemenu["occurenceMax"]);
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