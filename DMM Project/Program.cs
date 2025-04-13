using System;
using DMM_Project;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace DMM_Project
{
    class Program
    {
        static void Main()
        {
            int vertices = 160;        // Кількість вершин
            double density = 0.5;     // Щільність графа

            Graph graph = new Graph(vertices);
            graph.GenerateRandomEdges(density);

            Console.WriteLine("Adjacency List:");
            graph.PrintAdjacencyList();

            Console.WriteLine("\nAdjacency Matrix:");
            graph.PrintAdjacencyMatrix();

            Stopwatch sw = new();
            try
            {
                sw.Start();
                JohnsonAlgorithm johnson = new JohnsonAlgorithm(graph);
                var allPairs = johnson.FindAllPairsShortestPaths();

                Console.WriteLine("\nAll Pairs Shortest Paths (Johnson's Algorithm):");
                for (int i = 0; i < vertices; i++)
                {
                    for (int j = 0; j < vertices; j++)
                    {
                        if (allPairs[i, j] == int.MaxValue)
                            Console.Write("  INF");
                        else
                            Console.Write($"{allPairs[i, j],5}");
                    }
                    Console.WriteLine();
                }
                sw.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Johnson's Algorithm: {ex.Message}");
            }
            Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds:F6} ms");
            //Console.WriteLine($"Total Time (s): {sw.Elapsed.TotalSeconds:F6} s");
        }
    }
}

//На данному етапі 1,2(створення і наповнення графу) ШІ був використаний для ознайомлення і праці з бібліотекою Grphviz(візуалізація графу на картинці), практично весь код написаний в блоці VisualizeWithGraphviz був створений за допомогою ШІ
