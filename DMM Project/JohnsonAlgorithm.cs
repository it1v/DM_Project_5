using System;
using System.Collections.Generic;

namespace DMM_Project
{
    public class JohnsonAlgorithm
    {
        private readonly Graph graph;

        public JohnsonAlgorithm(Graph graph)
        {
            this.graph = graph;
        }

        public int[,] FindAllPairsShortestPaths()
        {
            int V = graph.Vertic;
            int[,] dist = new int[V, V];

            //додаємо нову вершину з номерами V до всіх інших вершин з вагою 0
            List<(int to, int weight)>[] extendedAdjList = new List<(int, int)>[V + 1];
            for (int i = 0; i <= V; i++)
                extendedAdjList[i] = new List<(int, int)>();

            for (int u = 0; u < V; u++)
            {
                foreach (var (v, w) in graph.GetAdjacencyList()[u])
                    extendedAdjList[u].Add((v, w));
                extendedAdjList[V].Add((u, 0));
            }

            //виконуємо Беллмана-Форда
            int[] h = BellmanFord(V, extendedAdjList);

            if (h == null)
            {
                Console.WriteLine("Graph contains a negative-weight cycle");
                return null;
            }

            //перевизначення ваг
            List<(int to, int weight)>[] reweighted = new List<(int, int)>[V];
            for (int u = 0; u < V; u++)
            {
                reweighted[u] = new List<(int, int)>();
                foreach (var (v, w) in graph.GetAdjacencyList()[u])
                {
                    int newWeight = w + h[u] - h[v];
                    reweighted[u].Add((v, newWeight));
                }
            }

            //запускаємо дейкстру з кожної вершини
            for (int u = 0; u < V; u++)
            {
                int[] d = Dijkstra(V, reweighted, u);
                for (int v = 0; v < V; v++)
                {
                    // відновлюємо оригінальні ваги
                    dist[u, v] = d[v] + h[v] - h[u];
                }
            }

            return dist;
        }

        private int[] BellmanFord(int V, List<(int to, int weight)>[] adj)
        {
            int[] dist = new int[V + 1];
            Array.Fill(dist, int.MaxValue);
            dist[V] = 0;

            for (int i = 0; i < V; i++)
            {
                for (int u = 0; u <= V; u++)
                {
                    foreach (var (v, w) in adj[u])
                    {
                        if (dist[u] != int.MaxValue && dist[u] + w < dist[v])
                        {
                            dist[v] = dist[u] + w;
                        }
                    }
                }
            }

            // перевірка на від’ємні цикли
            for (int u = 0; u <= V; u++)
            {
                foreach (var (v, w) in adj[u])
                {
                    if (dist[u] != int.MaxValue && dist[u] + w < dist[v])
                        return null; // від’ємний цикл
                }
            }

            return dist;
        }

        private int[] Dijkstra(int V, List<(int to, int weight)>[] adj, int src)
        {
            int[] dist = new int[V];
            bool[] visited = new bool[V];
            Array.Fill(dist, int.MaxValue);
            dist[src] = 0;

            SortedSet<(int dist, int node)> pq = new();
            pq.Add((0, src));

            while (pq.Count > 0)
            {
                var (d, u) = GetMin(pq);
                pq.Remove((d, u));

                if (visited[u]) continue;
                visited[u] = true;

                foreach (var (v, weight) in adj[u])
                {
                    if (dist[u] + weight < dist[v])
                    {
                        pq.Remove((dist[v], v));
                        dist[v] = dist[u] + weight;
                        pq.Add((dist[v], v));
                    }
                }
            }

            return dist;
        }

        private (int, int) GetMin(SortedSet<(int dist, int node)> pq)
        {
            foreach (var item in pq)
                return item;
            return (0, 0);
        }
    }

    // щоб граф був доступним для цього класу
    public static class GraphExtensions
    {
        public static List<(int to, int weight)>[] GetAdjacencyList(this Graph graph)
        {
            var field = typeof(Graph).GetField("adjacencyList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (List<(int, int)>[])field.GetValue(graph);
        }
    }
}
