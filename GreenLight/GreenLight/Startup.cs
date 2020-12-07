using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    class Startup : Form
    {
        public Startup()
        {
            new Vehicle("Auto", 1353, 4.77, 100, 4223, 0, 0, 0.35, 2.65);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Startup
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Startup";
            this.Load += new System.EventHandler(this.Startup_Load);
            this.ResumeLayout(false);

        }

        private void Startup_Load(object sender, EventArgs e)
        {

        }
    }
}
