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
    /* This is the SimulationDataMenu class, it is a user control that shows a timer keeping track of the time lapsed in the simulation. 
       This class contains methods to start, stop, reset and set the timer. */
    public partial class SimulationDataMenu : UserControl
    {
        private FontFamily dosisFontFamily;
        public Label Time;
        public int Multiplier = 1;
        public TimeSpan Last_Simulation_time;
        public TimeSpan Simulation_time;
        public Stopwatch stopWatch = new Stopwatch();
        System.Windows.Forms.Timer Timer;

        public SimulationDataMenu(int _submenuwidth, Form _form, int _height, FontFamily _dosisfontfamily)
        {
            this.Location = new Point(0, 0);
            this.dosisFontFamily = _dosisfontfamily;
            this.BackColor = Color.DarkGray;
            this.Size = new Size(_form.Width - _submenuwidth, 60);    
            Last_Simulation_time = new TimeSpan(00, 00, 00);
            Initialize();
        }

        public void AdjustSize(Form _form, int _submenuwidth, int _height)
        {
            this.Location = new Point(0, 0);
            this.Size = new Size(_form.Width - _submenuwidth, _height);
            this.Controls.Clear();
            Initialize();
        }

        public void StartTimer()
        {
            stopWatch.Start();
            this.Timer = new System.Windows.Forms.Timer();
            this.Timer.Interval = 1;
            this.Timer.Tick += new EventHandler(SetTime);
            this.Timer.Enabled = true;
            GPS GPS = new GPS();
        }

        public void Stop_timer()
        {
            stopWatch.Stop();
        }

        public void ResetTimer()
        {
            stopWatch.Reset();
            Last_Simulation_time = new TimeSpan(00, 00, 00);
            Simulation_time = new TimeSpan(00, 00, 00);
        }
        public void ValueChanged(int _multiplier)
        {
            Multiplier = (int)_multiplier;
            Last_Simulation_time = Simulation_time;

            General_Form.Main.SimulationScreen.Simulator.ChangeSimIntervalTimer(_multiplier);
            if (stopWatch.IsRunning)
                stopWatch.Restart();
            else stopWatch.Reset();
        }

        private void SetTime(object o, EventArgs EA)
        {
            TimeSpan Time_elapsed = TimeSpan.FromTicks(stopWatch.ElapsedTicks * Multiplier);
            Simulation_time = Last_Simulation_time.Add(Time_elapsed);
            Time.Text = (int)Simulation_time.Hours + ":" + (int)Simulation_time.Minutes +  ":" + (int)Simulation_time.Seconds ;
        }

        private void Initialize()
        {
            Dictionary<string, int> menu = UserControls.Config.simDataMenu;

            Label Time_label = new Label();
            Time_label.Text = "Simulation Time:";
            Time_label.ForeColor = Color.White;
            Time_label.Location = new Point(menu["timeLabelX"], menu["timeLabelY"]);
            Time_label.Size = new Size(menu["timeLabelSizeX"], menu["timeLabelSizeY"]);
            Time_label.Font = new Font(dosisFontFamily, 15, FontStyle.Bold);
            this.Controls.Add(Time_label);

            Time = new Label();
            Time.Text = "0:0:0";
            Time.ForeColor = Color.White;
            Time.Font = new Font(dosisFontFamily, 15, FontStyle.Bold);
            Time.Location = new Point(menu["timeX"], menu["timeY"]);
            Time.Size = new Size(menu["timeSizeX"], menu["timeSizeY"]);
            this.Controls.Add(Time);
        }
    }
}
