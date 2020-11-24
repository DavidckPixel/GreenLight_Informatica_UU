using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    class Move_panel : Panel
    {
        bool Draggable = false;
        int Mouse_X = 0;
        int Mouse_Y = 0;
        public Move_panel(General_form General_form) 
        {
            this.Cursor = Cursors.SizeAll;
            this.Location = new Point(0, 0);
            this.Size = new Size(250, 100);
            this.BackColor = Color.White;
            this.MouseDown += (object o, MouseEventArgs MEA) => { Draggable = true; Mouse_X = Cursor.Position.X - General_form.Left; Mouse_Y = Cursor.Position.Y - General_form.Top; };
            this.MouseMove += (object o, MouseEventArgs MEA) =>
            {
                if (Draggable)
                {
                    General_form.Top = Cursor.Position.Y - Mouse_Y;
                    General_form.Left = Cursor.Position.X - Mouse_X;
                }
            };

            this.MouseUp += (object o, MouseEventArgs MEA) => { Draggable = false; };
        }
    }
}
