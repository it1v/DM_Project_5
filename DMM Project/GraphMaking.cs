using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace DMM_Project
{

    public class Graph
    {
        private readonly List<(int to, int weight)>[] adjacencyList;
        public int Vertic => adjacencyList.Length;
        public List<(int to, int weight)>[] AdjacencyList => adjacencyList;

        public Graph(int vertices)
        {
            adjacencyList = new List<(int, int)>[vertices];
            for (int i = 0; i < vertices; i++)
                adjacencyList[i] = new List<(int, int)>();
        }

        public void AddEdge(int u, int v, int weight)
        {
            adjacencyList[u].Add((v, weight));
        }

        public static Graph GenerateRandomGraph(int vertices, int edges, int minWeight = -5, int maxWeight = 10)
        {
            var rand = new Random();
            var g = new Graph(vertices);
            var added = new HashSet<(int, int)>();

            while (edges > 0)
            {
                int u = rand.Next(vertices);
                int v = rand.Next(vertices);
                if (u != v && !added.Contains((u, v)))
                {
                    int weight = rand.Next(minWeight, maxWeight + 1);
                    g.AddEdge(u, v, weight);
                    added.Add((u, v));
                    edges--;
                }
            }

            return g;
        }
    }
}