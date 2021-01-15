using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public struct PlacedSign
    {
        public RectHitbox Hitbox;
        public Point Location;
        public string Direction;
        public AbstractSign Sign;

        public PlacedSign(Point _location, string _direction, AbstractSign _sign, AbstractRoad _read = null)
        {
            this.Location = _location;
            this.Direction = _direction;
            this.Sign = _sign;
            this.Hitbox = new RectHitbox(new Point(Location.X - 5, Location.Y - 5), new Point(Location.X + 15, Location.Y - 5), new Point(Location.X - 5, Location.Y + 15), new Point(Location.X + 15, Location.Y + 15), Color.Red);
        }

        public void draw(Graphics g)
        {
            Console.WriteLine(Sign.ToString());
            //Brush Notsolid = new SolidBrush(Color.FromArgb(100, Color.Red));
            switch (General_Form.Main.BuildScreen.builder.signController.signType)
            {
                case "X":
                    break;
                case "speedSign":
                    Console.WriteLine("hoi");
                    g.DrawImage(Image.FromFile("../../User Interface Recources/Speed_Sign.png"),  Location.X -5 , Location.Y - 5, 20, 20);
                    break;
                case "yieldSign":
                    //DrawImage("../../User Interface Recources/Speed_Sign_Button.png", Location);
                    break;
                case "prioritySign":
                    //DrawImage("../../User Interface Recources/Speed_Sign_Button.png", Location);
                    break;
                case "stopSign":
                    //DrawImage("../../User Interface Recources/Speed_Sign_Button.png", Location);
                    break;
            }
            this.Hitbox.Draw(g);
            //g.FillRectangle(Notsolid, this.Hitbox);
        }
    }
}
