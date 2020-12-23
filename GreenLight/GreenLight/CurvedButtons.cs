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
        int curve;
        Color Backcolor;
        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath p = new GraphicsPath();
            Rectangle r = new Rectangle(0, 0, Width, Height);
            int d = curve;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            p.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            p.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            this.Region = new Region(p);
            base.OnPaint(pe);
            if (Backcolor != null)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Pen Pen = new Pen(Backcolor, 1);
                pe.Graphics.DrawArc(Pen, r.X, r.Y, d, d, 180, 90);
                pe.Graphics.DrawArc(Pen, r.X + r.Width - d, r.Y, d, d, 270, 90);
                pe.Graphics.DrawArc(Pen, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
                pe.Graphics.DrawArc(Pen, r.X, r.Y + r.Height - d, d, d, 90, 90);
            }
        }
        // Divider 
        public CurvedButtons()
        {
            curve = 1;
            this.Size = new Size(250, 5);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = Image.FromFile("../../User Interface Recources/Sub_Menu_Divider.png");
        }
        // Header
        public CurvedButtons(Size Header_size, Point Location, string FilePath)
        {
            curve = 1;
            this.Size = Header_size;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Location = Location;
            this.BringToFront();
            this.Image = Image.FromFile(FilePath);
        }
        // Curved Buttons
        public CurvedButtons(Size Button_size, Point Location, int Curve, string FilePath, Color BackColor)
        {
            curve = Curve;
            Backcolor = BackColor;
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 4) + "_On_Hover.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
        }
        // Logo
        public CurvedButtons(Form Form, int Curve)
        {
            curve = Curve;
            this.Size = new Size(175, 70);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = Image.FromFile("../../User Interface Recources/Logo.png");
            this.SizeMode = PictureBoxSizeMode.Zoom;
            Size s = new Size(30, 30);
            RoundButtons Green = new RoundButtons(s, this.Location, "Green", Form);
            this.Controls.Add(Green);
            RoundButtons Yellow = new RoundButtons(s, this.Location, "Yellow", Form);
            this.Controls.Add(Yellow);
            RoundButtons Red = new RoundButtons(s, this.Location, "Red", Form);
            this.Controls.Add(Red);
        }
        // Curved buttons with text
        public CurvedButtons(Size Button_size, Point Location, int Curve, string FilePath, string Text, FontFamily Dosis_font_family, Form Form, Color BackColor)
        {
            curve = Curve;
            Backcolor = BackColor;
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 4) + "_On_Hover.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.Font = new Font(Dosis_font_family, 15, FontStyle.Bold);
            label.Text = Text;
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.FromArgb(142, 140, 144);
            label.Size = Button_size;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(label.Location.X+2, label.Location.Y - 2);
            label.Parent = this;
            label.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Label_click(Text); };
            label.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 4) + "_On_Hover.png"); };
            label.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
            this.Controls.Add(label);
        }
    }
}
