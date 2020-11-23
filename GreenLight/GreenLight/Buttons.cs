using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    class Buttons : PictureBox
    {
        public Buttons(Size Button_size, Point Location, string FilePath, string FilePath_on_hover) 
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath_on_hover);};
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath);};
        }
    }
}
