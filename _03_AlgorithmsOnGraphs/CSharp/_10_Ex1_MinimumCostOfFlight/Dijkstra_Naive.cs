using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Ex1_MinimumCostOfFlight // PATHS IN GRAPHS 2: EXCERCISE 1 - Given an directed graph with positive edge weights and with n vertices and m edges as well as two vertices u and v, compute the weight of a shortest path between u and v (that is, the minimum total weight of a path from u to v).
{
    class Dijkstra
    {
        static long Distance(List<int>[] adj, List<int>[] cost, int s, int t) // based on iterative correction of distances from origin until there is nothing to update anymore
        {
            long[] nodes = new long[adj.Length]; // storage for the graph with values that are on purpose bigger than allowed
            for (int i = 0; i < nodes.Length; i++) { nodes[i] = int.MaxValue; }
            nodes[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
            bool is_changed = false;

            do {
                is_changed = false;
                for (int u = 0; u < adj.Length; u++)
                {
                    for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    {
                        int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                        if (nodes[u] == int.MaxValue) // if the starting node u hasn't been discovered yet, we just move on
                            continue;
                        else if ((nodes[v] == int.MaxValue) || (nodes[v] > nodes[u] + cost[u][i])) // for visited neighbours we RELAX THE EDGES if possible
                        {
                            nodes[v] = nodes[u] + cost[u][i];
                            is_changed = true; // this variable will become true each time we change something
                        }
                    }
                }
            } while (is_changed); // the 'do' loop stops as soon as there is an iteration with no further updates

            if (nodes[t] == int.MaxValue)
                return -1;
            else
                return nodes[t];
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
                cost[x - 1].Add(w); // the weights on the edges are placed in 'cost', in the same places as the edges themselves are inside the 'adj' array
            }

            string[] xy = Console.ReadLine().Split(); // the last line is for storing the 'u' and 'v' VERTICES which represent start and end point for the flight
            x = int.Parse(xy[0]) - 1;
            y = int.Parse(xy[1]) - 1;

            Console.WriteLine(Distance(adj, cost, x, y)); // Good job! (Max time used: 0.21/3.00, max memory used: 43380736/536870912.)

            Console.ReadKey();
        }
    }
}
