using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

//This is the controller that handles all the sign placement, every Sign has its own controller because every sign requires its own input
//for example: speed needs a set speed but other signs need other things.
//This controller also has the function that checks the mouse position, and finds the closest point to it
//so there is still alot of flexibilty where on the road the sign needs to be placed.


namespace GreenLight
{
    public class MainSignController : EntityController
    {

        public List<AbstractSign> Signs = new List<AbstractSign>();
        public List<PlacedSign> placedSign = new List<PlacedSign>();
        public SpeedSignController speedSign;
        public StopSignController stopSign;
        public YieldSignController yieldSignC;
        public PrioritySignController prioritySignC;

        public AbstractRoad selectedRoad;
        public bool dragMode, _flipped = false;
        public int SignCount = 0;
        private Point MouseClick;
        public PlacedSign selectedSign;

        Form main;
        PictureBox screen;

        public string signType = "X";

        public MainSignController(Form _main, PictureBox _screen)
        {
            this.main = _main;
            this.screen = _screen;
            this.screen.MouseMove += mouseMove;
            this.screen.MouseClick += mouseClick;

            this.speedSign = new SpeedSignController(_main, this);
            this.speedSign.initSettingScreen();

            this.stopSign = new StopSignController(_main, this);
            this.stopSign.initSettingScreen();

            this.stopSign = new StopSignController(_main, this);
            this.stopSign.initSettingScreen();

            this.prioritySignC = new PrioritySignController(_main, this);
            this.yieldSignC = new YieldSignController(_main, this);

            this.prioritySignC.initSettingScreen();
            this.yieldSignC.initSettingScreen();
        }

        public override void Initialize()
        {

        }

        public void setDragMode(AbstractRoad _road)
        {
            if (_road == null)
            {
                return;
            }

            this.selectedRoad = _road;
            this.dragMode = true;
        }

        public void closeDragMode()
        {
            this.selectedRoad = null;
            this.dragMode = false;
            this.screen.Invalidate();
        }

        public void mouseMove(object o, MouseEventArgs mea)
        {
            MouseClick = mea.Location;

            if (!dragMode || selectedRoad == null)
            {
                return;
            }

            int _dir = (int)this.selectedRoad.Drivinglanes.First().AngleDir;

            if (selectedRoad.Type == "Diagonal")
            {
                if (_dir >= 0 && _dir < 135)
                {
                    _flipped = false;
                }
                else if (_dir >= 135 && _dir < 225)
                {
                    _flipped = true;
                }
                else if (_dir >= 225 && _dir <= 270)
                {
                    _flipped = true;
                }
                else
                {
                    _flipped = false;
                }
            }
            else if (selectedRoad.Type == "Curved")
            {
                _flipped = false;
            }
            else if(selectedRoad.Type == "Curved2")
            {
                _flipped = true;
            }
        }
        

