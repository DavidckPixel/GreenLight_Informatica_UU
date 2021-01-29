using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    /* This is the Build main menu class. This class has a method AdjustSize to fit the size of the users window.
       In the initialize void the controls are added to the submenu.
       This user control is shown when the user is in the building screen. 
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */

    public partial class OldBuildMainMenu : UserControl
    {
        public OldBuildMainMenu(int _width, Form _form, FontFamily _dosisfontfamily)
        {
           this.BackColor = Color.FromArgb(196, 196, 198);
            this.Size = new Size(50,50);
            this.Location = new Point(0, _form.Height - 50);
            Initialize(_form, 50);

            _form.SizeChanged += (object o, EventArgs EA) =>
            {
                this.Size = new Size(_form.Width - _width, _form.Height);
                this.Controls.Clear();
                Initialize(_form, _width);
            };
        }

        public void AdjustSize(Form _form, int _submenuwidth)
        {
            /*this.Size = new Size(50,50);
            this.Location = new Point(0, _form.Height - 50);
            this.Controls.Clear();
            Initialize(_form,_submenuwidth);*/
        }

        private void Initialize(Form _form, int _submenuwidth) 
        {

            //this.Controls.Add(_infobutton);
        }

        
        protected override CreateParams CreateParams
       {
            get
           {
               CreateParams cp = base.CreateParams;
               cp.ExStyle |= 0x00000020;
               return cp;
           }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected void InvalidateEx()
        {
            if (this.Parent == null)
                return;
            Rectangle rc = new Rectangle(this.Location, this.Size);
            this.Parent.Invalidate(rc, true);
        }
    }
}
