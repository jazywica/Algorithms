using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Ex2_DeterminingOrderOfCourses // DECOMPOSITION OF GRAPHS 2: EXCERCISE 2 - Compute a topological ordering of a given directed acyclic graph (DAG) with 𝑛 vertices and 𝑚 edges.
{
    class TopologicalSort // the whole solution is based on STATIC method 'Toposort' as such was the provided Java template.
    {
        static Stack<int> Toposort(List<int>[] adj) // we are going to put the elements on a STACK, as we start placing the elements from the end to the start
        {
            bool[] nodes = new bool[adj.Length]; // the storage for node status has to be initialized with unvisited values
            Stack<int> order = new Stack<int>(); // this is for storing the TOPOLOGICAL SORT

            for (int v = 0; v < nodes.Length; v++)
            {
                if (!nodes[v])
                    DFS(adj, nodes, order, v);
            }

            return order;
        }


        static void DFS(List<int>[] adj, bool[] nodes, Stack<int> order, int v) // helper function that will look around the given VERTEX and find all other connected VERTICES (direct and indirect)
        {
            nodes[v] = true;

            foreach (int w in adj[v])
                if (!nodes[w])
                    DFS(adj, nodes, order, w);

            order.Push(v); // this is where we normally store the 'post-order' value for topological sort - where we exit the recursion
        }


        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            List<int>[] adj = new List<int>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            for (int i = 0; i < m; i++)
            {
                int x, y;
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1); // in ORDERED GRAPHS we take the start vertex and append the END VERTEX to it, applying correction for 0-based indexing
            }

            Stack<int> order = Toposort(adj); // Good job! (Max time used: 0.54/3.00, max memory used: 37621760/536870912.)
            foreach (int x in order)
                Console.Write((x + 1) + " ");

            Console.ReadKey();
        }
    }
}
