using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public class Speedsign : PictureBox
    {
        public int speed;
        public Speedsign(Size size_, Point location_)
        {
            this.Size = size_;
            this.Location = location_;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Image = speedImage();

        }
        public Image speedImage()
        {
            switch (speed)
            {
                case 30:
                    return Image.FromFile("../../src/User Interface Recources/30Speed_sign.png");
                case 40:
                    return Image.FromFile("../../src/User Interface Recources/40Speed_sign.png");
                case 50:
                    return Image.FromFile("../../src/User Interface Recources/50Speed_sign.png");
                case 60:
                    return Image.FromFile("../../src/User Interface Recources/60Speed_sign.png");
                case 70:
                    return Image.FromFile("../../src/User Interface Recources/70Speed_sign.png");
                case 80:
                    return Image.FromFile("../../src/User Interface Recources/80Speed_sign.png");
                case 90:
                    return Image.FromFile("../../src/User Interface Recources/90Speed_sign.png");
                case 100:
                    return Image.FromFile("../../src/User Interface Recources/100Speed_sign.png");
                case 110:
                    return Image.FromFile("../../src/User Interface Recources/110Speed_sign.png");
                case 120:
                    return Image.FromFile("../../src/User Interface Recources/120Speed_sign.png");
                case 130:
                    return Image.FromFile("../../src/User Interface Recources/130Speed_sign.png");
                case 0:
                    return Image.FromFile("../../src/User Interface Recources/QMSpeed_sign.png");
                default:
                    return null;
                    break;
            }
        }
    }
}
