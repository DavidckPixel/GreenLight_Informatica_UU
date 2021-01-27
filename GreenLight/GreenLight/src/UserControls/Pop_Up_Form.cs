using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace GreenLight
{
    public partial class Pop_Up_Form : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public Pop_Up_Form(Size Form_size)
        {
            this.Size = Form_size;

            this.StartPosition = FormStartPosition.CenterParent;

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
        }
    }
}
