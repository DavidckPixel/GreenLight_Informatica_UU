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
        public Move_panel(Form Form) 
        {
            if(Form.WindowState != FormWindowState.Maximized) this.Cursor = Cursors.SizeAll;
            this.Location = new Point(0, 0);
            this.Size = new Size(250, 100);
            this.BackColor = Color.White;
            this.MouseDown += (object o, MouseEventArgs MEA) => { Draggable = true; Mouse_X = Cursor.Position.X - Form.Left; Mouse_Y = Cursor.Position.Y - Form.Top; };
            this.MouseMove += (object o, MouseEventArgs MEA) =>
            {
                if (Draggable)
                {
                    Form.Top = Cursor.Position.Y - Mouse_Y;
                    Form.Left = Cursor.Position.X - Mouse_X;
                }
            };

            this.MouseUp += (object o, MouseEventArgs MEA) => { Draggable = false; };
        }
    }
}
