using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public class GPS
    {
        public int? ForcedLane = null;
        List<OriginPoints> OriginPointsList = new List<OriginPoints> ();
        int[,] graph;
        public GPS()
        {
            Console.WriteLine("IN GPS");
            OriginPointsList = General_Form.Main.BuildScreen.builder.roadBuilder.OPC.OriginPointsList;
            
            foreach (AbstractRoad road in General_Form.Main.BuildScreen.builder.roadBuilder.roads)
            {
                /*foreach (lane item in collection)
                {

                }*/
            }
            //lanes = General_Form.Main.BuildScreen.builder.roadBuilder.roads[i].Drivinglanes[0].points[40];

            //Method to check which index is associated with the OriginPoint of the vehicle
        }

        public GPS(List<AbstractRoad> roads)
        {
            Console.WriteLine("INDE INITGPS");
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

            for (int o1 = 0; o1 < OriginPointsList.Count; o1++)
            {Console.WriteLine("X van OP: " + OriginPointsList[o1].X + " OP nummer: " + o1);

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
                Dijkstra.dijkstra(graph, 0);
            }
        }

        int RoadLengthIfAvailable(OriginPoints _op, OriginPoints _dp)
        {
            List<AbstractRoad> roads = General_Form.Main.BuildScreen.builder.roadBuilder.roads;
            bool _startPoint = false;
            foreach (AbstractRoad _r in roads)
            {
                if (_r.point1.X < _op.X + 4 && _r.point1.X > _op.X -4 && _r.point1.Y < _op.Y + 4 && _r.point1.Y > _op.Y - 4)
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
    }
}