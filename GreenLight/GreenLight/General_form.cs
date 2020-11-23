using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            this.SizeChanged += (object o, EventArgs EA) => { Size_adjust(); };

            SSM = new Start_sub_menu(this.Width, this.Height, 250, this);
            SMM = new Start_main_menu(this.Width - 250, this.Height, this);

            BSM = new Build_sub_menu(this.Width, this.Height, 250, this);
            BMM = new Build_main_menu(this.Width - 250, this.Height, this);

            SimSM = new Simulation_sub_menu(this.Width, this.Height, 250, this);
            SimMM = new Simulation_main_menu(this.Width - 250, this.Height, this);

            this.Controls.Add(SMM);
            this.Controls.Add(SSM);
            this.Controls.Add(BMM);
            this.Controls.Add(BSM);
            this.Controls.Add(SimMM);
            this.Controls.Add(SimSM);

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
