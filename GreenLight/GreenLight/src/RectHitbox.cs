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
        //public Point midTop, midTopLeft, midTopRight, midBottom, midBottomLeft, midBottomRight, midLeft, midLeftBottom, midLeftTop, midRight, midRightBottom, midRightTop;
        //public Point mid, midTopMid, midBottomMid, midLeftMid, midRightMid, midTopLeftMid, midTopRightMid, midBottomLeftMid, midBottomRightMid;

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

            this.Type = "Rect";

            this.lanepoints = CalculateLanePoints();
            //----------------------------------------------------------------



            //----------------------------------------------------------------

            if ((toprightX - topleftX) != 0)
            {
                rcTop = (toprightY - topleftY) / (toprightX - topleftX);
                bTop = topleftY - rcTop * topleftX;
            }
            else
            {
                //Console.WriteLine("??");
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
                //Console.WriteLine("??");
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
                //Console.WriteLine("??");
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
                //Console.WriteLine("??");
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
                else if (rcTop == null || rcBottom == null)
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
                

                /*Point boxmidTop = new Point((box.topright.X + box.topleft.X) / 2, (box.topright.Y + box.topleft.Y) / 2);
                Point boxmidTopLeft = new Point((box.topleft.X + boxmidTop.X) / 2, (box.topleft.Y + boxmidTop.Y) / 2);
                Point boxmidTopRight = new Point((box.topright.X + boxmidTop.X) / 2, (box.topright.Y + boxmidTop.Y) / 2);

                bool top = (this.Contains(boxmidTop) || this.Contains(boxmidTopLeft) || this.Contains(boxmidTopRight));

                Point boxmidBottom = new Point((box.bottomright.X + box.bottomleft.X) / 2, (box.bottomright.Y + box.bottomleft.Y) / 2);
                Point boxmidBottomLeft = new Point((box.bottomleft.X + boxmidBottom.X) / 2, (box.bottomleft.Y + boxmidBottom.Y) / 2);
                Point boxmidBottomRight = new Point((box.bottomright.X + boxmidBottom.X) / 2, (box.bottomright.Y + boxmidBottom.Y) / 2);

                bool bottom = (this.Contains(boxmidBottom) || this.Contains(boxmidBottomLeft) || this.Contains(boxmidBottomRight));

                Point boxmidLeft = new Point((box.topleft.X + box.bottomleft.X) / 2, (box.bottomleft.Y + box.topleft.Y) / 2);
                Point boxmidLeftBottom = new Point((box.bottomleft.X + boxmidLeft.X) / 2, (box.bottomleft.Y + boxmidLeft.Y) / 2);
                Point boxmidLeftTop = new Point((box.topleft.X + boxmidLeft.X) / 2, (box.topleft.Y + boxmidLeft.Y) / 2);

                bool left = (this.Contains(boxmidLeft) || this.Contains(boxmidLeftBottom) || this.Contains(boxmidLeftTop));

                Point boxmidRight = new Point((box.topright.X + box.bottomright.X) / 2, (box.bottomright.Y + box.topright.Y) / 2);
                Point boxmidRightBottom = new Point((box.bottomright.X + boxmidRight.X) / 2, (box.bottomright.Y + boxmidRight.Y) / 2);
                Point boxmidRightTop = new Point((box.topright.X + boxmidRight.X) / 2, (box.topright.Y + boxmidRight.Y) / 2);

                bool right = (this.Contains(boxmidRight) || this.Contains(boxmidRightBottom) || this.Contains(boxmidRightTop));

                Point boxmid = new Point((boxmidTop.X + boxmidBottom.X) / 2, (boxmidTop.Y + boxmidBottom.Y) / 2);
                Point boxmidTopMid = new Point((boxmid.X + boxmidTop.X) / 2, (boxmid.Y + boxmidTop.Y) / 2);
                Point boxmidBottomMid = new Point((boxmid.X + boxmidBottom.X) / 2, (boxmid.Y + boxmidBottom.Y) / 2);
                Point boxmidLeftMid = new Point((boxmid.X + boxmidLeft.X) / 2, (boxmid.Y + boxmidLeft.Y) / 2);
                Point boxmidRightMid = new Point((boxmid.X + boxmidRight.X) / 2, (boxmid.Y + boxmidRight.Y) / 2);

                bool mid = (this.Contains(boxmid) || this.Contains(boxmidTopMid) || this.Contains(boxmidBottomMid) || this.Contains(boxmidLeftMid) || this.Contains(boxmidRightMid));

                Point boxmidTopLeftMid = new Point((boxmidTop.X + boxmidLeft.X) / 2, (boxmidTop.X + boxmidLeft.X) / 2);
                Point boxmidTopRightMid = new Point((boxmidTop.X + boxmidRight.X) / 2, (boxmidTop.X + boxmidRight.X) / 2);
                Point boxmidBottomLeftMid = new Point((boxmidBottom.X + boxmidLeft.X) / 2, (boxmidBottom.X + boxmidLeft.X) / 2);
                Point boxmidBottomRightMid = new Point((boxmidBottom.X + boxmidRight.X) / 2, (boxmidBottom.X + boxmidRight.X) / 2);

                bool midmid = (this.Contains(boxmidTopLeftMid) || this.Contains(boxmidTopRightMid) || this.Contains(boxmidBottomLeftMid) || this.Contains(boxmidBottomRightMid));
                bool corners = (this.Contains(box.topright) || this.Contains(box.topleft) || this.Contains(box.bottomright) || this.Contains(box.bottomleft));

                _temp = (top || bottom || left || right || mid || midmid || corners);*/
            }
            else if (_h.Type == "Curved")
            {
                Console.WriteLine("Is checking for a Curvedhitbox");
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
                _temp = (this.Contains(box.max_start) || this.Contains(box.max_end) || this.Contains(box.min_end) || this.Contains(box.min_start));
                //Console.WriteLine(_h.Type);
            }

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

        public void ShowOverlap(Graphics g)
        {
            Point[] _points = new Point[]
            {
                topleft, topright, bottomright, bottomleft
            };

            g.FillPolygon(new SolidBrush(Color.FromArgb(150, Color.Red)), _points);
        }

        public List<LanePoints> CalculateLanePoints()
        {
                        
            List<LanePoints> _lanepoints = new List<LanePoints>();

            if ((Math.Abs(topleft.Y - topright.Y) < 5 || Math.Abs(topleft.X - topright.X) < 5) && (Math.Abs(topleft.X - bottomleft.X) < 5 || Math.Abs(topleft.Y - bottomleft.Y) < 5))
            {

                if (RoadMath.Distance(topleft, topright) < RoadMath.Distance(topleft, bottomleft))
                {

                    Point boxmidTop = new Point((topright.X + topleft.X) / 2, (topright.Y + topleft.Y) / 2);
                    Point boxmidBottom = new Point((bottomright.X + bottomleft.X) / 2, (bottomright.Y + bottomleft.Y) / 2);
                    _lanepoints = LanePoints.CalculateDiagonalLane(boxmidTop, boxmidBottom);
                }
                else
                {
                    Point boxmidLeft = new Point((topleft.X + bottomleft.X) / 2, (bottomleft.Y + topleft.Y) / 2);
                    Point boxmidRight = new Point((topright.X + bottomright.X) / 2, (bottomright.Y + topright.Y) / 2);
                    _lanepoints = LanePoints.CalculateDiagonalLane(boxmidLeft, boxmidRight);

                }
            }
            else if (Math.Abs(topleft.Y - topright.Y) < 5 || Math.Abs(topleft.X - topright.X) < 5)
            {

                Point boxmidTop = new Point((topright.X + topleft.X) / 2, (topright.Y + topleft.Y) / 2);
                Point boxmidBottom = new Point((bottomright.X + bottomleft.X) / 2, (bottomright.Y + bottomleft.Y) / 2);
                _lanepoints = LanePoints.CalculateDiagonalLane(boxmidTop, boxmidBottom);
            }
            else if (Math.Abs(topleft.X - bottomleft.X) < 5 || Math.Abs(topleft.Y - bottomleft.Y) < 5)
            {

                Point boxmidLeft = new Point((topleft.X + bottomleft.X) / 2, (bottomleft.Y + topleft.Y) / 2);
                Point boxmidRight = new Point((topright.X + bottomright.X) / 2, (bottomright.Y + topright.Y) / 2);
                _lanepoints = LanePoints.CalculateDiagonalLane(boxmidLeft, boxmidRight);
            }
            return _lanepoints;
        }
    }
}
