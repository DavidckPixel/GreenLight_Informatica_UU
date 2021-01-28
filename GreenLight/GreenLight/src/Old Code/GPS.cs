using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    // This is the GPS class, it was supossed to do the pathfinding for vehicles/drivers.
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project. 
    // (Both constructor methods are commented out to avoid reference errors after deleting some code in the RoadController class)

    public class GPS
    {
        public int? ForcedLane = null;
        List<OriginPoints> OriginPointsList = new List<OriginPoints>();
        static int[,] graph;
        Dijkstra getPath = new Dijkstra();
        public List<AbstractRoad> roadPath = new List<AbstractRoad>();

        public GPS()
        {
            /*
            OriginPointController OPC = General_Form.Main.BuildScreen.builder.roadBuilder.OPC;
            OriginPointsList = General_Form.Main.BuildScreen.builder.roadBuilder.OPC.OriginPointsList;
            List<AbstractRoad> roads = General_Form.Main.BuildScreen.builder.roadBuilder.roads;


            int n = 0;
            bool noPath = true;
            while (noPath)
            {
                Point p = OPC.GetSpawnPoint;
                if (graph == null || p == new Point(0, 0))
                {

                    return;
                }
                n++;

                getPath.dijkstra(graph, OriginPointNumber(p));
                for (int o = 0; o < OriginPointsList.Count; o++)
                {
                    for (int v = 0; v < getPath.vertices.Count; v++)
                    {
                        if (getPath.vertices[v] == o && !OriginPointsList[o].isConnection && getPath.distancelist[v] < int.MaxValue)
                        {
                            makePath(OriginPointNumber(p), v, roads, 1);
                            noPath = false;
                        }
                    }
                }
                getPath.vertices.Clear();
                getPath.path.Clear();
                getPath.distancelist.Clear();
                getPath.pathnr = 0;
            }
            Console.WriteLine(roadPath.Count);
            if (roadPath.Count == 0)
            {
                MessageBox.Show("No routes available, make sure to add an endpoint. You will return to the builder.");
            }
            */
        }

        public GPS(List<AbstractRoad> roads)
        {
            /*
            OriginPointController OPC = General_Form.Main.BuildScreen.builder.roadBuilder.OPC;
            OriginPointsList = General_Form.Main.BuildScreen.builder.roadBuilder.OPC.OriginPointsList;
            for (int o = 0; o < OriginPointsList.Count; o++)
            {
                bool _endpoint = false, _startpoint = false;
                for (int r = 0; r < roads.Count; r++)
                {
                    int X1 = roads[r].point1.X;
                    int Y1 = roads[r].point1.Y;
                    int X2 = roads[r].point2.X;
                    int Y2 = roads[r].point2.Y;
                    if (X1 < OriginPointsList[o].X + 4 && X1 > OriginPointsList[o].X - 4 && Y1 < OriginPointsList[o].Y + 4 && Y1 > OriginPointsList[o].Y - 4)
                        _startpoint = true;
                    if (X2 < OriginPointsList[o].X + 4 && X2 > OriginPointsList[o].X - 4 && Y2 < OriginPointsList[o].Y + 4 && Y2 > OriginPointsList[o].Y - 4)
                        _endpoint = true;
                    if (_endpoint && _startpoint)
                        OriginPointsList[o].isConnection = true;
                }
            }

            graph = new int[OriginPointsList.Count, OriginPointsList.Count];

            for (int i = 0; i < OriginPointsList.Count; i++)
            {
                for (int j = 0; j < OriginPointsList.Count; j++)
                {
                    graph[i, j] = RoadLengthIfAvailable(OriginPointsList[i], OriginPointsList[j]);
                }
            }
            */
        }

        int RoadLengthIfAvailable(OriginPoints _op, OriginPoints _dp)
        {
            List<AbstractRoad> roads = General_Form.Main.BuildScreen.builder.roadBuilder.roads;
            bool _startPoint = false;
            foreach (AbstractRoad _r in roads)
            {
                if (_r.point1.X < _op.X + 4 && _r.point1.X > _op.X - 4 && _r.point1.Y < _op.Y + 4 && _r.point1.Y > _op.Y - 4)
                {
                    _startPoint = true;
                }
                if (_r.point2.X < _dp.X + 4 && _r.point2.X > _dp.X - 4 && _r.point2.Y < _dp.Y + 4 && _r.point2.Y > _dp.Y - 4 && _startPoint)
                {
                    return _r.Drivinglanes[0].points.Count;
                }
                _startPoint = false;
            }
            return 0;
        }

        int OriginPointNumber(Point _op)
        {
            for (int opn = 0; opn < OriginPointsList.Count; opn++)
            {
                if (_op.X == OriginPointsList[opn].X && _op.Y == OriginPointsList[opn].Y)
                {
                    return opn;
                }
            }
            return 0;
        }

        void makePath(int startIndex, int endIndex, List<AbstractRoad> roads, int i)
        {
            foreach (AbstractRoad road in roads)
            {
                if (road.point1.X < OriginPointsList[startIndex].X + 4 && road.point1.X > OriginPointsList[startIndex].X - 4 && road.point1.Y < OriginPointsList[startIndex].Y + 4 && road.point1.Y > OriginPointsList[startIndex].Y - 4 && road.point2.X < OriginPointsList[getPath.path[endIndex][i]].X + 4 && road.point2.X > OriginPointsList[getPath.path[endIndex][i]].X - 4 && road.point2.Y < OriginPointsList[getPath.path[endIndex][i]].Y + 4 && road.point2.Y > OriginPointsList[getPath.path[endIndex][i]].Y - 4)
                {
                    roadPath.Add(road);
                    if (i < getPath.path[endIndex].Count - 1)
                    {
                        i++;
                        makePath(startIndex, endIndex, roads, i);
                    }
                }
            }
        }
    }
}