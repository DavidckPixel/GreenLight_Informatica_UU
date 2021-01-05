using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class RectHitbox : Hitbox
    {
        public Point topleft, topright, bottomleft, bottomright;

        double rcTop, rcBottom, rcLeft, rcRight;
        double bTop, bBottom, bLeft, bRight;

        public RectHitbox(Point _topleft, Point _topright, Point _bottomleft, Point _bottomright)
        {
            bottomleft = _bottomleft;
            bottomright = _bottomright;
            topleft = _topleft;
            topright = _topright;

            double topleftX, topleftY, toprightX, toprightY, bottomleftX, bottomleftY, bottomrightX, bottomrightY;

            topleftX = _topleft.X;
            topleftY = _topleft.Y;

            toprightX = _topright.X;
            toprightY = _topright.Y;

            bottomleftX = _bottomleft.X;
            bottomleftY = _bottomleft.Y;

            bottomrightX = _bottomright.X;
            bottomrightY = _bottomright.Y;

            if ((toprightX - topleftX) != 0)
            {
                rcTop = (toprightY - topleftY) / (toprightX - topleftX);
                bTop = topleftY - rcTop * topleftX;
            }
            else
            {
                 Console.WriteLine("??");
                 rcTop = 1;
                 bTop = topleftX;
            }
            
            if((bottomrightX - bottomleftX) != 0)
            {
                rcBottom = (bottomrightY - bottomleftY) / (bottomrightX - bottomleftX);
                bBottom = bottomleftY - rcBottom * bottomleftX;
            }
            else
            {
                Console.WriteLine("??");
                rcBottom = 1;
                bBottom = bottomleftX;
            }

            if((topleftX - bottomleftX) != 0)
            {
                rcLeft = (topleftY - bottomleftY) / (topleftX - bottomleftX);
                bLeft = bottomleftY - rcLeft * bottomleftX;
            }
            else
            {
                Console.WriteLine("??");
                rcLeft = 1;
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
                rcRight = 1;
                bRight = toprightX;
            }
        }

        public override bool Contains(Point _p)
        {
            Console.WriteLine("{0}\n{1}\n{2}\n{3}", (_p.Y - bLeft) / rcLeft <= _p.X, (_p.Y - bRight) / rcRight >= _p.X, _p.X * rcTop + bTop <= _p.Y, _p.X * rcBottom + bBottom >= _p.Y);

            try
            {
                if (rcLeft == 0 || rcRight == 0)
                {
                    Console.WriteLine("hier wel eens??");
                    if(_p.X > bRight && bLeft > _p.X && _p.Y > bTop && bBottom > _p.Y)
                    {
                        return true;
                    }
                }
                else if (rcTop == 0 || rcBottom == 0)
                {
                    //if (ditobject.Beginpunt.Y  >= p.Y && ditobject.Beginpunt.Y  <= p.Y && p.X >= ditobject.Beginpunt.X && p.X <= ditobject.Eindpunt.X)
                    if (bLeft <= _p.X && bRight >= _p.X && bTop <= _p.Y && bBottom >= _p.Y)
                    {
                        Console.WriteLine("Kom je hier??");
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
            catch (Exception)
            {
                Console.WriteLine("Devided By 0");
            }
            return false;
        }


        public override bool Collide(Hitbox _h)
        {
            throw new NotImplementedException();
        }

        /*public int KlikOpLijn(Point p, MijnObject ditobject, MijnObject indexobject, int lijstPositie)
        {
            double dx = (ditobject.Eindpunt.X - ditobject.Beginpunt.X);
            double dy = (ditobject.Eindpunt.Y - ditobject.Beginpunt.Y);

            if (dx == 0)
            {
                double laagsteY = ditobject.Beginpunt.Y;
                double hoogsteY = ditobject.Eindpunt.Y;
                if (dy < 0)
                {
                    laagsteY = ditobject.Eindpunt.Y;
                    hoogsteY = ditobject.Beginpunt.Y;
                }

                if (ditobject.Beginpunt.X + (ditobject.KwastFormaat + 3) >= p.X && ditobject.Beginpunt.X - (ditobject.KwastFormaat + 3) <= p.X && p.Y >= laagsteY && p.Y <= hoogsteY)
                {
                    lijstPositie = ObjectIsHoger(indexobject, lijstPositie);
                }
            }

            else if (dy == 0)
            {
                if (ditobject.Beginpunt.Y + (ditobject.KwastFormaat + 3) >= p.Y && ditobject.Beginpunt.Y - (ditobject.KwastFormaat + 3) <= p.Y && p.X >= ditobject.Beginpunt.X && p.X <= ditobject.Eindpunt.X)
                {
                    lijstPositie = ObjectIsHoger(indexobject, lijstPositie);
                }
            }

            else
            {
                double rc = dy / dx;
                double b = ditobject.Beginpunt.Y - rc * ditobject.Beginpunt.X;

                if (((p.X * rc + b <= p.Y + (ditobject.KwastFormaat + 3) && p.X * rc + b >= p.Y - (ditobject.KwastFormaat + 3)) || ((p.Y - b) / rc <= p.X + (ditobject.KwastFormaat + 3) && (p.Y - b) / rc >= p.X - (ditobject.KwastFormaat + 3))) && p.X >= ditobject.Beginpunt.X && p.X <= ditobject.Eindpunt.X)
                {
                    lijstPositie = ObjectIsHoger(indexobject, lijstPositie);
                }

            }
            return lijstPositie;
        }*/
    }
}
