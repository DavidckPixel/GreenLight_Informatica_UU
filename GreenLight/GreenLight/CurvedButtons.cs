using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GreenLight
{
    class CurvedButtons : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath p = new GraphicsPath();
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Width, Height);
            int d = 100;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            p.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            p.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            this.Region = new Region(p);
            base.OnPaint(pe);
        }
        public CurvedButtons(Size Button_size, Point Location, string FilePath, string FilePath_on_hover)
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath_on_hover); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
        }
        public CurvedButtons(Size Button_size, Point Location, string FilePath, string FilePath_on_hover, int index)
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath_on_hover); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
        }
    }
}
