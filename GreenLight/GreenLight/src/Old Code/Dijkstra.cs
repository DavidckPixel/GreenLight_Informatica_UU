using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    // This is the Dijkstra class, it was the algorithm behind the GPS class's pathfinding
    // It was part of the simulation/GPS system before we decided to rewrite most of it.
    // This class is now old code and not used anywhere in our project.

    public class Dijkstra
    {
        private static readonly int NO_PARENT = -1;
        public List<int> vertices = new List<int>();
        public List<int> distancelist = new List<int>();
        public List<List<int>> path = new List<List<int>>();
        public int pathnr = 0;

        public void dijkstra(int[,] adjacencyMatrix,
                                            int startVertex)
        {
            int nVertices = adjacencyMatrix.GetLength(0); 
            int[] shortestDistances = new int[nVertices];
            bool[] added = new bool[nVertices];

            for (int vertexIndex = 0; vertexIndex < nVertices;
                                                vertexIndex++)
            {
                shortestDistances[vertexIndex] = int.MaxValue;
                added[vertexIndex] = false;
            }

            shortestDistances[startVertex] = 0;
            int[] parents = new int[nVertices];
            parents[startVertex] = NO_PARENT;

            for (int i = 1; i < nVertices; i++)
            {
                int nearestVertex = -1;
                int shortestDistance = int.MaxValue;
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    if (!added[vertexIndex] &&
                        shortestDistances[vertexIndex] <
                        shortestDistance)
                    {
                        nearestVertex = vertexIndex;
                        shortestDistance = shortestDistances[vertexIndex];
                    }
                }

                if (nearestVertex >= 0)
                {
                    added[nearestVertex] = true;

                    for (int vertexIndex = 0;
                            vertexIndex < nVertices;
                            vertexIndex++)
                    {
                        int edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                        if (edgeDistance > 0
                            && ((shortestDistance + edgeDistance) <
                                shortestDistances[vertexIndex]))
                        {
                            parents[vertexIndex] = nearestVertex;
                            shortestDistances[vertexIndex] = shortestDistance +
                                                            edgeDistance;
                        }
                    }
                }
            }

            printSolution(startVertex, shortestDistances, parents);
        }

        private void printSolution(int startVertex,
                                        int[] distances,
                                        int[] parents)
        {
            int nVertices = distances.Length;

            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++)
            {
                if (vertexIndex != startVertex)
                {
                    vertices.Add(vertexIndex);
                    distancelist.Add(distances[vertexIndex]);
                    path.Add(new List<int>());
                    printPath(vertexIndex, parents);
                    pathnr++;
                }
            }
        }

        private void printPath(int currentVertex, int[] parents)
        {
            if (currentVertex <= NO_PARENT)
            {
                return;
            }
            if (currentVertex == parents[currentVertex])
            {
                return;
            }
            printPath(parents[currentVertex], parents);
            path[pathnr].Add(currentVertex);
        }
    }
}
// This code has been contributed by 29AjayKumar