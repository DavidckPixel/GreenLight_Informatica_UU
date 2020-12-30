using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    class RoadController
    {

        //Very early version of the actual code that WILL connect the road system to the rest of our project
        //For now it just holds a calculate direction function
        //Nothing really of interest here yet, Come back later :)

        int drivingLaneDistance = 40;
        int roadAmount;
        protected List<DrivingLane> drivinglanes;
        
        string Selected;
        
        AbstractRoad roadType;

        public static string Direction(Point _firstPoint, Point _secondPoint)
        {
            string RoadDirection = "";
            string RoadType = "";
            switch (RoadType)
            {
                case "CurvedRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                        {
                            if (_firstPoint.Y < _secondPoint.X)
                                RoadDirection = "SW";
                            else
                                RoadDirection = "NW";
                        }
                        else
                        {
                            if (_firstPoint.Y < _secondPoint.X)
                                RoadDirection = "SE";
                            else
                                RoadDirection = "NE";
                        }
                    }
                    break;
                case "DiagonalRoad":
                    {
                        RoadDirection = "";
                    }
                    break;
                case "StraightRoad":
                    {
                        if (_firstPoint.X < _secondPoint.X)
                            RoadDirection = "E";
                        else if (_secondPoint.X < _firstPoint.X)
                            RoadDirection = "W";
                        else if (_firstPoint.Y < _secondPoint.Y)
                            RoadDirection = "Z";
                        else if (_firstPoint.Y > _secondPoint.Y)
                            RoadDirection = "N";
                    }
                    break;

            }
            return RoadDirection;
        }
    }
}
