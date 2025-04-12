using System;
namespace DMM_Project
{
    class Program
    {
        static void Main()
        {
            int vertic = 30; // розмірність матриці nxn
            double density = 0.2; // щільність графа

            Graph graph = new Graph(vertic); //те шо нам створює граф
            graph.GenerateRandomEdges(density);

            Console.WriteLine("Adjacency List:");
            graph.PrintAdjacencyList();//списочок суміжності в консолі
            Console.WriteLine("\nAdjacency Matrix:");
            graph.PrintAdjacencyMatrix();//матриця в консолі

            graph.VisualizeWithGraphviz();//малюночок(візуалізація)
            
            //На данному етапі 1,2(створення і наповнення графу) ШІ був використаний для ознайомлення і праці з бібліотекою Grphviz(візуалізація графу на картинці), практично весь код написаний в блоці VisualizeWithGraphviz був створений за допомогою ШІ
            
            // застосування алгоритму Джонсона
            JohnsonAlgorithm johnson = new JohnsonAlgorithm(graph);
            int[,] shortestPaths = johnson.FindAllPairsShortestPaths();

            if (shortestPaths != null)
            {
                Console.WriteLine("\nShortest paths between all pairs (Johnson's Algorithm):");
                for (int i = 0; i < vertic; i++)
                {
                    for (int j = 0; j < vertic; j++)
                    {
                        string output = shortestPaths[i, j] == int.MaxValue ? "INF" : shortestPaths[i, j].ToString();
                        Console.Write($"{output,5}");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("The graph contains a negative-weight cycle. Johnson's algorithm cannot be applied.");
            }
        }
    }
}
