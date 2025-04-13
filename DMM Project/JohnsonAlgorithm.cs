

using System;
using System.Collections.Generic;

namespace DMM_Project
{
    public class JohnsonAlgorithm
    {
        public readonly Graph graph;

        public JohnsonAlgorithm(Graph graph)
        {
            this.graph = graph;
        }

        public int[,] FindAllPairsShortestPaths()
        {
            int V = graph.Vertic;
            int[,] dist = new int[V, V];

            // Додаємо нову вершину
            List<(int to, int weight)>[] extendedAdjList = new List<(int, int)>[V + 1];
            for (int i = 0; i <= V; i++)
                extendedAdjList[i] = new List<(int, int)>();

            for (int u = 0; u < V; u++)
            {
                foreach (var (v, w) in graph.AdjacencyList[u])
                    extendedAdjList[u].Add((v, w));
                extendedAdjList[V].Add((u, 0));
            }

            // Беллман-Форд
            int[] h = BellmanFord(V, extendedAdjList);
            if (h == null)
            {
                Console.WriteLine("Граф містить цикл з від’ємною вагою. Розрахунок припинено.");
                return null;
            }

            // Перевизначаємо ваги
            List<(int to, int weight)>[] reweighted = new List<(int, int)>[V];
            for (int u = 0; u < V; u++)
            {
                reweighted[u] = new List<(int, int)>();
                foreach (var (v, w) in graph.AdjacencyList[u])
                {
                    int newWeight = w + h[u] - h[v];
                    reweighted[u].Add((v, newWeight));
                }
            }

            // Дейкстра
            for (int u = 0; u < V; u++)
            {
                int[] d = Dijkstra(V, reweighted, u);
                for (int v = 0; v < V; v++)
                {
                    dist[u, v] = d[v] == int.MaxValue ? int.MaxValue : d[v] + h[v] - h[u];
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
                bool updated = false;
                for (int u = 0; u <= V; u++)
                {
                    if (dist[u] == int.MaxValue) continue;
                    foreach (var (v, w) in adj[u])
                    {
                        if (dist[u] + w < dist[v])
                        {
                            dist[v] = dist[u] + w;
                            updated = true;
                        }
                    }
                }
                if (!updated) break;
            }

            for (int u = 0; u <= V; u++)
            {
                if (dist[u] == int.MaxValue) continue;
                foreach (var (v, w) in adj[u])
                {
                    if (dist[u] + w < dist[v])
                    {
                        Console.WriteLine($"❗ Виявлено цикл з від’ємною вагою між {u} → {v}");
                        return null;
                    }
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

            var pq = new SortedSet<(int dist, int node)>();
            pq.Add((0, src));

            while (pq.Count > 0)
            {
                var (d, u) = GetMin(pq);
                pq.Remove((d, u));

                if (visited[u]) continue;
                visited[u] = true;

                foreach (var (v, weight) in adj[u])
                {
                    if (dist[u] != int.MaxValue && dist[u] + weight < dist[v])
                    {
                        if (dist[v] != int.MaxValue)
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
}
   
