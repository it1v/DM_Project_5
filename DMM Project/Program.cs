using System.Diagnostics;
using System;
namespace DMM_Project
{
    class Program
    {
        static void Main()
        {
            int vertices = 10;//розмірність матриці nxn
            double density = 0.2; //так звана щільність

            Graph graph = new Graph(vertices);//те шо нам створює граф
            graph.GenerateRandomEdges(density);

            Console.WriteLine("Adjacency List:");
            graph.PrintAdjacencyList();//списочок суміжності в консолі
            Console.WriteLine("\nAdjacency Matrix:");
            graph.PrintAdjacencyMatrix();//матриця в консолі

            graph.VisualizeWithGraphviz();//малюночок(візуалізація)
        }
    }
}
//На данному етапі 1,2(створення і наповнення графу) ШІ був використаний для ознайомлення і праці з бібліотекою Grphviz(візуалізація графу на картинці), практично весь код написаний в блоці VisualizeWithGraphviz був створений за допомогою ШІ