using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GreenLight
{
    // CurvedHitbox is a object that can be created given 4 points. It then uses a bunch of math to calculate a arc-shaped polygon
    // it also contains a Contain function that can be used to to see if a certain point is within the hitbox.

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
            rect = Rect();
            this.PenWidth = Math.Abs((int)(max_radiusX - min_radiusX));
        }

        // This method creates the actual CurvedHitbox
        public Rectangle Rect()
        {
            Rectangle _rect = new Rectangle(-100,-100,1,1);

            double max_startX, max_startY, min_startX, min_startY, max_endX, max_endY, min_endX, min_endY;

            max_startX = max_start.X;
            max_startY = max_start.Y;

            min_startX = min_start.X;
            min_startY = min_start.Y;

            max_endX = max_end.X;
            max_endY = max_end.Y;

            min_endX = min_end.X;
            min_endY = min_end.Y;

            if (_lanes != 0 && _lanes % 2 == 0 && (dir == "SE" || dir == "NW"))
            {
                max_startX += 20;
                min_startX += 20;

                max_endY += 20;
                min_endY += 20;
            }

            max_radiusX = Math.Abs(max_endX - max_startX);
            max_radiusY = Math.Abs(max_startY - max_endY);

            min_radiusX = Math.Abs(min_endX - min_startX);
            min_radiusY = Math.Abs(min_startY - min_endY);

            int mid_radiusX = (int)(max_radiusX + min_radiusX) / 2;
            int mid_radiusY = (int)(max_radiusY + min_radiusY) / 2;


            switch (dir)
            {
                case "SE":
                    {
                        midX = max_endX;
                        midY = max_startY;
                        start_angle = 180;
                        mid_startX = (int)(max_startX + min_startX) / 2 + 10;
                        mid_startY = (int)max_startY;
                        mid_endX = (int)max_endX;
                        mid_endY = (int)(max_endY + min_endY) / 2 + 10;


                       _rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
                    }
                    break;
                case "SW":
                    {

                        midX = max_startX;
                        midY = max_endY;

                        start_angle = 270;
                        mid_startX = (int)max_startX;
                        mid_startY = (int)(max_startY + min_startY) / 2;

                        _rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
                    }
                    break;
                case "NW":
                    {
                        midX = max_endX;
                        midY = max_startY;
                        start_angle = 0;
                        mid_startX = (int)(max_startX + min_startX) / 2;
                        mid_startY = (int)max_startY;

                        _rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
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
                        
                        _rect = new Rectangle(new Point((int)midX - mid_radiusX, (int)midY - mid_radiusY), new Size(mid_radiusX * 2, mid_radiusY * 2));
                    }
                    break;
            }
            return _rect;
        }

        // This method is used to check if a point is inside the CurvedHitbox and is important for a lot of parts of our code.
        public override bool Contains(Point _p)
        {
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

        // This method builds upon the Contains method to see if two Hitboxes overlap
        public override bool Collide(Hitbox _h, Graphics g)
        {
            bool _temp = false;
            if (_h.Type == "Rect")
            {
                for (int x = 5; x < _h.lanepoints.Count() - 10; x += 4)
                {
                    _temp = this.Contains(_h.lanepoints[x].cord);

                    if (_temp == true)
                    {
                        return _temp;
                    }
                }
            }
            else if (_h.Type == "Curved")
            {
                CurvedHitbox box = (CurvedHitbox)_h;
                List<LanePoints> _lanepoints = LanePoints.CalculateCurveLane(new Point(box.mid_startX, box.mid_startY), new Point(box.mid_endX, box.mid_endY), box.dir);
                for (int x = 5; x < _lanepoints.Count() - 10; x += 12)
                {
                    _temp = this.Contains(_lanepoints[x].cord);

                    if (_temp == true)
                    {
                        return _temp;
                    }
                }
            }

            return _temp;
        }

        public override void Draw(Graphics g)
        {
            Pen _pen = new Pen(new SolidBrush(Color.FromArgb(100, this.color)), PenWidth);

            if (_lanes != 0 && _lanes % 2 == 0)
                rect = Rect();

            bool draw = General_Form.Main == null || General_Form.Main.BuildScreen.Toggle;

            if (draw)
            {
                g.DrawArc(_pen, rect, start_angle, 90);
            }

        }

        public override void ShowOverlap(Graphics g)
        {
            Pen _pen = new Pen(new SolidBrush(Color.FromArgb(100, this.color)), PenWidth);
            g.DrawArc(_pen, rect, start_angle, 90);
        }
    }
}
