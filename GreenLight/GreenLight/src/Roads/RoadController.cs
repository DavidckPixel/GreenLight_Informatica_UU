using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{
    public class RoadController : EntityController
    {

        //Very early version of the actual code that WILL connect the road system to the rest of our project
        //For now it just holds a calculate direction function
        //Nothing really of interest here yet, Come back later :)

        public List<AbstractRoad> roads = new List<AbstractRoad>();
        public PictureBox Screen;
        public string roadType = "D";

        private Form settingScreen;
        private PictureBox settingScreenImage;

        private AbstractRoad selectedRoad;
        private AbstractRoad ghostRoad;

        public RoadController(PictureBox _screen)
        {
            this.Screen = _screen;
            this.Screen.MouseClick += RoadClick;

            initSettingScreen();
        }

        public void BuildStraightRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "StraightRoad");
            AbstractRoad _road = new StraightRoad(_point1, _point2, 1, _dir, "Straight");

            roads.Add(_road);
        }

        private void initSettingScreen()
        {
            this.settingScreen = new Form();
            this.settingScreen.Hide();
            //is.settingScreen.MdiParent = this.mainScreen;

            this.settingScreen.Size = new Size(300, 600);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);

            settingScreenImage = new PictureBox();

            settingScreenImage.Paint += SettingBoxDraw;
            settingScreenImage.MouseClick += SettingBoxClick;

            settingScreenImage.Size = new Size(400, 400);
            settingScreenImage.Location = new Point(100, 100);
            settingScreenImage.BackColor = Color.Black;

            this.settingScreen.Controls.Add(settingScreenImage);
        }

        public void BuildDiagnolRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "DiagonalRoad");
            AbstractRoad _road = new DiagonalRoad(_point1, _point2, 3, _dir, "Diagonal");

            roads.Add(_road);
        }

        public void BuildCurvedRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "CurvedRoad");
            Console.WriteLine(_dir);
            AbstractRoad _road = new CurvedRoad(_point1, _point2, 3, _dir, "Curved");

            roads.Add(_road);
        }
        
        public static string Direction(Point _firstPoint, Point _secondPoint, string _Roadtype)
        {
            string RoadDirection = "";
            string RoadType = _Roadtype;
            switch (RoadType)
            {
                case "CurvedRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NE";
                            else
                                RoadDirection = "SE";
                        }
                        else
                        {
                            if (_firstPoint.Y < _secondPoint.Y)
                                RoadDirection = "NW";
                            else
                                RoadDirection = "SW";
                        }
                    }
                    break;
                case "DiagonalRoad":
                    {
                        RoadDirection = "D";
                    }
                    break;
                case "StraightRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                            RoadDirection = "E";
                        else if (_secondPoint.X < _firstPoint.X)
                            RoadDirection = "W";
                        else if (_firstPoint.Y < _secondPoint.Y)
                            RoadDirection = "S";
                        else if (_firstPoint.Y > _secondPoint.Y)
                            RoadDirection = "N";
                    }
                    break;

            }
            return RoadDirection;
        }

        public override void Initialize()
        {

        }

        public void RoadClick(object o, MouseEventArgs mea)
        {
            if (this.roadType == "D") //Menu is Disabled
            {
                return;
            }

            this.selectedRoad = roads.Find(x => x.Hitbox2.Contains(mea.Location));
            if (this.selectedRoad == null)
            {
                Console.Write("No Road Clicked!");
                return;
            }

            if (this.roadType == "X")
            {
                EnableSettingScreen();
            }
        }

        private void EnableSettingScreen()
        {
            settingScreen.Show();
            settingScreen.BringToFront();
            settingScreen.Invalidate();
        }

        private void SettingBoxClick(object o, MouseEventArgs mea)
        {

        }

        private void SettingBoxDraw(object o, PaintEventArgs pea)
        {

            Console.WriteLine(this.Screen.Image.Size);

            Console.WriteLine("Test");

            Graphics g = pea.Graphics;

            Hitbox _hitbox = selectedRoad.Hitbox2;

            Bitmap b = new Bitmap(Screen.Width, Screen.Height);
            Screen.DrawToBitmap(b, new Rectangle(new Point(0, 0), Screen.Size));

            Rectangle _rec = new Rectangle(_hitbox.topleft, new Size(Math.Abs(_hitbox.topright.X - _hitbox.bottomleft.X), Math.Abs(_hitbox.topright.X - _hitbox.bottomleft.X)));

            Console.WriteLine(_rec.Location);

            g.DrawImage(b, 0, 0, _rec, GraphicsUnit.Pixel);
            
        }
    }
}