        public void mouseClick(object o, MouseEventArgs mea)
        {

            if(signType == "D") //SIGNMENU IS DISABLED
            {
                return;
            }

            List<AbstractRoad> _roadlist = General_Form.Main.BuildScreen.builder.roadBuilder.roads;
            AbstractRoad _selectedRoad = _roadlist.Find(x => x.hitbox.Contains(mea.Location));

            if(_selectedRoad == null)
            {
                return;
            }

            if(_selectedRoad.roadtype == "Cross")
            {
                MessageBox.Show("You are not allowed to place a sign on a Crossroad!");
                return;
            }

            if (signType == "X")
            {
                try
                {
                    if (_selectedRoad == null)
                    {
                        return;
                    }
                    selectedSign = _selectedRoad.Signs.Find(x => x.Hitbox.Contains(mea.Location));
                    if (selectedSign == null)
                    {
                        Console.WriteLine("No sign could be found");
                        return;
                    }

                    Console.WriteLine("SIGN FOUND!");
                    selectedSign.Sign.controller.onSignClick(selectedSign.Sign);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }

            if (!this.dragMode)
            {
                General_Form.Main.BuildScreen.builder.signController.setDragMode(_selectedRoad);
                return;
            }

            if (!selectedRoad.hitbox.Contains(mea.Location))
            {
                closeDragMode();
                return;
            }

            AbstractSign _temp = CreateSign();
            if (_temp == null)
            {
                return;
            }
            Console.WriteLine("LocationSelected!!!");

            Image _sign_image = Image.FromFile("../../src/User Interface Recources/Speed_Sign.png"); 
            switch (signType)
            {
                case "X":
                    break;
                case "speedSign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Empty_Speed_Sign.png");
                    break;
                case "yieldSign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Yield_Sign.png");
                    break;
                case "prioritySign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Priority_Sign.png");
                    break;
                case "stopSign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Stop_Sign.png");
                    break;
            }
             this.selectedSign = this.selectedRoad.Signs.Last();

            PlacedSign _placed = new PlacedSign(new Point(-100, -100), "", _temp, _sign_image, _selectedRoad, signType, new Point(0, 0), MouseClick, _flipped);
            this.selectedRoad.Signs.Add(_placed);
            SignCount++;
            closeDragMode();
        }

        private AbstractSign CreateSign()
        {
            switch (signType)
            {
                case "X":
                    break;
                case "speedSign":
                    return speedSign.newSign();
                case "yieldSign":
                    return yieldSignC.newSign();
                case "prioritySign":
                    return prioritySignC.newSign();
                case "stopSign":
                    Point _begin = selectedRoad.getPoint1();
                    Point _end = selectedRoad.getPoint2();
                    return stopSign.newSign();
            }

            return null;
        }

        public void flipSign()
        {
            Console.WriteLine("try flip sign");

            if (this.selectedSign != null)
            {
                Console.WriteLine("flipping sign!");
                selectedSign.setFlipped();
            }

            this.screen.Invalidate();
        }


        public void deleteSign(AbstractSign _abstractSign = null)
        {
            List<AbstractRoad> _roadlist = General_Form.Main.BuildScreen.builder.roadBuilder.roads;

            foreach(AbstractRoad _road in _roadlist)
            {
                _road.Signs.RemoveAll(x => x.Sign == _abstractSign);
            }

            this.screen.Invalidate();
        }

        public void loadSigns(string[] _signWords, AbstractRoad _selectedRoad)
        {
            this.selectedRoad = _selectedRoad;
            Point _tempPoint = new Point(int.Parse(_signWords[1]), int.Parse(_signWords[2]));
            string _signType = _signWords[3];
            AbstractSign _temp = null;
            Point Hitboxoffset = new Point(int.Parse(_signWords[4]), int.Parse(_signWords[5]));
            Point Mouseclick = new Point(int.Parse(_signWords[6]), int.Parse(_signWords[7]));
            bool Flipped = bool.Parse(_signWords[8]);

            Point[] p = new Point[4];
            p[0] = new Point(int.Parse(_signWords[9]), int.Parse(_signWords[10]));
            p[1] = new Point(int.Parse(_signWords[11]), int.Parse(_signWords[12]));
            p[2] = new Point(int.Parse(_signWords[13]), int.Parse(_signWords[14]));
            p[3] = new Point(int.Parse(_signWords[15]), int.Parse(_signWords[16]));

            Image _sign_image = null;
            switch (_signType)
            {
                case "X":
                    break;
                case "speedSign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Speed_Sign.png");
                    _temp = new SpeedSign(speedSign);
                    _temp.speed = int.Parse(_signWords[17]);
                    break;
                case "yieldSign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Yield_Sign.png");
                    _temp = new YieldSign(yieldSignC);
                    break;
                case "prioritySign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Priority_Sign.png");
                    _temp = new PrioritySign(prioritySignC);
                    break;
                case "stopSign":
                    _sign_image = Image.FromFile("../../src/User Interface Recources/Stop_Sign.png");
                    _temp = new StopSign(stopSign);
                    break;
            }

            Signs.Add(_temp);

            PlacedSign _placed = new PlacedSign(_tempPoint, "", _temp, _sign_image, _selectedRoad, _signType, Hitboxoffset, Mouseclick, Flipped);
            _placed._points = p;
            this.selectedRoad.Signs.Add(_placed);
            this.placedSign.Add(_placed);
            SignCount++;
        }
    }
}
