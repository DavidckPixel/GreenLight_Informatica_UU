using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    // All Curved- and DiagonalRoads have a list of these DrivingLanes, a driving lane consists of a list of LanePoints
    // Each object from this class also has its own Draw function, this draw feature
    // draws every lane of the road next to each other.
    // All DrivingLanes also have their own hitbox, and a function to change their direction around

    public class DrivingLane : Lane
    {
        int roadLanes;
        private LanePoints middle;
        public Hitbox hitbox;

        public DrivingLane(List<LanePoints> _points, string _dir, int _roadLanes, int _thisLane, Hitbox _hitbox)
        {
            this.points = _points;
            this.dir = _dir;
            this.roadLanes = _roadLanes;
            this.thisLane = _thisLane;
            this.hitbox = _hitbox;
            this.flipped = true;

            middle = this.points[this.points.Count() / 2];
            AngleDir = middle.degree;
        }

        // Reverses the List of LanePoints and flips their angles to make cars drive in the opposite direction
        public override void FlipPoints()
        {
            List<LanePoints> _templist = new List<LanePoints>();

            points.Reverse();
            flipped = !flipped;

            foreach (LanePoints x in points)
            {
                x.Flip();
                _templist.Add(x);
            }

            points = _templist;

            middle = this.points[this.points.Count() / 2]; 
            AngleDir = middle.degree;

            if (beginConnectedTo.Count != 0 && endConnectedTo.Count != 0)
            {
                List<Lane> _tempconnections = new List<Lane>();

                foreach (Lane _l in beginConnectedTo)
                    _tempconnections.Add(_l);

                beginConnectedTo.Clear();

                foreach (Lane _l in endConnectedTo)
                    beginConnectedTo.Add(_l);

                endConnectedTo.Clear();

                foreach (Lane _l in _tempconnections)
                    endConnectedTo.Add(_l);
            }
            else if (beginConnectedTo.Count != 0)
            {
                foreach (Lane _l in beginConnectedTo)
                    endConnectedTo.Add(_l);

                beginConnectedTo.Clear();
            }
            else if (endConnectedTo.Count != 0)
            {
                foreach (Lane _l in endConnectedTo)
                    beginConnectedTo.Add(_l);

                endConnectedTo.Clear();
            }
        }

        // Decides if a dashed yellow line or a solid white line needs to be drawn at a side of a DrivingLane
        public Pen getPen(int _side)
        {
            Pen p = new Pen(Color.FromArgb(248, 185, 0), 3);

            if (thisLane > 1 && thisLane < roadLanes)
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            }
            else if (roadLanes == 1)
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                p.Width = 5;
                p.Color = Color.White;
            }
            else if (thisLane == 1)
            {
                if (_side == 1)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    p.Width = 5;
                    p.Color = Color.White;
                }
                else
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
            }
            else if (thisLane == roadLanes)
            {
                if (_side == 1)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
                else
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    p.Width = 5;
                    p.Color = Color.White;
                }
            }
            return p;
        }

        // Draws the DrivingLane on the screen
        public override void Draw(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p = new Pen(Color.FromArgb(21, 21, 21), Roads.Config.laneWidth);
            Brush b = new SolidBrush(Color.FromArgb(21, 21, 21));


            int drivingLaneDistance = Roads.Config.laneWidth;
            double slp, slpPer, oneX, amountX;
            int startAngle = 0, sweepAngle = 90;
            Rectangle rect = new Rectangle();
            Size size;
            Rectangle outer, inner;
            int side1 = 0, side2 = 0;

            if (dir.Length > 1) //CurvedRoad
            {
                size = new Size(Math.Abs(points[points.Count - 1].cord.X - points[0].cord.X) * 2, Math.Abs(points[points.Count - 1].cord.Y - points[0].cord.Y) * 2);
                switch (dir)
                {
                    case "SE":
                        {
                            startAngle = 180;
                            rect = new Rectangle(new Point(Math.Min(points[0].cord.X, points[points.Count - 1].cord.X), Math.Min(points[0].cord.Y, points[points.Count - 1].cord.Y)), size);

                            side1 = 1;
                            side2 = 2;
                        }
                        break;
                    case "SW":
                        {
                            startAngle = 270;
                            rect = new Rectangle(new Point(Math.Max(points[0].cord.X, points[points.Count - 1].cord.X) - size.Width, Math.Min(points[0].cord.Y, points[points.Count - 1].cord.Y)), size);

                            side1 = 2;
                            side2 = 1;
                        }
                        break;
                    case "NW":
                        {
                            startAngle = 0;
                            rect = new Rectangle(new Point(Math.Max(points[0].cord.X, points[points.Count - 1].cord.X) - size.Width, Math.Max(points[0].cord.Y, points[points.Count - 1].cord.Y) - size.Height), size);
                            
                            side1 = 2;
                            side2 = 1;
                        }
                        break;
                    case "NE":
                        {
                            startAngle = 90;
                            rect = new Rectangle(new Point(Math.Min(points[0].cord.X, points[points.Count - 1].cord.X), Math.Max(points[0].cord.Y, points[points.Count - 1].cord.Y) - size.Height), size);

                            side1 = 1;
                            side2 = 2;
                        }
                        break;
                }

                try
                {
                    g.DrawArc(p, rect, startAngle, sweepAngle);

                    outer = new Rectangle(new Point(rect.Location.X - drivingLaneDistance / 2, rect.Location.Y - drivingLaneDistance / 2), new Size(rect.Width + drivingLaneDistance, rect.Height + drivingLaneDistance));
                    inner = new Rectangle(new Point(rect.Location.X + drivingLaneDistance / 2, rect.Location.Y + drivingLaneDistance / 2), new Size(rect.Width - drivingLaneDistance, rect.Height - drivingLaneDistance));

                    g.DrawArc(getPen(side1), outer, startAngle, sweepAngle);
                    g.DrawArc(getPen(side2), inner, startAngle, sweepAngle);
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                if (dir == "D")  //DiagonalRoad
                {
                    Point[] polygon = new Point[4];

                    if (points[0].cord.X != points[points.Count - 1].cord.X && points[0].cord.Y != points[points.Count - 1].cord.Y)
                    {
                        slp = (double)(points[0].cord.Y - points[points.Count - 1].cord.Y) / (double)(points[points.Count - 1].cord.X - points[0].cord.X);
                        if (slp <= -1 || slp >= 1)
                        {
                            polygon[0] = new Point(points[0].cord.X - drivingLaneDistance / 2, points[0].cord.Y);
                            polygon[1] = new Point(points[0].cord.X + drivingLaneDistance / 2, points[0].cord.Y);
                            polygon[2] = new Point(points[points.Count - 1].cord.X + drivingLaneDistance / 2, points[points.Count - 1].cord.Y);
                            polygon[3] = new Point(points[points.Count - 1].cord.X - drivingLaneDistance / 2, points[points.Count - 1].cord.Y);
                        }
                        else
                        {
                            polygon[0] = new Point(points[0].cord.X, points[0].cord.Y - drivingLaneDistance / 2);
                            polygon[1] = new Point(points[0].cord.X, points[0].cord.Y + drivingLaneDistance / 2);
                            polygon[2] = new Point(points[points.Count - 1].cord.X, points[points.Count - 1].cord.Y + drivingLaneDistance / 2);
                            polygon[3] = new Point(points[points.Count - 1].cord.X, points[points.Count - 1].cord.Y - drivingLaneDistance / 2);
                        }


                        g.FillPolygon(b, polygon);
                        g.DrawLine(getPen(1), polygon[0], polygon[3]);
                        g.DrawLine(getPen(2), polygon[1], polygon[2]);
                    }
                    else if (points[0].cord.X == points[points.Count - 1].cord.X)
                    {
                        g.DrawLine(p, points[0].cord, points[points.Count - 1].cord);
                        g.DrawLine(getPen(1), new Point(points[0].cord.X - drivingLaneDistance / 2, points[0].cord.Y), new Point(points[points.Count - 1].cord.X - drivingLaneDistance / 2, points[points.Count - 1].cord.Y));
                        g.DrawLine(getPen(2), new Point(points[0].cord.X + drivingLaneDistance / 2, points[0].cord.Y), new Point(points[points.Count - 1].cord.X + drivingLaneDistance / 2, points[points.Count - 1].cord.Y));
                    }
                    else if (points[0].cord.Y == points[points.Count - 1].cord.Y)
                    {
                        g.DrawLine(p, points[0].cord, points[points.Count - 1].cord);
                        g.DrawLine(getPen(1), new Point(points[0].cord.X, points[0].cord.Y - drivingLaneDistance / 2), new Point(points[points.Count - 1].cord.X, points[points.Count - 1].cord.Y - drivingLaneDistance / 2));
                        g.DrawLine(getPen(2), new Point(points[0].cord.X, points[0].cord.Y + drivingLaneDistance / 2), new Point(points[points.Count - 1].cord.X, points[points.Count - 1].cord.Y + drivingLaneDistance / 2));
                    }

                }
            }

            hitbox.Draw(g);

            Bitmap _bitmap = DrawData.RotateImage(General_Form.Main.BuildScreen.builder.roadBuilder.ArrowBitmap, AngleDir);
            g.DrawImage(_bitmap, new Rectangle(new Point(middle.cord.X - 7, middle.cord.Y - 7), new Size(15, 15)));

        }

        public override void DrawoffsetHitbox(Graphics g)
        {
            if (offsetHitbox != null)
            {
                offsetHitbox.Draw(g);
            }
        }

        
    }
}
    


