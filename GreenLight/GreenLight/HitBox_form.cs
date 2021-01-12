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

        Hitbox test1, test2, test3, test4, test5, test6,test7,test8,test9,test10, test11, test12, test13, test14;
        Hitbox curftest1, curftest2, curftest3, curftest4, curftest5;

        Hitbox Weirdbox;

        private void HitBox_form_Load(object sender, EventArgs e)
        {

        }

        RectHitbox Under1, Under2, Under3, Under4, Under5, Under6, Under7, Under8;
        RectHitbox Up1, Up2, Up3, Up4, Up5;
        RectHitbox Right1, Right2, Right3;
        CurvedHitbox Q1, Q2, Q3, Q4;

        List<testcord> Points1 = new List<testcord>();
        List<Hitbox> hitboxes = new List<Hitbox>();
        List<Hitbox> hitboxes2 = new List<Hitbox>();
        List<Color> colours = new List<Color>()
        {
            Color.Green,
            Color.DarkViolet,
            Color.Red,
            Color.DarkBlue,
            Color.DarkMagenta,
            Color.DarkOrange,
            Color.Black,
            Color.DarkGreen,
            Color.DarkViolet,
            Color.Brown,
            Color.Wheat,
            Color.YellowGreen,
            Color.Khaki,
            Color.Gold,
            Color.DarkOliveGreen,
            Color.PaleVioletRed,
            Color.Honeydew,
            Color.SpringGreen,
            Color.MediumSlateBlue,
            Color.Crimson,
            Color.Fuchsia,
            Color.PapayaWhip,
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
            this.Size = new Size(1000, 1000);

            /*
            test1 = new RectHitbox(new Point(50, 50), new Point(150, 50), new Point(150, 200), new Point(250, 200));
            test2 = new RectHitbox(new Point(300, 50), new Point(400, 50), new Point(300, 200), new Point(400, 200));
            test3 = new RectHitbox(new Point(550, 50), new Point(650, 50), new Point(450, 200), new Point(550, 200));
            test4 = new RectHitbox(new Point(50, 250), new Point(150, 250), new Point(200, 400), new Point(300, 400));
            test5 = new RectHitbox(new Point(550, 250), new Point(650, 250), new Point(400, 400), new Point(500, 400));
            test6 = new RectHitbox(new Point(150, 400), new Point(300, 450), new Point(150, 500), new Point(300, 550));
            test7 = new RectHitbox(new Point(450, 450), new Point(600, 400), new Point(450, 550), new Point(600, 500));
            test8 = new RectHitbox(new Point(750, 100), new Point(950, 100), new Point(750, 150), new Point(950, 150));
            test9 = new RectHitbox(new Point(700, 250), new Point(850, 200), new Point(700, 350), new Point(850, 300));
            test10 = new RectHitbox(new Point(950, 200), new Point(1100, 250), new Point(950, 300), new Point(1100, 350));
            test11 = new RectHitbox(new Point(800, 400), new Point(900, 400), new Point(650, 550), new Point(750, 550));
            test12 = new RectHitbox(new Point(950, 400), new Point(1050, 400), new Point(1100, 550), new Point(1200, 550));
            test13 = new RectHitbox(new Point(800, 600), new Point(900, 600), new Point(700, 750), new Point(800, 750));
            //test14 = new RectHitbox(new Point(950, 600), new Point(1050, 600), new Point(1050, 750), new Point(1150, 750));
            test14 = new RectHitbox(new Point(350, 850), new Point(400, 900), new Point(300, 900), new Point(350, 950));

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

            /*
            
            Up1 = new RectHitbox(new Point(175, 250), new Point(225, 250), new Point(275, 350), new Point(325, 350),Color.Yellow);
            Up2 = new RectHitbox(new Point(275, 225), new Point(325, 225), new Point(325, 350), new Point(375, 350),Color.Yellow);
            Up3 = new RectHitbox(new Point(375, 200), new Point(425, 200), new Point(375, 350), new Point(425, 350), Color.Yellow);
            Up4 = new RectHitbox(new Point(475, 225), new Point(525, 225), new Point(425,350), new Point(475,350), Color.Yellow);
            Up5 = new RectHitbox(new Point(575,250), new Point(625,250), new Point(475,350), new Point(525,350), Color.Yellow);

            hitboxes.Add(Up1);
            hitboxes.Add(Up2);
            hitboxes.Add(Up3);
            hitboxes.Add(Up4);
            hitboxes.Add(Up5);

            Right1 = new RectHitbox(new Point(525, 350), new Point(625, 300), new Point(525, 400), new Point(625, 350), Color.Yellow);
            Right2 = new RectHitbox(new Point(525, 400), new Point(650, 400), new Point(525, 450), new Point(650, 450), Color.Yellow);
            Right3 = new RectHitbox(new Point(525, 450), new Point(625, 500), new Point(525, 500), new Point(625, 550), Color.Yellow);

            hitboxes.Add(Right1);
            hitboxes.Add(Right2);
            hitboxes.Add(Right3);

            Under1 = new RectHitbox(new Point(175,300), new Point(275,350), new Point(175, 350), new Point(275,400), Color.Yellow);
            Under2 = new RectHitbox(new Point(150,400), new Point(275,400), new Point(150,450), new Point(275,450), Color.Yellow);
            Under3 = new RectHitbox(new Point(175,500), new Point(275,450), new Point(175,550), new Point(275,500), Color.Yellow); //
            Under4 = new RectHitbox(new Point(275,500), new Point(325,500), new Point(175,600), new Point(225,600), Color.Yellow);
            Under5 = new RectHitbox(new Point(325,500), new Point(375,500), new Point(275,625), new Point(325,625), Color.Yellow);
            Under6 = new RectHitbox(new Point(375,500), new Point(425,500), new Point(375,650), new Point(425,650), Color.Yellow);
            Under7 = new RectHitbox(new Point(425,500), new Point(475,500), new Point(475,625), new Point(525,625), Color.Yellow);
            Under8 = new RectHitbox(new Point(475,500), new Point(525,500), new Point(575,600), new Point(625,600), Color.Yellow);

            hitboxes.Add(Under1);
            hitboxes.Add(Under2);
            hitboxes.Add(Under3);
            hitboxes.Add(Under4);
            hitboxes.Add(Under5);
            hitboxes.Add(Under6);
            hitboxes.Add(Under7);
            hitboxes.Add(Under8);
            */

            Weirdbox = new RectHitbox(new Point(32,93), new Point(32,163), new Point(496,200), new Point(496,270), Color.Red);
            hitboxes.Add(Weirdbox);


            Q1 = new CurvedHitbox(new Point(25 , 425 ), new Point( 75, 425), new Point(400, 75 ), new Point(400, 125), "SE", Color.Yellow);
            Q2 = new CurvedHitbox(new Point(400, 75), new Point(400, 125), new Point( 775, 425), new Point(725, 425), "SW", Color.Yellow);
            Q3 = new CurvedHitbox(new Point(775, 425), new Point(725, 425), new Point( 400, 775), new Point( 400, 725 ), "NW", Color.Yellow);
            Q4 = new CurvedHitbox(new Point(400, 775), new Point(400, 725), new Point(25, 425), new Point(75, 425), "NE", Color.Yellow);

            hitboxes2.Add(Q1);
            hitboxes2.Add(Q2);
            hitboxes2.Add(Q3);
            hitboxes2.Add(Q4); 
            




            //curftest1 = new CurvedHitbox(new Point(100, 300), new Point(150, 300), new Point(300, 100), new Point(300, 150), "SE");
            // hitboxes.Add(curftest1);

            //curftest2 = new CurvedHitbox(new Point(300, 100), new Point(300, 150), new Point(500, 300), new Point(450, 300), "SW");
            //hitboxes.Add(curftest2);

            //curftest3 = new CurvedHitbox(new Point(500, 300), new Point(450, 300), new Point(300, 500), new Point(300, 450), "NW");
            //hitboxes.Add(curftest3);

            //curftest4 = new CurvedHitbox(new Point(300, 500), new Point(300, 450), new Point(100, 300), new Point(150, 300), "NE");
            //hitboxes.Add(curftest4);

            //curftest5 = new CurvedHitbox(new Point (100, 600), new Point(150, 600), new Point(800, 400), new Point(800, 450), "SE");
            //hitboxes.Add(curftest5);

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

            foreach (Hitbox _temp in hitboxes2)
            {
                _brush = new SolidBrush(colours[hitboxes2.IndexOf(_temp)]);
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
            Console.WriteLine();
        }

        public void DoTest(ref List<testcord> Points)
        {
            Points = new List<testcord>();
            int x, y;
            Random ran = new Random();
            Point _point;
            Color _color;

            for (int z = 0; z < 20000; z++)
            {
                x = ran.Next(0, this.Width);
                y = ran.Next(0, this.Height);

                _point = new Point(x, y);
                _color = Color.Gray;

                foreach(Hitbox _temp in hitboxes)
                {
                    if (_temp.Contains(_point) )
                    {
                        _color = colours[hitboxes.IndexOf(_temp)];
                    }
                    Points.Add(new testcord(x, y, _color));
                }

                foreach (Hitbox _temp in hitboxes2)
                {
                    if (_temp.Contains(_point))
                    {
                        _color = colours[hitboxes2.IndexOf(_temp)];
                    }
                    Points.Add(new testcord(x, y, _color));
                }

            }
        }
    }
}
