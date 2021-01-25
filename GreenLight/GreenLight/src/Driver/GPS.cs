using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class GPS
    {
        public int? ForcedLane = null;
        List<OriginPoints> OriginPointsList = new List<OriginPoints>();
        static int[,] graph;
        Dijkstra getPath = new Dijkstra();
        public List<AbstractRoad> roadPath = new List<AbstractRoad>();

        public GPS()
        {
            //Console.WriteLine("IN GPS");
            OriginPointController OPC = General_Form.Main.BuildScreen.builder.roadBuilder.OPC;
            OriginPointsList = General_Form.Main.BuildScreen.builder.roadBuilder.OPC.OriginPointsList;
            List<AbstractRoad> roads = General_Form.Main.BuildScreen.builder.roadBuilder.roads;


            /*foreach (AbstractRoad road in General_Form.Main.BuildScreen.builder.roadBuilder.roads)
            {
               
            }*/
            //lanes = General_Form.Main.BuildScreen.builder.roadBuilder.roads[i].Drivinglanes[0].points[40];

            //Method to check which index is associated with the OriginPoint of the vehicle

            int n = 0;
            bool noPath = true;
            while (noPath)
            {
                Point p = OPC.GetSpawnPoint;
                if (graph == null || p == new Point(0, 0))
                {
                    MessageBox.Show("No spawnpoints available, you will return to the builder.");
                    //Hier ga je terug naar de builder
                    return;
                }
                n++;
                /*if (n == 200)
                {
                    MessageBox.Show("No routes available, make sure to add an endpoint. You will return to the builder.");
                    //Return to builder
                    return;
                }*/
                getPath.dijkstra(graph, OriginPointNumber(p));
                for (int o = 0; o < OriginPointsList.Count; o++)
                {
                    for (int v = 0; v < getPath.vertices.Count; v++)
                    {
                        if (getPath.vertices[v] == o && !OriginPointsList[o].isConnection && getPath.distancelist[v] < int.MaxValue)
                        {
                            //Console.WriteLine("In if statement in GPS");
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
                //Return to builder
            }
        }

        public GPS(List<AbstractRoad> roads)
        {
            //Console.WriteLine("INDE INITGPS");
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

            foreach (AbstractRoad r1 in roads)
            {
                Console.WriteLine("X van roads.p1: " + r1.point1.X);
                Console.WriteLine("X van roads.p2: " + r1.point2.X);
            }

            graph = new int[OriginPointsList.Count, OriginPointsList.Count];

            for (int i = 0; i < OriginPointsList.Count; i++)
            {
                for (int j = 0; j < OriginPointsList.Count; j++)
                {
                    graph[i, j] = RoadLengthIfAvailable(OriginPointsList[i], OriginPointsList[j]);
                }
            }

            //ALLES HIERONDER VOOR TESTEN

            /*for (int o1 = 0; o1 < OriginPointsList.Count; o1++)
            {
                Console.WriteLine("X van OP: " + OriginPointsList[o1].X + " OP nummer: " + o1 + " isConnection: " + OriginPointsList[o1].isConnection);
            }


            int rowLength = graph.GetLength(0);
            int colLength = graph.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", graph[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            if (graph.Length > 0)
            {
                //getPath.dijkstra(graph, OriginPointNumber(OPC.GetSpawnPoint));
            }*/
            //DIT WAS VOOR TESTS
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
            return 0;   //no road found from i to j
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
                //Console.WriteLine("OriginPointsX punt 2: " + OriginPointsList[getPath.path[endIndex][i]].X + "   endIndex: " + endIndex + "   i: " + i + "   path.Count: " + getPath.path.Count);
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
            for(int r = 0; r < roadPath.Count; r++)
            {
                Console.WriteLine("Road nummer: " + r + "   Road point 1: " + roadPath[r].point1 + "   Road point 2: " + roadPath[r].point2);
            }
        }
    }
}