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
        int drivingLaneDistance = 40;
        int roadAmount;
        protected List<DrivingLane> drivinglanes;
        
        string Selected;
        
        AbstractRoad roadType;

        public void CreateRoad(Gridpoint _firstPoint, Gridpoint _secondPoint, string _roadType, int _lanes)
        {
            AbstractRoad newRoad;
            string RoadType = _roadType;
            int lanes = _lanes;

            switch (RoadType)
            {
                case "CurvedRoad":
                    
                        
                        roadAmount++;
                    
                    break;

                case "DiagonalRoad":
                    for (int t = 1; t <= lanes; t++)
                    {
                        
                        newRoad = new DiagonalRoad(_firstPoint.Cords, _secondPoint.Cords, lanes, Direction(_firstPoint.Cords, _secondPoint.Cords));
                        //drivinglanes.Add(new DrivingLane(newRoad._lanePoints, roadAmount + 1));
                    }
                    roadAmount++;
                    break;
            }
        }

        public static String Direction(Point _firstPoint, Point _secondPoint)
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
