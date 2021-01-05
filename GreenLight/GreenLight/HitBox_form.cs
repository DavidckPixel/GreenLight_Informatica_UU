using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public partial class HitBox_form : Form
    {

        RectHitbox test1, test2, test3, test4;

        List<testcord> Points1 = new List<testcord>();

        public class testcord
        {

            public testcord(bool _a, int _x, int _y)
            {
                answer = _a;
                x = _x;
                y = _y;
            }

            public bool answer;
            public int x;
            public int y;
        }
        public HitBox_form()
        {
            this.Size = new Size(1000, 1000);

            test1 = new RectHitbox(new Point(100, 100), new Point(200, 100), new Point(100, 200), new Point(200, 200));
            test2 = new RectHitbox(new Point(300, 100), new Point(100, 300), new Point(500, 300), new Point(300, 500));
            test3 = new RectHitbox(new Point(600, 100), new Point(650, 100), new Point(450, 800), new Point(500, 800));
            test4 = new RectHitbox(new Point(200, 400), new Point(200, 500), new Point(100, 400), new Point(100, 500));

            this.Paint += teken;
            this.MouseClick += klik;


            Points1 = DoTest(Points1, "test3");

            this.Invalidate();
        }

        public void teken(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            g.DrawLine(Pens.Green, test1.topright, test1.topleft);
            g.DrawLine(Pens.Green, test1.topleft, test1.bottomleft);
            g.DrawLine(Pens.Green, test1.bottomright, test1.topright);
            g.DrawLine(Pens.Green, test1.bottomleft, test1.bottomright);

            g.DrawLine(Pens.Blue, test2.topright, test2.topleft);
            g.DrawLine(Pens.Blue, test2.topleft, test2.bottomleft);
            g.DrawLine(Pens.Blue, test2.bottomright, test2.topright);
            g.DrawLine(Pens.Blue, test2.bottomleft, test2.bottomright);

            g.DrawLine(Pens.Red, test3.topright, test3.topleft);
            g.DrawLine(Pens.Red, test3.topleft, test3.bottomleft);
            g.DrawLine(Pens.Red, test3.bottomright, test3.topright);
            g.DrawLine(Pens.Red, test3.bottomleft, test3.bottomright);

            g.DrawLine(Pens.Purple, test4.topright, test4.topleft);
            g.DrawLine(Pens.Purple, test4.topleft, test4.bottomleft);
            g.DrawLine(Pens.Purple, test4.bottomright, test4.topright);
            g.DrawLine(Pens.Purple, test4.bottomleft, test4.bottomright);

            foreach (testcord test in Points1)
            {
                if (test.answer)
                {
                    g.FillRectangle(Brushes.Red, new Rectangle(new Point(test.x, test.y), new Size(4, 4)));
                }
                else
                {
                    g.FillRectangle(Brushes.Orange, new Rectangle(new Point(test.x, test.y), new Size(4, 4)));
                }
            }
        }

        public void klik(object o, MouseEventArgs mea)
        {
            Console.WriteLine("Rectangle SQUARE: {0}", test1.Contains(mea.Location));
            Console.WriteLine("Rectangle TITLED: {0}", test2.Contains(mea.Location));
            Console.WriteLine("Rectangle ROAD: {0}", test3.Contains(mea.Location));
            Console.WriteLine("---------------------------------");
        }

        public List<testcord> DoTest(List<testcord> Points, string testt)
        {
            Points = new List<testcord>();
            int x, y;
            Random ran = new Random();

            for (int z = 0; z < 3000; z++)
            {
                x = ran.Next(0, 1000);
                y = ran.Next(0, 1000);

                if (testt == "test3")
                {
                    Points.Add(new testcord(test3.Contains(new Point(x, y)), x, y));
                }
                if (testt == "test2")
                {
                    Points.Add(new testcord(test2.Contains(new Point(x, y)), x, y));
                }
                if (testt == "test1")
                {
                    Points.Add(new testcord(test1.Contains(new Point(x, y)), x, y));
                }
            }

            return Points;
        }
    }
}
