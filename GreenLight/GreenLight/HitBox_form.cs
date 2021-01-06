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

        RectHitbox test1, test2, test3, test4, test5, test6,test7,test8,test9,test10, test11, test12, test13, test14;
        CurvedHitbox curftest1, curftest2, curftest3, curftest4;

        List<testcord> Points1 = new List<testcord>();
        List<Hitbox> hitboxes = new List<Hitbox>();
        List<Color> colours = new List<Color>()
        {
            Color.Green,
            //Color.DarkViolet,
            Color.Red,
            //Color.DarkBlue,
            Color.DarkMagenta,
            Color.DarkOrange,
            Color.Black,
            Color.DarkGreen,
            Color.DarkViolet,
            Color.Brown,
            Color.Wheat,
            Color.YellowGreen,
            Color.Khaki,
            Color.Gold
        };

        public class testcord
        {

            public testcord(int _x, int _y, Color _color)
            {
                x = _x;
                y = _y;
                color = _color;
            }

            public Color color;
            public int x;
            public int y;
        }
        public HitBox_form()
        {
            this.Size = new Size(1500, 1000);

            /*
            test1 = new RectHitbox(new Point(50, 50), new Point(150, 50), new Point(150, 200), new Point(250, 200));
            test2 = new RectHitbox(new Point(300, 50), new Point(400, 50), new Point(300, 200), new Point(400, 200));
            test3 = new RectHitbox(new Point(550, 50), new Point(650, 50), new Point(450, 200), new Point(550, 200));
            test4 = new RectHitbox(new Point(50, 250), new Point(150, 250), new Point(200, 400), new Point(300, 400));
            test5 = new RectHitbox(new Point(550, 250), new Point(650, 250), new Point(400, 400), new Point(500, 400));
            test6 = new RectHitbox(new Point(150,400), new Point(300,450), new Point(150,500), new Point(300,550));
            test7 = new RectHitbox(new Point(450,450), new Point(600,400), new Point(450,550), new Point(600,500));
            test8 = new RectHitbox(new Point(750, 100), new Point(950, 100), new Point(750, 150), new Point(950, 150));
            test9 = new RectHitbox(new Point(700, 250), new Point(850, 200), new Point(700, 350), new Point(850, 300));
            test10 = new RectHitbox(new Point(950, 200), new Point(1100, 250), new Point(950, 300), new Point(1100, 350));
            test11 = new RectHitbox(new Point(800, 400), new Point(900, 400), new Point(650, 550), new Point(750, 550));
            test12 = new RectHitbox(new Point(950, 400), new Point(1050, 400), new Point(1100, 550), new Point(1200, 550));
            test13 = new RectHitbox(new Point(800, 600), new Point(900, 600), new Point(700, 750), new Point(800, 750));
            //test14 = new RectHitbox(new Point(950, 600), new Point(1050, 600), new Point(1050, 750), new Point(1150, 750));
            test14 = new RectHitbox(new Point(350, 850), new Point(400, 900),  new Point(300, 900), new Point(350, 950));

            //test7 = new RectHitbox(new Point(,), new Point(,), new Point(,), new Point(,));
            hitboxes.Add(test1);
            hitboxes.Add(test2);
            hitboxes.Add(test3);
            hitboxes.Add(test4);
            hitboxes.Add(test5);
            hitboxes.Add(test6);
            hitboxes.Add(test7);
            hitboxes.Add(test8);
            hitboxes.Add(test9);
            hitboxes.Add(test10);
            hitboxes.Add(test11);
            hitboxes.Add(test12);
            hitboxes.Add(test13);
            hitboxes.Add(test14);
            */


            curftest1 = new CurvedHitbox(new Point(100, 300), new Point(150, 300), new Point(300, 100), new Point(300, 150), "SE");
            hitboxes.Add(curftest1);

            curftest2 = new CurvedHitbox(new Point(300, 100), new Point(300, 150), new Point(500, 300), new Point(450, 300), "SW");
            hitboxes.Add(curftest2);

            curftest3 = new CurvedHitbox(new Point(500, 300), new Point(450, 300), new Point(300, 500), new Point(300, 450), "NW");
            hitboxes.Add(curftest3);

            curftest4 = new CurvedHitbox(new Point(300, 500), new Point(300, 450), new Point(100, 300), new Point(150, 300), "NE");
            hitboxes.Add(curftest4);

            this.Paint += teken;
            this.MouseClick += klik;


            DoTest(ref Points1);

            this.Invalidate();
        }

        public void teken(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            Pen _pen;
            Brush _brush;

            foreach(Hitbox _temp in hitboxes)
            {
                _brush = new SolidBrush(colours[hitboxes.IndexOf(_temp)]);
                _pen = new Pen(_brush);

                _temp.Draw(g);
            }

            foreach (testcord test in Points1)
            {
                _brush = new SolidBrush(test.color);
                g.FillRectangle(_brush, new Rectangle(new Point(test.x, test.y), new Size(4, 4)));
            }

            
        }

        public void klik(object o, MouseEventArgs mea)
        {
            //RectHitbox _temp = test4;

            /*
            Console.WriteLine("RC's: {0} - {1} - {2} - {3}",_temp.rcTop, _temp.rcBottom, _temp.rcLeft, _temp.rcRight);
            Console.WriteLine("B's: {0} - {1} - {2} - {3}", _temp.bTop, _temp.bBottom, _temp.bLeft, _temp.bRight);
            Console.WriteLine(colours[hitboxes.IndexOf(_temp)]);
            Console.WriteLine("---------------------------------"); */

            //Console.WriteLine(curftest1.midX + " -- " + curftest1.midY);

            Console.WriteLine(curftest1.Contains(mea.Location));
            Console.WriteLine();
        }

        public void DoTest(ref List<testcord> Points)
        {
            Points = new List<testcord>();
            int x, y;
            Random ran = new Random();
            Point _point;
            Color _color;

            for (int z = 0; z < 15000; z++)
            {
                x = ran.Next(0, this.Width);
                y = ran.Next(0, this.Height);

                _point = new Point(x, y);
                _color = Color.Gray;

                foreach(Hitbox _temp in hitboxes)
                {
                    if (_temp.Contains(_point))
                    {
                        _color = colours[hitboxes.IndexOf(_temp)];
                    }
                    Points.Add(new testcord(x, y, _color));
                }

            }
        }
    }
}
