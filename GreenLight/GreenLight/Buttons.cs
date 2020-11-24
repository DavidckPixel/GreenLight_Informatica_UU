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
        public Buttons(Size Button_size, Point Location, string FilePath) 
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length-4)+ "_On_Hover.png");};
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath);};
        }

        public Buttons(Size Button_size, Point Location, string FilePath, string Text, FontFamily Dosis_font_family, General_form General_form)
        {
            this.Cursor = Cursors.Hand;
            this.Location = Location;
            this.Size = Button_size;
            this.Image = Image.FromFile(FilePath);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 4) + "_On_Hover.png"); };
            this.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };

            Label label = new Label();
            label.Font = new Font(Dosis_font_family, 15, FontStyle.Bold);
            label.Text = Text;
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.FromArgb(142, 140, 144);
            label.Size = Button_size;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Parent = this;
            label.Click += (object o, EventArgs EA) => { General_form.Label_click(Text); };
            label.MouseEnter += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath.Remove(FilePath.Length - 4) + "_On_Hover.png"); };
            label.MouseLeave += (object o, EventArgs EA) => { this.Image = Image.FromFile(FilePath); };
            this.Controls.Add(label);
        }
        public Buttons(General_form General_form)
        {
            this.Size = new Size(175, 70);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = Image.FromFile("../../User Interface Recources/Logo.png");
            this.SizeMode = PictureBoxSizeMode.Zoom;
            Size s = new Size(30, 30);
            RoundButtons Green = new RoundButtons(s, this.Location, "Green", General_form);
            this.Controls.Add(Green);
            RoundButtons Yellow = new RoundButtons(s, this.Location, "Yellow", General_form);
            this.Controls.Add(Yellow);
            RoundButtons Red = new RoundButtons(s, this.Location, "Red", General_form);
            this.Controls.Add(Red);

        }
        public Buttons(string Name)
        {
            this.Size = new Size(250, 5);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = Image.FromFile("../../User Interface Recources/Sub_Menu_Divider.png");
        }

        
    }
}
