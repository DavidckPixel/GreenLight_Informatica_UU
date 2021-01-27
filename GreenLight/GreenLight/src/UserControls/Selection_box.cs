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
        public int Selected_index = 0;
        public bool Selected_left_bool = true;

        public List<string> Elements_available = new List<string>();
        public List<string> Elements_selected = new List<string>();

        public List<string> allElement = new List<string>();
        private Action functionClick;
        private Action functionAdd;
        private Action functionRemove;

        private Form Form;

        public Selection_box(Form _Form, FontFamily Dosis_font_family, List<string> _available, Action _functionClick, Action _functionAdd, Action _functionRemove)
        {
            Elements_selected = _available;

            Elements_selected.ForEach(x => allElement.Add(x));

            /*Elements_selected.Add("Test 1");
            Elements_selected.Add("Test 2");
            Elements_selected.Add("Test 3");
            Elements_selected.Add("Test 4");
            Elements_selected.Add("Test 5"); */

            this.BackgroundImage = Image.FromFile("../../src/User Interface Recources/Selection_Box.png");
            this.Size = new Size(225, 117);
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.Form = _Form;
            Elements_draw(Elements_selected, Elements_available, this.Form, Dosis_font_family);

            this.Selected_index = 0;
            this.Selected_left_bool = false;

            this.functionClick = _functionClick;
            this.functionAdd = _functionAdd;
            this.functionRemove = _functionRemove;
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
                if (i == Selected_index && Selected_left_bool) Prime_Color = Color.FromArgb(142, 140, 144);
                else Prime_Color = Color.FromArgb(196, 196, 198); 

                PictureBox PB = new PictureBox();
                PB.BackColor = Prime_Color;
                PB.Location = new Point(10, 20 * i + 30);
                PB.Size = new Size(101, 20);
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
                PB_label.Click += (object o, EventArgs EA) => 
                { Selected_index = Elements_selected.IndexOf(element);
                    Selected_left_bool = true;
                    Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family);
                    this.functionClick();
                };
                i++;
            }

            int j = 0;
            foreach (string element in Elements_available)
            {
                Color Hover_Color = Color.FromArgb(109, 109, 109);
                Color Prime_Color;
                if (j == Selected_index && !Selected_left_bool) Prime_Color = Color.FromArgb(142, 140, 144);
                else Prime_Color = Color.FromArgb(196, 196, 198);

                PictureBox PB = new PictureBox();
                PB.BackColor = Prime_Color;
                PB.Location = new Point(114, 20 * j + 30);
                PB.Size = new Size(100, 20);
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
                PB_label.Click += (object o, EventArgs EA) => {
                    Selected_index = Elements_available.IndexOf(element);
                    Selected_left_bool = false;
                    Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family);
                    this.functionClick();
                };
                j++;
            }

            CurvedButtons Remove = new CurvedButtons(new Size(17, 17), new Point(104, 52), 10,
               "../../src/User Interface Recources/Selection_Box_Remove_Button.png", Color.FromArgb(255, 255, 255));
            Remove.Click += (object o, EventArgs EA) => {
                this.functionRemove();
            };
            this.Controls.Add(Remove);
            Remove.BringToFront();

            CurvedButtons Add = new CurvedButtons(new Size(17, 17), new Point(104, 66), 10,
                "../../src/User Interface Recources/Selection_Box_Add_Button.png", Color.FromArgb(255, 255, 255));
            Add.Click += (object o, EventArgs EA) => { this.functionAdd(); };
            this.Controls.Add(Add);
            Add.BringToFront();

            CurvedButtons To_left = new CurvedButtons(new Size(17, 17), new Point(104, 80), 10,
                "../../src/User Interface Recources/Selection_Box_To_Left_Button.png", Color.FromArgb(255, 255, 255));
            To_left.Click += (object o, EventArgs EA) => { Elements_switch(Elements_selected, Elements_available, Selected_index, Selected_left_bool, Form, Dosis_font_family, 0); };
            this.Controls.Add(To_left);
            To_left.BringToFront();

            CurvedButtons To_right = new CurvedButtons(new Size(17, 17), new Point(104, 94), 10,
                "../../src/User Interface Recources/Selection_Box_To_Right_Button.png", Color.FromArgb(255, 255, 255));
            To_right.Click += (object o, EventArgs EA) => { Elements_switch(Elements_selected, Elements_available, Selected_index, Selected_left_bool, Form, Dosis_font_family, 1); };
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
            else if (Selected_index >= 0 && Selected_index < Elements_available.Count)
            {
                Elements_selected.Add(Elements_available[Selected_index]);
                Elements_available.RemoveAt(Selected_index);
            }
            Elements_draw(Elements_selected, Elements_available, Form, Dosis_font_family);
        }

        public void Add_Element(string element)
        {
            Console.WriteLine("ADDED: " + element);
            Elements_selected.Add(element);
            this.Selected_index = this.Elements_selected.IndexOf(element);
            this.Selected_left_bool = true;
            Elements_draw(Elements_selected, Elements_available, Form, DrawData.Dosis_font_family);
        }

        public void Remove_Element(string element)
        {
            Elements_selected.Remove(element);
            Elements_available.Remove(element);

            Selected_index = 0;
            Selected_left_bool = true;

            if(Elements_selected.Count < 1)
            {
                if(Elements_available.Count > 0)
                {
                    Selected_left_bool = false;
                }
                else
                {
                    this.functionAdd();
                    return;
                }
            }

            this.functionClick();

            Elements_draw(Elements_selected, Elements_available, Form, DrawData.Dosis_font_family);
        }

    }
}
