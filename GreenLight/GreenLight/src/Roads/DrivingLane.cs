using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class DrivingLane : Lane
    {
        //Every road has a list of these DrivingLanes, a driving lane consists of a list of LanePoints
        //And for now an int that determines which type of road it is.
        //Each object from this class also has its own Draw feature, this draw feature
        //Draws a straight lane between all the points in the LanePoints list in order.
        //This is used for testing to see if our algorithm created a smooth road -- This will not be used in final release.

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
            this.flipped = true; //Base value

            middle = this.points[this.points.Count() / 2]; //THIS LINE GIVES PROBLEMS WHEN MAKING CURVED ROAD 2 up or down and 2 right or left..
            AngleDir = middle.degree;
        }

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

            middle = this.points[this.points.Count() / 2]; //THIS LINE GIVES PROBLEMS WHEN MAKING CURVED ROAD 2 up or down and 2 right or left..
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

        public Pen getPen(int _side)
        {
            Pen p = new Pen(Color.FromArgb(248, 185, 0), 3);

            if (thisLane <= roadLanes - 2)
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            }
            else if (roadLanes == 1)
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                p.Width = 5;
                p.Color = Color.White;
            }
            else if (thisLane == roadLanes - 1)
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

        public override void Draw(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p;
            if (thisLane == 1)
                p = new Pen(Color.Red, Roads.Config.laneWidth);
            else if (thisLane == 2)
                p = new Pen(Color.Green, Roads.Config.laneWidth);
            else
                p = new Pen(Color.FromArgb(21, 21, 21), Roads.Config.laneWidth);
            Brush b;
            if (thisLane == 1)
                b = new SolidBrush(Color.Red);
            else if (thisLane == 2)
                b = new SolidBrush(Color.Green);
            else
                b = new SolidBrush(Color.FromArgb(21, 21, 21));


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
                            if (roadLanes % 2 == 0)
                            {
                                side1 = 1;
                                side2 = 2;
                            }
                            else
                            {
                                side1 = 2;
                                side2 = 1;
                            }
                        }
                        break;
                    case "SW":
                        {
                            startAngle = 270;
                            rect = new Rectangle(new Point(Math.Max(points[0].cord.X, points[points.Count - 1].cord.X) - size.Width, Math.Min(points[0].cord.Y, points[points.Count - 1].cord.Y)), size);

                            side1 = 1;
                            side2 = 2;
                        }
                        break;
                    case "NW":
                        {
                            startAngle = 0;
                            rect = new Rectangle(new Point(Math.Max(points[0].cord.X, points[points.Count - 1].cord.X) - size.Width, Math.Max(points[0].cord.Y, points[points.Count - 1].cord.Y) - size.Height), size);
                            if (roadLanes % 2 == 0)
                            {
                                side1 = 2;
                                side2 = 1;
                            }
                            else
                            {
                                side1 = 1;
                                side2 = 2;
                            }
                        }
                        break;
                    case "NE":
                        {
                            startAngle = 90;
                            rect = new Rectangle(new Point(Math.Min(points[0].cord.X, points[points.Count - 1].cord.X), Math.Max(points[0].cord.Y, points[points.Count - 1].cord.Y) - size.Height), size);

                            side1 = 2;
                            side2 = 1;
                        }
                        break;
                }
                //Console.WriteLine(" ------ DrivingLane ------- "+ dir);


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
                //g.DrawLine(p, points[0].cord, points[points.Count - 1].cord);

                if (dir == "D")  //DiagonalRoad
                {
                    Point[] polygon = new Point[4];
                    //Console.WriteLine("tekentest");

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

                        //slpPer = -1 / slp;
                        //oneX = Math.Abs(Math.Sqrt(1 + Math.Pow(slpPer, 2)));
                        //amountX = (drivingLaneDistance / 2) / oneX;

                        g.FillPolygon(b, polygon);
                        g.DrawLine(getPen(1), polygon[0], polygon[3]);
                        g.DrawLine(getPen(2), polygon[1], polygon[2]);
                        //g.DrawLine(getPen(1), new Point(points[0].cord.X - (int)amountX, points[0].cord.Y + (int)(slpPer * amountX)), new Point(points[points.Count - 1].cord.X - (int)amountX, points[points.Count - 1].cord.Y + (int)(slpPer * amountX)));
                        //g.DrawLine(getPen(2), new Point(points[0].cord.X + (int)amountX, points[0].cord.Y - (int)(slpPer * amountX)), new Point(points[points.Count - 1].cord.X + (int)amountX, points[points.Count - 1].cord.Y - (int)(slpPer * amountX)));
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
                /*else //StraightRoad
                {
                    if (points[0].cord.X == points[points.Count - 1].cord.X)
                    {
                        g.DrawLine(getPen(1), new Point(points[0].cord.X - drivingLaneDistance / 2, points[0].cord.Y), new Point(points[points.Count - 1].cord.X - drivingLaneDistance / 2, points[points.Count - 1].cord.Y));
                        g.DrawLine(getPen(2), new Point(points[0].cord.X + drivingLaneDistance / 2, points[0].cord.Y), new Point(points[points.Count - 1].cord.X + drivingLaneDistance / 2, points[points.Count - 1].cord.Y));
                    }
                    else
                    {
                        g.DrawLine(getPen(1), new Point(points[0].cord.X, points[0].cord.Y - drivingLaneDistance / 2), new Point(points[points.Count - 1].cord.X, points[points.Count - 1].cord.Y - drivingLaneDistance / 2));
                        g.DrawLine(getPen(2), new Point(points[0].cord.X, points[0].cord.Y + drivingLaneDistance / 2), new Point(points[points.Count - 1].cord.X, points[points.Count - 1].cord.Y + drivingLaneDistance / 2));
                    }
                }*/
            }

            hitbox.Draw(g);

            Bitmap _bitmap = DrawData.RotateImage(General_Form.Main.BuildScreen.builder.roadBuilder.ArrowBitmap, AngleDir);  //HIER MOET NOG NAAR GEKEKEN WORDEN!!!!
            g.DrawImage(_bitmap, new Rectangle(new Point(middle.cord.X - 7, middle.cord.Y - 7), new Size(15, 15)));

            //Console.WriteLine(AngleDir);}


        }

        public override void DrawoffsetHitbox(Graphics g)
        {
            if (offsetHitbox != null)
            {
                offsetHitbox.Draw(g);
            }
        }

        public void LogPoints()
        {
            foreach (LanePoints _point in this.points)
            {
                Console.WriteLine(_point.cord);
            }
        }
    }
}
    


