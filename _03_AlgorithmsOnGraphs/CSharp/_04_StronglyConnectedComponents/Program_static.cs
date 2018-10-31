using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_ConnectedComponents // Connectivity in DIRECTED GRAPHS
{
    // CONNECTION - Two vertices 'v' and 'w' in a directed graph are connected if you can reach 'v' from 'w' and can reach 'w' from 'v'
    // STRONGLY CONNECTED COMPONENTS - a partition of a directed graph, where two vertices are connected if and only if they are in the same component. Once you leave the SCC you can't come back
    // METAGRAPH - is a way to show how the SCC connect to one another, so it always becomes a DAG because the SCC has been 'compressed' into partitions, so there are no cycles anymore

    class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
    {
        public bool visited = false; // flags if a VERTEX was visited or not
        public int? group = null; // keeps track of CONNECTED COMPONENTS within the graph
        public int? previsit = null; // these two are for storing the order in which DFS went in and out each NODE
        public int? postvisit = null;
    }

    class Graph
    {
        static List<int>[] adj = { new List<int>() { 1 }, new List<int>() { 2, 3 }, new List<int>() { 0, 1, 4, 6 }, new List<int>() { 0 }, new List<int>() { 0, 2 }, new List<int>() { 3 }, new List<int>(), new List<int>() { 3, 4, 5 } };
        static Vertex[] nodes = { new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex() };
        static List<int>[] adj_rev = { new List<int>() { 2, 3, 4 }, new List<int>() { 0, 2 }, new List<int>() { 1, 4 }, new List<int>() { 1, 5, 7 }, new List<int>() { 2, 7 }, new List<int>() { 7 }, new List<int>() { 2 }, new List<int>() };
        static Vertex[] nodes_rev = { new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex(), new Vertex() };

        static Stack<int> postValue = new Stack<int>();
        static Stack<int> postIndex = new Stack<int>(); // this is the order of the post-visit VERTICES we are going to follow

        static List<List<int>> groups = new List<List<int>>(); // this will store a list of SCC grouped also into the lists
        static List<int> g = new List<int>(); // this is a container for elements in each group

        static int counter; // this is just a variable (for the 'group' function) of VALUE type so it doesn't have to be initialized
        static int clock = 1; // this variable is for the pre and post visiting functions

        static void Explore(int v) // the standard 'Explode' function is just to aid the DFS procedure of the REVERSED GRAPH
        {
            nodes_rev[v].visited = true; // before we do anything we first mark the NODE we are at as 'visited'
            nodes_rev[v].group = counter; // at this point we can use the property called 'group' that will segregate all the reachable nodes into one group
            PreVisit(v); // we first introduce the pre-visit method before we start the recursion

            foreach (int w in adj_rev[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
                if (!nodes_rev[w].visited) // if we haven't explored the vertex yet (false), then we go further
                    Explore(w); // here we call 'Explore' recursively

            postValue.Push(clock);
            postIndex.Push(v);
            PostVisit(v); // we kind of 'wrap up' the post-order as we go out of RECURSION
        }

        static void PreVisit(int v) { nodes_rev[v].previsit = clock; clock += 1; } // these two functions here are supposed to keep track of what the DFS is doing inside the GRAPH
        static void PostVisit(int v) { nodes_rev[v].postvisit = clock; clock += 1; } // we note when a group opens and closes , so in this example everything between 7 and 16 is within a group that starts with '2'

        static void DepthFirstSearch() // 'DFS' in this case is only cover the REVERSED graph search for a SINK VERTEX
        {
            counter = 1; // we are going to use the 'counter' at this point to group the connected nodes separately
            for (int v = 0; v < nodes_rev.Length; v++) // we used 'for' loop to have the INDEXES available as an argument for 'Explore' method
                if (!nodes_rev[v].visited) // if we haven't explored the vertex yet (false), then we go further
                {
                    Explore(v);
                    counter += 1; // when we finished with our recursive calls and we are on the way out, we increment the counter 
                }
            DisplayingValues(); // this is just for testing*******************************************************************************************************************************************
        }

        static int ConnectedComponents() // in order to get the SCCs right we have to run 'explore' procedures on the postorder values, so each time we go through the SCC we start from a SINK VERTEX
        {
            DepthFirstSearch(); // before we start anything, we need to run DFS on the REVERSED Graph in order to find the SINK VERTICES, for the SCC procedure to work
            counter = 0; // we are going to use the 'counter' at this point to group the connected nodes separately

            foreach(var v in postIndex) // here we go by the whole list of recorded post-visit values
            {
                if (!nodes[v].visited) // if we haven't explored the vertex yet (false), then we go further
                {
                    ExploreComponents(v);
                    counter += 1; // when we finished with our recursive calls and we are on the way out, we increment the counter
                    groups.Add(g.ToList()); // we have to use the 'To.List()' method to pass the new list as a VALUE, otherwise we will clear it in the next line and therefore won't change anything
                    g.Clear();
                }
            }
            PrintGroup(); // this is just to display the elements grouped into the 'group' function
            return counter;
        }

        static void ExploreComponents(int v) // this is the version of the 'explore' procedure for SCC
        {
            nodes[v].visited = true; // before we do anything we first mark the NODE we are at as 'visited'

            foreach (int w in adj[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
                if (!nodes[w].visited) // if we haven't explored the vertex yet (false), then we go further
                    ExploreComponents(w); // here we call 'Explore' recursively

            g.Add(v); // we kind of 'wrap up' the post-order as we go out of RECURSION by adding the element to the list and then removing it from the STACK
        }

        static void DisplayingValues() // this is a testing procedure for displaying the details of the REVERSED DFS
        {
            for (int i = 0; i < nodes.Length; i++)
                Console.WriteLine("node index: {0}, belongs to group: {1}, pre: {2}, post: {3}", i, nodes_rev[i].group, nodes_rev[i].previsit, nodes_rev[i].postvisit);

            foreach (var i in postValue)
                Console.Write(i + " ");
            Console.WriteLine();
            foreach (var i in postIndex)
                Console.Write(i + " ");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void PrintGroup()
        {
            foreach (var i in groups)
            {
                foreach (var j in i)
                    Console.Write(j + " ");
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(ConnectedComponents());
            Console.ReadKey();
        }
    }
}
