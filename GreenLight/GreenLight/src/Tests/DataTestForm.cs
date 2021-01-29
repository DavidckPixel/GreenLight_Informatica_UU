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
        //This class is used for testing the data form on which the data of all the vehicles will be shown.
        //This includes data like average speed, current speed, braking ticks, etc.

        DataScreen dataScreen;

        //This initializes the testform and its data
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
