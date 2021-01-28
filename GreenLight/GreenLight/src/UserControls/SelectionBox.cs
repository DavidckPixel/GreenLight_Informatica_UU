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
    /* This is the SelectionBox class. This class is used in the driver and vehicle submenu.
       The left side of the box consists of the available elements and
       the right side of the box consists of the selected elements.
       There are four buttons in the middle of the box:
       Add button - Lets the user add an element to the box after entering a name in the popup window.
       Remove button - Removes the selected element.
       Move left button - Moves selected element from left to right.
       Move right button - Moves chosen element from right to left.                              */
    public class SelectionBox : Panel
    {
        public int selectedIndex = 0;
        public bool selectedLeftBool = true;

        public List<string> elementsAvailable = new List<string>();
        public List<string> elementsSelected = new List<string>();
        public List<string> allElements = new List<string>();

        private Action functionClick;
        private Action functionAdd;
        private Action functionRemove;

        private Form Form;

        public SelectionBox(Form _form, FontFamily _dosisfontfamily, List<string> _available, Action _functionClick, Action _functionAdd, Action _functionRemove)
        {
            this.elementsSelected = _available;
            this.elementsSelected.ForEach(x => allElements.Add(x));
            this.BackgroundImage = Image.FromFile("../../src/User Interface Recources/Selection_Box.png");
            this.Size = new Size(225, 117);
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.Form = _form;
            ElementsDraw(elementsSelected, elementsAvailable, this.Form, _dosisfontfamily);

            this.selectedIndex = 0;
            this.selectedLeftBool = false;

            this.functionClick = _functionClick;
            this.functionAdd = _functionAdd;
            this.functionRemove = _functionRemove;
        }

        public void UpdateSelectionBox(List<string> _selected, List<string> _available, Form _form, FontFamily _dosisfontfamily)
        {
            ElementsDraw(_selected, _available, _form, _dosisfontfamily);
        }

        private void ElementsDraw(List<string> _selected, List<string> _available, Form _form, FontFamily _dosisfontfamily)
        {
            this.Controls.Clear();

            int _counter1 = 0;
            foreach (string _element in _selected)
            {
                Color _hovercolor = Color.FromArgb(109, 109, 109);
                Color _primecolor;
                if (_counter1 == selectedIndex && selectedLeftBool) _primecolor = Color.FromArgb(142, 140, 144);
                else _primecolor = Color.FromArgb(196, 196, 198); 

                PictureBox PB = new PictureBox();
                PB.BackColor = _primecolor;
                PB.Location = new Point(10, 20 * _counter1 + 30);
                PB.Size = new Size(101, 20);
                PB.Cursor = Cursors.Hand;
                this.Controls.Add(PB);
                Label PB_label = new Label();
                PB_label.Parent = PB;
                PB_label.Text = _element;
                PB_label.TextAlign = ContentAlignment.TopCenter;
                PB_label.ForeColor = Color.White;
                PB_label.Font = new Font(_dosisfontfamily, 8, FontStyle.Bold);
                PB_label.MouseEnter += (object o, EventArgs EA) => { PB.BackColor = _hovercolor; };
                PB_label.MouseLeave += (object o, EventArgs EA) => { PB.BackColor = _primecolor; };
                PB_label.Click += (object o, EventArgs EA) => 
                { selectedIndex = _selected.IndexOf(_element);
                    selectedLeftBool = true;
                    ElementsDraw(_selected, _available, _form, _dosisfontfamily);
                    this.functionClick();
                };
                _counter1++;
            }

            int _counter2 = 0;
            foreach (string element in _available)
            {
                Color _hovercolor = Color.FromArgb(109, 109, 109);
                Color _primecolor;
                if (_counter2 == selectedIndex && !selectedLeftBool) _primecolor = Color.FromArgb(142, 140, 144);
                else _primecolor = Color.FromArgb(196, 196, 198);

                PictureBox PB = new PictureBox();
                PB.BackColor = _primecolor;
                PB.Location = new Point(114, 20 * _counter2 + 30);
                PB.Size = new Size(100, 20);
                PB.Cursor = Cursors.Hand;
                this.Controls.Add(PB);
                Label PB_label = new Label();
                PB_label.Parent = PB;
                PB_label.Text = element;
                PB_label.TextAlign = ContentAlignment.TopCenter;
                PB_label.ForeColor = Color.White;
                PB_label.Font = new Font(_dosisfontfamily, 8, FontStyle.Bold);
                PB_label.MouseEnter += (object o, EventArgs EA) => { PB.BackColor = _hovercolor; };
                PB_label.MouseLeave += (object o, EventArgs EA) => { PB.BackColor = _primecolor; };
                PB_label.Click += (object o, EventArgs EA) => {
                    selectedIndex = _available.IndexOf(element);
                    selectedLeftBool = false;
                    ElementsDraw(_selected, _available, _form, _dosisfontfamily);
                    this.functionClick();
                };
                _counter2++;
            }

            CurvedButtons _remove = new CurvedButtons(new Size(17, 17), new Point(104, 52), 10,
               "../../src/User Interface Recources/Selection_Box_Remove_Button.png", Color.FromArgb(255, 255, 255));
            _remove.Click += (object o, EventArgs EA) => { this.functionRemove(); };
            this.Controls.Add(_remove);
            _remove.BringToFront();

            CurvedButtons _add = new CurvedButtons(new Size(17, 17), new Point(104, 66), 10,
                "../../src/User Interface Recources/Selection_Box_Add_Button.png", Color.FromArgb(255, 255, 255));
            _add.Click += (object o, EventArgs EA) => { this.functionAdd(); };
            this.Controls.Add(_add);
            _add.BringToFront();

            CurvedButtons _toleft = new CurvedButtons(new Size(17, 17), new Point(104, 80), 10,
                "../../src/User Interface Recources/Selection_Box_To_Left_Button.png", Color.FromArgb(255, 255, 255));
            _toleft.Click += (object o, EventArgs EA) => { ElementsSwitch(_selected, _available, selectedIndex, selectedLeftBool, _form, _dosisfontfamily, 0); };
            this.Controls.Add(_toleft);
            _toleft.BringToFront();

            CurvedButtons _toright = new CurvedButtons(new Size(17, 17), new Point(104, 94), 10,
                "../../src/User Interface Recources/Selection_Box_To_Right_Button.png", Color.FromArgb(255, 255, 255));
            _toright.Click += (object o, EventArgs EA) => { ElementsSwitch(_selected, _available, selectedIndex, selectedLeftBool, _form, _dosisfontfamily, 1); };
            this.Controls.Add(_toright);
            _toright.BringToFront();
        }

        private void ElementsSwitch(List<string> _selected, List<string> _available, int _index, bool _left, Form Form, FontFamily _dosisfontfamily, int _direction)
        {
            if (_left && _index >= 0 && _index < _selected.Count && _direction == 1)
            {
                _available.Add(_selected[_index]);
                _selected.RemoveAt(_index);
            }
            else if (_index >= 0 && _index < _available.Count)
            {
                _selected.Add(_available[_index]);
                _available.RemoveAt(_index);
            }
            ElementsDraw(_selected, _available, Form, _dosisfontfamily);
        }

        public void AddElement(string _element)
        {
            Console.WriteLine("ADDED: " + _element);
            elementsSelected.Add(_element);
            this.selectedIndex = this.elementsSelected.IndexOf(_element);
            this.selectedLeftBool = true;
            ElementsDraw(elementsSelected, elementsAvailable, Form, DrawData.Dosis_font_family);
        }

        public void RemoveElement(string _element)
        {
            elementsSelected.Remove(_element);
            elementsAvailable.Remove(_element);

            selectedIndex = 0;
            selectedLeftBool = true;

            if(elementsSelected.Count < 1)
            {
                if(elementsAvailable.Count > 0)
                {
                    selectedLeftBool = false;
                }
                else
                {
                    this.functionAdd();
                    return;
                }
            }

            this.functionClick();
            ElementsDraw(elementsSelected, elementsAvailable, Form, DrawData.Dosis_font_family);
        }

    }
}
