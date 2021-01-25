using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//CurvedHitbox is a object that can be created given 4 points. It then uses a bunch of math to calculate a arc shape polygon
//it also contains a Contain(point p) function that can be used to to see if a certain point is within the hitbox.

namespace GreenLight
{
    class CurvedHitbox : Hitbox
    {
        public Point max_start, min_start, max_end, min_end;
        public string dir;
        public double midX, midY;
        double max_radiusX, max_radiusY;
        double min_radiusX, min_radiusY;
        int start_angle;
        public int mid_startX, mid_startY, mid_endX, mid_endY;
        Rectangle rect;
        

        int PenWidth;

        public CurvedHitbox(Point _start1, Point _start2, Point _end1, Point _end2, string _dir, Color _color) : base(_start1, _start2, _end1, _end2)
        {
            max_start = _start1;
            min_start = _start2;
            max_end = _end1;
            min_end = _end2;
            dir = _dir;

            this.color = _color;

            this.Type = "Curved";

            //----------------------------------------------- 
            // TYPE CASTING TO DOUBLE

            double max_startX, max_startY, min_startX, min_startY, max_endX, max_endY, min_endX, min_endY;

            max_startX = max_start.X;
            max_startY = max_start.Y;

            min_startX = min_start.X;
            min_startY = min_start.Y;

            max_endX = max_end.X;
            max_endY = max_end.Y;

            min_endX = min_end.X;
            min_endY = min_end.Y;

            Console.WriteLine("{0} - {1} - {2} - {3}", max_startX, max_startY, min_startX, min_startY);
            Console.WriteLine("{0} - {1} - {2} - {3}", max_endX, max_endY, min_endX, min_endY);

            //--------------------------------------------------
            //CALCULATE RADIUS

            max_radiusX = Math.Abs(max_endX - max_startX);
            max_radiusY = Math.Abs(max_startY - max_endY);

            min_radiusX = Math.Abs(min_endX - min_startX);
            min_radiusY = Math.Abs(min_startY - min_endY);

            int mid_radiusX = (int)(max_radiusX + min_radiusX) / 2;
            int mid_radiusY = (int)(max_radiusY + min_radiusY) / 2;

            //---------------------------------------------


            switch (_dir)
            {
                case "SE":
                    {
                        midX = max_endX;
                        midY = max_startY;
                        start_angle = 180;
                        mid_startX = (int)(max_startX + min_startX) / 2;
                        mid_startY = (int)max_startY;
                        mid_endX = (int)max_endX;
                        mid_endY = (int)(max_endY + min_endY) / 2;

                        rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));

                        //rect = new Rectangle(new Point(Math.Min(points[0].cord.X, points[points.Count - 1].cord.X), Math.Min(points[0].cord.Y, points[points.Count - 1].cord.Y)), size);

                    }
                    break;
                case "SW":
                    {
                        midX = max_startX;
                        midY = max_endY;

                        start_angle = 270;
                        mid_startX = (int)max_startX;
                        mid_startY = (int)(max_startY + min_startY) / 2;

                        rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
                    }
                    break;
                case "NW":
                    {
                        midX = max_endX;
                        midY = max_startY;
                        start_angle = 0;
                        mid_startX = (int)(max_startX + min_startX) / 2;
                        mid_startY = (int)max_startY;

                        rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
                    }
                    break;
                case "NE":
                    {
                        midX = max_startX;
                        midY = max_endY;
                        start_angle = 90;
                        mid_startX = (int)max_startX;
                        mid_startY = (int)(max_startY + min_startY) / 2;
                        mid_endX = (int)(max_endX + min_endX) / 2;
                        mid_endY = (int)max_endY;

                        rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
                    }
                    break;
            }

            //Console.WriteLine(max_radiusX);
            //Console.WriteLine(max_radiusY);
            //Console.WriteLine(min_radiusX);
            //Console.WriteLine(min_radiusY);
            //Console.WriteLine();

            this.PenWidth = Math.Abs((int)(max_radiusX - min_radiusX));
        }

        public override bool Contains(Point _p)
        {
            //Console.WriteLine((Math.Pow(_p.X - midX, 2 ) / Math.Pow(max_radiusX, 2) + Math.Pow(_p.Y - midY,2) / Math.Pow(max_radiusY,2)));
            //Console.WriteLine((Math.Pow(_p.X - midX, 2) / Math.Pow(min_radiusX, 2) + Math.Pow(_p.Y - midY, 2) / Math.Pow(min_radiusY, 2)));

            if ((Math.Pow(_p.X - midX, 2) / Math.Pow(max_radiusX, 2) + Math.Pow(_p.Y - midY, 2) / Math.Pow(max_radiusY, 2)) < 1
                            && (Math.Pow(_p.X - midX, 2) / Math.Pow(min_radiusX, 2) + Math.Pow(_p.Y - midY, 2) / Math.Pow(min_radiusY, 2)) > 1) {

                switch (dir)
                {
                    case "SE":
                        {
                            if (_p.X < midX && _p.Y < midY)
                            {
                                return true;
                            }
                        }
                        break;
                    case "SW":
                        {
                            if (_p.X > midX && _p.Y < midY)
                            {
                                return true;
                            }
                        }
                        break;
                    case "NW":
                        {
                            if (_p.X > midX && _p.Y > midY)
                            {
                                return true;
                            }
                        }
                        break;
                    case "NE":
                        {
                            if (_p.X < midX && _p.Y > midY)
                            {
                                return true;
                            }
                        }
                        break;
                }
            }

            return false;
        }

        public override bool Collide(Hitbox _h, Graphics g)
        {
            bool _temp = false;
            if (_h.Type == "Rect")
            {
                RectHitbox box = (RectHitbox)_h;
                _temp = (this.Contains(box.topright) || this.Contains(box.topleft) || this.Contains(box.bottomright) || this.Contains(box.bottomleft));
            }
            else if (_h.Type == "Curved")
            {
                CurvedHitbox box = (CurvedHitbox)_h;
                _temp = (this.Contains(box.max_start) || this.Contains(box.max_end) || this.Contains(box.min_end) || this.Contains(box.min_start));
            }

            return _temp;
        }

        public override void Draw(Graphics g)
        {
            Brush _brush = new SolidBrush(Color.FromArgb(100, Color.Black));
            Pen _pen = new Pen(new SolidBrush(Color.FromArgb(100, this.color)), PenWidth);

            bool draw = General_Form.Main == null ? true : General_Form.Main.BuildScreen.Toggle;

            //g.DrawRectangle(_pen, rect);
            if (draw)
            {
                g.DrawArc(_pen, rect, start_angle, 90);

                //g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), rect);
                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), new Rectangle(new Point((int)midX, (int)midY), new Size(5, 5)));


                g.FillRectangle(_brush, new Rectangle(max_start, new Size(5, 5)));
                g.FillRectangle(_brush, new Rectangle(min_start, new Size(5, 5)));
                g.FillRectangle(_brush, new Rectangle(max_end, new Size(5, 5)));
                g.FillRectangle(_brush, new Rectangle(min_end, new Size(5, 5)));
            }

        }

        /*public override bool Contains(RectHitbox _h)
        {
            throw new NotImplementedException();
        } */
    }
}
