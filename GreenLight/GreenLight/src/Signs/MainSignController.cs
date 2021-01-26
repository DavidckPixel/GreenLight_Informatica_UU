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
        public SpeedSignController speedSign;
        public StopSignController stopSign;
        public YieldSignController yieldSignC;
        public PrioritySignController prioritySignC;

        public AbstractRoad selectedRoad;
        public bool dragMode;
        private LanePoints closest;
        private LanePoints hitboxoffset;

        public int SignCount = 0;

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
            if (!dragMode || selectedRoad == null)
            {
                return;
            }

            int _outerLane = 0;
            int _lanes = this.selectedRoad.getLanes();
            int _dir = (int) this.selectedRoad.Drivinglanes.First().AngleDir;

            if(_dir >= 0 && _dir < 180  && _lanes != 1)
            {
                _outerLane = _lanes - 1;   
            }
            else if (_dir >= 180 && _dir < 360 && _lanes != 1)
            {
                _outerLane = _lanes - 2;
            }

            try
            {
                List<LanePoints> _lanepoints = this.selectedRoad.Drivinglanes[_outerLane].points;
                float _shortDistance = 2000;
                for (int i = 0; i < _lanepoints.Count; i++)
                {
                    int Xsign = mea.X - 10;
                    int Ysign = mea.Y - 10;
                    Console.WriteLine("i: " + i);
                    float _distance = (float)Math.Sqrt((Xsign - _lanepoints[i].cord.X) * (Xsign - _lanepoints[i].cord.X) + (Ysign - _lanepoints[i].cord.Y) * (Ysign - _lanepoints[i].cord.Y));

                    if (_shortDistance > _distance)
                    {
                        _shortDistance = _distance;
                        closest = _lanepoints[i];
                        if (i - 10 >= 0)
                            hitboxoffset = _lanepoints[i - 10];
                        if (i - 20 >= 0)
                            hitboxoffset = _lanepoints[i - 20];
                        else
                            hitboxoffset = _lanepoints[i];
                    }
                }

                Console.WriteLine(_shortDistance);
            }
            catch(Exception e)
            {

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

            if (signType == "X")
            {
                try
                {
                    if (_selectedRoad == null)
                    {
                        return;
                    }
                    AbstractSign _sign = _selectedRoad.Signs.Find(x => x.Hitbox.Contains(mea.Location)).Sign;
                    if (_sign == null)
                    {
                        return;
                    }
                    _sign.controller.onSignClick(_sign);

                    return;
                }
                catch (Exception)
                {
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

            Image _sign_image = Image.FromFile("../../User Interface Recources/Speed_Sign.png"); 
            switch (signType)
            {
                case "X":
                    break;
                case "speedSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Empty_Speed_Sign.png");
                    break;
                case "yieldSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Yield_Sign.png");
                    break;
                case "prioritySign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Priority_Sign.png");
                    break;
                case "stopSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Stop_Sign.png");
                    break;
            }

            //this.selectedRoad.Signs.Add(new PlacedSign(closest.cord, "", _temp, _sign_image, _selectedRoad, signType));
            this.selectedRoad.Signs.Add(new PlacedSign(closest.cord, "", _temp, _sign_image, _selectedRoad, signType, hitboxoffset.cord));

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

            Image _sign_image = null;
            switch (_signType)
            {
                case "X":
                    break;
                case "speedSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Speed_Sign.png");
                    _temp = new SpeedSign(speedSign);
                    _temp.speed = int.Parse(_signWords[6]);
                    break;
                case "yieldSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Yield_Sign.png");
                    _temp = new YieldSign(yieldSignC);
                    break;
                case "prioritySign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Priority_Sign.png");
                    _temp = new PrioritySign(prioritySignC);
                    break;
                case "stopSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Stop_Sign.png");
                    _temp = new StopSign(stopSign);
                    break;
            }

            Signs.Add(_temp);
            this.selectedRoad.Signs.Add(new PlacedSign(_tempPoint, "", _temp, _sign_image, _selectedRoad, _signType, Hitboxoffset));
            SignCount++;
        }
    }
}
