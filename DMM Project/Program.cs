using System;
namespace DMM_Project;
using DMM_Project;

class Program
{
    static void Main()
    {
        int vertices = 6;
        int edges = 12;
        Graph graph;
        JohnsonAlgorithm johnson;
        int[,] dist;

        // Генерація графа без від’ємного циклу
        do
        {
            graph = Graph.GenerateRandomGraph(vertices, edges, -5, 10);
            johnson = new JohnsonAlgorithm(graph);
            dist = johnson.FindAllPairsShortestPaths();
        }
        while (dist == null);

        // Виведення з нумерацією
        Console.WriteLine("Генерований граф:");
        for (int i = 0; i < graph.Vertic; i++)
        {
            foreach (var (to, weight) in graph.AdjacencyList[i])
            {
                Console.WriteLine($"Вершина {i} -> Вершина {to} (вага: {weight})");
            }
        }

        Console.WriteLine("\nМатриця найкоротших шляхів:");
        Console.Write("     ");
        for (int j = 0; j < vertices; j++)
            Console.Write($"{j,5}");
        Console.WriteLine();

        for (int i = 0; i < vertices; i++)
        {
            Console.Write($"{i,4} ");
            for (int j = 0; j < vertices; j++)
            {
                if (dist[i, j] == int.MaxValue)
                    Console.Write($"{"INF",5}");
                else
                    Console.Write($"{dist[i, j],5}");
            }
            Console.WriteLine();
        }
    }
}
