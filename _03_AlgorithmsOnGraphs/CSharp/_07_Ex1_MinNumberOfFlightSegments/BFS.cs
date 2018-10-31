using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Ex1_MinNumberOfFlightSegments // PATHS IN GRAPHS 1: EXCERCISE 1 - Given an undirected graph with n vertices and m edges and two vertices u and v, compute the length of a shortest path between u and v
{
    class BFS
    {
        static int Distance(List<int>[] adj, int s, int t)
        {
            int n = adj.Length;
            int[] dist = new int[n]; // stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
            Queue<int> q = new Queue<int>(); // queue to store nodes in BFS

            for (int i = 0; i < n; i++)
                dist[i] = -1; // initialize the distance list with -1 as this is the expected 'no path' value

            dist[s] = 0; // zero distance to the starter node itself
            q.Enqueue(s);

            while (q.Count > 0)
            {
                int u = q.Dequeue();
                foreach (int v in adj[u])
                {
                    if (dist[v] == -1)
                    {
                        q.Enqueue(v);
                        dist[v] = dist[u] + 1;
                    }
                }
            }

            return dist[t]; // since distance 'dist' was initiated with -1, then it is enough to return the appropriate value from the array
        }


        static void Main(string[] args)
        {
            int x, y;
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            List<int>[] adj = new List<int>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            for (int i = 0; i < m; i++)
            {
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1);
                adj[y - 1].Add(x - 1);
            }

            string[] xy = Console.ReadLine().Split(); // the last line is for storing the 'u' and 'v' VERTICES which represent start and end point for the flight
            x = int.Parse(xy[0]) - 1;
            y = int.Parse(xy[1]) - 1;

            Console.WriteLine(Distance(adj, x, y)); // Good job! (Max time used: 0.27/3.00, max memory used: 42741760/536870912.)

            Console.ReadKey();
        }
    }
}
