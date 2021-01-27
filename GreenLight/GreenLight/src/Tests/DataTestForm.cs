using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight.src.Data_Collection
{
    public partial class DataTestForm : Form
    {
        DataScreen dataScreen;

        public DataTestForm()
        {
            this.Size = new Size(1000, 1000);

            dataScreen = new DataScreen(this);

            this.dataScreen.Initialize();

            DataCollector collector = this.dataScreen.getCollector();

            this.dataScreen.Activate();

            this.Paint += draw;
        }

        private void draw(object sender, PaintEventArgs e)
        {
            
        }
    }
}
