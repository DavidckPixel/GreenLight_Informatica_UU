using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class MainSignController : EntityController
    {

        public List<AbstractSign> Signs = new List<AbstractSign>();
        public SpeedSignController speedSign;
        public StopSignController stopSign;
        public YieldSignController yieldSignC;
        public PrioritySignController prioritySignC;

        public bool yieldSign = false;
        public bool prioritySign = false;

        public AbstractRoad selectedRoad;
        public bool dragMode;
        private LanePoints closest;

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

        public void placePriorityLevelSign()
        {
            if (yieldSign == true && prioritySign == false)
            {
                yieldSignC.placeSign();
            }
            else if (yieldSign == false && prioritySign == true)
            {
                prioritySignC.placeSign();
            }
        }

        public void deletePriorityLevelSign()
        {
            if (yieldSign == true && prioritySign == false)
            {
                yieldSignC.deleteSign();
            }
            else if (yieldSign == false && prioritySign == true)
            {
                prioritySignC.deleteSign();
            }
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

            List<LanePoints> _lanepoints = this.selectedRoad.Drivinglanes.First().points;
            float _shortDistance = 2000; 
            foreach (LanePoints _lanepoint in _lanepoints)
            {
                float _distance = (float)Math.Sqrt((mea.Location.X - _lanepoint.cord.X) * (mea.Location.X - _lanepoint.cord.X) + (mea.Location.Y - _lanepoint.cord.Y) * (mea.Location.Y - _lanepoint.cord.Y));

                if (_shortDistance > _distance)
                {
                    _shortDistance = _distance;
                    closest = _lanepoint;
                }
            }

            Console.WriteLine(_shortDistance);
        }

        public void mouseClick(object o, MouseEventArgs mea)
        {

            if(signType == "D") //SIGNMENU IS DISABLED
            {
                return;
            }

            List<AbstractRoad> _roadlist = General_Form.Main.BuildScreen.builder.roadBuilder.roads;
            AbstractRoad _selectedRoad = _roadlist.Find(x => x.Hitbox2.Contains(mea.Location));

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

            if (!selectedRoad.Hitbox.Contains(mea.Location))
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
            this.selectedRoad.Signs.Add(new PlacedSign(closest.cord, "", _temp));
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
                    yieldSign = true;
                    prioritySign = false;
                    return yieldSignC.newSign();
                case "prioritySign":
                    yieldSign = false;
                    prioritySign = true;
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

    }
}
