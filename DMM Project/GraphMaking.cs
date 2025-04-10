using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace DMM_Project
{
    public class Graph
    {
        public int Vertic { get; }
        private List<(int to, int weight)>[] adjacencyList; //ну тут ми просто все ініціалізуємо(можна було і через інтерфейс але це запарно)
        private int[,] adjacencyMatrix;
        private Random random = new Random();

        public Graph(int vertic)//створюємо список для нашого графу
        {
            Vertic = vertic;
            adjacencyList = new List<(int, int)>[vertic];
            for (int i = 0; i < vertic; i++)
                adjacencyList[i] = new List<(int, int)>();

            adjacencyMatrix = new int[vertic, vertic];
        }

        public void AddEdge(int from, int to, int weight)
        {
            if (from == to) return; // прибирає можливість циклів
            adjacencyList[from].Add((to, weight));
            adjacencyMatrix[from, to] = weight;
        }

        public void GenerateRandomEdges(double density)//наповнює нам граф рандомними значеннями в рандомні вершини
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
                    int weight = random.Next(-10, 21); // тут давати ренж від  яких до яких чисел буде рандом
                    AddEdge(from, to, weight);
                    existingEdges.Add((from, to));
                }
            }
        }

        public void PrintAdjacencyList()//тут ми просто прінтимо шо з чим пов'язано(виводимо в консоль)
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
            Console.Write("    "); // Відступ для того шоб нумерація не поїхала і все норм було
            for (int j = 0; j < Vertic; j++)
                Console.Write($"{j,4}");
            Console.WriteLine();

            for (int i = 0; i < Vertic; i++)
            {
                Console.Write($"{i,3} "); // Те шо по вертикалі нумерує + виводить
                for (int j = 0; j < Vertic; j++)
                    Console.Write($"{adjacencyMatrix[i, j],4}");
                Console.WriteLine();
            }
        }

        public void VisualizeWithGraphviz()
        {
            string dotPath = "graph.dot";
            string pngPath = "graph.png";

            using (StreamWriter writer = new StreamWriter(dotPath))
            {
                writer.WriteLine("digraph G {");
                for (int from = 0; from < Vertic; from++)
                {
                    foreach (var (to, weight) in adjacencyList[from])
                    {
                        writer.WriteLine($"    {from} -> {to} [label=\"{weight}\"];");
                    }
                }
                writer.WriteLine("}");
            }

            Console.WriteLine($"DOT file saved to {Path.GetFullPath(dotPath)}");

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-Tpng \"{dotPath}\" -o \"{pngPath}\"",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                };

                Process.Start(psi)?.WaitForExit();
                Console.WriteLine("PNG file saved to: " + Path.GetFullPath(pngPath));

                // Open PNG automatically if exists
                if (File.Exists(pngPath))
                {
                    Process.Start(new ProcessStartInfo(pngPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not generate image using Graphviz: " + ex.Message);
            }
        }
    }
}