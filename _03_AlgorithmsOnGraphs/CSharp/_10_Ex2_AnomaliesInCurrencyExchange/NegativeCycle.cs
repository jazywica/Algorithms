using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Ex2_AnomaliesInCurrencyExchange // PATHS IN GRAPHS 2: EXCERCISE 2 - Given an directed graph with possibly negative edge weights and with n vertices and m edges, check whether it contains a cycle of negative weight.
{
    class Negative_Cycle
    {
        static int NegativeCycle(List<int>[] adj, List<int>[] cost)
        {
            int n = adj.Length;
            long[] dist = new long[n];
            for (int i = 0; i < n; i++)
                dist[i] = 0;  // since there is no start point we may as well initialize everything with 0, as the negative cycle will decrease the values to negative anyway

            for (int iter = 0; iter < n; iter++) // theoretically it is enough to run |V| - 1 iterations, so if something changes on the |V|th then we detected a cycle
            {
                bool is_changed = false; // extra flag to close the procedure quicker if an iteration doesn't change anything
                for (int u = 0; u < n; u++) // here we start running and optimizing all edges in a graph
                {
                    for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    {
                        int v = adj[u][i]; // v stores the index of the node on the other end of the edge, as usual

                        if (dist[v] > dist[u] + cost[u][i]) // for visited neighbours we RELAX THE EDGES if possible
                        {
                            dist[v] = dist[u] + cost[u][i];
                            is_changed = true;

                            if (iter == n - 1) // if there is a cycle, it will be detected on the |V|-1 iteration
                                return 1;
                        }
                    }
                }
                if (is_changed == false) // here we check after each iteration if there has been a change, we return if there wasn't
                    return 0;
            }
            return 0; // if no cycles have been found we just return an empty list
        }


        static void Main(string[] args)
        {
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
                int x, y, w;
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                w = int.Parse(input[2]);
                adj[x - 1].Add(y - 1);
                cost[x - 1].Add(w); // the weights on the edges are placed in 'cost', in the same places as the edges are inside the 'adj' array
            }

            Console.WriteLine(NegativeCycle(adj, cost)); // Good job!(Max time used: 0.39 / 3.00, max memory used: 11689984 / 536870912.)

            Console.ReadKey();
        }
    }
}
