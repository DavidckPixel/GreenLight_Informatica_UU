using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Recthitbox is our own better version of the Rect Class in c#, it inherits from the abstract hitbox class.
//It allows for the creation of odd shaped hitboxes (like the shape of our roads) and does a bunch of math to
//figure out if a given point is inside this shape not.

namespace GreenLight
{
    public class RectHitbox : Hitbox
    {
        public Point topleft, topright, bottomleft, bottomright;

        public double? rcTop, rcBottom, rcLeft, rcRight;
        public double? bTop, bBottom, bLeft, bRight;

        public RectHitbox(Point _topleft, Point _topright, Point _bottomleft, Point _bottomright, Color _color) : base(_topleft, _topright, _bottomleft, _bottomright)
        {
            bottomleft = _bottomleft;
            bottomright = _bottomright;
            topleft = _topleft;
            topright = _topright;

            Topcord = new Point(BetterMin(_topleft.X, _topright.X, _bottomleft.X, _bottomright.X), BetterMin(_topleft.Y, _topright.Y, _bottomleft.Y, _bottomright.Y));

            double topleftX, topleftY, toprightX, toprightY, bottomleftX, bottomleftY, bottomrightX, bottomrightY;

            topleftX = _topleft.X;
            topleftY = _topleft.Y;

            toprightX = _topright.X;
            toprightY = _topright.Y;

            bottomleftX = _bottomleft.X;
            bottomleftY = _bottomleft.Y;

            bottomrightX = _bottomright.X;
            bottomrightY = _bottomright.Y;

            this.color = _color;

            //----------------------------------------------------------------

            if ((toprightX - topleftX) != 0)
            {
                rcTop = (toprightY - topleftY) / (toprightX - topleftX);
                bTop = topleftY - rcTop * topleftX;
            }
            else
            {
                Console.WriteLine("??");
                rcTop = null;
                bTop = topleftX;
            }

            if ((bottomrightX - bottomleftX) != 0)
            {
                rcBottom = (bottomrightY - bottomleftY) / (bottomrightX - bottomleftX);
                bBottom = bottomleftY - rcBottom * bottomleftX;
            }
            else
            {
                Console.WriteLine("??");
                rcBottom = null;
                bBottom = bottomleftX;
            }

            if ((topleftX - bottomleftX) != 0)
            {
                rcLeft = (topleftY - bottomleftY) / (topleftX - bottomleftX);
                bLeft = bottomleftY - rcLeft * bottomleftX;
            }
            else
            {
                Console.WriteLine("??");
                rcLeft = null;
                bLeft = bottomleftX;
            }

            if ((toprightX - bottomrightX) != 0)
            {
                rcRight = (toprightY - bottomrightY) / (toprightX - bottomrightX);
                bRight = toprightY - rcRight * toprightX;
            }
            else
            {
                Console.WriteLine("??");
                rcRight = null;
                bRight = toprightX;
            }
        }

        public override bool Contains(Point _p)
        {
            try
            {
                if (rcLeft == 0 || rcRight == 0)
                {
                    if (_p.X > bRight && bLeft > _p.X && _p.Y > bTop && bBottom > _p.Y)
                    {
                        return true;
                    }
                }
                else if ((rcTop == 0 || rcBottom == 0) && (rcLeft == null || rcRight == null))
                {
                    if (bLeft <= _p.X && bRight >= _p.X && bTop <= _p.Y && bBottom >= _p.Y)
                    {
                        return true;
                    }

                }
                else if(rcTop == null || rcBottom == null)
                {
                    int _maxX = Math.Max(this.topleft.X, this.bottomleft.X);
                    int _minX = Math.Min(this.topleft.X, this.bottomleft.X);

                    if (_p.X >= _minX && _p.X <= _maxX && _p.X * rcLeft + bLeft <= _p.Y && _p.X * rcRight + bRight >= _p.Y)
                    {
                        return true;
                    }
                }
                else if (rcLeft == null || rcRight == null)
                {
                    if (_p.X > this.topleft.X && _p.X < this.topright.X && _p.X * rcTop + bTop <= _p.Y && _p.X * rcBottom + bBottom >= _p.Y)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((_p.Y - bLeft) / rcLeft <= _p.X && (_p.Y - bRight) / rcRight >= _p.X && _p.X * rcTop + bTop <= _p.Y && _p.X * rcBottom + bBottom >= _p.Y)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public override bool Collide(RectHitbox _h)
        {
            bool _temp = (this.Contains(_h.topright) || this.Contains(_h.topleft) || this.Contains(_h.bottomright) || this.Contains(_h.bottomleft));
            return _temp;
        }

        public override bool Equals(object o)
        {
            try
            {
                RectHitbox _h = (RectHitbox)o;
                bool _temp = (this.topright == _h.topright && this.topleft == _h.topleft && this.bottomleft == _h.bottomleft && this.bottomright == _h.bottomright);
                return _temp;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public override void Draw(Graphics g)
        {
            bool draw = General_Form.Main == null ? true : General_Form.Main.BuildScreen.Toggle;

            if (draw)
            {
                Brush Notsolid = new SolidBrush(Color.FromArgb(100, this.color));
                Point[] _points = new Point[]
                {
                topleft, topright, bottomright, bottomleft
                };

                g.FillPolygon(Notsolid, _points);
            }
        }
    }
}
