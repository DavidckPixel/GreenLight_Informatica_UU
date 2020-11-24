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

        public General_form()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 600);
            this.MinimumSize = new Size(800, 400);
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 50, 50));
            this.Icon = new Icon("../../User Interface Recources/Logo.ico");

            PrivateFontCollection Font_collection = new PrivateFontCollection();
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            FontFamily Dosis_font_family = Font_collection.Families[0];

            this.SizeChanged += (object o, EventArgs EA) => { Size_adjust(); };

            SSM = new Start_sub_menu(250, this, Dosis_font_family);
            SMM = new Start_main_menu(this.Width - 250, this, Dosis_font_family);

            BSM = new Build_sub_menu(250, this, Dosis_font_family);
            BMM = new Build_main_menu(this.Width - 250, this, Dosis_font_family);

            SimSM = new Simulation_sub_menu(250, this, Dosis_font_family);
            SimMM = new Simulation_main_menu(this.Width - 250, this, Dosis_font_family);

            this.Controls.Add(SMM);
            this.Controls.Add(SSM);
            this.Controls.Add(BMM);
            this.Controls.Add(BSM);
            this.Controls.Add(SimMM);
            this.Controls.Add(SimSM);
            Hide_all_menus();
            Menu_to_start();

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
        private void Hide_all_menus()
        {
            SSM.Hide();
            SMM.Hide();
            BSM.Hide();
            BMM.Hide();
            SimSM.Hide();
            SimMM.Hide();
        }
    }
}
