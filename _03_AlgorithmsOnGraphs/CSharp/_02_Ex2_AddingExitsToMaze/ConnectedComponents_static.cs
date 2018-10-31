using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Ex2_AddingExitsToMaze // DECOMPOSITION OF GRAPHS 1: EXCERCISE 2 - Given an undirected graph with 𝑛 vertices and 𝑚 edges, compute the number of connected components
{
    class ConnectedComponents // the whole solution is based on STATIC method 'NumberOfComponents' as such was the provided Java template. This implies the use of STATIC FIELDS
    {
        static List<int>[] adj; // global adjacency list
        static bool[] nodes; // global array to store the nodes

        static int NumberOfComponents()
        {
            int result = 0; // this is for counting the number of groups, we only need to return the last value
            for (int v = 0; v < nodes.Length; v++) // we use 'for' loop to have the INDEXES available as an argument for 'Explore' method
            {
                if (!nodes[v])
                {
                    Explore(v);
                    result += 1; // when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter
                }
            }
            return result;
        }
    
        static void Explore(int v) // recursive DFS method that looks around the given VERTEX and find all other connected VERTICES (direct and indirect)
        {
            nodes[v] = true;
            foreach (int w in adj[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
            {
                if (!nodes[w])
                    Explore(w);
            }
        }


        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            adj = new List<int>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            nodes = new bool[adj.Length];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = false;

            for (int i = 0; i < m; i++)
            {
                int x, y;
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1);
                adj[y - 1].Add(x - 1);
            }

            Console.WriteLine(NumberOfComponents()); // Good job! (Max time used: 0.06/1.50, max memory used: 8589312/536870912.)

            Console.ReadKey();
        }
    }
}
