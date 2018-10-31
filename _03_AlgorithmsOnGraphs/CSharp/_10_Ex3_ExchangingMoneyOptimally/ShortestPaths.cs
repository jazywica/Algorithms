using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Ex3_ExchangingMoneyOptimally // PATHS IN GRAPHS 2: EXCERCISE 3 - Given an directed graph with possibly negative edge weights and with n vertices and m edges as well as its vertex s, compute the length of shortest paths from s to all other vertices of the graph.
{
    public class Shortest_Paths
    {
        public static void ShortestPaths(List<int>[] adj, List<int>[] cost, int s, long[] distance, int[] reachable, int[] shortest)
        {
            int n = adj.Length;
            HashSet<int> a_nodes = new HashSet<int>(); // stores the set A of all affected nodes. since we only want to add a node once, it is best to use a set
            int[] path = new int[n];
            bool[] visited = new bool[n];
            for (int i = 0; i < n; i++) { path[i] = -1; visited[i] = false; }
            List<int> current = new List<int>(); // list for excluding non-reachable nodes
            Queue<int> q = new Queue<int>(new int[] { s });


            // 1. Checking connectivity with BFS
            while (q.Count > 0) // Breadth-First Search for looking for all reachable bodes
            {
                int u = q.Dequeue();
                reachable[u] = 1; // all reachable nodes are going to be enqueued sooner all later, including the first element
                current.Add(u);
                foreach (int v in adj[u])
                {
                    if (visited[v] == false)
                    {
                        q.Enqueue(v);
                        visited[v] = true;
                    }
                }
            }

            // 2. Detecting negative cycles - there must be 'n' iterations over all reachable nodes
            distance[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
            for (int iter = 0; iter < n; iter++) {
                bool is_changed = false; // extra flag to close the procedure quicker, if an iteration doesn't change anything
                foreach (int u in current) {
                    for (int i = 0; i < adj[u].Count; i++) {
                        int v = adj[u][i]; // v stores the index of the node on the other end of the edge, as usual

                        if (distance[u] == long.MaxValue) // if the starting node u hasn't been discovered yet, we just move on
                            continue;

                        else if (distance[v] > distance[u] + cost[u][i]) // for visited neighbours we RELAX THE EDGES if possible
                        {
                            distance[v] = distance[u] + cost[u][i];
                            path[v] = u; // here we store the fastest path, we can't do it in the BFS above, as we are going to backtrack the cycle, as it was discovered
                            is_changed = true;

                            if (iter == n - 1)
                            {
                                a_nodes.Add(u);
                                foreach (int k in adj[u])
                                    a_nodes.Add(k);
                            }
                        }
                    }
                }
                if (is_changed == false) // here we check after each iteration if there has been a change, we exit function if it hasn't
                    return;
            }

            // 3. Track all the cycles from the discovered nodes - we are going to remove all nodes present in the current cycle from 'a_nodes' on the fly
            if (a_nodes.Count > 0)
            {
                HashSet<int> result = new HashSet<int>();

                while (a_nodes.Count > 0)
                {
                    int u = a_nodes.First();
                    result.UnionWith(ReconstructCycle(a_nodes, path, u));
                    a_nodes.ExceptWith(result); // here we take out all the nodes in the current cycle form the main set, just so we don't repeat the reconstruction more than once for each cycle
                }

                if (result.Contains(s)) // if the start node is in the cycle, then we have to also check the path from the main node to the cycle
                    for (int i = 0; i < n; i++)
                         shortest[i]= 0;
                else
                    foreach (int cyc in result)
                        shortest[cyc] = 0;
            }
        }


        static HashSet<int> ReconstructCycle(HashSet<int> a_nodes, int[] path, int x) // helper function that gathers the cycle and returns it
        {
            int u = x; // this is to store the initial value that should end the cycle search
            int count = 0;
            do{
                if (count > path.Length) // extra condition when we can't get to where we started from by backtracking
                    break;
                count++;
                a_nodes.Add(u);
                u = path[u];
            } while (u != x);

            return a_nodes;
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
                cost[x - 1].Add(w); // the weights on the edges are placed in 'cost', in the same places as the edges themselves are inside the 'adj' array
            }

            int s = int.Parse(Console.ReadLine()) - 1;
            long[] distance = new long[n];
            int[] reachable = new int[n];
            int[] shortest = new int[n];

            for (int i = 0; i < n; i++)
            {
                distance[i] = long.MaxValue;
                reachable[i] = 0;
                shortest[i] = 1;
            }

            ShortestPaths(adj, cost, s, distance, reachable, shortest); // Good job! (Max time used: 0.33/3.00, max memory used: 13094912/536870912.)

            for (int i = 0; i < n; i++)
            {
                if (reachable[i] == 0) // output "*", if there is no path from s to u;
                    Console.WriteLine('*');
                else if (shortest[i] == 0) // "-", if there is a path from s to u, but there is no shortest path from s to u (that is, the distance from s to u is -infinity);
                    Console.WriteLine('-');
                else
                    Console.WriteLine(distance[i]); // the length of a shortest path otherwise.
            }

            Console.ReadKey();
        }
    }
}
