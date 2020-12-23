using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class DrivingLane
    {
        //Every road has a list of these DrivingLanes, a driving lane consists of a list of LanePoints
        //And for now an int that determines which type of road it is.
        //Each object from this class also has its own Draw feature, this draw feature
        //Draws a straight lane between all the points in the LanePoints list in order.
        //This is used for testing to see if our algorithm created a smooth road -- This will not be used in final release.


        public List<LanePoints> points;
        Bitmap Lane;
        Bitmap Verticallane;

        public DrivingLane(List<LanePoints> _points)
        {
            this.points = _points;
            Lane = new Bitmap(Properties.Resources.Lane);
            Verticallane = new Bitmap(Properties.Resources.Road_Verticaal);
        }

        public void Draw(Graphics g)
        {
            try
            {
                Point _pointtemp = points[0].cord;

                foreach (LanePoints x in points)
                {
                    g.FillEllipse(Brushes.Gray, _pointtemp.X - 20, _pointtemp.Y - 20, 40, 40);
                    _pointtemp = x.cord;
                   
                }
            }
           catch(Exception e) { }
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

        public void LogPoints()
        {
            foreach(LanePoints _point in this.points)
            {
                Console.WriteLine(_point.cord);
            }
        }
    }
}
