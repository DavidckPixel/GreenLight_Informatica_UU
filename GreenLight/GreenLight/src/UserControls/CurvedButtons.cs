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
    /* This is the CurvedButtons class, we use this class to create the buttons that can be 
       found in our Userinterface menus. We override the OnPaint to create our custom button 
       design. Different buttons are created depending on the parameters that you give to the
       CurvedButton. You can create: buttons with text, buttons with an image, and dividers.  */
    public class CurvedButtons : PictureBox
    {
        
        public string Image_path = "";
        public bool Selected;
        public int Curve;
        public Color Backcolor;

        protected override void OnPaint(PaintEventArgs pea)
        {
            GraphicsPath p = new GraphicsPath();
            Rectangle _size = new Rectangle(0, 0, Width, Height);
            int _curve = Curve;

            p.AddArc(_size.X, _size.Y, _curve, _curve, 180, 90);
            p.AddArc(_size.X + _size.Width - _curve, _size.Y, _curve, _curve, 270, 90);
            p.AddArc(_size.X + _size.Width - _curve, _size.Y + _size.Height - _curve, _curve, _curve, 0, 90);
            p.AddArc(_size.X, _size.Y + _size.Height - _curve, _curve, _curve, 90, 90);
            this.Region = new Region(p);
            base.OnPaint(pea);

            if (Backcolor != null)
            {
                Pen Pen = new Pen(Backcolor, 1);
                pea.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                pea.Graphics.DrawArc(Pen, _size.X, _size.Y, _curve, _curve, 180, 90);
                pea.Graphics.DrawArc(Pen, _size.X + _size.Width - _curve, _size.Y, _curve, _curve, 270, 90);
                pea.Graphics.DrawArc(Pen, _size.X + _size.Width - _curve, _size.Y + _size.Height - _curve, _curve, _curve, 0, 90);
                pea.Graphics.DrawArc(Pen, _size.X, _size.Y + _size.Height - _curve, _curve, _curve, 90, 90);
            }
        }
      
        public CurvedButtons() // Divider 
        {
            this.Curve = 1;
            this.Size = new Size(250, 5);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = Image.FromFile("../../src/User Interface Recources/Sub_Menu_Divider.png");
        }
        
        public CurvedButtons(Size Header_size, Point Location, string FilePath) // Header
        {
            this.Curve = 1;
            this.Size = Header_size;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Location = Location;
            this.BringToFront();
            this.Image = Image.FromFile(FilePath);
        }
        
        public CurvedButtons(Size _buttonsize, Point _location, int _curve, string _filepath, Color _backcolor) // Curved buttons
        {
            this.Selected = false;
            this.Image_path = _filepath;
            this.Curve = _curve;
            this.Backcolor = _backcolor;
            this.Cursor = Cursors.Hand;
            this.Location = _location;
            this.Size = _buttonsize;
            this.Image = Image.FromFile(_filepath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(Image_path.Remove(Image_path.Length - 10) + "Select.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { if (!Selected) { this.Image = Image.FromFile(Image_path); }; };
        }

        
        public CurvedButtons(Form _form, int _curve) // Logo
        {
            this.Curve = _curve;
            this.Size = new Size(175, 70);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = Image.FromFile("../../src/User Interface Recources/Logo.png");
            this.SizeMode = PictureBoxSizeMode.Zoom;
            Size _size = new Size(30, 30);

            RoundButtons _green = new RoundButtons(_size, this.Location, "Green", _form);   
            RoundButtons _yellow = new RoundButtons(_size, this.Location, "Yellow", _form); 
            RoundButtons _red = new RoundButtons(_size, this.Location, "Red", _form);

            this.Controls.Add(_green);
            this.Controls.Add(_yellow);
            this.Controls.Add(_red);
        }
        
        public CurvedButtons(Size _buttonsize, Point _location, int _curve, string _filepath, string _text, FontFamily _dosisfontfamily, Form _form, Color _backcolor) // Curved buttons with text
        {
            this.Curve = _curve;
            this.Backcolor = _backcolor;
            this.Cursor = Cursors.Hand;
            this.Location = _location;
            this.Size = _buttonsize;
            this.Image = Image.FromFile(_filepath);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(_filepath.Remove(_filepath.Length - 10) + "Select.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(_filepath); };

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.Font = new Font(_dosisfontfamily, 15, FontStyle.Bold);
            label.Text = _text;
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.FromArgb(142, 140, 144);
            label.Size = _buttonsize;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(label.Location.X+2, label.Location.Y - 2);
            label.Parent = this;
            label.Click += (object o, EventArgs EA) => { this.OnClick(EA); };
            label.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(_filepath.Remove(_filepath.Length - 10) + "Select.png"); };
            label.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(_filepath); };

            this.Controls.Add(label);
        }

        public CurvedButtons(Size _buttonsize, Point _location, int _curve, string _filepath, string _text, FontFamily _dosisfontfamily, Form _form, Color _backcolor, int _empty)
        {
            this.Curve = _curve;
            this.Backcolor = Color.FromArgb(142, 140, 144);
            this.Cursor = Cursors.Hand;
            this.Location = _location;
            this.Size = _buttonsize;
            this.Image = LoadBitmap(_filepath);
            this.SizeMode = PictureBoxSizeMode.StretchImage;

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.Font = new Font(_dosisfontfamily, 15, FontStyle.Bold);
            label.Text = _text;
            label.Location = new Point(_location.X + 2, _location.Y + _buttonsize.Height + 10); ;
            label.Click += (object o, EventArgs EA) => { this.OnClick(EA); };

            this.Controls.Add(label);
        }

        public void SetImage(string _filepath)
        {
            this.Image_path = _filepath;
        }

        private Bitmap LoadBitmap(string _filename)
        {
            using (Bitmap _bitmap = new Bitmap(_filename))
            {
                return new Bitmap(_bitmap);
            }
        }
    }
}
