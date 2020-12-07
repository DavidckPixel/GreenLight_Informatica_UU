﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace GreenLight
{
    class Selection_box : Panel
    {
        public Selection_box(General_form General_form, FontFamily Dosis_font_family)
        {
            List<string> Elements_selected = new List<string>();
            List<string> Elements_available = new List<string>();

            Elements_selected.Add("Test 1");
            Elements_selected.Add("Test 2");
            Elements_selected.Add("Test 3");
            Elements_selected.Add("Test 4");
            Elements_selected.Add("Test 5");

            this.BackgroundImage = Image.FromFile("../../User Interface Recources/Selection_Box.png");
            this.Size = new Size(225, 117);
            this.BackgroundImageLayout = ImageLayout.Zoom;
            Elements_draw(Elements_selected, Elements_available, General_form, Dosis_font_family);
        }

        public void Update_Selection_box(List<string> Elements_selected, List<string> Elements_available, General_form General_form, FontFamily Dosis_font_family)
        {
            Elements_draw(Elements_selected, Elements_available, General_form, Dosis_font_family);
        }

        private void Elements_draw(List<string> Elements_selected, List<string> Elements_available, General_form General_form, FontFamily Dosis_font_family) 
        {
            this.Controls.Clear();
            int Selected_index = 0;
            bool Selected_left_bool = true;

            int i = 0;
            foreach (string element in Elements_selected)
            {
                PictureBox PB = new PictureBox();
                PB.BackColor = Color.FromArgb(142, 140, 144);
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
                PB_label.MouseEnter += (object o, EventArgs EA) => { PB.BackColor = Color.FromArgb(109, 109, 109); };
                PB_label.MouseLeave += (object o, EventArgs EA) => { PB.BackColor = Color.FromArgb(142, 140, 144); };
                PB_label.Click += (object o, EventArgs EA) => { Selected_index = Elements_selected.IndexOf(element); Selected_left_bool = true;};
                i++;
            }

            int j = 0;
            foreach (string element in Elements_available)
            {
                PictureBox PB = new PictureBox();
                PB.BackColor = Color.FromArgb(142, 140, 144);
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
                PB_label.MouseEnter += (object o, EventArgs EA) => { PB.BackColor = Color.FromArgb(109, 109, 109); };
                PB_label.MouseLeave += (object o, EventArgs EA) => { PB.BackColor = Color.FromArgb(142, 140, 144); };
                PB_label.Click += (object o, EventArgs EA) => { Selected_index = Elements_available.IndexOf(element); Selected_left_bool = false;};
                j++;
            }

            PictureBox To_left = new PictureBox();
            To_left.Image = Image.FromFile("../../User Interface Recources/Selection_Box_To_Left.png");
            To_left.Cursor = Cursors.Hand;
            To_left.SizeMode = PictureBoxSizeMode.StretchImage;
            To_left.Size = new Size(17,17);
            To_left.Location = new Point(104,80);
            To_left.Click += (object o, EventArgs EA) => { Elements_switch(Elements_selected, Elements_available, Selected_index, Selected_left_bool,General_form,Dosis_font_family,0); };
            this.Controls.Add(To_left);
            To_left.BringToFront();

            PictureBox To_right = new PictureBox();
            To_right.Image = Image.FromFile("../../User Interface Recources/Selection_Box_To_Right.png");
            To_right.Cursor = Cursors.Hand;
            To_right.SizeMode = PictureBoxSizeMode.StretchImage;
            To_right.Size = new Size(17, 17);
            To_right.Location = new Point(104, 94);
            To_right.Click += (object o, EventArgs EA) => { Elements_switch(Elements_selected, Elements_available, Selected_index, Selected_left_bool,General_form,Dosis_font_family,1); };
            this.Controls.Add(To_right);
            To_right.BringToFront();
        }

        private void Elements_switch(List<string> Elements_selected, List<string> Elements_available, int Selected_index, bool Selected_left_bool, General_form General_form, FontFamily Dosis_font_family, int direction)
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
            Elements_draw(Elements_selected, Elements_available, General_form, Dosis_font_family);
        }
    }
}
