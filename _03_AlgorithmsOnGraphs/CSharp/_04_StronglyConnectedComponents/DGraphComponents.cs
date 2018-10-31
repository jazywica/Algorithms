using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_ConnectedComponents // Simple example showing Connectivity in DIRECTED GRAPHS, based on the '_04_StronglyConnectedComponents.JPG'
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


    class DGraphComponents // class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests
    {
        List<int>[] adj;
        List<int>[] adj_rev;
        Vertex[] nodes;
        Vertex[] nodes_rev;

        Stack<int> postValue = new Stack<int>(); // order of the post-visit VERTICES we are going to follow
        Stack<int> postIndex = new Stack<int>();
        List<List<int>> groups = new List<List<int>>(); // stores a list of SCC grouped also into lists
        List<int> g = new List<int>(); // container for elements in each group
        int counter; // field for the 'group' function
        int clock = 1; // field for the pre and post visiting functions


        DGraphComponents(List<int>[] inputList, List<int>[] inputList_rev) // inside the constructor we commence all the preparatory work
        {
            adj = inputList;
            adj_rev = inputList_rev;
            nodes = new Vertex[inputList.Length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects
            nodes_rev = new Vertex[inputList.Length];
            for (int i = 0; i < nodes.Length; i++) { nodes[i] = new Vertex(); nodes_rev[i] = new Vertex(); }
        }


        void PreVisit(int v) { nodes_rev[v].previsit = clock; clock += 1; } // these two functions here are supposed to keep track of what the DFS is doing inside the GRAPH
        void PostVisit(int v) { nodes_rev[v].postvisit = clock; clock += 1; }


        void Explore(int v) // the standard 'Explore' function is just to aid the DFS procedure of the REVERSED GRAPH
        {
            nodes_rev[v].visited = true;
            nodes_rev[v].group = counter;
            PreVisit(v);

            foreach (int w in adj_rev[v])
                if (!nodes_rev[w].visited)
                    Explore(w);

            postValue.Push(clock); // here we note the post-order as we are backtracking
            postIndex.Push(v);
            PostVisit(v);
        }


        void DepthFirstSearch() // 'DFS' procedure scans the REVERSED graph for SINK VERTEXES in SCCs
        {
            counter = 1;
            for (int v = 0; v < nodes_rev.Length; v++)
                if (!nodes_rev[v].visited) 
                {
                    Explore(v);
                    counter += 1; // when we finished with our recursive calls and we are on the way out, we increment the counter for the group
                }

            DisplayingValues();
        }


        int ConnectedComponents() // in order to get the SCCs right, we have to run 'explore' procedures on the postorder values, so each time we go through the SCC we start from a SINK VERTEX
        {
            DepthFirstSearch(); // before we start anything, we need to run DFS on the REVERSED Graph in order to find the SINK VERTICES, for the SCC procedure to work
            counter = 0;

            foreach(var v in postIndex) // here we go by the whole list of recorded post-visit values that we obtained from DFS
            {
                if (!nodes[v].visited)
                {
                    ExploreComponents(v);
                    counter += 1;
                    groups.Add(g.ToList()); // we have to use the 'To.List()' method to pass the new list as a VALUE, otherwise we will clear it in the next line and therefore won't change anything
                    g.Clear();
                }
            }

            PrintGroup();
            return counter;
        }


        void ExploreComponents(int v) // version of the 'Explore' procedure for SCC
        {
            nodes[v].visited = true;

            foreach (int w in adj[v])
                if (!nodes[w].visited) 
                    ExploreComponents(w);

            g.Add(v); // here we append all the elements of the group while backtracking
        }


        void DisplayingValues() // testing procedure for displaying the details of the REVERSED DFS
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


        void PrintGroup()
        {
            foreach (List<int> i in groups)
            {
                foreach (int j in i)
                    Console.Write(j + " ");
                Console.WriteLine();
            }
        }

        static List<int>[] ReverseGraph(List<int>[] lst)
        {
            List<int>[] reversedList = new List<int>[lst.Length];
            for (int i = 0; i < lst.Length; i++)
                reversedList[i] = new List<int>();

            for (int i = 0; i < lst.Length; i++)
            {
                foreach (var element in lst[i])
                    reversedList[element].Add(i);
            }

            return reversedList;
        }


        static void Main(string[] args)
        {
            List<int>[] adjList = { new List<int>() { 1 }, new List<int>() { 2, 3 }, new List<int>() { 0, 1, 4, 6 }, new List<int>() { 0 }, new List<int>() { 0, 2 }, new List<int>() { 3 }, new List<int>(), new List<int>() { 3, 4, 5 } };

            List<int>[] adjList_rev = ReverseGraph(adjList);
            DGraphComponents dag = new DGraphComponents(adjList, adjList_rev);
            Console.WriteLine(dag.ConnectedComponents());

            Console.ReadKey();
        }
    }
}
