using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Ex3_StronglyConnectedComponents // DECOMPOSITION OF GRAPHS 2: EXCERCISE 3 - Compute the number of strongly connected components of a given directed graph with 𝑛 vertices and 𝑚 edges.
{
    class StronglyConnected // the whole solution is based on STATIC method 'NumberOfStronglyConnectedComponents' as such was the provided Java template. This implies the use of STATIC FIELDS
    {
        static List<int>[] adjList; // global adjacency list
        static List<int>[] adjList_rev;
        static bool[] nodes; // global array to store the nodes
        static bool[] nodes_rev;
        static Stack<int> postIndex = new Stack<int>(); // this is for storing the list of reversed 'post' indexes


        static int NumberOfStronglyConnectedComponents(List<int>[] adj) // in order to get the SCCs right we have to run 'explore' procedures on the postorder values, so each time we go through the SCC we start from a SINK VERTEX
        {
            adjList = adj;
            //adjList_rev = new List<int>[adj.Length];
            nodes = new bool[adj.Length]; // the storage for node status has to be initialized with unvisited values
            nodes_rev = new bool[adj.Length];

            for (int v = 0; v < nodes_rev.Length; v++) // before we start anything, we need to run DFS on the REVERSED Graph in order to find the SINK VERTICES, for the SCC procedure to work
                if (!nodes_rev[v])
                    Explore(v);

            int result = 0; // we are going to use the 'counter' at this point to group the SCCs

            foreach (var v in postIndex) // here we go by the whole list of recorded post-visit values that we obtained from DFS
                if (!nodes[v]) // if we haven't explored the vertex yet (false), then we go further
                {
                    ExploreComponents(v);
                    result += 1; // when we finished with our recursive calls and we are on the way out, we increment the counter
                }

            return result;
        }


        static void Explore(int v) // the standard 'Explode' function is just to aid the DFS procedure of the REVERSED GRAPH
        {
            nodes_rev[v] = true; // before we do anything we first mark the NODE we are at as 'visited'

            foreach (int w in adjList_rev[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
                if (!nodes_rev[w]) // if we haven't explored the vertex yet (false), then we go further
                    Explore(w); // here we call 'Explore' recursively

            postIndex.Push(v); // we kind of 'wrap up' the post-order values into a list as we go out of RECURSION
        }


        static void ExploreComponents(int v) // this is the version of the 'explore' procedure for SCC
        {
            nodes[v] = true; // before we do anything we first mark the NODE we are at as 'visited'

            foreach (int w in adjList[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
                if (!nodes[w]) // if we haven't explored the vertex yet (false), then we go further
                    ExploreComponents(w); // here we call 'Explore' recursively
        }
       

        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            List<int>[] adj = new List<int>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            adjList_rev = new List<int>[n];
            for (int i = 0; i < n; i++)
                adjList_rev[i] = new List<int>();

            for (int i = 0; i < m; i++)
            {
                int x, y;
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1);
                adjList_rev[y - 1].Add(x - 1); // here we create a REVERSED ADJACENCY LIST, by swapping the order of all edges
            }

            Console.WriteLine(NumberOfStronglyConnectedComponents(adj)); // Good job! (Max time used: 0.08/1.50, max memory used: 15384576/536870912.)

            Console.ReadKey();
        }
    }
}
