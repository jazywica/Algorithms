using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_MostDirectRoute // Simple example showing a BFS algorithm on a directed graph's G(V, E), based on the '_06_BreadthFirstSearch.JPG'
{
    class MostDirectRoute
    {
        // LENGTH of the path L(P) in the number of edges in the path
        // DISTANCE between two vertices is the length of the shortest path between them

        static void BFS(List<int>[] adj, int s) // Breadth-First Search for exploring a graph
        {
            int n = adj.Length;
            int[] dist = new int[n]; // stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
            Queue<int> q = new Queue<int>(); // queue to store nodes in BFS
            List<List<int>> layers = new List<List<int>>() { new List<int>(s) }; // extra item for grouping the nodes into the layers

            for (int i = 0; i < n; i++)
                dist[i] = n; // the biggest possible distance is the number of nodes - 1
                    
            dist[s] = 0; // zero distance to the starter node itself
            q.Enqueue(s); // the starting node is the only one that gets into the queue prior to BFS

            while (q.Count > 0)
            {
                int u = q.Dequeue();
                foreach (int v in adj[u])
                {
                    if (dist[v] == n)
                    {
                        q.Enqueue(v);
                        dist[v] = dist[u] + 1;
                        if (layers.Count < dist[u] + 2) // extra item. the inner lists must be initialized before we can use them
                            layers.Add(new List<int>());
                        layers[dist[u] + 1].Add(v);
                    }
                }
            }

            PrintGroup(layers);
            Console.WriteLine();
            PrintDistance(dist);
        }


        static Stack<int> BFS_ShortestPath(List<int>[] adj, int s) // Breadth-First Search for looking for the shortest path in a graph by storing the nodes that a certain node was discovered from
        {
            int n = adj.Length;
            int[] dist = new int[n]; // stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
            int?[] prev = new int?[n]; // stores the previous node, which is the node that the current node was discovered from - for the shortest path algorithm
            Queue<int> q = new Queue<int>(); // queue to store nodes in BFS

            for (int i = 0; i < n; i++)
            {
                dist[i] = n; // the biggest possible distance is the number of nodes - 1
                prev[i] = null;
            }

            dist[s] = 0; // zero distance to the starter node itself
            q.Enqueue(s); // the starting node is the only one that gets into the queue prior to BFS

            while (q.Count > 0)
            {
                int u = q.Dequeue();
                foreach (int v in adj[u])
                {
                    if (dist[v] == n)
                    {
                        q.Enqueue(v);
                        dist[v] = dist[u] + 1;
                        prev[v] = u; // here we store the 'previous node, so we can backtrack it and find the shortest path
                    }
                }
            }

            return ReconstructPath(s, 7, prev);
        }

        static Stack<int> ReconstructPath(int s, int u, int?[] prev)
        {
            Stack<int> result = new Stack<int>();
            while (u != s)
            {
                result.Push(u);
                u = (int)prev[u];
            }
            result.Push(s); // the loop above stops when reaching the start node, so if we want, we can include it here
            return result;
        }


        static void PrintGroup(List<List<int>> group)
        {
            for (int i = 0; i < group.Count; i++)
            {
                Console.Write("to layer {0} belong nodes: ", i);
                foreach (int j in group[i])
                    Console.Write(j + " ");
                Console.WriteLine();
            }
        }

        static void PrintDistance(int[] distance)
        {
            for (int i = 0; i < distance.Length; i++)
                Console.WriteLine("the node {0} is at layer: {1}", i, distance[i]);
        }


        static void Main(string[] args)
        {
            List<int>[] adjLst = { new List<int>() { 2 }, new List<int>() { 2, 3 }, new List<int>() { 1, 4, 5 }, new List<int>() { 1 }, new List<int>() { 7 }, new List<int>(), new List<int>() { 2 }, new List<int>() { 5 } };

            BFS(adjLst, 0);
            Console.WriteLine();

            Stack<int> shortestPath = BFS_ShortestPath(adjLst, 0);
            foreach (int x in shortestPath)
                Console.Write(x + " ");

            Console.ReadKey();
        }
    }
}
