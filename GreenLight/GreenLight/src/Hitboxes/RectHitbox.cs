using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GreenLight
{
    // RectHitbox is our own better version of the Rect Class in c#, it inherits from the abstract hitbox class.
    // It allows for the creation of odd shaped hitboxes (like the shape of our roads) and does a bunch of math to
    // figure out if a given point is inside this shape not.

    public class RectHitbox : Hitbox
    {
        public Point topleft, topright, bottomleft, bottomright;

        public double? rcTop, rcBottom, rcLeft, rcRight;
        public double? bTop, bBottom, bLeft, bRight;

        // The constructor creates the actual RectHitbox
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

            if ((toprightX - topleftX) != 0)
            {
                rcTop = (toprightY - topleftY) / (toprightX - topleftX);
                bTop = topleftY - rcTop * topleftX;
            }
            else
            {
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
                rcRight = null;
                bRight = toprightX;
            }
        }

        // This method is used to check if a point is inside the CurvedHitbox and is important for a lot of parts of our code.
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

            if (draw && Visible)
            {
                Brush Notsolid = new SolidBrush(Color.FromArgb(100, this.color));
                Point[] _points = new Point[]
                {
                topleft, topright, bottomright, bottomleft
                };

                g.FillPolygon(Notsolid, _points);
            }
        }

        public override void ShowOverlap(Graphics g)
        {
            Point[] _points = new Point[]
            {
                topleft, topright, bottomright, bottomleft
            };

            g.FillPolygon(new SolidBrush(Color.FromArgb(150, Color.Red)), _points);
        }

        // This method Calculates a line exactly in the middle of the Hitbox, used in the Collide method
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
