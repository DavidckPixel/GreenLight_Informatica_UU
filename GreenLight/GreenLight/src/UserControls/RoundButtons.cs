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
    /* This is the RoundButtons class. The round buttons in our userinterface are created using this class.
       It overrides the OnPaint to make a custom button design. The buttons that use this class are the 
       info button and the control buttons to minimize, maximize and exit the program.                      */
    class RoundButtons : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pea)
        {
            GraphicsPath _graphics = new GraphicsPath();
            _graphics.AddEllipse(0, 0, Size.Width-1, Size.Height-1);
            this.Region = new Region(_graphics);
            base.OnPaint(pea);
        }
  
        public RoundButtons(Size _buttonsize, Point _location, string _filepath)
        {
            this.Cursor = Cursors.Hand;
            this.Location = _location;
            this.Size = _buttonsize;
            this.Image = Image.FromFile(_filepath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseHover += (object o, EventArgs EA) => { this.Image = Image.FromFile(_filepath.Remove(_filepath.Length - 10) + "Select.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(_filepath); };
        }
        
        public RoundButtons(Size _size, Point _location, string _color, Form _form)
        {
            this.Cursor = Cursors.Hand;
            this.Size = _size;
            this.SizeMode = PictureBoxSizeMode.Zoom;

            switch (_color)
            {
                case "Green":
                    this.Location = new Point(_location.X + 20, _location.Y + 3);
                    this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Minimalize.png");
                    this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Minimalize_Select.png"); };
                    this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Minimalize.png"); };
                    this.Click += (object o, EventArgs EA) => { _form.WindowState = FormWindowState.Minimized; };
                    break;
                case "Yellow":
                    this.Location = new Point(_location.X + 72, _location.Y + 3);
                    this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Maximize.png");
                    this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Maximize_Select.png"); };
                    this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Maximize.png"); };
                    this.Click += (object o, EventArgs EA) =>
                    {
                        if (_form.WindowState == FormWindowState.Maximized)
                        {
                            _form.WindowState = FormWindowState.Normal;
                            General_Form.Main.UserInterface.Refresh_region(_form);
                            _form.Refresh();
                        }
                        else
                        {
                            _form.WindowState = FormWindowState.Maximized;
                            General_Form.Main.UserInterface.Refresh_region(_form);
                            _form.Refresh();
                        }
                    };
                    break;
                case "Red":
                    this.Location = new Point(_location.X + 124, _location.Y + 3);
                    this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Exit.png");
                    this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Exit_Select.png"); };
                    this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile("../../src/User Interface Recources/Logo_Exit.png"); };
                    this.Click += (object o, EventArgs EA) => { Application.Exit(); };
                    break;
            }
        }
    }
}
