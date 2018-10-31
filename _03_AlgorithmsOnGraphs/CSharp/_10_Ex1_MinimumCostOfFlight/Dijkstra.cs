using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Ex1_MinimumCostOfFlight // PATHS IN GRAPHS 2: EXCERCISE 1 - Given an directed graph with positive edge weights and with n vertices and m edges as well as two vertices u and v, compute the weight of a shortest path between u and v (that is, the minimum total weight of a path from u to v).
{
    class Dijkstra
    {
        static long Distance(List<int>[] adj, List<int>[] cost, int s, int t) // Dijkstra algorithm implemented with two simple arrays as a queue data structure
        {
            long[] dist = new long[adj.Length]; // storage for the graph with values that are on purpose bigger than allowed
            bool[] known = new bool[adj.Length];
            for (int i = 0; i < adj.Length; i++) { dist[i] = int.MaxValue; known[i] = false; } // we use max. integer values while 'dist' was declared as long
            dist[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

            while (true)
            {
                int u = ExtractMin(dist, known); // the whole point of this algorithm is to follow the smallest edge, as the other ones can only be bigger (relevant for non-negative) values
                if (u == -1 || known.All(x => x == true)) // first condition checks for non-connected nodes and the second if all nodes have been verified
                    break;

                for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                {
                    int v = adj[u][i]; // v stores the index of the node on the other end of the edge, as usual

                    if (dist[v] > dist[u] + cost[u][i]) // for direct neighbours we RELAX THE EDGES if possible
                        dist[v] = dist[u] + cost[u][i];
                }
                known[u] = true; // this is how we can CHANGE THE PRIORITY
            }

            if (dist[t] == int.MaxValue)
                return -1;
            else
                return dist[t];
        }


        static int ExtractMin(long[] dist, bool[] known) // helper function that returns the element u - index which has the smallest distance
        {
            long smallest = int.MaxValue;
            int index = -1;
            for (int i = 0; i < dist.Length; i++)
            {
                if (dist[i] < smallest && known[i] == false)
                {
                    smallest = dist[i];
                    index = i;
                }
            }
            return index; // we always compare the distances, but we return the index
        }
  

        static void Main(string[] args)
        {
            int x, y, w;
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            List<int>[] adj = new List<int>[n];
            List<int>[] cost = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                adj[i] = new List<int>();
                cost[i] = new List<int>();
            }

            for (int i = 0; i < m; i++)
            {
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                w = int.Parse(input[2]);
                adj[x - 1].Add(y - 1);
                cost[x - 1].Add(w); // the weights on the edges are placed in 'cost', in the same places as the edges are inside the 'adj' array
            }

            string[] xy = Console.ReadLine().Split(); // the last line is for storing the 'u' and 'v' VERTICES which represent start and end point for the flight
            x = int.Parse(xy[0]) - 1;
            y = int.Parse(xy[1]) - 1;

            Console.WriteLine(Distance(adj, cost, x, y)); // Good job! (Max time used: 0.16/3.00, max memory used: 43388928/536870912.)

            Console.ReadKey();
        }
    }
}
