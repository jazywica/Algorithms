using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_MostDirectRoute // Simple example showing a BFS algorithm on a directed graph's G(V, E), based on the '_06_BreadthFirstSearch.JPG'
{
    // LENGTH of the path L(P) in the number of edges in the path
    // DISTANCE between two vertices is the length of the shortest path between them

    class MostDirectRoute
    {
        List<int>[] adj;
        int[] dist; // stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
        Queue<int> q; // queue to store nodes in BFS
        int?[] prev; // stores the previous node, which is the node that the current node was discovered from - for the shortest path algorithm
        int n;

        MostDirectRoute(List<int>[] inputList) // inside the constructor we commence all the preparatory work
        {
            adj = inputList;
            dist = new int[adj.Length];
            prev = new int?[adj.Length];
            q = new Queue<int>();
            n = adj.Length;
        }


        void BFS(int s) // Breadth-First Search for exploring a graph
        {
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
            PrintDistance();
        }


        Stack<int> BFS_ShortestPath(int s, int t) // Breadth-First Search for looking for the shortest path in a graph by storing the nodes that the current node was discovered from
        {
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

            return ReconstructPath(s, t);
        }


        Stack<int> ReconstructPath(int s, int u)
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


        void PrintGroup(List<List<int>> group) // extra method for an extra parameter, which we pass as an argument
        {
            for (int i = 0; i < group.Count; i++)
            {
                Console.Write("to layer {0} belong nodes: ", i);
                foreach (int j in group[i])
                    Console.Write(j + " ");
                Console.WriteLine();
            }
        }


        void PrintDistance()
        {
            for (int i = 0; i < dist.Length; i++)
                Console.WriteLine("the node {0} is at layer: {1}", i, dist[i]);
        }


        static void Main(string[] args)
        {
            List<int>[] adjList = { new List<int>() { 2 }, new List<int>() { 2, 3 }, new List<int>() { 1, 4, 5 }, new List<int>() { 1 }, new List<int>() { 7 }, new List<int>(), new List<int>() { 2 }, new List<int>() { 5 } };

            MostDirectRoute bfs_1 = new MostDirectRoute(adjList);
            bfs_1.BFS(0);
            Console.WriteLine();

            MostDirectRoute bfs_2 = new MostDirectRoute(adjList);
            Stack<int> shortestPath = bfs_2.BFS_ShortestPath(0, 7);
            foreach (int x in shortestPath)
                Console.Write(x + " ");

            Console.ReadKey();
        }
    }
}
