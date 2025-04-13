using System;
using System.Collections.Generic;

namespace DMM_Project
{
    public class JohnsonAlgorithm
    {
        private readonly Graph graph;
        private int V;

        public JohnsonAlgorithm(Graph g)
        {
            graph = g;
            V = graph.Vertic;
        }

        public int[,] FindAllPairsShortestPaths()
        {
            // нова вершина до графа
            var newGraph = new Graph(V + 1);
            for (int u = 0; u < V; u++)
                foreach (var (v, w) in graph.GetAdjacencyList(u))
                {
                    if (w >= 0)
                        newGraph.AddEdge(u, v, w);
                }

            for (int i = 0; i < V; i++)
                newGraph.AddEdge(V, i, 0); // дод. ребра з нової вершини до всіх з вагою 0

            // беллман-форд
            int[] h = BellmanFord(newGraph, V);
            if (h == null)
                throw new Exception("Граф містить цикл з від’ємною вагою");

            // перерахунок ваг
            var reweightedGraph = new Graph(V);
            for (int u = 0; u < V; u++)
                foreach (var (v, w) in graph.GetAdjacencyList(u))
                {
                    if (w >= 0) 
                    {
                        int newWeight = w + h[u] - h[v];
                        reweightedGraph.AddEdge(u, v, newWeight);
                    }
                }

            // dijkstra
            int[,] dist = new int[V, V];
            for (int u = 0; u < V; u++)
            {
                int[] d = Dijkstra(reweightedGraph, u);
                for (int v = 0; v < V; v++)
                {
                    if (d[v] == int.MaxValue)
                        dist[u, v] = int.MaxValue;
                    else
                        dist[u, v] = d[v] + h[v] - h[u]; //оригінальні ваги
                }
            }

            return dist;
        }

        private int[] BellmanFord(Graph g, int source)
        {
            int[] dist = new int[g.Vertic];
            Array.Fill(dist, int.MaxValue);
            dist[source] = 0;

            for (int i = 1; i < g.Vertic; i++)
            {
                for (int u = 0; u < g.Vertic; u++)
                    foreach (var (v, w) in g.GetAdjacencyList(u))
                    {
                        if (dist[u] != int.MaxValue && dist[u] + w < dist[v])
                            dist[v] = dist[u] + w;
                    }
            }

            for (int u = 0; u < g.Vertic; u++)
                foreach (var (v, w) in g.GetAdjacencyList(u))
                {
                    if (dist[u] != int.MaxValue && dist[u] + w < dist[v])
                        return null;
                }

            return dist;
        }

        private int[] Dijkstra(Graph g, int src)
        {
            int[] dist = new int[g.Vertic];
            bool[] visited = new bool[g.Vertic];
            Array.Fill(dist, int.MaxValue);
            dist[src] = 0;

            for (int i = 0; i < g.Vertic; i++)
            {
                int u = -1;
                int min = int.MaxValue;
                for (int j = 0; j < g.Vertic; j++)
                    if (!visited[j] && dist[j] < min)
                    {
                        min = dist[j];
                        u = j;
                    }

                if (u == -1) break;

                visited[u] = true;

                foreach (var (v, weight) in g.GetAdjacencyList(u))
                    if (!visited[v] && dist[u] + weight < dist[v])
                        dist[v] = dist[u] + weight;
            }

            return dist;
        }
    }
}
