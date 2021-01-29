using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace GreenLight.src.Data_Collection
{
    //ScreenController for the dataScreen within the program, very similar to every other ScreenController

    public class DataScreen : ScreenController
    {
        Form main;
        public DataController dataController;
        
        public DataScreen(Form _main)
        {
            this.main = _main;

            this.Screen = new PictureBox();
            this.Screen.Width = this.main.Width - 250;
            this.Screen.Height = this.main.Height;
            this.Screen.Location = new Point(0, 0);
            this.Screen.BackColor = Color.FromArgb(196, 196, 198);
            this.Screen.Paint += DrawPictureBox;

            this.main.SizeChanged += ChangeSize;

            dataController = new DataController(this.Screen);

            this.main.Controls.Add(this.Screen);
        }

        //When the dataScreen is loaded (activated), it will give the charts a big update and recalibrate them.

        public override void Activate()
        {
            General_Form.Main.UserInterface.Menu_to_data();
            Console.WriteLine("Activating DataScreen");
            this.Screen.Show();
            this.dataController.UpdateBrakeChart();
            this.dataController.UpdateBrakePerTickChart();
            this.Screen.Invalidate();
            this.Screen.BringToFront();
            Console.WriteLine("this.dataController.brakeChart.Location : " + this.dataController.brakeChart.Location);
            Console.WriteLine("IS VISABLE: " + this.dataController.averageSpeed.Visible);

            this.dataController.brakeChart.Show();
            this.dataController.averageSpeed.Show();

            this.Screen.Invalidate();
        }

        public override void DeActivate()
        {
            this.dataController.brakeChart.Hide();
            this.dataController.averageSpeed.Hide();

            if (this.Screen != null)
            {
                this.Screen.Hide();
            }
        }

        public override void Initialize()
        {
            dataController.Initialize();
        }

        public void DrawPictureBox(object o, PaintEventArgs pea)
        {
            Console.WriteLine("Drawing PictureBox from DataSCreen");
            this.dataController.DrawCharts(pea.Graphics);
        }

        public DataCollector getCollector()
        {
            return dataController.collector;
        }

        private void ChangeSize(object o, EventArgs ea)
        {
            Console.WriteLine("Size changed!!");
            this.Screen.Width = main.Width - 250;
            this.Screen.Height = main.Height;
            this.Screen.Invalidate();
        }
    }
}
