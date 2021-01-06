using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualBasic;

namespace GreenLight
{
    public class Selection_box : Panel
    {
        int Selected_index = 0;
        bool Selected_left_bool = true;

        public List<string> Elements_available = new List<string>();

        public Selection_box(Form Form, FontFamily Dosis_font_family, List<string> _available)
        {
            List<string> Elements_selected = _available;

            /*Elements_selected.Add("Test 1");
            Elements_selected.Add("Test 2");
            Elements_selected.Add("Test 3");
            Elements_selected.Add("Test 4");
            Elements_selected.Add("Test 5"); */

            this.BackgroundImage = Image.FromFile("../../User Interface Recources/Selection_Box.png");
            this.Size = new Size(225, 117);
            this.BackgroundImageLayout = ImageLayout.Zoom;
            Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family);
        }

        public void Update_Selection_box(List<string> Elements_selected, List<string> Elements_available, Form Form, FontFamily Dosis_font_family)
        {
            Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family);
        }

        private void Elements_draw(List<string> Elements_selected, List<string> Elements_available, Form Form, FontFamily Dosis_font_family) 
        {
            this.Controls.Clear();

            int i = 0;
            foreach (string element in Elements_selected)
            {
                Color Hover_Color = Color.FromArgb(109, 109, 109);
                Color Prime_Color;
                if (i == Selected_index && Selected_left_bool) Prime_Color = Color.FromArgb(196, 196, 198);
                else Prime_Color = Color.FromArgb(142, 140, 144);

                PictureBox PB = new PictureBox();
                PB.BackColor = Prime_Color;
                PB.Location = new Point(10, 15 * i+30);
                PB.Size = new Size(101, 15);
                PB.Cursor = Cursors.Hand;
                this.Controls.Add(PB);
                Label PB_label = new Label();
                PB_label.Parent = PB;
                PB_label.Text = element;
                PB_label.TextAlign = ContentAlignment.TopCenter;
                PB_label.ForeColor = Color.White;
                PB_label.Font = new Font(Dosis_font_family, 8, FontStyle.Bold);
                PB_label.MouseEnter += (object o, EventArgs EA) => { PB.BackColor = Hover_Color; };
                PB_label.MouseLeave += (object o, EventArgs EA) => { PB.BackColor = Prime_Color; };
                PB_label.Click += (object o, EventArgs EA) => { Selected_index = Elements_selected.IndexOf(element); Selected_left_bool = true; Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family); };
                i++;
            }

            int j = 0;
            foreach (string element in Elements_available)
            {
                Color Hover_Color = Color.FromArgb(109, 109, 109);
                Color Prime_Color;
                if (j == Selected_index && !Selected_left_bool) Prime_Color = Color.FromArgb(196, 196, 198);
                else Prime_Color = Color.FromArgb(142, 140, 144);

                PictureBox PB = new PictureBox();
                PB.BackColor = Prime_Color;
                PB.Location = new Point(114, 15 * j + 30);
                PB.Size = new Size(100, 15);
                PB.Cursor = Cursors.Hand;
                this.Controls.Add(PB);
                Label PB_label = new Label();
                PB_label.Parent = PB;
                PB_label.Text = element;
                PB_label.TextAlign = ContentAlignment.TopCenter;
                PB_label.ForeColor = Color.White;
                PB_label.Font = new Font(Dosis_font_family, 8, FontStyle.Bold);
                PB_label.MouseEnter += (object o, EventArgs EA) => { PB.BackColor = Hover_Color; };
                PB_label.MouseLeave += (object o, EventArgs EA) => { PB.BackColor = Prime_Color; };
                PB_label.Click += (object o, EventArgs EA) => { Selected_index = Elements_available.IndexOf(element); Selected_left_bool = false; Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family); };
                j++;
            }
           
          
            CurvedButtons Add = new CurvedButtons(new Size(17, 17), new Point(104, 66), 10,
                "../../User Interface Recources/Selection_Box_Add.png", Color.FromArgb(255, 255, 255));
            Add.Click += (object o, EventArgs EA) => { string name = Interaction.InputBox("Enter Name: ", "Driver", "no name", 100, 100); Elements_selected.Add(name); Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family); };
            this.Controls.Add(Add);
            Add.BringToFront();

            CurvedButtons To_left = new CurvedButtons(new Size(17, 17), new Point(104, 80), 10,
                "../../User Interface Recources/Selection_Box_To_Left.png", Color.FromArgb(255,255,255));
            To_left.Click += (object o, EventArgs EA) => { Elements_switch(Elements_selected, Elements_available, Selected_index, Selected_left_bool,Form,Dosis_font_family,0); };
            this.Controls.Add(To_left);
            To_left.BringToFront();

            CurvedButtons To_right = new CurvedButtons(new Size(17, 17), new Point(104, 94), 10,
                "../../User Interface Recources/Selection_Box_To_Right.png", Color.FromArgb(255, 255, 255));
            To_right.Click += (object o, EventArgs EA) => { Elements_switch(Elements_selected, Elements_available, Selected_index, Selected_left_bool,Form,Dosis_font_family,1); };
            this.Controls.Add(To_right);
            To_right.BringToFront();
        }

        private void Elements_switch(List<string> Elements_selected, List<string> Elements_available, int Selected_index, bool Selected_left_bool, Form Form, FontFamily Dosis_font_family, int direction)
        {
            if (Selected_left_bool && Selected_index >= 0 && Selected_index < Elements_selected.Count && direction == 1)
            {
                Elements_available.Add(Elements_selected[Selected_index]);
                Elements_selected.RemoveAt(Selected_index);
            }
            else if(Selected_index >= 0 && Selected_index < Elements_available.Count)
            {
                Elements_selected.Add(Elements_available[Selected_index]);
                Elements_available.RemoveAt(Selected_index);
            }
            Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family);
        }

        public void Add_Element(string element)
        {
            Elements_available.Add(element);
        }
    }
}
