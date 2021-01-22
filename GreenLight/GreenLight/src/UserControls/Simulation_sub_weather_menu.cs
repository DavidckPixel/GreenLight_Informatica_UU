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
            Dictionary<string, int> menu = User_Controls.Config.simElementsMenu;
            int _sliderX = menu["sliderX"];
            int _start = menu["sliderStart"];
            int _diff = menu["sliderDiffY"];
            int _textX = menu["textX"];
            int _startText = menu["textStart"];

            ListBox worlds = new ListBox();
            WorldConfig.physics.ForEach(x => worlds.Items.Add(x));
            worlds.Location = new Point(10, 10);
            worlds.Size = new Size(100, 20);
            worlds.GotFocus += (object o, EventArgs ea) =>
            {
                worlds.Items.Clear();
                WorldConfig.physics.ForEach(x => worlds.Items.Add(x));
            };
            this.Controls.Add(worlds);

            CurvedButtons Editbutton = new CurvedButtons(new Size(70, 30), new Point(10, 50), 25, "../../User Interface Recources/Custom_Small_Button.png", "Edit", DrawData.Dosis_font_family, null, this.BackColor);
            CurvedButtons Newbutton = new CurvedButtons(new Size(70, 30), new Point(100, 50), 25, "../../User Interface Recources/Custom_Small_Button.png", "New", DrawData.Dosis_font_family, null, this.BackColor);

            Editbutton.Click += (object o, EventArgs ea) => { General_Form.Main.SimulationScreen.Simulator.worldController.EditWorld((World)worlds.SelectedItem); };
            Newbutton.Click += (object o, EventArgs ea) => { General_Form.Main.SimulationScreen.Simulator.worldController.CreateNewWorld(); };

            this.Controls.Add(Editbutton);
            this.Controls.Add(Newbutton);

            Slider carSpawn = new Slider(new Point(10, 130), 0, 100);
            this.Controls.Add(carSpawn);

            SliderText carSpawn_label = new SliderText(DrawData.Dosis_font_family, new Point(10, 100), "Car Spawn:");
            this.Controls.Add(carSpawn_label);

            SliderText carSpawn_value = new SliderText(DrawData.Dosis_font_family, new Point(120, 100), carSpawn.Value.ToString() + " %");
            this.Controls.Add(carSpawn_value);

            carSpawn.ValueChanged += (object o, EventArgs EA) => { carSpawn_value.Text = carSpawn.Value.ToString() + " %"; };


            /*


            Slider Slippery = new Slider(new Point(_sliderX, _start + _diff * 3), 0, 100);
            this.Controls.Add(Slippery);

            SliderText Slippery_label = new SliderText(Dosis_font_family, new Point(_sliderX, _startText + _diff * 3), "Slipperiness:"); //startText //textX
            this.Controls.Add(Slippery_label);

            SliderText Slippery_Value = new SliderText(Dosis_font_family, new Point(_textX, _startText + _diff * 3), Slippery.Value.ToString() + " %");
            this.Controls.Add(Slippery_Value);
            Slippery.ValueChanged += (object o, EventArgs EA) => { Slippery_Value.Text = Slippery.Value.ToString() + " %"; };

            Slider Sight = new Slider(new Point(_sliderX, _start + _diff * 2), 0, 100);
            this.Controls.Add(Sight);

            SliderText Sight_label = new SliderText(Dosis_font_family, new Point(_sliderX, _startText + _diff * 2), "Sight:");
            this.Controls.Add(Sight_label);

            SliderText Sight_Value = new SliderText(Dosis_font_family, new Point(_textX, _startText + _diff * 2), Sight.Value.ToString() + " m");
            this.Controls.Add(Sight_Value);
            Sight.ValueChanged += (object o, EventArgs EA) => { Sight_Value.Text = Sight.Value.ToString() + " m"; };

            Slider Snow = new Slider(new Point(_sliderX, _start + _diff), 0, 50);
            this.Controls.Add(Snow);

            SliderText Snow_label = new SliderText(Dosis_font_family, new Point(_sliderX, _startText + _diff), "Snow:");
            this.Controls.Add(Snow_label);

            SliderText Snow_Value = new SliderText(Dosis_font_family, new Point(_textX, _startText + _diff), (Snow.Value / 10).ToString() + " mm");
            this.Controls.Add(Snow_Value);
            Snow.ValueChanged += (object o, EventArgs EA) => { Snow_Value.Text = (((double)(Snow.Value)) / 10).ToString() + " mm"; };

            Slider Rainfall = new Slider(new Point(_sliderX, _start), 0, 100);
            this.Controls.Add(Rainfall);

            SliderText Rainfall_label = new SliderText(Dosis_font_family, new Point(_sliderX, _startText), "Rainfall:");
            this.Controls.Add(Rainfall_label);

            SliderText Rainfall_Value = new SliderText(Dosis_font_family, new Point(_textX, _startText), (Rainfall.Value / 10).ToString() + " mm/h");
            this.Controls.Add(Rainfall_Value);
            Rainfall.ValueChanged += (object o, EventArgs EA) => { Rainfall_Value.Text = (((double)(Rainfall.Value)) / 10).ToString() + " mm/h"; };

            CurvedButtons Weather_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../User Interface Recources/Weather_Header.png");
            this.Controls.Add(Weather_header);
            */
        }
    }
}
