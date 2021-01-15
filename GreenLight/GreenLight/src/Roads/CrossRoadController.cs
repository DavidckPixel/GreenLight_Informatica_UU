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
    public class CrossRoadController : EntityController
    {

        //This class contains methods to build crossroad, and immediately open a settingsscreen in which the user can determine which paths a driver can take over the crossroad.
        //Connectionpoints are part of the crossroad, and can be enabled or disabled on the settingscreen, linked, or selected.
        //This class also manages clicking on crossroads.
        //It contains a method to calculate drivinglanes out of links, and crosslanes out of those. 



        public PictureBox Screen;

        public Form settingScreen;
        private PictureBox settingScreenImage;

        public List<AbstractRoad> roads = new List<AbstractRoad>();

        public CrossRoad selectedRoad;

        private CurvedButtons selectButton, linkButton, disableButton, errorButton, saveButton, deleteButton;

        private Label error;

        string Button = "Select";


        public CrossRoadController(PictureBox _screen)
        {
            this.Screen = _screen;

            initSettingScreen();
        }

        public AbstractRoad newCrossRoad(Point _point1, int _lanes, string _dir)
        {
            CrossRoad _temp = new CrossRoad(_point1, _point1, _lanes, "Cross", false, false);
            this.selectedRoad = _temp;

            ShowSettingScreen(_temp);
            //Open het menu;
            Console.WriteLine("New CrossRoad Made!");

            return _temp;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        private void initSettingScreen()
        {
            this.settingScreen = new Form();

            this.settingScreen.Hide();

            this.settingScreen.Size = new Size(520, 700);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            this.settingScreenImage = new PictureBox();
            this.settingScreenImage.Paint += SettingBoxDraw;
            this.settingScreenImage.MouseClick += SettingBoxClick;
            this.settingScreenImage.Size = new Size(500, 500);
            this.settingScreenImage.Location = new Point(10, 10);
            this.settingScreenImage.BackColor = Color.Black;
            //TEMP

            selectButton = new CurvedButtons(new Size(80, 40), new Point(10, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Select", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            selectButton.Click += (object o, EventArgs ea) => { this.Button = "Select";  };

            linkButton = new CurvedButtons(new Size(80, 40), new Point(100, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Link", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            linkButton.Click += (object o, EventArgs ea) => 
            {
                if(selectedRoad.selectedPoint == null)
                {
                    this.Button = "Select";
                }
                this.Button = "Link";
            };

            disableButton = new CurvedButtons(new Size(80, 40), new Point(190, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Disable", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            disableButton.Click += (object o, EventArgs ea) => 
            {
                this.Button = "Disable";
                this.selectedRoad.SwitchSelectedPoint(null);
            };

            saveButton = new CurvedButtons(new Size(80, 40), new Point(10, 600), 25, "../../User Interface Recources/Custom_Button_Small.png", "Save", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            saveButton.Click += (object o, EventArgs ea) => { CreateDrivingLanes(); this.settingScreenImage.Invalidate(); };

            deleteButton = new CurvedButtons(new Size(80, 40), new Point(100, 600), 25, "../../User Interface Recources/Custom_Button_Small.png", "Delete", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            deleteButton.Click += (object o, EventArgs ea) => { DeleteCrossroad(this.selectedRoad); };

            error = new Label();
            error.Text = "";
            error.ForeColor = Color.Red;
            error.Location = new Point(10,650);
            error.Size = new Size(390, 50);
            error.Hide();

            errorButton = new CurvedButtons(new Size(40, 40), new Point(400, 650), 25, "../../User Interface Recources/Custom_Button_Small.png", "Oke!", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            errorButton.Click += HideError;
            errorButton.Hide();

            this.settingScreen.Controls.Add(error);
            this.settingScreen.Controls.Add(errorButton);

            this.settingScreen.Controls.Add(selectButton);
            this.settingScreen.Controls.Add(linkButton);
            this.settingScreen.Controls.Add(disableButton);

            this.settingScreen.Controls.Add(saveButton);
            this.settingScreen.Controls.Add(deleteButton);

            this.settingScreen.Controls.Add(this.settingScreenImage);
        }

        public void ShowSettingScreen(CrossRoad _road)
        {
            General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "D";

            this.selectedRoad = _road;
            this.settingScreen.Show();
            this.settingScreen.BringToFront();
            this.settingScreenImage.Invalidate();
        }
          
        private void SettingBoxClick(object o, MouseEventArgs mea)
        {
            ConnectionPoint _conpoint = this.selectedRoad.connectPoints.Find(x => x.Hitbox.Contains(mea.Location));

            if(_conpoint == null)
            {
                return;
            }

            if (this.Button == "Disable")
            {
                if (_conpoint.Active)
                {
                    _conpoint.setActive(false);
                    _conpoint.Hitbox.color = Color.Red;

                    this.selectedRoad.connectLinks.RemoveAll(x => x.begin == _conpoint || x.end == _conpoint);
                }
                else
                {
                    _conpoint.setActive(true);
                    _conpoint.Hitbox.color = Color.Green;
                }
            }
            else if (this.Button == "Select" && _conpoint.Active == true)
            {
                selectedRoad.SwitchSelectedPoint(_conpoint);
            }
            else if (this.Button == "Link" && _conpoint.Active == true)
            {
                MakeLink(selectedRoad.selectedPoint, _conpoint);
            }

            this.settingScreenImage.Invalidate();
        }

        private void SettingBoxDraw(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            this.selectedRoad.connectPoints.ForEach(x => x.Draw(g));

            foreach(ConnectionLink _link in this.selectedRoad.connectLinks)
            {
                if(selectedRoad.selectedPoint == null)
                {
                    g.DrawLine(Pens.Orange, _link.begin.Location, _link.end.Location);
                }
                else
                {
                    if(_link.begin == selectedRoad.selectedPoint)
                    {
                        g.DrawLine(Pens.Orange, _link.begin.Location, _link.end.Location);
                    }
                }
            }
        }

        private void MakeLink(ConnectionPoint _begin, ConnectionPoint _end)
        {
            List<ConnectionLink> _links = selectedRoad.connectLinks;

            if (_begin.Side == _end.Side)
            {
                DisplayError("Road Cannot connect to the same side!");
                return;
            }

            if(_links.Any(x => x.begin == _end))
            {
                DisplayError("There is already a link in the opposite direction!");
                return;
            }

            if(_links.Any(x => x.begin == _begin && x.end == _end))
            {
                _links.RemoveAll(x => x.begin == _begin && x.end == _end);
                return;
            }

            _links.Add(new ConnectionLink(_begin, _end));
        }

        private void DisplayError(string _text)
        {
            error.Text = _text;
            error.Show();
            errorButton.Show();

            this.settingScreen.Invalidate();
        }

        private void HideError(object o, EventArgs ea)
        {
            error.Text = "";
            error.Hide();
            errorButton.Hide();

            this.settingScreen.Invalidate();
        }

        private void DeleteCrossroad(AbstractRoad _deletedroad)
        {
            General_Form.Main.BuildScreen.builder.roadBuilder.roads.Remove(_deletedroad);

            if (_deletedroad == this.selectedRoad)
            {
                this.selectedRoad = null;
                DisableSettingScreen();
            }
        }

        private void DisableSettingScreen()
        {
            General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "X";
            this.settingScreen.Hide();

            this.Screen.Invalidate();
        }

        private void CreateDrivingLanes()
        {
            List<LanePoints> _temp = null;
            selectedRoad.Drivinglanes.Clear();
            Point _end, _begin;

            foreach(ConnectionLink _link in selectedRoad.connectLinks)
            {

                _end = _link.end.Location;
                _begin = _link.begin.Location;

                TranslatePoints(ref _begin, ref _end, selectedRoad);

                Console.WriteLine("Crossroad: {0} - {1},", _begin, _end);

                if (((_link.end.Side == "Top" || _link.end.Side == "Bottom") && (_link.begin.Side == "Top" || _link.begin.Side == "Bottom"))
                    || ((_link.end.Side == "Left" || _link.end.Side == "Right") && (_link.begin.Side == "Left" || _link.begin.Side == "Right")))
                {
                    _temp = LanePoints.CalculateDiagonalLane(_begin, _end);
                }
                else
                {   //-----------------------------------------------------
                    if (_link.begin.Side == "Left" && _link.end.Side == "Top")
                    {
                        //omgedraaid
                        _temp = LanePoints.CalculateCurveLane(_end, _begin, "NW");

                    }
                    else if (_link.begin.Side == "Top" && _link.end.Side == "Left")
                    {
                        _temp = LanePoints.CalculateCurveLane(_begin, _end, "NW");

                    }
                    //------------------------------------------------
                    else if (_link.begin.Side == "Right" && _link.end.Side == "Bottom")
                    {
                        //omgedraaid
                        _temp = LanePoints.CalculateCurveLane(_end, _begin, "SE");

                    }
                    else if (_link.begin.Side == "Bottom" && _link.end.Side == "Right")
                    {
                        _temp = LanePoints.CalculateCurveLane(_begin, _end, "SE");

                    }
                    //---------------------------------------------
                    else if (_link.begin.Side == "Bottom" && _link.end.Side == "Left")
                    {
                        //omgedraaid
                        _temp = LanePoints.CalculateCurveLane(_end, _begin, "SW");

                    }
                    else if (_link.begin.Side == "Left" && _link.end.Side == "Bottom")
                    {
                        _temp = LanePoints.CalculateCurveLane(_begin, _end, "SW");

                    }
                    //---------------------------------------------
                    else if (_link.begin.Side == "Top" && _link.end.Side == "Right")
                    {
                        //omgedraaid
                        _temp = LanePoints.CalculateCurveLane(_end, _begin, "NE");

                    }
                    else if (_link.begin.Side == "Right" && _link.end.Side == "Top")
                    {
                        _temp = LanePoints.CalculateCurveLane(_begin, _end, "NE");

                    }
                }

                if (_temp != null)
                {   
                this.selectedRoad.Drivinglanes.Add(new CrossLane(_temp, _link));  
                }
            }

            this.Screen.Invalidate();
            this.DisableSettingScreen();
        }

        private void TranslatePoints(ref Point _begin, ref Point _end, CrossRoad _road)
        {
            double _lanes = 0.5 * 20;

            double _beginX = _begin.X / _road.Scale + _road.hitbox.Topcord.X - _lanes;
            double _beginY = _begin.Y / _road.Scale + _road.hitbox.Topcord.Y - _lanes;

            double _endX = _end.X / _road.Scale + _road.hitbox.Topcord.X - _lanes;
            double _endY = _end.Y / _road.Scale + _road.hitbox.Topcord.Y - _lanes;

            _begin = new Point((int)Math.Ceiling(_beginX), (int)Math.Ceiling(_beginY));
            _end = new Point((int)Math.Ceiling(_endX), (int)Math.Ceiling(_endY));
        }
    }
}
