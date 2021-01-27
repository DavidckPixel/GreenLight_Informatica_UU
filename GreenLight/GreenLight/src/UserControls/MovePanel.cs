using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    /* This is the class Move_panel.
       Because of this class the user can drag the program to his desire */
    class MovePanel : Panel
    {
        public MovePanel(Form _form) 
        {
            bool _draggable = false;
            int _mouseX = 0;
            int _mouseY = 0;

            if (_form.WindowState != FormWindowState.Maximized)
            {
                this.Cursor = Cursors.SizeAll; 
            }

            this.Location = new Point(0, 0);
            this.Size = new Size(250, 100);
            this.BackColor = Color.White;
            this.MouseDown += (object o, MouseEventArgs MEA) => { _draggable = true; _mouseX = Cursor.Position.X - _form.Left; _mouseY = Cursor.Position.Y - _form.Top; };
            
            this.MouseMove += (object o, MouseEventArgs MEA) =>
            {
                if (_draggable)
                {
                    _form.Top = Cursor.Position.Y - _mouseY;
                    _form.Left = Cursor.Position.X - _mouseX;
                }
            };

            this.MouseUp += (object o, MouseEventArgs MEA) => { _draggable = false; };
        }
    }
}
