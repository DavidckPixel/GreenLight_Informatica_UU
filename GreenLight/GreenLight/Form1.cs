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

            GridController test = new GridController();
            test.CreateGridPoints();
            foreach(Gridpoint x in test.Gridpoints)
            {
                Console.WriteLine(x);
            }

            this.Paint += test.DrawGridPoints;
            this.MouseClick += test.OnClick;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
