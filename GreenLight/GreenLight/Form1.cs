using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Log.Write("Test");

            AbstractRoad test = new StraightRoad(new Point(100, 100), new Point(80, 20), 1, 'N');
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
