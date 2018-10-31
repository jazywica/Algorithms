using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_MinimumSpanningTrees // Example showing Spanning Trees on an undirected graph G(V, E) with positive edges, based on the '_11_SpanningTrees_1.JPG'
{
    // A TREE is an undirected graph that is connected and ACYCLIC
    // TREE with 'n' vertices has n-1 edges
    // Any connected undirected graph G(V,E) with |E| = |V|-1 is a TREE
    //An undirected graph is a tree if there is an unique path between any pair of its vertices

    class CustomComparer : IComparer<long[]> // custom comparer for sorting elements in SortedSet by distance (For PRIMS algorithm)
    {
        public int Compare(long[] left, long[] right)
        {
            int comp = left[1].CompareTo(right[1]);
            if (comp == 0)
                return left[0].CompareTo(right[0]); // this will prevent from deleting all nodes with the same initial value
            return comp;
        }
    }

    class SpanningTrees
    {
        List<int>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        List<int>[] cost; // list for storing the initial uncorrected costs
        int distance = 0; // total distance of the minimum path

        HashSet<HashSet<string>> nodes = new HashSet<HashSet<string>>(); // Containers for Kruskal
        Dictionary<int, string> Labels = new Dictionary<int, string> { { 0, "A" }, { 1, "B" }, { 2, "C" }, { 3, "D" }, { 4, "E" }, { 5, "F" } }; // just to present nodes the same way as in lectures
        HashSet<string> X = new HashSet<string>(); // final output showing the picked edges

        long[] dist; // containers for Prim
        SortedSet<long[]> priorityQueue = new SortedSet<long[]>(new CustomComparer()); // this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly

        SpanningTrees(List<int>[] adjList, List<int>[] costList)
        {
            adj = adjList;
            cost = costList;
            dist = new long[adj.Length];
            for (int i = 0; i < adj.Length; i++) {
                nodes.Add(new HashSet<string>() { Labels[i] }); // initializing the initial sets for Kruskal as : A, B, C, D, E, F
                dist[i] = int.MaxValue; // initializing distances for Prim
            }
        }

        // I. Kruskal algorithm - doesn't require a start point, as it sorts and picks the shortest edges independently from vertices
        int Kruskal()
        {
            List<int[]> edges = MakeEdges();
            edges.Sort((a, b) => (a[0].CompareTo(b[0])));

            for (int idx = 0; idx < edges.Count; idx++)
            {
                HashSet<string> u = Find(Labels[edges[idx][1]]);
                HashSet<string> v = Find(Labels[edges[idx][2]]);
                if (!u.SetEquals(v))
                {
                    u.UnionWith(v);
                    nodes.Remove(v);
                    X.Add(Labels[edges[idx][1]] + Labels[edges[idx][2]]);
                    distance += edges[idx][0];
                }
            }

            DisplayValues();

            return distance;
        }


        HashSet<string> Find(string seq)
        {
            foreach (HashSet<string> item in nodes)
            {
                if (item.Contains(seq))
                    return item;
            }
            return new HashSet<string>();
        }


        List<int[]> MakeEdges() // helper function to convert adjacency list into single edges
        {
            List<int[]> result = new List<int[]>();
            HashSet<string> duplicates = new HashSet<string>();

            for (int i = 0; i < adj.Length; i++){
                for (int j = 0; j < adj[i].Count; j++){
                    if (!duplicates.Contains(i.ToString() + adj[i][j].ToString())) // here we check if we are not taking any duplicates, we only need an edge once
                    {
                        duplicates.Add(adj[i][j].ToString() + i.ToString());
                        result.Add(new int[3]);
                        int[] current = result.Last();
                        current[0] = cost[i][j];
                        current[1] = i;
                        current[2] = adj[i][j];
                    }
                }
            }
            return result;
        }

        void DisplayValues() // testing function that prints all node properties
        {
            Console.WriteLine("the following edges were picked as the shortest (only true for Kruskal):");
            foreach (var item in X)
                Console.Write(item + " ");
            Console.WriteLine();
        }


        // 2. Prim's algorithm, implemented with a Priority Queue (modified SortedSet) - it requires a start point and works the same way as Dijkstra
        long Prim(int s)
        {
            long result = 0;
            dist[s] = 0;

            for (int i = 0; i < adj.Length; i++) {
                if (i == s)
                    priorityQueue.Add(new long[] { (int)i, 0 }); // since we can't address the element of a set, we can put it in while populating the collection
                else
                    priorityQueue.Add(new long[] { (int)i, int.MaxValue });
            }

            while (priorityQueue.Count != 0)
            {
                long[] U = priorityQueue.First(); // we need a handle for this element to update it later on
                priorityQueue.Remove(U);
                int u = (int)U[0];

                for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                {
                    int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                    if (dist[v] > cost[u][i] && priorityQueue.Contains(new long[] { v, dist[v] })) // since we are only comparing single distances, we can't look back, so we only take into account
                    {
                        long oldDist = dist[v]; // for direct neighbours we RELAX THE EDGES if possible
                        dist[v] = cost[u][i];

                        priorityQueue.Remove(new long[] { (int)v, oldDist }); // here we Change Priority by subtracting and adding again a new value, as this is the only way to get the things sorted
                        priorityQueue.Add(new long[] { (int)v, cost[u][i] });
                    }
                }
                result += dist[u];
            }

            return result;
        }


        static void Main(string[] args)
        {
            List<int>[] adj = { new List<int>() { 1, 3, 4 }, new List<int>() { 0, 2, 4, 5 }, new List<int>() { 1, 5 }, new List<int>() { 0, 4 }, new List<int>() { 0, 1, 3, 5 }, new List<int>() { 1, 2, 4 } };
            List<int>[] adj_cost = { new List<int>() { 4, 2, 1 }, new List<int>() { 4, 8, 5, 6 }, new List<int>() { 8, 1 }, new List<int>() { 2, 3 }, new List<int>() { 1, 5, 3, 9 }, new List<int>() { 6, 1, 9 } };

            Console.WriteLine("\nKruskal's Algorithm:");
            SpanningTrees graph_kruskal = new SpanningTrees(adj, adj_cost);
            int route_kruskal = graph_kruskal.Kruskal();
            Console.WriteLine(route_kruskal);

            Console.WriteLine("\nPrim's Algorithm:");
            SpanningTrees graph_prim = new SpanningTrees(adj, adj_cost);
            long route_prim = graph_prim.Prim(2); // we start from the same node is the same as node C in the picture
            Console.WriteLine(route_prim);

            Console.ReadKey();
        }
    }
}
