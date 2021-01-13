﻿using System;
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
        public PictureBox Screen;

        public Form settingScreen;
        private PictureBox settingScreenImage;

        public List<AbstractRoad> roads = new List<AbstractRoad>();

        public CrossRoad selectedRoad;

        private CurvedButtons selectButton, linkButton, disableButton, errorButton, saveButton, deleteButton;

        private Label error;

        string Button = "Select";

        PrivateFontCollection Font_collection = new PrivateFontCollection(); //TEMP DIT MOET NOG GLOBAAL

        public CrossRoadController(PictureBox _screen)
        {
            this.Screen = _screen;

            initSettingScreen();
        }

        public AbstractRoad newCrossRoad(Point _point1, int _lanes, string _dir)
        {
            CrossRoad _temp = new CrossRoad(_point1, _point1, _lanes, _dir, false, false);
            this.selectedRoad = _temp;

            ShowSettingScreen();
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

            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            FontFamily Dosis_font_family = Font_collection.Families[0];


            selectButton = new CurvedButtons(new Size(80, 40), new Point(10, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Select", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            selectButton.Click += (object o, EventArgs ea) => { this.Button = "Select";  };

            linkButton = new CurvedButtons(new Size(80, 40), new Point(100, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Link", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            linkButton.Click += (object o, EventArgs ea) => 
            {
                if(selectedRoad.selectedPoint == null)
                {
                    this.Button = "Select";
                }
                this.Button = "Link";
            };

            disableButton = new CurvedButtons(new Size(80, 40), new Point(190, 500), 25, "../../User Interface Recources/Custom_Button_Small.png", "Disable", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            disableButton.Click += (object o, EventArgs ea) => 
            {
                this.Button = "Disable";
                this.selectedRoad.SwitchSelectedPoint(null);
            };

            saveButton = new CurvedButtons(new Size(80, 40), new Point(10, 600), 25, "../../User Interface Recources/Custom_Button_Small.png", "Save", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            saveButton.Click += (object o, EventArgs ea) => { CreateDrivingLanes(); this.settingScreenImage.Invalidate(); };

            deleteButton = new CurvedButtons(new Size(80, 40), new Point(100, 600), 25, "../../User Interface Recources/Custom_Button_Small.png", "Delete", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            deleteButton.Click += (object o, EventArgs ea) => { DeleteCrossroad(this.selectedRoad); };

            error = new Label();
            error.Text = "";
            error.ForeColor = Color.Red;
            error.Location = new Point(10,650);
            error.Size = new Size(390, 50);
            error.Hide();

            errorButton = new CurvedButtons(new Size(40, 40), new Point(400, 650), 25, "../../User Interface Recources/Custom_Button_Small.png", "Oke!", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
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

        public void ShowSettingScreen()
        {
            General_Form.Main.BuildScreen.builder.roadBuilder.roadType = "D";

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
            //Teken hier het plaatje
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
            Point _old;

            foreach(Lane _lane in selectedRoad.Drivinglanes)
            {
                _old = _lane.points.First().cord;
                foreach(LanePoints _point in _lane.points)
                {
                    g.DrawLine(Pens.Purple, _point.cord, _old);
                    _old = _point.cord;
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
            DrivingLane _temp = null;

            foreach(ConnectionLink _link in selectedRoad.connectLinks)
            {

                if (((_link.end.Side == "Top" || _link.end.Side == "Bottom") && (_link.begin.Side == "Top" || _link.begin.Side == "Bottom"))
                    || ((_link.end.Side == "Left" || _link.end.Side == "Right") && (_link.begin.Side == "Left" || _link.begin.Side == "Right")))
                {
                    _temp = DiagonalRoad.CalculateDrivingLane(_link.begin.Location, _link.end.Location, 1, selectedRoad, "");
                }
                else
                {   //-----------------------------------------------------
                    if (_link.begin.Side == "Left" && _link.end.Side == "Top")
                    {
                        //omgedraaid
                        _temp = CurvedRoad.CalculateDrivingLane(_link.end.Location, _link.begin.Location, 1, selectedRoad, "NW");

                    }
                    else if (_link.begin.Side == "Top" && _link.end.Side == "Left")
                    {
                        _temp = CurvedRoad.CalculateDrivingLane(_link.begin.Location, _link.end.Location, 1, selectedRoad, "NW");

                    }
                    //------------------------------------------------
                    else if (_link.begin.Side == "Right" && _link.end.Side == "Bottom")
                    {
                        //omgedraaid
                        _temp = CurvedRoad.CalculateDrivingLane(_link.end.Location, _link.begin.Location, 1, selectedRoad, "SE");

                    }
                    else if (_link.begin.Side == "Bottom" && _link.end.Side == "Right")
                    {
                        _temp = CurvedRoad.CalculateDrivingLane(_link.begin.Location, _link.end.Location, 1, selectedRoad, "SE");

                    }
                    //---------------------------------------------
                    else if (_link.begin.Side == "Bottom" && _link.end.Side == "Left")
                    {
                        //omgedraaid
                        _temp = CurvedRoad.CalculateDrivingLane(_link.end.Location, _link.begin.Location, 1, selectedRoad, "SW");

                    }
                    else if (_link.begin.Side == "Left" && _link.end.Side == "Bottom")
                    {
                        _temp = CurvedRoad.CalculateDrivingLane(_link.begin.Location, _link.end.Location, 1, selectedRoad, "SW");

                    }
                    //---------------------------------------------
                    else if (_link.begin.Side == "Top" && _link.end.Side == "Right")
                    {
                        //omgedraaid
                        _temp = CurvedRoad.CalculateDrivingLane(_link.end.Location, _link.begin.Location, 1, selectedRoad, "NE");

                    }
                    else if (_link.begin.Side == "Right" && _link.end.Side == "Top")
                    {
                        _temp = CurvedRoad.CalculateDrivingLane(_link.begin.Location, _link.end.Location, 1, selectedRoad, "NE");

                    }
                }

                if (_temp != null)
                {   
                this.selectedRoad.Drivinglanes.Add(new CrossLane(_temp.points, _link));  
                }
            }


            
        }
    }
}
