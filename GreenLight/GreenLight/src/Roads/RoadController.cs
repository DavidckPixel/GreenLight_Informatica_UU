using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

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
        private CurvedButtons doneButton, deleteButton, curveFlipButton;

        private AbstractRoad selectedRoad;

        PrivateFontCollection Font_collection = new PrivateFontCollection();

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

            this.settingScreen.Size = new Size(520, 600);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            settingScreenImage = new PictureBox();

            settingScreenImage.Paint += SettingBoxDraw;
            settingScreenImage.MouseClick += SettingBoxClick;

            settingScreenImage.Size = new Size(500, 500);
            settingScreenImage.Location = new Point(10, 10);
            settingScreenImage.BackColor = Color.Black;

            //TEMP HERE
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            FontFamily Dosis_font_family = Font_collection.Families[0];

            doneButton = new CurvedButtons(new Size(80, 40), new Point(10, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Save", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            doneButton.Click += (object o, EventArgs ea) => { DoneSettingScreen(); };

            deleteButton = new CurvedButtons(new Size(80, 40), new Point(100, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Delete", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            deleteButton.Click += (object o, EventArgs ea) => { DeleteRoad(this.selectedRoad); };

            settingScreen.Controls.Add(doneButton);
            settingScreen.Controls.Add(deleteButton);

            curveFlipButton = new CurvedButtons(new Size(80, 40), new Point(190, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Flip", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            curveFlipButton.Click += (object o, EventArgs ea) => { FlipRoad(this.selectedRoad); };
            settingScreen.Controls.Add(curveFlipButton);



        

            

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
            if (this.roadType == "D" || settingScreen.Visible == true) //Menu is Disabled
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
            selectedRoad.Hitbox2.color = Color.Pink;

            if(selectedRoad.offsetlaneHitbox.Count == 0)
            {
                DrivingLaneHitbox();
            }

            if (selectedRoad.Type == "Curved")
            {
                curveFlipButton.Visible = true;
            }
            else
            {
                curveFlipButton.Visible = false;
            }

            settingScreen.Show();
            settingScreen.BringToFront();
            settingScreen.Invalidate();
        }

        private void DisableSettingScreen()
        {
            settingScreen.Hide();
            Screen.Invalidate();
        }

        private void DoneSettingScreen()
        {
            selectedRoad.Hitbox2.color = Color.Yellow;
            DisableSettingScreen();
        }

        private void DeleteRoad(AbstractRoad _deletedroad)
        {
            roads.Remove(_deletedroad);

            if (_deletedroad == this.selectedRoad)
            {
                this.selectedRoad = null;
                DisableSettingScreen();
            }
        }

        private void SettingBoxClick(object o, MouseEventArgs mea)
        {

        }

        private void SettingBoxDraw(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            Bitmap b = new Bitmap(Screen.Width, Screen.Height);
            Screen.DrawToBitmap(b, new Rectangle(new Point(0, 0), Screen.Size));

            Hitbox _hitbox = selectedRoad.Hitbox2;

            int _maxSize = Math.Max(_hitbox.Size.Width, _hitbox.Size.Height) + 20;
            int _diff = Math.Abs(_hitbox.Size.Width - _hitbox.Size.Height) / 2;

            Rectangle _rec;

            if(_hitbox.Size.Width > _hitbox.Size.Height)
            {
                _rec = new Rectangle(_hitbox.Topcord.X - 10, _hitbox.Topcord.Y - 10 - _diff, _maxSize, _maxSize);
            }
            else if(_hitbox.Size.Width == _hitbox.Size.Height)
            {
                _rec = new Rectangle(_hitbox.Topcord.X - 10, _hitbox.Topcord.Y - 10, _maxSize, _maxSize);
            }
            else
            {
                _rec = new Rectangle(_hitbox.Topcord.X - 10 - _diff, _hitbox.Topcord.Y - 10, _maxSize, _maxSize);
            }

            //Image is too distorted, Take Max of WIdth and Height = use in square as both X and Y;

            
            Rectangle _des = new Rectangle(0, 0, this.settingScreenImage.Width, this.settingScreenImage.Height);

            g.DrawImage(b, _des, _rec, GraphicsUnit.Pixel);

            selectedRoad.offsetlaneHitbox.ForEach(x => x.Draw(g));
            selectedRoad.offsetlaneHitbox.ForEach(x => Console.WriteLine(x.Topcord));
            selectedRoad.offsetlaneHitbox.ForEach(x => Console.WriteLine(x.Size));
        }

        private void DrivingLaneHitbox()
        {
            Point _diff = selectedRoad.Hitbox2.Topcord;

            foreach (DrivingLane _drivinglane in selectedRoad.Drivinglanes)
            {
                Point _one = _drivinglane.points.First().cord;
                Point _two = _drivinglane.points.Last().cord;

                Console.WriteLine("DRIVING POINTS FOR THE CURVED LINE ARE: {0} -- {1}", _one, _two);

                Point _oneoffset = new Point(_one.X - _diff.X, _one.Y - _diff.Y);
                Point _twooffset = new Point(_two.X - _diff.X, _two.Y - _diff.Y);

                double _scale;

                double diff = Math.Max(selectedRoad.Hitbox2.Size.Width, selectedRoad.Hitbox2.Size.Height) + 20;

                if (selectedRoad.Hitbox2.Size.Width > selectedRoad.Hitbox2.Size.Height)
                {
                    _scale = (double)(this.settingScreenImage.Width) / diff;
                }
                else
                {
                    _scale = (double)(this.settingScreenImage.Height) / diff;
                }

                double? offset;



                int Graden = AbstractRoad.CalculateAngle(_one, _two);

                if (selectedRoad.Hitbox2.Size.Width >= selectedRoad.Hitbox2.Size.Height)
                {
                    if ((Graden >= 315 && Graden < 360) || (Graden >= 0 && Graden < 45) || (Graden >= 135 && Graden < 225))
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.Hitbox2.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE
                        }
                        else
                        {
                            int temp = Math.Abs(_oneoffset.Y - _twooffset.Y);
                            offset = (double)this.settingScreenImage.Height / 2 - temp / 2 * _scale;
                        }

                        _oneoffset = new Point((int)((_oneoffset.X + 10) * _scale), (int)(((_oneoffset.Y) * _scale) + offset));
                        _twooffset = new Point((int)((_twooffset.X + 10) * _scale), (int)(((_twooffset.Y) * _scale) + offset));
                    }
                    else
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.Hitbox2.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE
                        }
                        else
                        {
                            int temp = Math.Abs(_oneoffset.Y - _twooffset.Y);
                            offset = (double)this.settingScreenImage.Height / 2 - ((((double)selectedRoad.getLanes() * 40) / 2) * _scale) - temp / 2 * _scale;
                        }

                        _oneoffset = new Point((int)((_oneoffset.X + 10) * _scale), (int)(((_oneoffset.Y) * _scale) + offset));
                        _twooffset = new Point((int)((_twooffset.X + 10) * _scale), (int)(((_twooffset.Y) * _scale) + offset));
                    }
                }
                else if (selectedRoad.Hitbox2.Size.Width < selectedRoad.Hitbox2.Size.Height)
                {
                    if ((Graden >= 315 && Graden < 360) || (Graden >= 0 && Graden < 45) || (Graden >= 135 && Graden < 225))
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Width / 2 - selectedRoad.Hitbox2.Size.Width / 2 * _scale; //WERKT VOOR CIRCLE
                        }
                        else
                        {
                            int temp = Math.Abs(_oneoffset.X - _twooffset.X);
                            offset = (double)this.settingScreenImage.Width / 2 - ((((double)selectedRoad.getLanes() * 40) / 2) * _scale) - temp / 2 * _scale;
                        }


                        _oneoffset = new Point((int)(((_oneoffset.X) * _scale) + offset), (int)((_oneoffset.Y + 10) * _scale));
                        _twooffset = new Point((int)(((_twooffset.X) * _scale) + offset), (int)((_twooffset.Y + 10) * _scale));
                    }
                    else
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.Hitbox2.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE
                        }
                        else
                        {
                            int temp = Math.Abs(_oneoffset.X - _twooffset.X);
                            offset = (double)this.settingScreenImage.Width / 2 - temp / 2 * _scale;
                        }

                        //double offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.Hitbox2.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE

                        _oneoffset = new Point((int)(((_oneoffset.X) * _scale) + offset), (int)((_oneoffset.Y + 10) * _scale));
                        _twooffset = new Point((int)(((_twooffset.X) * _scale) + offset), (int)((_twooffset.Y + 10) * _scale));
                    }
                }

                Point[] _points = selectedRoad.hitBoxPoints(_oneoffset, _twooffset, 1, (int)(40 * _scale));

                Hitbox _hitbox = selectedRoad.CreateHitbox(_points);

                this.selectedRoad.offsetlaneHitbox.Add(_hitbox);
            }
        }

        private void FlipRoad (AbstractRoad _flippedroad)
        {
            roads.Remove(_flippedroad);

            if (_flippedroad == this.selectedRoad)
            {
                string _oldDir = selectedRoad.Dir;
                string _newDir = selectedRoad.Dir;
                if (_oldDir == "SE")
                {
                    _newDir = "SEccw";
                }
                else if(_oldDir == "SW")
                {
                    _newDir = "SWccw";
                }
                else if (_oldDir == "NW")
                {
                    _newDir = "NWccw";
                }
                else if (_oldDir == "NE")
                {
                    _newDir = "NEccw";
                }
                else if (_oldDir == "SEccw")
                {
                    _newDir = "SE";
                }
                else if (_oldDir == "SWccw")
                {
                    _newDir = "SW";
                }
                else if (_oldDir == "NWccw")
                {
                    _newDir = "NW";
                }
                else if (_oldDir == "NEccw")
                {
                    _newDir = "NE";
                }

                CurvedRoad _temp = new CurvedRoad(selectedRoad.getPoint1(), selectedRoad.getPoint2(), selectedRoad.getLanes(), _newDir, "Curved");
                this.selectedRoad = _temp;
                roads.Add(_temp);                                
                DisableSettingScreen();
                EnableSettingScreen();

            }
        }
    }
}
