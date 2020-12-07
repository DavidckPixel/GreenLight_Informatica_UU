using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace GreenLight
{
    public partial class General_form : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(
                int left,
                int top,
                int right,
                int bottom,
                int width,
                int height
                );

        Start_sub_menu SSM;
        Start_main_menu SMM;
        Build_sub_menu BSM;
        Build_main_menu BMM;
        Simulation_sub_menu SimSM;
        Simulation_main_menu SimMM;
        Simulation_sub_weather_menu SimSWM;
        Simulation_sub_vehicle_menu SimSVM;
        Simulation_sub_driver_menu SimSDM;
        Elements_sub_buildings_menu ElemSBM;
        Elements_sub_lights_menu ElemSLM;
        Elements_sub_roads_menu ElemSRM;
        Elements_sub_signs_menu ElemSSM;



        int Sub_menu_width = 250;
        FontFamily Dosis_font_family;
        string[] Recent_projects = new string[3] {"project 1", "project 2", "project 3" };

        public General_form()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 600);
            Refresh_region(this);
            this.MinimumSize = new Size(800, 400);
            this.Icon = new Icon("../../User Interface Recources/Logo.ico");

            PrivateFontCollection Font_collection = new PrivateFontCollection();
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            Dosis_font_family = Font_collection.Families[0];

            this.SizeChanged += (object o, EventArgs EA) => { Size_adjust(); };

            SSM = new Start_sub_menu(Sub_menu_width, this, Dosis_font_family, Recent_projects);
            SMM = new Start_main_menu(this.Width - Sub_menu_width, this, Dosis_font_family);

            BSM = new Build_sub_menu(Sub_menu_width, this, Dosis_font_family);
            BMM = new Build_main_menu(this.Width - Sub_menu_width, this, Dosis_font_family);

            SimSM = new Simulation_sub_menu(Sub_menu_width, this, Dosis_font_family);
            SimMM = new Simulation_main_menu(this.Width - Sub_menu_width, this, Dosis_font_family);
            SimSWM = new Simulation_sub_weather_menu(Sub_menu_width, this, Dosis_font_family);
            SimSVM = new Simulation_sub_vehicle_menu(Sub_menu_width, this, Dosis_font_family);
            SimSDM = new Simulation_sub_driver_menu(Sub_menu_width, this, Dosis_font_family);

            ElemSBM = new Elements_sub_buildings_menu(Sub_menu_width, this, Dosis_font_family);
            ElemSLM = new Elements_sub_lights_menu(Sub_menu_width, this, Dosis_font_family);
            ElemSRM = new Elements_sub_roads_menu(Sub_menu_width, this, Dosis_font_family);
            ElemSSM = new Elements_sub_signs_menu(Sub_menu_width, this, Dosis_font_family);

            this.Controls.Add(SMM);
            this.Controls.Add(SSM);
            this.Controls.Add(BMM);
            this.Controls.Add(BSM);
            this.Controls.Add(SimMM);
            this.Controls.Add(SimSM);
            this.Controls.Add(SimSWM);
            this.Controls.Add(SimSVM);
            this.Controls.Add(SimSDM);
            this.Controls.Add(ElemSBM);
            this.Controls.Add(ElemSLM);
            this.Controls.Add(ElemSRM);
            this.Controls.Add(ElemSSM);

            Hide_all_menus();
            Menu_to_start();
        }

        public void Refresh_region(General_form General_form)
        {
            if (this.WindowState == FormWindowState.Normal)
                General_form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.ClientSize.Width, this.ClientSize.Height, 50, 50));
            else General_form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.ClientSize.Width, this.ClientSize.Height, 0, 0));
        }
        public void Label_click(string Text)
        {
            switch (Text)
            {
                case "About":
                    System.Diagnostics.Process.Start("https://github.com/DavidckPixel/GreenLight_Informatica_UU");
                    break;
                case "Start simulation":
                    Menu_to_simulation();
                    break;
                case "Home":
                    Menu_to_start();
                    break;

                case "Save":
                    break;
            }
        }
        public void Size_adjust()
        {
            SMM.Size_adjust(this, Sub_menu_width);
            SSM.Size_adjust(this, Sub_menu_width, Dosis_font_family, Recent_projects);
            BMM.Size_adjust(this, Sub_menu_width);
            BSM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            SimMM.Size_adjust(this, Sub_menu_width);
            SimSM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            SimSWM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            SimSVM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            SimSDM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            ElemSRM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            ElemSLM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            ElemSBM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
            ElemSSM.Size_adjust(this, Sub_menu_width, Dosis_font_family);
        }
        public void Open(string File_name)
        {
            StreamReader Open = new StreamReader(File_name);
        }
        public void Menu_to_start()
        {
            Hide_all_menus();
            SSM.Show();
            SMM.Show();
        }
        public void Menu_to_build()
        {
            Hide_all_menus();
            BSM.Show();
            BMM.Show();
        }
        public void Menu_to_simulation()
        {
            Hide_all_menus();
            SimSM.Show();
            SimMM.Show();
        }
        public void Menu_to_simulation_weather()
        {
            SimSVM.Hide();
            SimSDM.Hide();
            SimSWM.Show();
            SimSWM.BringToFront();
        }
        public void Menu_to_simulation_vehicle()
        {
            SimSWM.Hide();
            SimSDM.Hide();
            SimSVM.Show();
            SimSVM.BringToFront();
        }
        public void Menu_to_simulation_driver()
        {
            SimSWM.Hide();
            SimSVM.Hide();
            SimSDM.Show();
            SimSDM.BringToFront();
        }
        public void Menu_to_roads()
        {
            ElemSLM.Hide();
            ElemSBM.Hide();
            ElemSSM.Hide();
            ElemSRM.Show();
            ElemSRM.BringToFront();
        }
        public void Menu_to_signs()
        {
            ElemSLM.Hide();
            ElemSBM.Hide();
            ElemSRM.Hide();
            ElemSSM.Show();
            ElemSSM.BringToFront();
        }
        public void Menu_to_lights()
        { 
            ElemSBM.Hide();
            ElemSSM.Hide();
            ElemSRM.Hide();
            ElemSLM.Show();
            ElemSLM.BringToFront();
        }
        public void Menu_to_buildings()
        {
            ElemSLM.Hide();
            ElemSSM.Hide();
            ElemSRM.Hide();
            ElemSBM.Show();
            ElemSBM.BringToFront();
        }
        private void Hide_all_menus()
        {
            SSM.Hide();
            SMM.Hide();
            BSM.Hide();
            BMM.Hide();
            SimSM.Hide();
            SimMM.Hide();
            SimSWM.Hide();
            SimSVM.Hide();
            SimSDM.Hide();
            ElemSLM.Hide();
            ElemSSM.Hide();
            ElemSRM.Hide();
            ElemSBM.Hide();
        }
    }
}
