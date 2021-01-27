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
    public partial class SimulationDataMenu : UserControl
    {
        FontFamily Dosis_font_family;
        Label time;
        int _multiplier = 1;
        TimeSpan Last_Simulation_time;
        TimeSpan Simulation_time;
        public SimulationDataMenu(int Sub_menu_width, Form Form, int Height, FontFamily Dosis_font_family_in)
        {
            this.Location = new Point(0, 0);
            Dosis_font_family = Dosis_font_family_in;
            this.BackColor = Color.DarkGray;
            this.Size = new Size(Form.Width - Sub_menu_width, 60);
            Initialize();
            Last_Simulation_time = new TimeSpan(00, 00, 00);
        }

        public void Size_adjust(Form Form, int Sub_menu_width, int Height)
        {
            this.Location = new Point(0, 0);
            this.Size = new Size(Form.Width - Sub_menu_width, Height);
            this.Controls.Clear();
            Initialize();
        }

        System.Windows.Forms.Timer Timer;
        public Stopwatch Stopwatch = new Stopwatch();
        public void Start_timer()
        {
            Stopwatch.Start();
            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 1;
            Timer.Tick += new EventHandler(Set_time);
            Timer.Enabled = true;
            GPS bo = new GPS();
        }

        public void Stop_timer()
        {
            Stopwatch.Stop();
        }

        public void Reset_timer()
        {
            Stopwatch.Reset();
            Last_Simulation_time = new TimeSpan(00, 00, 00);
            Simulation_time = new TimeSpan(00, 00, 00);
        }
        public void Value_changed(int multiplier)
        {
            _multiplier = (int)multiplier;
            Last_Simulation_time = Simulation_time;
            General_Form.Main.SimulationScreen.Simulator.ChangeSimIntervalTimer(multiplier);
            if (Stopwatch.IsRunning)
                Stopwatch.Restart();
            else Stopwatch.Reset();
        }

        private void Set_time(object o, EventArgs EA)
        {
            TimeSpan Time_elapsed = TimeSpan.FromTicks(Stopwatch.ElapsedTicks * _multiplier);
            Simulation_time = Last_Simulation_time.Add(Time_elapsed);
            time.Text = (int)Simulation_time.Hours + ":" + (int)Simulation_time.Minutes +  ":" + (int)Simulation_time.Seconds ;
        }

        private void Initialize()
        {
            Dictionary<string, int> menu = UserControls.Config.simDataMenu;

            Label Time_label = new Label();
            Time_label.Text = "Simulation Time:";
            Time_label.ForeColor = Color.White;
            Time_label.Location = new Point(menu["timeLabelX"], menu["timeLabelY"]);
            Time_label.Size = new Size(menu["timeLabelSizeX"], menu["timeLabelSizeY"]);
            Time_label.Font = new Font(Dosis_font_family, 15, FontStyle.Bold);
            this.Controls.Add(Time_label);

            time = new Label();
            time.Text = "0:0:0";
            time.ForeColor = Color.White;
            time.Font = new Font(Dosis_font_family, 15, FontStyle.Bold);
            time.Location = new Point(menu["timeX"], menu["timeY"]);
            time.Size = new Size(menu["timeSizeX"], menu["timeSizeY"]);
            this.Controls.Add(time);
        }
    }
}
