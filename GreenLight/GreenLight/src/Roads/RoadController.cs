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

        public RoadController(PictureBox _screen)
        {
            this.Screen = _screen;
            this.Screen.MouseClick += RoadClick;
        }

        public void BuildStraightRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "StraightRoad");
            AbstractRoad _road = new StraightRoad(_point1, _point2, 1, _dir);

            roads.Add(_road);
        }

        public void BuildDiagnolRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "DiagonalRoad");
            AbstractRoad _road = new DiagonalRoad(_point1, _point2, 3, _dir);

            roads.Add(_road);
        }

        public void BuildCurvedRoad(Point _point1, Point _point2)
        {
            string _dir = Direction(_point1, _point2, "CurvedRoad");
            Console.WriteLine(_dir);
            AbstractRoad _road = new CurvedRoad(_point1, _point2, 3, _dir);

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
            AbstractRoad _selectedRoad = roads.Find(x => x.Hitbox.Contains(mea.Location));
            if (_selectedRoad == null)
            {
                return;
            }

            Console.WriteLine(_selectedRoad.Cords.ToString());

            switch (General_Form.Main.BuildScreen.builder.signType)
            {
                case "X":
                    break;                
                case "speedSign":
                    General_Form.Main.BuildScreen.builder.signController.speedSign.newSign();
                    break;
                case "yieldSign":
                    
                    break;
                case "prioritySign":
                    
                    break;
                case "stopSign":
                    Point _begin = _selectedRoad.getPoint1();
                    Point _end = _selectedRoad.getPoint2();
                    General_Form.Main.BuildScreen.builder.signController.stopSign.newSign(_begin, _end);
                    
                    break;
            }
        }
    }
}
