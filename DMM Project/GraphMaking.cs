using System;
using System.Collections.Generic;

namespace DMM_Project
{
    public class Graph
    {
        public int Vertic { get; }

        private List<(int to, int weight)>[] adjacencyList;
        private int[,] adjacencyMatrix;
        private Random random = new Random();

        public Graph(int vertic)
        {
            Vertic = vertic;
            adjacencyList = new List<(int, int)>[vertic];
            for (int i = 0; i < vertic; i++)
                adjacencyList[i] = new List<(int, int)>();

            adjacencyMatrix = new int[vertic, vertic];

            // ініціалізуємо матрицю 
            for (int i = 0; i < vertic; i++)
                for (int j = 0; j < vertic; j++)
                    adjacencyMatrix[i, j] = int.MaxValue;
        }

        public void AddEdge(int from, int to, int weight)
        {
            if (from == to) return;

            adjacencyList[from].Add((to, weight));
            adjacencyMatrix[from, to] = weight;
        }

        public void GenerateRandomEdges(double density)
        {
            int maxEdges = Vertic * (Vertic - 1);
            int edgeCount = (int)(density * maxEdges);
            HashSet<(int, int)> existingEdges = new();

            while (existingEdges.Count < edgeCount)
            {
                int from = random.Next(Vertic);
                int to = random.Next(Vertic);

                if (from != to && !existingEdges.Contains((from, to)))
                {
                    int weight = random.Next(-10, 21); //від -10 до 20
                    AddEdge(from, to, weight);
                    existingEdges.Add((from, to));
                }
            }
        }

        public void PrintAdjacencyList()
        {
            for (int i = 0; i < Vertic; i++)
            {
                Console.Write($"{i}: ");
                foreach (var (to, weight) in adjacencyList[i])
                    Console.Write($"-> {to}({weight}) ");
                Console.WriteLine();
            }
        }

        public void PrintAdjacencyMatrix()
        {
            Console.Write("     ");
            for (int j = 0; j < Vertic; j++)
                Console.Write($"{j,5}");
            Console.WriteLine();

            for (int i = 0; i < Vertic; i++)
            {
                Console.Write($"{i,3} ");
                for (int j = 0; j < Vertic; j++)
                {
                    if (adjacencyMatrix[i, j] == int.MaxValue)
                        Console.Write("  INF");
                    else
                        Console.Write($"{adjacencyMatrix[i, j],5}");
                }
                Console.WriteLine();
            }
        }

        public List<(int to, int weight)> GetAdjacencyList(int v)
        {
            return adjacencyList[v];
        }

        public int[,] GetAdjacencyMatrix()
        {
            return adjacencyMatrix;
        }
    }
}
