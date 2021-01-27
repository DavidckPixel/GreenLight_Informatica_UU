using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace GreenLight
{
    public static class DrawData
    {
        //General Tools used in visual effects

        static PrivateFontCollection Font_collection = new PrivateFontCollection(); 
        public static FontFamily Dosis_font_family;

        static DrawData()
        {
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            Dosis_font_family = Font_collection.Families[0];
        }

        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }

        public static Image BetterRotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
    }
}
