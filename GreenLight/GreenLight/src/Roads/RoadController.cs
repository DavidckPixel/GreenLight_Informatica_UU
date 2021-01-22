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

        //This class contains methods to build a straight (horizontal, vertical or diagonal) road, a curved road.
        //It also contains a method to build a crossroad, by redirecting to the newcrossroad method in the crossroadcontroller.
        //This class manages clicking on roads. 
        //When a road is clicked, a window opens, in which you can change the direction of the driving lanes of that particular road, 
        //by clicking on a lane in a Image of the road, shown on the settingsscreen.
        //Because a hitbox is re-calculated for every driving lane of the road on the Image on the settingscreen.
        //You can also delete the road you've selected, with the delete Button.
        //A method to calculate the direction of the road is here as well. 
        //(Not sure it's correct in how curved road direction is calculated a the moment?).
        //There is also a method that manages connections between roads.


        public List<AbstractRoad> roads = new List<AbstractRoad>();
        public PictureBox Screen;
        public OriginPointController OPC = new OriginPointController();
        public string roadType = "D";

        private Form settingScreen;
        private PictureBox settingScreenImage;
        private CurvedButtons doneButton, deleteButton;

        private AbstractRoad selectedRoad;

        public CrossRoadController crossRoadController;

        public bool visualizeLanePoints = true; //Boolean whether or not the lanePoints are visualised

        PrivateFontCollection Font_collection = new PrivateFontCollection();

        public RoadController(PictureBox _screen)
        {
            this.Screen = _screen;
            this.Screen.MouseClick += RoadClick;
            //this.Screen.Image = new Bitmap(Screen.Width, Screen.Height);
            crossRoadController = new CrossRoadController(this.Screen);
            initSettingScreen();
        }


        //Not in use anymore, since straightroad merged in diagonalroad.
        /*public void BuildStraightRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "StraightRoad");
            AbstractRoad _road = new StraightRoad(_point1, _point2, 1, _dir);

            roads.Add(_road);
        }*/
        private void initSettingScreen()
        {
            this.settingScreen = new Form();
            this.settingScreen.Hide();

            this.settingScreen.Size = new Size(520, 570);
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

            doneButton = new CurvedButtons(new Size(80, 40), new Point(10, 520), 25, "../../User Interface Recources/Custom_Small_Button.png", "Save", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            doneButton.Click += (object o, EventArgs ea) => { DoneSettingScreen(); };

            deleteButton = new CurvedButtons(new Size(90, 40), new Point(100, 520), 25, "../../User Interface Recources/Custom_Small_Button.png", "Delete", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            deleteButton.Click += (object o, EventArgs ea) => { DeleteRoad(this.selectedRoad); };

            Move_panel move_panel = new Move_panel(settingScreen);
            move_panel.Location = new Point(200, 510);
            move_panel.Size = new Size(400, 80);
            settingScreen.Controls.Add(move_panel);


            settingScreen.Controls.Add(doneButton);
            settingScreen.Controls.Add(deleteButton);

            this.settingScreen.Controls.Add(settingScreenImage);
        }

        public void BuildDiagonalRoad(Point _point1, Point _point2, int _lanes, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo)
        {
            string _dir = Direction(_point1, _point2, "DiagonalRoad");
            //Console.WriteLine("build" + _beginconnection + "-----" + _endconnection);
            AbstractRoad _road = new DiagonalRoad(_point1, _point2, _lanes, _dir, "Diagonal", _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
            roads.Add(_road);
            Connection(_point1, _point2, _lanes, _dir, _road, _beginconnection, _endconnection);
            OPC.AddOriginPoint(80, _point1);
            OPC.AddOriginPoint(80, _point2);
            //Console.WriteLine(OPC.GetSpawnPoint);
        }

        public void BuildCrossRoad(Point _point1, int _lanes, bool _beginconnection, bool _endconnection)
        {
            AbstractRoad _temp = crossRoadController.newCrossRoad(_point1, _lanes, "CrossRoad");
            this.roads.Add(_temp);
        }

        public void BuildCurvedRoad(Point _point1, Point _point2, int _lanes, string _type, bool _beginconnection, bool _endconnection, AbstractRoad _beginConnectedTo, AbstractRoad _endConnectedTo)
        {
            string _dir = Direction(_point1, _point2, "CurvedRoad");
            Point _temp1 = _point1;
            Point _temp2 = _point2;

            if (_type == "Curved")
            {
                if (_dir == "NW")
                {
                    _dir = "SE";
                    _point1 = _temp2;
                    _point2 = _temp1;
                }
                else if (_dir == "NE")
                {
                    _dir = "SW";
                    _point1 = _temp2;
                    _point2 = _temp1;
                }
            }
            else if (_type == "Curved2")
            {
                if (_dir == "SE")
                {
                    _dir = "NW";
                    _point1 = _temp2;
                    _point2 = _temp1;
                }
                else if (_dir == "SW")
                {
                    _dir = "NE";
                    _point1 = _temp2;
                    _point2 = _temp1;
                }
            }

            AbstractRoad _road = new CurvedRoad(_point1, _point2, _lanes, _dir, _type, _beginconnection, _endconnection, _beginConnectedTo, _endConnectedTo);
            roads.Add(_road);
            Connection(_point1, _point2, _lanes, _dir, _road, _beginconnection, _endconnection);
        }

        public void Connection(Point _point1, Point _point2, int _lanes, string _dir, AbstractRoad _road, bool _beginconnection, bool _endconnection)
        {
            Console.WriteLine(_beginconnection + "Builder" + _endconnection);
            try
            {
                foreach (AbstractRoad x in roads)
                {
                    if (x != _road && (_road.Type != "Cross" || x.Type != "Cross"))
                    {
                        Point _temp1, _temp2;
                        _temp1 = x.getPoint1();
                        _temp2 = x.getPoint2();

                        if (x.getLanes() == _lanes)
                        {
                            if (_point1 == _temp1 || Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) <= 21)
                            {
                                if (_beginconnection == false)
                                {
                                    Connection _connection = new Connection(_point1, _temp1, _lanes, _dir, x.Dir, _road, x);
                                }
                                else 
                                {
                                    Console.WriteLine(x.beginconnection + "Builder" + x.endconnection);
                                    x.beginconnection = true;
                                    x.beginConnectedTo = _road;
                                    _road.beginConnectedTo = x;
                                }
                            }
                            else if (_point1 == _temp2 || Math.Sqrt(Math.Pow(_point1.X - _temp2.X, 2) + Math.Pow(_point1.Y - _temp2.Y, 2)) <= 21)
                            {
                                if (_beginconnection == false)
                                {
                                    Connection _connection = new Connection(_point1, _temp2, _lanes, _dir, x.Drivinglanes[0].dir, _road, x);
                                }
                                else 
                                {
                                    Console.WriteLine(x.beginconnection + "Builder" + x.endconnection);
                                    x.endconnection = true;
                                    x.endConnectedTo = _road;
                                    _road.beginConnectedTo = x;
                                }
                            }
                            else if (_point2 == _temp1 || Math.Sqrt(Math.Pow(_point2.X - _temp1.X, 2) + Math.Pow(_point2.Y - _temp1.Y, 2)) <= 21)
                            {
                                if (_endconnection == false)
                                {
                                    Connection connection = new Connection(_point2, _temp1, _lanes, _dir, x.Drivinglanes[0].dir, _road, x);
                                }
                                else 
                                {
                                    Console.WriteLine(x.beginconnection + "Builder" + x.endconnection);
                                    x.beginconnection = true;
                                    x.beginConnectedTo = _road;
                                    _road.endConnectedTo = x;

                                }
                            }
                            else if (_point2 == _temp2 || Math.Sqrt(Math.Pow(_point2.X - _temp2.X, 2) + Math.Pow(_point2.Y - _temp2.Y, 2)) <= 21)
                            {
                                if (_endconnection == false)
                                {
                                    Connection _connection = new Connection(_point2, _temp2, _lanes, _dir, x.Drivinglanes[0].dir, _road, x);
                                }
                                else
                                {
                                    Console.WriteLine(x.beginconnection + "Builder" + x.endconnection);
                                    x.endconnection = true;
                                    x.endConnectedTo = _road;
                                    _road.endConnectedTo = x;
                                }
                            }
                        }
                    }
                    foreach(CrossLane c in x.Drivinglanes)
                    {
                        Point _temp1 = c.points.First().cord;
                        Point _temp2 = c.points.Last().cord;

                        if (_point1 == _temp1 || Math.Sqrt(Math.Pow(_point1.X - _temp1.X, 2) + Math.Pow(_point1.Y - _temp1.Y, 2)) <= 21)
                        {
                            if (_beginconnection == false)
                            {
                                //Connection _connection = new Connection(_point1, _temp1, _lanes, _dir, x.Dir, _road, x);
                            }
                            else
                            {
                                Console.WriteLine(x.beginconnection + "Builder" + x.endconnection);
                                x.beginconnection = true;
                                x.beginConnectedTo = _road;
                                _road.beginConnectedTo = x;
                            }
                        }
                    }
                }
            }
            catch (Exception e) { };
        }

        public static string Direction(Point _firstPoint, Point _secondPoint, string _Roadtype)
        {
            string RoadDirection = "x";
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
                    /*case "StraightRoad":
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
                        break;*/

            }
            return RoadDirection;
        }
        public void UndoRoad()
        {
            if (roads.Count != 0)
            {
                roads.RemoveAt(roads.Count - 1);
                General_Form.Main.BuildScreen.Screen.Invalidate();
            }
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

            try
            {
                this.selectedRoad = roads.Find(x => x.hitbox.Contains(mea.Location));
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
            catch (Exception e)
            {

            }
        }

        private void EnableSettingScreen()
        {
            Console.WriteLine(selectedRoad.Type);

            if (selectedRoad.Type == "Cross")
            {
                crossRoadController.ShowSettingScreen((CrossRoad)selectedRoad);
                return;
            }

            selectedRoad.hitbox.color = Color.Pink;

            if (selectedRoad.Drivinglanes.All(x => x.offsetHitbox == null))
            {
                DrivingLaneHitbox();
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
            selectedRoad.hitbox.color = Color.Yellow;
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
            if (selectedRoad.Drivinglanes.All(x => x.offsetHitbox == null))
            {
                //Console.WriteLine("No DrivingLane hitboxes have been Created");
                return;
            }

            DrivingLane _lane = (DrivingLane)selectedRoad.Drivinglanes.Find(x => x.offsetHitbox.Contains(mea.Location));

            if (_lane == null)
            {
                return;
            }

            _lane.FlipPoints();

            Screen.Invalidate();
            settingScreen.Invalidate();
            settingScreenImage.Invalidate();
        }

        private void SettingBoxDraw(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            Bitmap b = new Bitmap(Screen.Width, Screen.Height);
            Screen.DrawToBitmap(b, new Rectangle(new Point(0, 0), Screen.Size));

            Hitbox _hitbox = selectedRoad.hitbox;

            int _maxSize = Math.Max(_hitbox.Size.Width, _hitbox.Size.Height) + 20;
            int _diff = Math.Abs(_hitbox.Size.Width - _hitbox.Size.Height) / 2;

            Rectangle _rec;

            if (_hitbox.Size.Width > _hitbox.Size.Height)
            {
                _rec = new Rectangle(_hitbox.Topcord.X - 10, _hitbox.Topcord.Y - 10 - _diff, _maxSize, _maxSize);
            }
            else if (_hitbox.Size.Width == _hitbox.Size.Height)
            {
                _rec = new Rectangle(_hitbox.Topcord.X - 10, _hitbox.Topcord.Y - 10, _maxSize, _maxSize);
            }
            else
            {
                _rec = new Rectangle(_hitbox.Topcord.X - 10 - _diff, _hitbox.Topcord.Y - 10, _maxSize, _maxSize);
            }



            Rectangle _des = new Rectangle(0, 0, this.settingScreenImage.Width, this.settingScreenImage.Height);

            g.DrawImage(b, _des, _rec, GraphicsUnit.Pixel);

            selectedRoad.Drivinglanes.ForEach(x => x.DrawoffsetHitbox(g));
        }

        private void DrivingLaneHitbox()
        {
            Point _diff = selectedRoad.hitbox.Topcord;

            foreach (DrivingLane _drivinglane in selectedRoad.Drivinglanes)
            {
                Point _one = _drivinglane.points.First().cord;
                Point _two = _drivinglane.points.Last().cord;

                //Console.WriteLine("DRIVING POINTS FOR THE CURVED LINE ARE: {0} -- {1}", _one, _two);

                Point _oneoffset = new Point(_one.X - _diff.X, _one.Y - _diff.Y);
                Point _twooffset = new Point(_two.X - _diff.X, _two.Y - _diff.Y);

                double _scale;

                double diff = Math.Max(selectedRoad.hitbox.Size.Width, selectedRoad.hitbox.Size.Height) + 20;

                if (selectedRoad.hitbox.Size.Width > selectedRoad.hitbox.Size.Height)
                {
                    _scale = (double)(this.settingScreenImage.Width) / diff;
                }
                else
                {
                    _scale = (double)(this.settingScreenImage.Height) / diff;
                }

                double? offset;



                int Graden = RoadMath.CalculateAngle(_one, _two);

                if (selectedRoad.hitbox.Size.Width >= selectedRoad.hitbox.Size.Height)
                {
                    if ((Graden >= 315 && Graden < 360) || (Graden >= 0 && Graden < 45) || (Graden >= 135 && Graden < 225))
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.hitbox.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE
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
                            offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.hitbox.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE
                        }
                        else
                        {
                            int temp = Math.Abs(_oneoffset.Y - _twooffset.Y);
                            offset = (double)this.settingScreenImage.Height / 2 - ((((double)selectedRoad.getLanes() * 20) / 2) * _scale) - temp / 2 * _scale;
                        }

                        _oneoffset = new Point((int)((_oneoffset.X + 10) * _scale), (int)(((_oneoffset.Y) * _scale) + offset));
                        _twooffset = new Point((int)((_twooffset.X + 10) * _scale), (int)(((_twooffset.Y) * _scale) + offset));
                    }
                }
                else if (selectedRoad.hitbox.Size.Width < selectedRoad.hitbox.Size.Height)
                {
                    if ((Graden >= 315 && Graden < 360) || (Graden >= 0 && Graden < 45) || (Graden >= 135 && Graden < 225))
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Width / 2 - selectedRoad.hitbox.Size.Width / 2 * _scale; //WERKT VOOR CIRCLE
                        }
                        else
                        {
                            int temp = Math.Abs(_oneoffset.X - _twooffset.X);
                            offset = (double)this.settingScreenImage.Width / 2 - ((((double)selectedRoad.getLanes() * 20) / 2) * _scale) - temp / 2 * _scale;
                        }


                        _oneoffset = new Point((int)(((_oneoffset.X) * _scale) + offset), (int)((_oneoffset.Y + 10) * _scale));
                        _twooffset = new Point((int)(((_twooffset.X) * _scale) + offset), (int)((_twooffset.Y + 10) * _scale));
                    }
                    else
                    {

                        if (selectedRoad.Type == "Curved")
                        {
                            offset = (double)this.settingScreenImage.Height / 2 - selectedRoad.hitbox.Size.Height / 2 * _scale; //WERKT VOOR CIRCLE
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

                Point[] _points = selectedRoad.hitBoxPoints(_oneoffset, _twooffset, 1, (int)(20 * _scale));

                Hitbox _hitbox = selectedRoad.CreateHitbox(_points);

                //Console.WriteLine("HITBOX CREATED!!!");

                _drivinglane.offsetHitbox = _hitbox;
            }
        }

        public void loadRoads(string[] _roadWords)
        {
            if (_roadWords[1] == "Diagonal")
            {
                Console.WriteLine("LoadDiagonal Road");
                BuildDiagonalRoad(new Point(int.Parse(_roadWords[2]), int.Parse(_roadWords[3])), new Point(int.Parse(_roadWords[4]), int.Parse(_roadWords[5])), int.Parse(_roadWords[6]), bool.Parse(_roadWords[7]), bool.Parse(_roadWords[8]), null, null);
            }
            else if (_roadWords[1] == "Curved" || _roadWords[1] == "Curved2")
            {
                Console.WriteLine("LoadCurved Road");
                BuildCurvedRoad(new Point(int.Parse(_roadWords[2]), int.Parse(_roadWords[3])), new Point(int.Parse(_roadWords[4]), int.Parse(_roadWords[5])), int.Parse(_roadWords[6]), _roadWords[1], bool.Parse(_roadWords[7]), bool.Parse(_roadWords[8]), null, null);
            }
            else if (_roadWords[1] == "Cross")
            {
                Console.WriteLine("LoadCross Road");
                CrossRoad _new = new CrossRoad(new Point(int.Parse(_roadWords[2]), int.Parse(_roadWords[3])), new Point(int.Parse(_roadWords[2]), int.Parse(_roadWords[3])), int.Parse(_roadWords[6]), _roadWords[1], bool.Parse(_roadWords[7]), bool.Parse(_roadWords[8]), null, null);
                
                for (int t = 0; t < int.Parse(_roadWords[9]); t++)
                {
                    foreach (ConnectionPoint x in _new.connectPoints)
                    {
                        if (x.Location.X == int.Parse(_roadWords[10 + t * 4]) && x.Location.Y == int.Parse(_roadWords[11 + t * 4]))
                            foreach (ConnectionPoint y in _new.connectPoints)
                            {
                                if (y.Location.X == int.Parse(_roadWords[12 + t * 4]) && y.Location.Y == int.Parse(_roadWords[13 + t * 4]))
                                     _new.connectLinks.Add(new ConnectionLink(x, y));
                            }
                    }
                }
                roads.Add(_new);
                crossRoadController.roads.Add(_new);
                crossRoadController.selectedRoad = _new;
                crossRoadController.CreateDrivingLanes();
            }
            General_Form.Main.BuildScreen.Screen.Invalidate();
        }
    }
}
