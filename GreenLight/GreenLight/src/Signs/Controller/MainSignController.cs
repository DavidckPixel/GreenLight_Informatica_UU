﻿using System;
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
                if (_dir >= 0 && _dir < 180)
                {
                    _flipped = false;
                }
                else if (_dir >= 180 && _dir < 360)
                {
                    _flipped = true;
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

            if (signType == "X")
            {
                try
                {
                    if (_selectedRoad == null)
                    {
                        return;
                    }
                    PlacedSign _sign = _selectedRoad.Signs.Find(x => x.Hitbox.Contains(mea.Location));
                    if (_sign == null)
                    {
                        _sign = _selectedRoad.Signs.First();
                        Console.WriteLine(_sign.Hitbox.Topcord);
                        Console.WriteLine(mea.Location);
                        Console.WriteLine("No sign could be found");
                        return;
                    }

                    Console.WriteLine("SIGN FOUND!");
                    _sign.Sign.controller.onSignClick(_sign.Sign);
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
            this.selectedRoad.Signs.Add(new PlacedSign(new Point(-100, -100), "", _temp, _sign_image, _selectedRoad, signType, new Point(0, 0), MouseClick, _flipped));

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

        public void flipSing(AbstractSign _temp)
        {
            PlacedSign _placed = this.placedSign.Find(x => x.Sign == _temp);
            if(_placed == null)
            {
                return;
            }
            _placed.SignFlip();
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
            Image _sign_image = null;
            switch (_signType)
            {
                case "X":
                    break;
                case "speedSign":
                    _sign_image = Image.FromFile("../../User Interface Recources/Speed_Sign.png");
                    _temp = new SpeedSign(speedSign);
                    _temp.speed = int.Parse(_signWords[9]);
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
            this.selectedRoad.Signs.Add(new PlacedSign(_tempPoint, "", _temp, _sign_image, _selectedRoad, _signType, Hitboxoffset, Mouseclick, Flipped));
            SignCount++;
        }
    }
}