using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Ex2_IsGraphBipartite // PATHS IN GRAPHS 1: EXCERCISE 2 - Given an undirected graph with n vertices and m edges, check whether it is bipartite.
{
    class Bipartite // An undirected graph is called bipartite if its vertices can be split into two parts such that each edge of the graph joins to vertices from different parts.
    {
        static int bipartite(List<int>[] adj)
        {
            int n = adj.Length;
            string[] color = new string[n]; // An alternative definition: a graph is bipartite if its vertices can be colored with two colors (black & white) such that the endpoints of each edge have different colors.
            Queue<int> q = new Queue<int>(); // queue to store nodes in BFS

            for (int i = 0; i < n; i++) // initialize the color array
                color[i] = "";

            color[0] = "white"; // we must always start from the first element available
            q.Enqueue(0); // the starting node is the only one that gets into the queue prior to BFS

            while (q.Count > 0) // algorithm is based on the fact that we always enter the next level after we are finished with the current one
            {
                int u = q.Dequeue();
                foreach (int v in adj[u])
                {
                    string cur_color = color[u];

                    if (color[v] == "") // for unvisited neighbours we enqueue and assign the other color
                    {
                        q.Enqueue(v);
                        color[v] = (cur_color == "white") ? "black" : "white";
                    }
                    else // for visited neighbours we check if the color is opposite
                    {
                        if (cur_color == color[v])
                            return 0;
                    }
                }
            }

            return 1;
        }


        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            List<int>[] adj = new List<int>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            for (int i = 0; i < m; i++)
            {
                int x, y;
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1);
                adj[y - 1].Add(x - 1);
            }

            Console.WriteLine(bipartite(adj)); // Good job! (Max time used: 0.23/3.00, max memory used: 43917312/536870912.)

            Console.ReadKey();
        }
    }
}
