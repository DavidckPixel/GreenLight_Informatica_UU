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
    class RoundButtons : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddEllipse(0, 0, Size.Width-1, Size.Height-1);
            this.Region = new Region(p);
            base.OnPaint(pe);
        }
        //HelpButton
        public RoundButtons(Size Button_size, Point Location, string FilePath)
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 4) + "_On_Hover.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
        }
        
        public RoundButtons(Size s, Point p, string Color, General_form General_form)
        {
            this.Cursor = Cursors.Hand;
            this.Size = s;
            this.SizeMode = PictureBoxSizeMode.Zoom;

            switch (Color)
            {
                case "Green":
                    this.Location = new Point(p.X + 20, p.Y + 3);
                    this.Image = Image.FromFile("../../User Interface Recources/Logo_Minimalize.png");
                    this.Click += (object o, EventArgs EA) => { General_form.WindowState = FormWindowState.Minimized; };
                    break;
                case "Yellow":
                    this.Location = new Point(p.X + 72, p.Y + 3);
                    this.Image = Image.FromFile("../../User Interface Recources/Logo_Maximize.png");
                    this.Click += (object o, EventArgs EA) =>
                    {
                        if (General_form.WindowState == FormWindowState.Maximized) General_form.WindowState = FormWindowState.Normal;
                        else General_form.WindowState = FormWindowState.Maximized;
                    };
                    break;
                case "Red":
                    this.Location = new Point(p.X + 124, p.Y + 3);
                    this.Image = Image.FromFile("../../User Interface Recources/Logo_Exit.png");
                    this.Click += (object o, EventArgs EA) => { Application.Exit(); };
                    break;
            }

        }
    }
}
