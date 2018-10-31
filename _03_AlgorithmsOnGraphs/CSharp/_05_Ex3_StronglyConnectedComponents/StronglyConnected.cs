using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Ex3_StronglyConnectedComponents // DECOMPOSITION OF GRAPHS 2: EXCERCISE 3 - Compute the number of strongly connected components of a given directed graph with 𝑛 vertices and 𝑚 edges.
{
    class StronglyConnected // the whole solution is based on STATIC method 'NumberOfStronglyConnectedComponents' as such was the provided Java template
    {
        static int NumberOfStronglyConnectedComponents(List<int>[] adj) // we have to run 'Explore' procedures on the postorder values, so each time we go through a new SCC we start from a SINK VERTEX
        {
            bool[] nodes = new bool[adj.Length]; // the storage for node status has to be initialized with unvisited values
            bool[] nodes_rev = new bool[adj.Length];
            Stack<int> postIndex = new Stack<int>(); // stack for storing the list of reversed 'post' indexes

            List<int>[] adj_rev = new List<int>[adj.Length]; // initializing the reversed list
            for (int i = 0; i < adj.Length; i++)
                adj_rev[i] = new List<int>();

            for (int i = 0; i < adj.Length; i++)  // reversing the order of the graph edges for DFS
            {
                foreach (var element in adj[i])
                    adj_rev[element].Add(i);
            }

            for (int v = 0; v < nodes_rev.Length; v++)  // 'DFS' procedure scans the REVERSED graph for SINK VERTEXES in SCCs
            {
                if (!nodes_rev[v])
                    Explore(adj_rev, nodes_rev, postIndex, v, true);
            }

            int result = 0; // we are going to use the 'counter' at this point to group the SCCs

            foreach (int v in postIndex) // here we go by the whole list of recorded post-visit values that we obtained from DFS and explore the real graph
            {
                if (!nodes[v])
                {
                    Explore(adj, nodes, postIndex, v, false);
                    result += 1; // when we finished with our recursive calls and we are on the way out, we increment the SCC counter
                }
            }

            return result;
        }


        static void Explore(List<int>[] adj, bool[] nodes, Stack<int> postIndex, int v, bool storeIndex) // multipurpose 'Explode' function for both the standard and reversed graph
        {
            nodes[v] = true; // before we do anything we first mark the NODE we are at as 'visited'

            foreach (int w in adj[v])
                if (!nodes[w])
                    Explore(adj, nodes, postIndex, w, storeIndex);

            if (storeIndex)
                postIndex.Push(v); // we store the post-order values as we go out of RECURSION only when we scan the REVERSED GRAPH
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
                adj[x - 1].Add(y - 1);
            }

            Console.WriteLine(NumberOfStronglyConnectedComponents(adj)); // Good job! (Max time used: 0.11/1.50, max memory used: 12660736/536870912.)

            Console.ReadKey();
        }
    }
}
