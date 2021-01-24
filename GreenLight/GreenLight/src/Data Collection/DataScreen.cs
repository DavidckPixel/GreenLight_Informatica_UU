using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace GreenLight.src.Data_Collection
{
    class DataScreen : ScreenController
    {
        Form main;
        DataController dataController;

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
            this.main.Controls.Add(this.Screen);

            dataController = new DataController(this.Screen);

        }

        public override void Activate()
        {
            //General_Form.Main.UserInterface.Menu_to_simulation();
            Console.WriteLine("Activating DataScreen");
            this.Screen.Show();
            this.Screen.Invalidate();
            this.dataController.UpdateBrakeChart();
        }

        public override void DeActivate()
        {
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
