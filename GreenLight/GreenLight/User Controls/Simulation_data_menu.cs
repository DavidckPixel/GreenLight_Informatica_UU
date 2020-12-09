using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace GreenLight
{
    public partial class Simulation_data_menu : UserControl
    {
        FontFamily Dosis_font_family;

        Label time;
        public Simulation_data_menu(int Sub_menu_width, General_form General_form, int Height, FontFamily Dosis_font_family_in)
        {
            Dosis_font_family = Dosis_font_family_in;
            this.BackColor = Color.DarkGray;
            this.Size = new Size(General_form.Width - Sub_menu_width, Height);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(General_form.Width - Sub_menu_width, Height);
                this.Controls.Clear();
                Initialize();
            };
            Initialize();
        }

        System.Windows.Forms.Timer Timer;
        Stopwatch Stopwatch = new Stopwatch();
        public void Start_timer()
        {
            Stopwatch.Start();
            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 1;
            Timer.Tick += new EventHandler(Set_time);
            Timer.Enabled = true;
        }

        public void Stop_timer()
        {
            Stopwatch.Stop();
        }

        public void Reset_timer()
        {
            Stopwatch.Restart();
        }

        private void Set_time(object o, EventArgs EA)
        {
            TimeSpan Time_elapsed = Stopwatch.Elapsed;
            time.Text = (int)Time_elapsed.Hours + ":" + (int)Time_elapsed.Minutes + ":" + (int)Time_elapsed.Seconds;
        }

        private void Initialize()
        {
            Label Time_label = new Label();
            Time_label.Text = "Simulation Time:";
            Time_label.ForeColor = Color.White;
            Time_label.Location = new Point(5, 0);
            Time_label.Size = new Size(148, 30);
            Time_label.Font = new Font(Dosis_font_family, 15, FontStyle.Bold);
            this.Controls.Add(Time_label);

            time = new Label();
            time.Text = "0:0:0";
            time.ForeColor = Color.White;
            time.Font = new Font(Dosis_font_family, 15, FontStyle.Bold);
            time.Location = new Point(160, 0);
            time.Size = new Size(90, 30);
            this.Controls.Add(time);
        }
    }
}
