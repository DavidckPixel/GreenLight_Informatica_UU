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
        protected override void OnPaint(PaintEventArgs pea)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddEllipse(0, 0, Size.Width-1, Size.Height-1);
            this.Region = new Region(p);
            base.OnPaint(pea);
        }
        //HelpButton
        public RoundButtons(Size _buttonsize, Point Location, string FilePath)
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = _buttonsize;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 10) + "Select.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
        }
        
        public RoundButtons(Size s, Point p, string Color, Form Form)
        {
            this.Cursor = Cursors.Hand;
            this.Size = s;
            this.SizeMode = PictureBoxSizeMode.Zoom;

            switch (Color)
            {
                case "Green":
                    this.Location = new Point(p.X + 20, p.Y + 3);
                    this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Minimalize.png");
                    this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Minimalize_Select.png"); };
                    this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Minimalize.png"); };
                    this.Click += (object o, EventArgs EA) => { Form.WindowState = FormWindowState.Minimized; };
                    break;
                case "Yellow":
                    this.Location = new Point(p.X + 72, p.Y + 3);
                    this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Maximize.png");
                    this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Maximize_Select.png"); };
                    this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Maximize.png"); };
                    this.Click += (object o, EventArgs EA) =>
                    {
                        if (Form.WindowState == FormWindowState.Maximized)
                        {
                            Form.WindowState = FormWindowState.Normal;
                            General_Form.Main.UserInterface.Refresh_region(Form);
                            Form.Refresh();
                        }
                        else
                        {
                            Form.WindowState = FormWindowState.Maximized;
                            General_Form.Main.UserInterface.Refresh_region(Form);
                            Form.Refresh();
                        }
                    };
                    break;
                case "Red":
                    this.Location = new Point(p.X + 124, p.Y + 3);
                    this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Exit.png");
                    this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Exit_Select.png"); };
                    this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Exit.png"); };
                    this.Click += (object o, EventArgs EA) => { Application.Exit(); };
                    break;
            }

        }
    }
}
