using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Ex1_MinimumCostOfFlight // PATHS IN GRAPHS 2: EXCERCISE 1 - Given an directed graph with positive edge weights and with n vertices and m edges as well as two vertices u and v, compute the weight of a shortest path between u and v (that is, the minimum total weight of a path from u to v).
{
    class CustomComparer : IComparer<long[]> // custom comparer for sorting elements in SortedSet by distance
    {
        public int Compare(long[] left, long[] right) {
            int comp = left[1].CompareTo(right[1]);
            if(comp == 0)
                return left[0].CompareTo(right[0]); // this will prevent from deleting all nodes with the same initial value
            return comp;
        }
    }

    class Dijkstra
    {
        static long Distance(List<int>[] adj, List<int>[] cost, int s, int t) // Dijkstra algorithm implemented with priority queue data structure
        {
            long[] dist = new long[adj.Length];
            SortedSet<long[]> priorityQueue = new SortedSet<long[]>(new CustomComparer()); // this queue is just to keep track of the minimum distances, we still need arrays as we can't address sets directly

            for (int i = 0; i < adj.Length; i++)
            {
                dist[i] = int.MaxValue;
                if (i == s)
                    priorityQueue.Add(new long[] { i, 0 });
                else
                    priorityQueue.Add(new long[] { i, int.MaxValue });
            }

            dist[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

            while (priorityQueue.Count != 0)
            {
                long[] U = priorityQueue.First(); // we need a handle for this element to update it later on
                priorityQueue.Remove(U);
                int u = (int)U[0];

                for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                {
                    int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                    if (dist[v] > dist[u] + cost[u][i]) // for direct neighbours we RELAX THE EDGES if possible
                    {
						long oldDist = dist[v];
                        dist[v] = dist[u] + cost[u][i];

                        priorityQueue.Remove(new long[]{ v, oldDist}); // here we Change Priority by subtracting and adding again a new value, as this is the only way to get the things sorted
                        priorityQueue.Add(new long[] { v, dist[u] + cost[u][i] });
                    }
                }
            }

            if (dist[t] == int.MaxValue)
                return -1;
            else
                return dist[t];
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

            Console.WriteLine(Distance(adj, cost, x, y)); // Good job! (Max time used: 0.21/3.00, max memory used: 43376640/536870912.)

            Console.ReadKey();
        }
    }
}
